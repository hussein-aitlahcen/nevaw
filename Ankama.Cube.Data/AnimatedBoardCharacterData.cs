using Ankama.Animations;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public abstract class AnimatedBoardCharacterData : AnimatedCharacterData
	{
		[SerializeField]
		private AnimatedObjectDefinition m_animatedObjectDefinition;

		[SerializeField]
		private CharacterHeight m_height = CharacterHeight.Normal;

		public AnimatedObjectDefinition animatedObjectDefinition => m_animatedObjectDefinition;

		public CharacterHeight height => m_height;
	}
}
