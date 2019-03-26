using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.Fight.Windows
{
	public class FightTooltip : AbstractTooltipWindow
	{
		[SerializeField]
		private FightTooltipContent m_content;

		[SerializeField]
		private KeywordTooltipContainer m_keywordsContainer;

		public override void Awake()
		{
			base.Awake();
			m_content.Setup();
		}

		public void Initialize([NotNull] ITooltipDataProvider dataProvider)
		{
			m_content.Initialize(dataProvider);
			m_keywordsContainer.Initialize(dataProvider);
		}

		protected override void DisplayTooltip(Vector3 worldPosition)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.DisplayTooltip(worldPosition);
			SetKeywordContainerAlignment();
			m_keywordsContainer.Show();
		}

		private unsafe void SetKeywordContainerAlignment()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			_003F val = this.get_transform();
			Vector2 pivot = val.get_pivot();
			float num = ((IntPtr)(void*)val.get_lossyScale()).x * (m_keywordsContainer.width + m_keywordsContainer.spacing);
			KeywordTooltipContainer.HorizontalAlignment h = (!(((IntPtr)(void*)pivot).x > 0.5f)) ? KeywordTooltipContainer.HorizontalAlignment.Right : KeywordTooltipContainer.HorizontalAlignment.Left;
			KeywordTooltipContainer.VerticalAlignment v = (!(((IntPtr)(void*)pivot).y > 0.5f)) ? KeywordTooltipContainer.VerticalAlignment.Down : KeywordTooltipContainer.VerticalAlignment.Up;
			if (base.borderDistanceToScreen.right < num)
			{
				h = KeywordTooltipContainer.HorizontalAlignment.Left;
			}
			if (base.borderDistanceToScreen.left < num)
			{
				h = KeywordTooltipContainer.HorizontalAlignment.Right;
			}
			m_keywordsContainer.SetAlignement(h, v);
		}

		public override void Hide()
		{
			base.Hide();
			m_keywordsContainer.Hide();
		}
	}
}
