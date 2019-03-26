using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps.Feedbacks
{
	[ExecuteInEditMode]
	public class FloatingCounterFloatingObject : MonoBehaviour
	{
		private static readonly int s_colorPropertyId = Shader.PropertyToID("_Color");

		[SerializeField]
		private GameObject m_castFX;

		[SerializeField]
		private AnimationCurve m_spawnAnimationCurve;

		[SerializeField]
		private float m_oscillationSpeed;

		[SerializeField]
		private float m_oscillationAmplitude;

		[HideInInspector]
		[SerializeField]
		private Renderer[] m_renderers;

		private float m_oscillationPhase;

		private float m_radius;

		private float m_positionY;

		private MaterialPropertyBlock m_colorModifierPropertyBlock;

		public unsafe void Spawn(Vector3 position, float duration, float phase, float radius)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			m_oscillationPhase = phase;
			m_radius = radius;
			m_positionY = ((IntPtr)(void*)position).y;
			Vector3 normalized = position.get_normalized();
			normalized.y = 0f;
			this.get_transform().set_localRotation(Quaternion.LookRotation(normalized, new Vector3(0f, 1f, 0f)));
			this.StartCoroutine(SpawnCoroutine(position, duration));
		}

		public unsafe void Reposition(float angle, float duration, Ease ease)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			TweenSettingsExtensions.SetEase<Tweener>(DOVirtual.Float(m_oscillationPhase, angle, duration, new TweenCallback<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/)), ease);
		}

		public unsafe void FadeOut(float duration)
		{
			DOVirtual.Float(1f, 0f, duration, new TweenCallback<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public void SetColorModifier(Color color)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			MaterialPropertyBlock val = m_colorModifierPropertyBlock;
			if (val == null)
			{
				val = (m_colorModifierPropertyBlock = new MaterialPropertyBlock());
			}
			val.SetColor(s_colorPropertyId, color);
			Renderer[] renderers = m_renderers;
			int num = renderers.Length;
			for (int i = 0; i < num; i++)
			{
				Renderer val2 = renderers[i];
				if (null != val2)
				{
					SpriteRenderer val3 = val2 as SpriteRenderer;
					if (null != val3)
					{
						val3.set_color(color);
					}
					else
					{
						val2.SetPropertyBlock(val);
					}
				}
			}
		}

		public void Clear()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			Object.Instantiate<GameObject>(m_castFX, this.get_transform().get_position(), Quaternion.get_identity());
			Object.Destroy(this.get_gameObject());
		}

		private IEnumerator SpawnCoroutine(Vector3 position, float duration)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Transform selfTransform = this.get_transform();
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			Vector3 startPosition = position + position.get_normalized() * 3f;
			Vector3 startScale = new Vector3(0f, 0f, 0f);
			Vector3 endScale = new Vector3(1f, 1f, 1f);
			for (float f = 0f; f < 1f; f += Time.get_deltaTime() / duration)
			{
				float num = m_spawnAnimationCurve.Evaluate(f);
				selfTransform.set_localPosition(Vector3.LerpUnclamped(startPosition, position, num));
				selfTransform.set_localScale(Vector3.LerpUnclamped(startScale, endScale, f));
				yield return wait;
			}
			selfTransform.set_localPosition(position);
			selfTransform.set_localScale(endScale);
			if (!Mathf.Approximately(m_oscillationAmplitude, 0f))
			{
				while (true)
				{
					Vector3 localPosition = selfTransform.get_localPosition();
					localPosition.y = m_positionY + Mathf.Sin(m_oscillationSpeed * Time.get_time() + m_oscillationPhase) * m_oscillationAmplitude;
					selfTransform.set_localPosition(localPosition);
					yield return null;
				}
			}
		}

		private void RepositionTweenCallback(float phase)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			m_oscillationPhase = phase;
			this.get_transform().set_localPosition(new Vector3(Mathf.Cos(phase) * m_radius, m_positionY, Mathf.Sin(phase) * m_radius));
			Vector3 localPosition = this.get_transform().get_localPosition();
			Vector3 normalized = localPosition.get_normalized();
			normalized.y = 0f;
			this.get_transform().set_localRotation(Quaternion.LookRotation(normalized, new Vector3(0f, 1f, 0f)));
		}

		private void FadeOutTweenCallback(float alpha)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Color color = default(Color);
			color._002Ector(1f, 1f, 1f, alpha);
			Renderer[] renderers = m_renderers;
			int num = renderers.Length;
			for (int i = 0; i < num; i++)
			{
				renderers[i].get_material().set_color(color);
			}
		}

		public FloatingCounterFloatingObject()
			: this()
		{
		}
	}
}
