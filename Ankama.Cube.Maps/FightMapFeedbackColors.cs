using Ankama.Cube.Fight.Entities;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public sealed class FightMapFeedbackColors : ScriptableObject
	{
		[SerializeField]
		private Color m_localPlayerColor = new Color(0f, 58f / 85f, 1f);

		[SerializeField]
		private Color m_allyPlayerColor = new Color(0f, 1f, 1f);

		[SerializeField]
		private Color m_localOpponentColor = new Color(1f, 0f, 127f / 255f);

		[SerializeField]
		private Color m_opponentPlayerColor = new Color(1f, 0.1764706f, 71f / (339f * MathF.PI));

		[SerializeField]
		private Color m_targetableAreaColor = new Color(0f, 58f / 85f, 1f);

		public Color targetableAreaColor => m_targetableAreaColor;

		public Color GetPlayerColor(PlayerType playerType)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			switch (playerType)
			{
			case PlayerType.Player:
				return m_localPlayerColor;
			case PlayerType.Opponent | PlayerType.Local:
				return m_localOpponentColor;
			default:
				if (playerType.HasFlag(PlayerType.Ally))
				{
					return m_allyPlayerColor;
				}
				if (playerType.HasFlag(PlayerType.Opponent))
				{
					return m_opponentPlayerColor;
				}
				throw new ArgumentOutOfRangeException("playerType", playerType, null);
			}
		}

		public FightMapFeedbackColors()
			: this()
		{
		}//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)

	}
}
