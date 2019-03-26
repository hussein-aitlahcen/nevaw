using UnityEngine;

namespace Ankama.Cube.Animations
{
	public class SpriteAnimation : MonoBehaviour
	{
		[SerializeField]
		private Sprite[] m_sprites;

		[SerializeField]
		private SpriteRenderer[] m_spriteRenderers;

		[SerializeField]
		private float m_animationSpeed;

		private float m_animation;

		private float m_animationLength;

		private void Awake()
		{
			m_animationLength = m_sprites.Length;
		}

		private void Update()
		{
			m_animation = Mathf.Repeat(m_animation + m_animationSpeed * Time.get_deltaTime(), m_animationLength);
			Sprite sprite = m_sprites[Mathf.FloorToInt(m_animation)];
			for (int i = 0; i < m_spriteRenderers.Length; i++)
			{
				m_spriteRenderers[i].set_sprite(sprite);
			}
		}

		public SpriteAnimation()
			: this()
		{
		}
	}
}
