using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class MovementFeedbackResources : ScriptableObject
	{
		public const int CornerNegXNegZ = 0;

		public const int CornerNegXPosZ = 1;

		public const int CornerPosXNegZ = 2;

		public const int CornerPosXPosZ = 3;

		public const int EndNegX = 4;

		public const int EndPosZ = 5;

		public const int StartNegX = 6;

		public const int StartPosZ = 7;

		public const int ThroughNegZ = 8;

		public const int ThroughPosZ = 9;

		[SerializeField]
		private Sprite[] m_sprites;

		[Header("Cursors")]
		[SerializeField]
		private Sprite m_allyCursorSprite;

		public Sprite[] sprites => m_sprites;

		public Sprite allyCursorSprite => m_allyCursorSprite;

		public MovementFeedbackResources()
			: this()
		{
		}
	}
}
