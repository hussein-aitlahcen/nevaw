using Ankama.Cube.Data.UI;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ankama.Cube.UI.Components
{
	public abstract class AbstractTextField : UIBehaviour
	{
		[Flags]
		public enum FontStyle
		{
			[UsedImplicitly]
			Normal = 0x0,
			[UsedImplicitly]
			Bold = 0x1,
			[UsedImplicitly]
			Italic = 0x2,
			[UsedImplicitly]
			Underline = 0x4,
			[UsedImplicitly]
			Strikethrough = 0x40
		}

		public enum TextStyle
		{
			[UsedImplicitly]
			Normal = 0,
			[UsedImplicitly]
			LowerCase = 8,
			[UsedImplicitly]
			UpperCase = 0x10,
			[UsedImplicitly]
			SmallCaps = 0x20
		}

		public enum OverflowMode
		{
			[UsedImplicitly]
			Overflow,
			[UsedImplicitly]
			Ellipsis,
			[UsedImplicitly]
			Masking,
			[UsedImplicitly]
			Truncate
		}

		public delegate void TextComponentCreatedEventHandler(TMP_Text textComponent);

		[UsedImplicitly]
		[SerializeField]
		private string m_fontCollectionIdentifier = string.Empty;

		[UsedImplicitly]
		[SerializeField]
		private FontStyle m_fontStyle;

		[UsedImplicitly]
		[SerializeField]
		private TextStyle m_textStyle;

		[UsedImplicitly]
		[SerializeField]
		private Color m_color = Color.get_white();

		[UsedImplicitly]
		[SerializeField]
		private TextAlignmentOptions m_textAlignment = 257;

		[UsedImplicitly]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_textAlignmentWrapMix = 0.4f;

		[UsedImplicitly]
		[SerializeField]
		private bool m_enableWordWrapping = true;

		[UsedImplicitly]
		[SerializeField]
		private OverflowMode m_overflowMode;

		[UsedImplicitly]
		[SerializeField]
		private bool m_richText;

		private bool m_textDirty = true;

		[NonSerialized]
		private FontCollection m_fontCollection;

		[NonSerialized]
		private TextMeshProUGUICustom m_textMeshProComponent;

		[PublicAPI]
		public FontCollection fontCollection
		{
			get
			{
				return m_fontCollection;
			}
			set
			{
				if (value == m_fontCollection)
				{
					return;
				}
				if (this.IsActive())
				{
					if (null != m_fontCollection)
					{
						m_fontCollection.UnregisterTextField(this);
					}
					if (null != value)
					{
						value.RegisterTextField(this);
					}
				}
				m_fontCollection = value;
				if (null != m_textMeshProComponent)
				{
					ApplyFontCollection();
				}
			}
		}

		[PublicAPI]
		public FontStyle fontStyle
		{
			get
			{
				return m_fontStyle;
			}
			set
			{
				if (value != m_fontStyle)
				{
					m_fontStyle = value;
					if (null != m_textMeshProComponent)
					{
						m_textMeshProComponent.set_fontStyle((int)m_fontStyle | (int)m_textStyle);
					}
				}
			}
		}

		[PublicAPI]
		public TextStyle textStyle
		{
			get
			{
				return m_textStyle;
			}
			set
			{
				if (value != m_textStyle)
				{
					m_textStyle = value;
					if (null != m_textMeshProComponent)
					{
						m_textMeshProComponent.set_fontStyle((int)m_fontStyle | (int)m_textStyle);
					}
				}
			}
		}

		[PublicAPI]
		public Color color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_color;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				if (!(value == m_color))
				{
					m_color = value;
					if (null != m_textMeshProComponent)
					{
						m_textMeshProComponent.set_color(value);
					}
				}
			}
		}

		[PublicAPI]
		public Material material
		{
			get
			{
				if (null != m_textMeshProComponent)
				{
					return m_textMeshProComponent.get_fontMaterial();
				}
				return null;
			}
		}

		[PublicAPI]
		public TextAlignmentOptions alignment
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_textAlignment;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				if (value != m_textAlignment)
				{
					m_textAlignment = value;
					if (null != m_textMeshProComponent)
					{
						m_textMeshProComponent.set_alignment(value);
					}
				}
			}
		}

		[PublicAPI]
		public float alignmentWrapMix
		{
			get
			{
				return m_textAlignmentWrapMix;
			}
			set
			{
				value = Mathf.Clamp01(value);
				m_textAlignmentWrapMix = value;
				if (null != m_textMeshProComponent)
				{
					m_textMeshProComponent.set_wordWrappingRatios(value);
				}
			}
		}

		[PublicAPI]
		public bool enableWordWrapping
		{
			get
			{
				return m_enableWordWrapping;
			}
			set
			{
				if (value != m_enableWordWrapping)
				{
					m_enableWordWrapping = value;
					if (null != m_textMeshProComponent)
					{
						m_textMeshProComponent.set_enableWordWrapping(value);
					}
				}
			}
		}

		[PublicAPI]
		public OverflowMode overflowMode
		{
			get
			{
				return m_overflowMode;
			}
			set
			{
				if (value != m_overflowMode)
				{
					m_overflowMode = value;
					if (null != m_textMeshProComponent)
					{
						m_textMeshProComponent.set_overflowMode(m_overflowMode);
					}
				}
			}
		}

		[PublicAPI]
		public bool richText
		{
			get
			{
				return m_richText;
			}
			set
			{
				if (value != m_richText)
				{
					m_richText = value;
					if (null != m_textMeshProComponent)
					{
						m_textMeshProComponent.set_richText(value);
					}
				}
			}
		}

		[PublicAPI]
		public event TextComponentCreatedEventHandler TextComponentCreated;

		[PublicAPI]
		[NotNull]
		public string GetText()
		{
			if (m_textDirty || !(null != m_textMeshProComponent))
			{
				return GetFormattedText();
			}
			return m_textMeshProComponent.get_text();
		}

		[CanBeNull]
		public TMP_Text GetTextComponent()
		{
			return m_textMeshProComponent;
		}

		protected abstract string GetFormattedText();

		protected override void Start()
		{
			this.Start();
			if (null == m_fontCollection)
			{
				GetFontCollection();
			}
			if (null == m_textMeshProComponent)
			{
				CreateTextMeshProComponent();
			}
			if (null != m_fontCollection)
			{
				m_fontCollection.RegisterTextField(this);
			}
		}

		protected override void OnEnable()
		{
			this.OnEnable();
			if (!(null == m_textMeshProComponent))
			{
				m_textMeshProComponent.set_enabled(true);
				if (null != m_fontCollection)
				{
					m_fontCollection.RegisterTextField(this);
				}
				if (m_textDirty)
				{
					RefreshText();
				}
			}
		}

		protected override void OnDisable()
		{
			if (null != m_fontCollection)
			{
				m_fontCollection.UnregisterTextField(this);
			}
			if (null != m_textMeshProComponent)
			{
				m_textMeshProComponent.set_enabled(false);
			}
			this.OnDisable();
		}

		protected override void OnDestroy()
		{
			if (null != m_textMeshProComponent)
			{
				Object.Destroy(m_textMeshProComponent);
				m_textMeshProComponent = null;
			}
			if (m_fontCollection != null)
			{
				m_fontCollection.Unload();
			}
			this.OnDestroy();
		}

		private void GetFontCollection()
		{
			if (m_fontCollectionIdentifier.Length == 0)
			{
				m_fontCollection = null;
			}
			else if (!RuntimeData.TryGetFontCollection(m_fontCollectionIdentifier, out m_fontCollection))
			{
				Log.Warning("Could not find font collection for TextField named '" + this.get_name() + "'.", 483, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\AbstractTextField.cs");
			}
			else
			{
				m_fontCollection.Load();
			}
		}

		private void CreateTextMeshProComponent()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			TextMeshProUGUICustom textMeshProUGUICustom = this.GetComponent<TextMeshProUGUICustom>();
			if (null == textMeshProUGUICustom)
			{
				textMeshProUGUICustom = this.get_gameObject().AddComponent<TextMeshProUGUICustom>();
			}
			else
			{
				textMeshProUGUICustom.set_enabled(true);
			}
			m_textMeshProComponent = textMeshProUGUICustom;
			ApplyFontCollection();
			textMeshProUGUICustom.set_fontStyle((int)m_fontStyle | (int)m_textStyle);
			textMeshProUGUICustom.set_alignment(m_textAlignment);
			textMeshProUGUICustom.set_color(m_color);
			textMeshProUGUICustom.set_wordWrappingRatios(m_textAlignmentWrapMix);
			textMeshProUGUICustom.set_enableWordWrapping(m_enableWordWrapping);
			textMeshProUGUICustom.set_overflowMode(m_overflowMode);
			textMeshProUGUICustom.set_richText(m_richText);
			textMeshProUGUICustom.set_enableKerning(true);
			textMeshProUGUICustom.set_extraPadding(true);
			textMeshProUGUICustom.set_raycastTarget(false);
			if (m_textDirty)
			{
				textMeshProUGUICustom.set_text(GetFormattedText());
				m_textDirty = false;
			}
			this.TextComponentCreated?.Invoke(textMeshProUGUICustom);
		}

		private void ApplyFontCollection()
		{
			TextMeshProUGUICustom textMeshProComponent = m_textMeshProComponent;
			if (null == m_fontCollection)
			{
				textMeshProComponent.set_font(TMP_FontAsset.get_defaultFontAsset());
				return;
			}
			textMeshProComponent.set_font(m_fontCollection.fontAsset);
			Material styleMaterial = m_fontCollection.styleMaterial;
			if (null != styleMaterial)
			{
				textMeshProComponent.set_fontSharedMaterial(m_fontCollection.styleMaterial);
			}
			else
			{
				textMeshProComponent.set_fontSharedMaterial(textMeshProComponent.get_font().material);
			}
			FontData fontData = m_fontCollection.fontData;
			if (fontData != null)
			{
				textMeshProComponent.set_fontSize(fontData.fontSize);
				textMeshProComponent.set_characterSpacing(fontData.characterSpacing);
				textMeshProComponent.set_wordSpacing(fontData.wordSpacing);
				textMeshProComponent.set_lineSpacing(fontData.lineSpacing);
				textMeshProComponent.set_paragraphSpacing(fontData.paragraphSpacing);
			}
		}

		public void RefreshText()
		{
			if (null == m_textMeshProComponent)
			{
				m_textDirty = true;
				return;
			}
			m_textMeshProComponent.set_text(GetFormattedText());
			m_textDirty = false;
		}

		protected AbstractTextField()
			: this()
		{
		}//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)

	}
}
