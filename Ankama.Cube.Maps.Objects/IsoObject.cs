using Ankama.Cube.Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public abstract class IsoObject : MonoBehaviour
	{
		protected CellObject m_cellObject;

		protected Area m_area;

		protected Vector3 m_localRightVector;

		protected Vector3 m_localForwardVector;

		protected Vector3 m_localUpVector;

		public abstract IsoObjectDefinition definition
		{
			get;
			protected set;
		}

		public CellObject cellObject => m_cellObject;

		public Area area => m_area;

		protected virtual void OnEnable()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			Transform parent = this.get_transform().get_parent();
			if (null == parent)
			{
				m_localRightVector = Vector3.get_right();
				m_localForwardVector = Vector3.get_forward();
				m_localUpVector = Vector3.get_up();
			}
			else
			{
				m_localRightVector = parent.InverseTransformDirection(Vector3.get_right());
				m_localForwardVector = parent.InverseTransformDirection(Vector3.get_forward());
				m_localUpVector = parent.InverseTransformDirection(Vector3.get_up());
			}
		}

		public virtual void InitializeDefinitionAndArea([NotNull] IsoObjectDefinition isoObjectDefinition, int x, int y)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			definition = isoObjectDefinition;
			IObjectAreaDefinition areaDefinition = isoObjectDefinition.areaDefinition;
			if (areaDefinition != null)
			{
				Vector2Int position = default(Vector2Int);
				position._002Ector(x, y);
				m_area = isoObjectDefinition.areaDefinition.ToArea(position);
			}
		}

		public virtual void AttachToCell([NotNull] CellObject containerCell)
		{
			m_cellObject = containerCell;
		}

		public void DetachFromCell()
		{
			if (null != m_cellObject)
			{
				m_cellObject.RemoveIsoObject(this);
				m_cellObject = null;
			}
		}

		public virtual void Destroy()
		{
			Object.Destroy(this.get_gameObject());
		}

		protected IsoObject()
			: this()
		{
		}
	}
}
