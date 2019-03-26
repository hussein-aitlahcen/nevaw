using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	[UsedImplicitly]
	public sealed class DynamicObject : MonoBehaviour
	{
		private const float MaxFreeFallVelocity = 4.905f;

		private const float BounceVelocityCutoff = -1.22625f;

		[UsedImplicitly]
		[Range(0f, 1f)]
		[SerializeField]
		private float m_bounceFactor = 0.5f;

		private DynamicObject m_parent;

		private readonly List<DynamicObject> m_children = new List<DynamicObject>();

		private Vector3 m_localUpVector;

		private float m_inheritedVelocity;

		private float m_localHeight;

		[UsedImplicitly]
		private void Awake()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			m_localUpVector = this.get_transform().get_parent().InverseTransformDirection(Vector3.get_up());
		}

		public void BuildHierarchy()
		{
			Transform transform = this.get_transform();
			int childCount = transform.get_childCount();
			for (int i = 0; i < childCount; i++)
			{
				DynamicObject component = transform.GetChild(i).GetComponent<DynamicObject>();
				if (null != component)
				{
					component.m_parent = this;
					component.BuildHierarchy();
					m_children.Add(component);
				}
			}
		}

		public unsafe void SetCellObject([CanBeNull] CellObject previousCell, [NotNull] CellObject containerCell)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			DynamicObject parent = m_parent;
			if (null == parent)
			{
				if (null == previousCell)
				{
					containerCell.AddRootDynamicObject(this);
				}
				else
				{
					float num = containerCell.AcquireRootDynamicObject(this, previousCell);
					m_localHeight += num;
				}
			}
			else
			{
				parent.m_children.Remove(this);
				if (null == previousCell)
				{
					containerCell.AddRootDynamicObject(this);
				}
				else
				{
					float num2 = containerCell.AcquireDynamicObject(this, previousCell);
					float num3 = ((IntPtr)(void*)this.get_transform().get_position()).y - (((IntPtr)(void*)previousCell.get_transform().get_position()).y + 0.5f);
					m_localHeight = num2 + num3;
				}
			}
			m_localUpVector = this.get_transform().get_parent().InverseTransformDirection(Vector3.get_up());
			if (m_localHeight <= 0f)
			{
				Vector3 localPosition = this.get_transform().get_localPosition();
				localPosition += m_localUpVector * (0.5f - Vector3.Dot(localPosition, m_localUpVector));
				this.get_transform().set_localPosition(localPosition);
				m_localHeight = 0f;
				m_inheritedVelocity = 0f;
			}
		}

		public void ResolvePhysics(float deltaTime, float gravityVelocity)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			if (m_localHeight > 0f || m_inheritedVelocity > 0f)
			{
				float num = m_inheritedVelocity + gravityVelocity;
				float num2 = num * deltaTime;
				float num3 = m_localHeight + num2;
				if (num3 <= 0f)
				{
					if (num > -1.22625f || m_bounceFactor <= 0f)
					{
						Vector3 localPosition = this.get_transform().get_localPosition();
						localPosition -= m_localUpVector * m_localHeight;
						this.get_transform().set_localPosition(localPosition);
						m_localHeight = 0f;
						m_inheritedVelocity = 0f;
					}
					else
					{
						num3 = (0f - num3) * m_bounceFactor;
						num = (0f - num) * m_bounceFactor;
						num2 = num3 - m_localHeight;
						Vector3 localPosition2 = this.get_transform().get_localPosition();
						localPosition2 += m_localUpVector * num2;
						this.get_transform().set_localPosition(localPosition2);
						m_localHeight = num3;
						m_inheritedVelocity = num;
					}
				}
				else
				{
					Vector3 localPosition3 = this.get_transform().get_localPosition();
					localPosition3 += m_localUpVector * num2;
					this.get_transform().set_localPosition(localPosition3);
					m_localHeight = num3;
					m_inheritedVelocity = num;
				}
				int i = 0;
				for (int count = m_children.Count; i < count; i++)
				{
					m_children[i].ResolvePhysics(deltaTime, num2, num, gravityVelocity);
				}
			}
			else
			{
				int j = 0;
				for (int count2 = m_children.Count; j < count2; j++)
				{
					m_children[j].ResolvePhysics(deltaTime, gravityVelocity);
				}
			}
		}

		public void ResolvePhysics(float deltaTime, float parentDeltaHeight, float parentVelocity, float gravityVelocity)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			float num;
			float num2;
			if (m_localHeight <= 0f)
			{
				if (m_inheritedVelocity > parentVelocity)
				{
					num = m_inheritedVelocity;
					num2 = num * deltaTime;
					float num3 = num2 - parentDeltaHeight;
					Vector3 localPosition = this.get_transform().get_localPosition();
					localPosition += m_localUpVector * (num3 - m_localHeight);
					this.get_transform().set_localPosition(localPosition);
					m_localHeight = num3;
				}
				else
				{
					num = parentVelocity;
					num2 = parentDeltaHeight;
					m_inheritedVelocity = Mathf.Max(gravityVelocity, Mathf.Min(4.905f, num));
				}
			}
			else
			{
				num = m_inheritedVelocity + gravityVelocity;
				num2 = num * deltaTime;
				float num4 = m_localHeight + (num2 - parentDeltaHeight);
				if (num4 <= 0f)
				{
					if (num > -1.22625f || m_bounceFactor <= 0f)
					{
						Vector3 localPosition2 = this.get_transform().get_localPosition();
						localPosition2 -= m_localUpVector * m_localHeight;
						this.get_transform().set_localPosition(localPosition2);
						m_localHeight = 0f;
						m_inheritedVelocity = Mathf.Max(gravityVelocity, Mathf.Min(4.905f, parentVelocity));
					}
					else
					{
						num4 = (0f - num4) * m_bounceFactor;
						num = (0f - num) * m_bounceFactor;
						num2 = num4 - m_localHeight;
						Vector3 localPosition3 = this.get_transform().get_localPosition();
						localPosition3 += m_localUpVector * num2;
						this.get_transform().set_localPosition(localPosition3);
						m_localHeight = num4;
						m_inheritedVelocity = num;
					}
				}
				else
				{
					Vector3 localPosition4 = this.get_transform().get_localPosition();
					localPosition4 += m_localUpVector * (num4 - m_localHeight);
					this.get_transform().set_localPosition(localPosition4);
					m_localHeight = num4;
					m_inheritedVelocity = num;
				}
			}
			int i = 0;
			for (int count = m_children.Count; i < count; i++)
			{
				m_children[i].ResolvePhysics(deltaTime, num2, num, gravityVelocity);
			}
		}

		public DynamicObject()
			: this()
		{
		}
	}
}
