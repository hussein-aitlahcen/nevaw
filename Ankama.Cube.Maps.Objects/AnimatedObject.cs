using Ankama.Animations;
using Ankama.Animations.Events;
using Ankama.Cube.Render;
using Ankama.Cube.SRP;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	[SelectionBase]
	[ExecuteInEditMode]
	public class AnimatedObject : MonoBehaviour
	{
		public const float AnimatedObjectScale = 0.01f;

		public const int MinFrameCountDelay = 15;

		[SerializeField]
		private AnimatedObjectDefinition m_animatedObjectDefinition;

		[SerializeField]
		private string m_defaultAnimName;

		[SerializeField]
		private bool m_invertAxis;

		private Animator2D m_animator2D;

		private unsafe void OnEnable()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			m_animator2D = CreateAnimatorComponent();
			if (m_animatedObjectDefinition != null && m_animatedObjectDefinition != null)
			{
				m_animator2D.add_Initialised(new Animator2DInitialisedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				m_animator2D.SetDefinition(m_animatedObjectDefinition, null, (Graphic[])null);
			}
		}

		private void OnDisable()
		{
			if (null != m_animator2D)
			{
				DestroyUtility.Destroy(m_animator2D.get_gameObject());
			}
		}

		public void PlayAnim(string animName, bool loop, bool randomStartFrame = false)
		{
			if (!(m_animator2D == null) && !string.IsNullOrEmpty(animName))
			{
				m_animator2D.SetAnimation(animName, loop, false, true);
				if (randomStartFrame)
				{
					int currentFrame = Random.Range(0, Mathf.Min(15, m_animator2D.get_animationFrameCount()));
					m_animator2D.set_currentFrame(currentFrame);
				}
			}
		}

		private unsafe void InitCallback(object sender, Animator2DInitialisedEventArgs e)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			m_animator2D.remove_Initialised(new Animator2DInitialisedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			PlayAnim(GetDefaultAnim(), loop: true, randomStartFrame: true);
		}

		private string GetDefaultAnim()
		{
			if (!string.IsNullOrEmpty(m_defaultAnimName))
			{
				return m_defaultAnimName;
			}
			if (m_animatedObjectDefinition == null || m_animatedObjectDefinition == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(m_animatedObjectDefinition.defaultAnimationName))
			{
				return m_animatedObjectDefinition.defaultAnimationName;
			}
			if (m_animatedObjectDefinition.get_animationCount() > 0)
			{
				return m_animatedObjectDefinition.GetAnimationName(0);
			}
			return null;
		}

		private Animator2D CreateAnimatorComponent()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			GameObject val = new GameObject("Animator2D Hidden");
			val.get_transform().SetParent(this.get_transform(), false);
			val.get_transform().set_localScale(new Vector3(0.01f, 0.01f, 0.01f));
			val.get_transform().set_localRotation(Quaternion.Euler(0f, (float)(m_invertAxis ? 225 : 45), 0f));
			Animator2D result = val.AddComponent<Animator2D>();
			val.AddComponent<CharacterMeshShaderProperties>();
			return result;
		}

		public AnimatedObject()
			: this()
		{
		}
	}
}
