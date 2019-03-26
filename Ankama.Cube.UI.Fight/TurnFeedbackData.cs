using System;
using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	public class TurnFeedbackData : ScriptableObject
	{
		[Serializable]
		public struct PlayerSideData
		{
			[TextKey]
			[SerializeField]
			public int messageKey;

			[SerializeField]
			public Sprite icon;

			[SerializeField]
			public Color nameColor;

			[SerializeField]
			public Color titleColor;
		}

		[SerializeField]
		public PlayerSideData player;

		[SerializeField]
		public PlayerSideData playerTeam;

		[SerializeField]
		public PlayerSideData opponent;

		[SerializeField]
		public PlayerSideData opponentTeam;

		[SerializeField]
		public PlayerSideData boss;

		public TurnFeedbackData()
			: this()
		{
		}
	}
}
