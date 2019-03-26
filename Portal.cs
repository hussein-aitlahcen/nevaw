using Ankama.Cube.Audio.UI;
using Ankama.Cube.Maps;
using System;
using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
	[Serializable]
	private class AnimatedSprite
	{
		public SpriteRenderer spriteRenderer;

		public Color defaultColor = new Color(0f, 0f, 0f, 0f);

		public Color highlightColor = new Color(0f, 0f, 0f, 0f);

		public Color currentColor = new Color(0f, 0f, 0f, 0f);

		public Coroutine animationCoroutine;
	}

	[SerializeField]
	private ParticleSystem[] m_highLightParticleSystems;

	[SerializeField]
	private ParticleSystem[] m_openParticleSystems;

	[SerializeField]
	private AnimatedSprite[] m_animatedSprites;

	[SerializeField]
	private SpriteRenderer m_groundIcon;

	[SerializeField]
	private Transform m_scaleTransform;

	[SerializeField]
	private Vector3 m_defaultScale = new Vector3(1f, 1f, 1f);

	[SerializeField]
	private Vector3 m_highLigthScale = new Vector3(1.25f, 1.25f, 1.25f);

	[SerializeField]
	private AnimationCurve m_scaleCurve;

	[SerializeField]
	private float m_animationDuration;

	[SerializeField]
	private float m_groundEffectDuration;

	[Header("Audio")]
	[SerializeField]
	private AudioEventUIPlayWhileEnabled m_highlightLoopAudio;

	[SerializeField]
	private AudioEventUITriggerOnEnable m_openingAudio;

	private MaterialPropertyBlock m_materialPropertyBlock;

	private int m_openID;

	private int m_highLightID;

	private Vector3 m_currentScale;

	private Coroutine m_scaleCoroutine;

	private Coroutine m_animateGroundFXCoroutine;

	private float m_openCurrentValue;

	private float m_highLightCurrentValue;

	private MaterialPropertyBlock materialPropertyBlock
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (m_materialPropertyBlock == null)
			{
				m_materialPropertyBlock = new MaterialPropertyBlock();
				m_groundIcon.GetPropertyBlock(m_materialPropertyBlock);
			}
			return m_materialPropertyBlock;
		}
	}

	private void Awake()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		m_openID = Shader.PropertyToID("_Open");
		m_highLightID = Shader.PropertyToID("_HighLight");
		m_currentScale = m_defaultScale;
		m_scaleTransform.set_localScale(m_currentScale);
		m_openCurrentValue = 0f;
		m_highLightCurrentValue = 0f;
		for (int i = 0; i < m_animatedSprites.Length; i++)
		{
			m_animatedSprites[i].currentColor = m_animatedSprites[i].defaultColor;
			m_animatedSprites[i].spriteRenderer.set_color(m_animatedSprites[i].currentColor);
		}
		m_highlightLoopAudio.get_gameObject().SetActive(false);
		m_openingAudio.get_gameObject().SetActive(false);
	}

	public void SetState(ZaapObject.ZaapState zaapState)
	{
		bool highLight = false;
		switch (zaapState)
		{
		case ZaapObject.ZaapState.Normal:
			PlayParticles(m_highLightParticleSystems, play: false);
			PlayParticles(m_openParticleSystems, play: false);
			m_openingAudio.get_gameObject().SetActive(false);
			m_highlightLoopAudio.get_gameObject().SetActive(false);
			break;
		case ZaapObject.ZaapState.Highlight:
		case ZaapObject.ZaapState.Clicked:
			PlayParticles(m_highLightParticleSystems, play: true);
			PlayParticles(m_openParticleSystems, play: false);
			highLight = true;
			m_openingAudio.get_gameObject().SetActive(false);
			m_highlightLoopAudio.get_gameObject().SetActive(true);
			break;
		case ZaapObject.ZaapState.Open:
			PlayParticles(m_highLightParticleSystems, play: true);
			PlayParticles(m_openParticleSystems, play: true);
			highLight = true;
			m_openingAudio.get_gameObject().SetActive(true);
			m_highlightLoopAudio.get_gameObject().SetActive(false);
			break;
		}
		for (int i = 0; i < m_animatedSprites.Length; i++)
		{
			if (m_animatedSprites[i].animationCoroutine != null)
			{
				this.StopCoroutine(m_animatedSprites[i].animationCoroutine);
			}
			m_animatedSprites[i].animationCoroutine = this.StartCoroutine(AnimateSpriteColorCoroutine(m_animatedSprites[i], highLight));
		}
		if (m_scaleCoroutine != null)
		{
			this.StopCoroutine(m_scaleCoroutine);
		}
		m_scaleCoroutine = this.StartCoroutine(ScaleCoroutine(highLight));
		if (m_animateGroundFXCoroutine != null)
		{
			this.StopCoroutine(m_animateGroundFXCoroutine);
		}
		m_animateGroundFXCoroutine = this.StartCoroutine(AnimateGroundFXCoroutine(zaapState));
	}

	private IEnumerator AnimateSpriteColorCoroutine(AnimatedSprite animatedSprite, bool highLight)
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		if (highLight)
		{
			Color startColor2 = animatedSprite.currentColor;
			for (float f2 = 0f; f2 < 1f; f2 += Time.get_deltaTime() / m_animationDuration)
			{
				animatedSprite.currentColor = Color.Lerp(startColor2, animatedSprite.highlightColor, f2);
				animatedSprite.spriteRenderer.set_color(animatedSprite.currentColor);
				yield return wait;
			}
			animatedSprite.currentColor = animatedSprite.highlightColor;
			animatedSprite.spriteRenderer.set_color(animatedSprite.currentColor);
		}
		else
		{
			Color startColor2 = animatedSprite.currentColor;
			for (float f2 = 0f; f2 < 1f; f2 += Time.get_deltaTime())
			{
				animatedSprite.currentColor = Color.Lerp(startColor2, animatedSprite.defaultColor, f2);
				animatedSprite.spriteRenderer.set_color(animatedSprite.currentColor);
				yield return wait;
			}
			animatedSprite.currentColor = animatedSprite.defaultColor;
			animatedSprite.spriteRenderer.set_color(animatedSprite.currentColor);
		}
		animatedSprite.animationCoroutine = null;
	}

	private IEnumerator ScaleCoroutine(bool highLight)
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		Vector3 startScale = m_currentScale;
		Vector3 endScale = (!highLight) ? m_defaultScale : m_highLigthScale;
		for (float f = 0f; f < 1f; f += Time.get_deltaTime() / m_animationDuration)
		{
			float num = m_scaleCurve.Evaluate(f);
			m_currentScale = Vector3.LerpUnclamped(startScale, endScale, num);
			m_scaleTransform.set_localScale(m_currentScale);
			yield return wait;
		}
		m_currentScale = endScale;
		m_scaleTransform.set_localScale(m_currentScale);
		m_scaleCoroutine = null;
	}

	private IEnumerator AnimateGroundFXCoroutine(ZaapObject.ZaapState zaapState)
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float highLightStartValue = m_highLightCurrentValue;
		float openStartValue = m_openCurrentValue;
		float highLightEndValue = 0f;
		float openEndValue = 0f;
		switch (zaapState)
		{
		case ZaapObject.ZaapState.Highlight:
		case ZaapObject.ZaapState.Clicked:
			highLightEndValue = 1f;
			break;
		case ZaapObject.ZaapState.Open:
			highLightEndValue = 1f;
			openEndValue = 1f;
			break;
		}
		for (float f = 0f; f < 1f; f += Time.get_deltaTime() / m_groundEffectDuration)
		{
			m_highLightCurrentValue = Mathf.Lerp(highLightStartValue, highLightEndValue, f);
			m_openCurrentValue = Mathf.Lerp(openStartValue, openEndValue, f);
			materialPropertyBlock.SetFloat(m_highLightID, m_highLightCurrentValue);
			materialPropertyBlock.SetFloat(m_openID, m_openCurrentValue);
			m_groundIcon.SetPropertyBlock(materialPropertyBlock);
			yield return wait;
		}
		m_highLightCurrentValue = highLightEndValue;
		m_openCurrentValue = openEndValue;
		materialPropertyBlock.SetFloat(m_highLightID, m_highLightCurrentValue);
		materialPropertyBlock.SetFloat(m_openID, m_openCurrentValue);
		m_groundIcon.SetPropertyBlock(materialPropertyBlock);
		m_animateGroundFXCoroutine = null;
	}

	private void PlayParticles(ParticleSystem[] particleSystems, bool play)
	{
		if (play)
		{
			for (int i = 0; i < particleSystems.Length; i++)
			{
				particleSystems[i].Play(false);
			}
		}
		else
		{
			for (int j = 0; j < particleSystems.Length; j++)
			{
				particleSystems[j].Stop(false);
			}
		}
	}

	public Portal()
		: this()
	{
	}//IL_0010: Unknown result type (might be due to invalid IL or missing references)
	//IL_0015: Unknown result type (might be due to invalid IL or missing references)
	//IL_002a: Unknown result type (might be due to invalid IL or missing references)
	//IL_002f: Unknown result type (might be due to invalid IL or missing references)

}
