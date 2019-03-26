using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps.VisualEffects
{
	[SelectionBase]
	[ExecuteInEditMode]
	public sealed class VisualEffectGroup : VisualEffect
	{
		[SerializeField]
		private List<VisualEffect> m_children = new List<VisualEffect>();

		public override bool IsAlive()
		{
			List<VisualEffect> children = m_children;
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				if (children[i].IsAlive())
				{
					return true;
				}
			}
			return false;
		}

		protected override void PlayInternal()
		{
			List<VisualEffect> children = m_children;
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				children[i].GroupPlayedInternal();
			}
		}

		protected override void PauseInternal()
		{
			List<VisualEffect> children = m_children;
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				children[i].GroupPausedInternal();
			}
		}

		protected override void StopInternal(VisualEffectStopMethod stopMethod)
		{
			List<VisualEffect> children = m_children;
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				children[i].GroupStoppedInternal(stopMethod);
			}
		}

		protected override void ClearInternal()
		{
			List<VisualEffect> children = m_children;
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				children[i].GroupClearedInternal();
			}
		}

		public override void SetSortingOrder(int value)
		{
			List<VisualEffect> children = m_children;
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				children[i].SetSortingOrder(value);
			}
		}

		public override void SetColorModifier(Color color)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			List<VisualEffect> children = m_children;
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				children[i].SetColorModifier(color);
			}
		}
	}
}
