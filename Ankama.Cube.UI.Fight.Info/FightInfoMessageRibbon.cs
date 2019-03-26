using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.UI.Components;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight.Info
{
	public class FightInfoMessageRibbon : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup m_fieldView;

		[SerializeField]
		private GameObject m_visualRoot;

		[SerializeField]
		private TextField m_playerOriginalTextField;

		[Header("Ribbon")]
		[SerializeField]
		private float m_verticalSpacing = 45f;

		[Header("IconRoot")]
		[SerializeField]
		private Image m_iconImg;

		[SerializeField]
		private GameObject m_countRoot;

		[SerializeField]
		private UISpriteTextRenderer m_countText;

		[SerializeField]
		private FightInfoMessageRibbonData m_datas;

		private List<string> m_messageParameter;

		private FightInfoValueProvider m_valueProvider;

		public void Awake()
		{
			m_messageParameter = new List<string>();
		}

		public unsafe void PlayAnimation(int ribbonMessageID, Action<FightInfoMessageRibbon> callback)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Expected O, but got Unknown
			this.get_gameObject().SetActive(true);
			m_playerOriginalTextField.SetText(ribbonMessageID, GetProvider());
			m_fieldView.set_alpha(0f);
			m_visualRoot.get_transform().set_localPosition(new Vector3(0f, -100f, 0f));
			m_visualRoot.get_transform().set_localScale(new Vector3(3f, 3f, 3f));
			Sequence obj = DOTween.Sequence();
			TweenSettingsExtensions.Insert(obj, 0f, DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 1f, 0.5f));
			TweenSettingsExtensions.Insert(obj, 0f, DOTween.To(new DOGetter<Vector3>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Vector3>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), Vector3.get_zero(), 0.5f));
			TweenSettingsExtensions.Insert(obj, 0f, DOTween.To(new DOGetter<Vector3>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Vector3>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), Vector3.get_one(), 0.3f));
			TweenSettingsExtensions.Insert(obj, 2f, TweenSettingsExtensions.SetDelay<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, 0.3f), 2f));
			_003C_003Ec__DisplayClass9_0 _003C_003Ec__DisplayClass9_;
			TweenSettingsExtensions.OnComplete<Sequence>(obj, new TweenCallback((object)_003C_003Ec__DisplayClass9_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			TweenExtensions.Play<Sequence>(obj);
			this.get_transform().SetAsLastSibling();
		}

		public void Initialise(MessageInfoType msgType, int iconID, Color bgColor, int countValue = 0)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			m_countText.set_color(bgColor);
			MessageInfoIconData[] icons = m_datas.icons;
			if (iconID < icons.Length)
			{
				MessageInfoIconData messageInfoIconData = icons[iconID];
				m_iconImg.set_sprite(messageInfoIconData.visual);
				m_iconImg.set_color(messageInfoIconData.useColor ? bgColor : Color.get_white());
			}
			else
			{
				m_iconImg.get_gameObject().SetActive(false);
			}
			switch (msgType)
			{
			case MessageInfoType.Default:
				m_countRoot.SetActive(false);
				break;
			case MessageInfoType.Score:
				m_countRoot.SetActive(true);
				m_countText.text = countValue.ToString();
				break;
			default:
				throw new ArgumentOutOfRangeException("msgType", msgType, null);
			}
		}

		public void SetExpectedIndex(int expected, bool tween)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = default(Vector3);
			val._002Ector(0f, (0f - m_verticalSpacing) * (float)expected, 0f);
			if (tween)
			{
				this.get_transform().set_localPosition(new Vector3(0f, (0f - m_verticalSpacing) * (float)(expected + 1), 0f));
				ShortcutExtensions.DOLocalMove(this.get_transform(), val, 0.5f, false);
			}
			else
			{
				this.get_transform().set_localPosition(val);
			}
		}

		private Vector3 VisualRootLocalPositionGetter()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return m_visualRoot.get_transform().get_localPosition();
		}

		private void VisualRootLocalPositionSetter(Vector3 value)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			m_visualRoot.get_transform().set_localPosition(value);
		}

		private Vector3 VisualRootLocalScaleGetter()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return m_visualRoot.get_transform().get_localScale();
		}

		private void VisualRootLocalScaleSetter(Vector3 value)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			m_visualRoot.get_transform().set_localScale(value);
		}

		private float FieldViewAlphaGetter()
		{
			return m_fieldView.get_alpha();
		}

		private void FieldViewAlphaSetter(float value)
		{
			m_fieldView.set_alpha(value);
		}

		public void ClearParameters()
		{
			m_messageParameter?.Clear();
		}

		public void AddParameter(string parameter)
		{
			if (m_valueProvider == null)
			{
				m_valueProvider = new FightInfoValueProvider(this);
			}
			if (m_messageParameter == null)
			{
				m_messageParameter = new List<string>();
			}
			m_messageParameter.Add(parameter);
		}

		public FightInfoValueProvider GetProvider()
		{
			if (m_valueProvider == null)
			{
				m_valueProvider = new FightInfoValueProvider(this);
			}
			return m_valueProvider;
		}

		public IReadOnlyList<string> GetParameter()
		{
			return m_messageParameter;
		}

		public FightInfoMessageRibbon()
			: this()
		{
		}
	}
}
