using Ankama.Cube.Data;
using Ankama.Cube.Maps.Feedbacks;
using Ankama.Utilities;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	[UsedImplicitly]
	public sealed class CellObject : MonoBehaviour
	{
		[UsedImplicitly]
		[SerializeField]
		private Vector2Int m_coords;

		private readonly List<IsoObject> m_childrenIsoObjects = new List<IsoObject>();

		private readonly List<DynamicObject> m_rootDynamicChildren = new List<DynamicObject>();

		private Vector3 m_originalLocalPosition;

		private Vector3 m_localUpVector;

		private bool m_isSleeping = true;

		private float m_controlledHeightBuffer;

		private float m_controlledLocalHeight;

		private float m_controlledVelocity;

		public IMap parentMap
		{
			get;
			private set;
		}

		public Vector2Int coords => m_coords;

		public CellHighlight highlight
		{
			get;
			private set;
		}

		public void Initialize([NotNull] IMap map)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			parentMap = map;
			Transform transform = this.get_transform();
			Transform parent = transform.get_parent();
			m_originalLocalPosition = transform.get_localPosition();
			m_localUpVector = ((null == parent) ? Vector3.get_up() : parent.InverseTransformDirection(Vector3.get_up()));
			PrepareIsoObjects();
			BuildDynamicObjectsHierarchy(transform);
		}

		public void CreateHighlight(CellHighlight prefab, Material material, uint renderLayerMask)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			CellHighlight cellHighlight = Object.Instantiate<CellHighlight>(prefab, this.get_transform(), false);
			Transform transform = cellHighlight.get_transform();
			transform.set_localPosition(this.get_transform().InverseTransformDirection(Vector3.get_up()) * 0.501f);
			transform.set_forward(-Vector3.get_up());
			cellHighlight.Initialize(material, renderLayerMask);
			highlight = cellHighlight;
		}

		public void AcquireIsoObject([NotNull] IsoObject isoObject)
		{
			m_childrenIsoObjects.Add(isoObject);
			parentMap.AddArea(isoObject.area);
		}

		public void AcquireIsoObject([NotNull] IsoObject isoObject, [NotNull] CellObject other)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			m_childrenIsoObjects.Add(isoObject);
			other.m_childrenIsoObjects.Remove(isoObject);
			Area area = isoObject.area;
			Area copy = area.GetCopy();
			area.MoveTo(coords);
			parentMap.MoveArea(copy, area);
		}

		public void RemoveIsoObject([NotNull] IsoObject isoObject)
		{
			List<IsoObject> childrenIsoObjects = m_childrenIsoObjects;
			int count = childrenIsoObjects.Count;
			for (int i = 0; i < count; i++)
			{
				if (childrenIsoObjects[i] == isoObject)
				{
					parentMap.RemoveArea(isoObject.area);
					childrenIsoObjects.RemoveAt(i);
					return;
				}
			}
			Log.Warning($"Could not find IsoObject '{isoObject}' to remove in cell object named '{this.get_name()}'.", 113, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\CellObject.cs");
		}

		public bool TryGetIsoObject<T>(out T isoObject) where T : class, IIsoObject
		{
			List<IsoObject> childrenIsoObjects = m_childrenIsoObjects;
			int count = childrenIsoObjects.Count;
			for (int i = 0; i < count; i++)
			{
				T val;
				if ((val = (childrenIsoObjects[i] as T)) != null)
				{
					isoObject = val;
					return true;
				}
			}
			isoObject = null;
			return false;
		}

		private void PrepareIsoObjects()
		{
			List<IsoObject> childrenIsoObjects = m_childrenIsoObjects;
			this.GetComponentsInChildren<IsoObject>(childrenIsoObjects);
			int count = childrenIsoObjects.Count;
			for (int i = 0; i < count; i++)
			{
				childrenIsoObjects[i].AttachToCell(this);
			}
		}

		[PublicAPI]
		public void ApplyAnimation(float heightDelta)
		{
			m_controlledHeightBuffer += heightDelta;
			m_isSleeping = false;
		}

		[PublicAPI]
		public bool CleanupAnimation(float deltaTime, float gravityVelocity)
		{
			float controlledLocalHeight = m_controlledLocalHeight;
			if (!Mathf.Approximately(controlledLocalHeight, 0f))
			{
				float num = (m_controlledVelocity + controlledLocalHeight * gravityVelocity) * deltaTime;
				m_controlledHeightBuffer = ((controlledLocalHeight > 0f) ? Mathf.Max(0f, controlledLocalHeight + num) : Mathf.Min(0f, controlledLocalHeight + num));
				m_isSleeping = false;
				return true;
			}
			return false;
		}

		[PublicAPI]
		public void ResetAnimation()
		{
			m_controlledHeightBuffer = 0f;
			m_controlledVelocity = 0f;
			m_isSleeping = false;
		}

		public bool ResolvePhysics(float deltaTime, float gravityVelocity)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			List<DynamicObject> rootDynamicChildren = m_rootDynamicChildren;
			int count = rootDynamicChildren.Count;
			if (m_isSleeping)
			{
				for (int i = 0; i < count; i++)
				{
					rootDynamicChildren[i].ResolvePhysics(deltaTime, gravityVelocity);
				}
				return true;
			}
			float controlledHeightBuffer = m_controlledHeightBuffer;
			float num = controlledHeightBuffer - m_controlledLocalHeight;
			float num2 = num / deltaTime;
			this.get_transform().set_localPosition(m_originalLocalPosition + m_localUpVector * controlledHeightBuffer);
			for (int j = 0; j < count; j++)
			{
				rootDynamicChildren[j].ResolvePhysics(deltaTime, num, num2, gravityVelocity);
			}
			m_controlledVelocity = num2;
			m_controlledLocalHeight = controlledHeightBuffer;
			m_controlledHeightBuffer = 0f;
			m_isSleeping = true;
			return false;
		}

		public void CopyPhysics(CellObject referenceCell, bool isSleeping, float deltaTime, float gravityVelocity)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			List<DynamicObject> rootDynamicChildren = m_rootDynamicChildren;
			int count = rootDynamicChildren.Count;
			float controlledLocalHeight = referenceCell.m_controlledLocalHeight;
			if (isSleeping && m_isSleeping)
			{
				for (int i = 0; i < count; i++)
				{
					rootDynamicChildren[i].ResolvePhysics(deltaTime, gravityVelocity);
				}
				m_controlledVelocity = 0f;
			}
			else
			{
				float num = controlledLocalHeight - m_controlledLocalHeight;
				float num2 = num / deltaTime;
				this.get_transform().set_localPosition(m_originalLocalPosition + m_localUpVector * controlledLocalHeight);
				for (int j = 0; j < count; j++)
				{
					rootDynamicChildren[j].ResolvePhysics(deltaTime, num, num2, gravityVelocity);
				}
				m_controlledVelocity = num2;
			}
			m_controlledLocalHeight = controlledLocalHeight;
			m_controlledHeightBuffer = 0f;
			m_isSleeping = true;
		}

		public void AddRootDynamicObject([NotNull] DynamicObject dynamicObject)
		{
			m_rootDynamicChildren.Add(dynamicObject);
		}

		public float AcquireDynamicObject([NotNull] DynamicObject dynamicObject, [NotNull] CellObject other)
		{
			m_rootDynamicChildren.Add(dynamicObject);
			return other.m_controlledLocalHeight - m_controlledLocalHeight;
		}

		public float AcquireRootDynamicObject([NotNull] DynamicObject dynamicObject, [NotNull] CellObject other)
		{
			m_rootDynamicChildren.Add(dynamicObject);
			other.m_rootDynamicChildren.Remove(dynamicObject);
			return other.m_controlledLocalHeight - m_controlledLocalHeight;
		}

		private void BuildDynamicObjectsHierarchy(Transform t)
		{
			int childCount = t.get_childCount();
			for (int i = 0; i < childCount; i++)
			{
				Transform child = t.GetChild(i);
				DynamicObject component = child.GetComponent<DynamicObject>();
				if (null != component)
				{
					component.BuildHierarchy();
					m_rootDynamicChildren.Add(component);
				}
				else
				{
					BuildDynamicObjectsHierarchy(child);
				}
			}
		}

		public CellObject()
			: this()
		{
		}
	}
}
