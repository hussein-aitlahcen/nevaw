using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public abstract class MovableIsoObject : IsoObject
	{
		public Vector2 innerCellPosition
		{
			get;
			private set;
		}

		public override void AttachToCell(CellObject containerCell)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			base.AttachToCell(containerCell);
			Vector3 localPosition = this.get_transform().get_localPosition();
			float num = Vector3.Dot(m_localRightVector, localPosition);
			float num2 = Vector3.Dot(m_localForwardVector, localPosition);
			innerCellPosition = new Vector2(num, num2);
		}

		public virtual void SetCellObject([NotNull] CellObject containerCell)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			if (containerCell == m_cellObject)
			{
				return;
			}
			float num = Vector3.Dot(m_localUpVector, this.get_transform().get_localPosition());
			Transform transform = containerCell.get_transform();
			m_localRightVector = transform.InverseTransformDirection(Vector3.get_right());
			m_localForwardVector = transform.InverseTransformDirection(Vector3.get_forward());
			m_localUpVector = transform.InverseTransformDirection(Vector3.get_up());
			this.get_transform().SetParent(transform, true);
			Vector3 localPosition = this.get_transform().get_localPosition();
			float num2 = Vector3.Dot(m_localRightVector, localPosition);
			float num3 = Vector3.Dot(m_localForwardVector, localPosition);
			innerCellPosition = new Vector2(num2, num3);
			if (null == m_cellObject)
			{
				containerCell.AcquireIsoObject(this);
			}
			else
			{
				float num4 = Vector3.Dot(m_localUpVector, localPosition);
				if (!Mathf.Approximately(num, num4))
				{
					localPosition += m_localUpVector * (num - num4);
					this.get_transform().set_localPosition(localPosition);
				}
				containerCell.AcquireIsoObject(this, m_cellObject);
			}
			m_cellObject = containerCell;
		}

		public unsafe void SetCellObjectInnerPosition(Vector2 innerPosition)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			if (innerCellPosition != innerPosition)
			{
				Vector3 localPosition = this.get_transform().get_localPosition();
				localPosition += m_localRightVector * (((IntPtr)(void*)innerPosition).x - ((IntPtr)(void*)innerCellPosition).x);
				localPosition += m_localForwardVector * (((IntPtr)(void*)innerPosition).y - ((IntPtr)(void*)innerCellPosition).y);
				this.get_transform().set_localPosition(localPosition);
				innerCellPosition = innerPosition;
			}
		}
	}
}
