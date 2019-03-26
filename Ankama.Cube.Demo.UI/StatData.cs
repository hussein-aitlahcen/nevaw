using Ankama.Cube.Data;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class StatData : ScriptableObject
	{
		[SerializeField]
		private Color m_allyColor;

		[SerializeField]
		private Color m_opponentColor;

		[SerializeField]
		private Color m_bestValueColor;

		[SerializeField]
		private Color m_worstValueColor;

		[SerializeField]
		private Color m_neutralValueColor;

		[SerializeField]
		private float m_illuMvpScale;

		[SerializeField]
		private float m_illuNeutralScale;

		[SerializeField]
		private float m_openBoardDelay;

		[SerializeField]
		private float m_openBoardDuration;

		[SerializeField]
		private float m_openBoardLineTweenDuration;

		[SerializeField]
		private Ease m_openBoardLineTweenEase;

		[SerializeField]
		private FightStatTypeIconDico m_fightStatTypeIcons;

		public Color allyColor => m_allyColor;

		public Color opponentColor => m_opponentColor;

		public Color bestValueColor => m_bestValueColor;

		public Color worstValueColor => m_worstValueColor;

		public Color neutralValueColor => m_neutralValueColor;

		public float illuMvpScale => m_illuMvpScale;

		public float illuNeutralScale => m_illuNeutralScale;

		public float openBoardDelay => m_openBoardDelay;

		public float openBoardDuration => m_openBoardDuration;

		public float openBoardLineTweenDuration => m_openBoardLineTweenDuration;

		public Ease openBoardLineTweenEase => m_openBoardLineTweenEase;

		public Sprite GetIcon(FightStatType type)
		{
			if (((Dictionary<FightStatType, Sprite>)m_fightStatTypeIcons).TryGetValue(type, out Sprite value))
			{
				return value;
			}
			return null;
		}

		public StatData()
			: this()
		{
		}
	}
}
