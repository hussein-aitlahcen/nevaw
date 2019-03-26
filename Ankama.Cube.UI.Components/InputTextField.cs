using Ankama.Cube.Data.UI.Localization.TextFormatting;
using JetBrains.Annotations;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	[ExecuteInEditMode]
	public sealed class InputTextField : UIBehaviour
	{
		[UsedImplicitly]
		[SerializeField]
		private bool m_interactable = true;

		[UsedImplicitly]
		[SerializeField]
		private RawTextField m_text;

		[UsedImplicitly]
		[SerializeField]
		private TextField m_placeholderText;

		[UsedImplicitly]
		[SerializeField]
		private RectTransform m_viewport;

		[UsedImplicitly]
		[SerializeField]
		private ContentType m_contentType;

		[UsedImplicitly]
		[SerializeField]
		private LineType m_lineType;

		[UsedImplicitly]
		[SerializeField]
		private int m_characterLimit;

		[UsedImplicitly]
		[SerializeField]
		private Color m_selectionColor = new Color(56f / 85f, 206f / 255f, 1f, 64f / 85f);

		[UsedImplicitly]
		[SerializeField]
		private OnChangeEvent m_onValueChanged = new OnChangeEvent();

		[UsedImplicitly]
		[SerializeField]
		private SubmitEvent m_onEndEdit = new SubmitEvent();

		[UsedImplicitly]
		[SerializeField]
		private SubmitEvent m_onSubmit = new SubmitEvent();

		[UsedImplicitly]
		[SerializeField]
		private SelectionEvent m_onSelect = new SelectionEvent();

		[UsedImplicitly]
		[SerializeField]
		private SelectionEvent m_onDeselect = new SelectionEvent();

		[UsedImplicitly]
		[SerializeField]
		private TextSelectionEvent m_onTextSelection = new TextSelectionEvent();

		[UsedImplicitly]
		[SerializeField]
		private TextSelectionEvent m_onEndTextSelection = new TextSelectionEvent();

		[NonSerialized]
		private TMP_InputField m_inputFieldComponent;

		public Selectable selectable => m_inputFieldComponent;

		[PublicAPI]
		public bool interactable
		{
			get
			{
				return m_interactable;
			}
			set
			{
				if (value != m_interactable)
				{
					m_interactable = value;
					if (null != m_inputFieldComponent)
					{
						m_inputFieldComponent.set_interactable(value);
					}
				}
			}
		}

		[PublicAPI]
		public ContentType contentType
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_contentType;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				if (value != m_contentType)
				{
					m_contentType = value;
					if (null != m_inputFieldComponent)
					{
						m_inputFieldComponent.set_contentType(value);
					}
				}
			}
		}

		[PublicAPI]
		public LineType lineType
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_lineType;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				if (value != m_lineType)
				{
					m_lineType = value;
					if (null != m_inputFieldComponent)
					{
						m_inputFieldComponent.set_lineType(value);
					}
				}
			}
		}

		[PublicAPI]
		public int characterLimit
		{
			get
			{
				return m_characterLimit;
			}
			set
			{
				if (value != m_characterLimit)
				{
					m_characterLimit = value;
					if (null != m_inputFieldComponent)
					{
						m_inputFieldComponent.set_characterLimit(value);
					}
				}
			}
		}

		[PublicAPI]
		public Color selectionColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_selectionColor;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				if (!(value == m_selectionColor))
				{
					m_selectionColor = value;
					if (null != m_inputFieldComponent)
					{
						m_inputFieldComponent.set_selectionColor(value);
					}
				}
			}
		}

		[PublicAPI]
		public OnChangeEvent onValueChanged
		{
			get
			{
				return m_onValueChanged;
			}
			set
			{
				m_onValueChanged = value;
				if (null != m_inputFieldComponent)
				{
					m_inputFieldComponent.set_onValueChanged(value);
				}
			}
		}

		[PublicAPI]
		public SubmitEvent onEndEdit
		{
			get
			{
				return m_onEndEdit;
			}
			set
			{
				m_onEndEdit = value;
				if (null != m_inputFieldComponent)
				{
					m_inputFieldComponent.set_onEndEdit(value);
				}
			}
		}

		[PublicAPI]
		public SubmitEvent onSubmit
		{
			get
			{
				return m_onSubmit;
			}
			set
			{
				m_onSubmit = value;
				if (null != m_inputFieldComponent)
				{
					m_inputFieldComponent.set_onSubmit(value);
				}
			}
		}

		[PublicAPI]
		public SelectionEvent onSelect
		{
			get
			{
				return m_onSelect;
			}
			set
			{
				m_onSelect = value;
				if (null != m_inputFieldComponent)
				{
					m_inputFieldComponent.set_onSelect(value);
				}
			}
		}

		[PublicAPI]
		public SelectionEvent onDeselect
		{
			get
			{
				return m_onDeselect;
			}
			set
			{
				m_onDeselect = value;
				if (null != m_inputFieldComponent)
				{
					m_inputFieldComponent.set_onDeselect(value);
				}
			}
		}

		[PublicAPI]
		public TextSelectionEvent onTextSelection
		{
			get
			{
				return m_onTextSelection;
			}
			set
			{
				m_onTextSelection = value;
				if (null != m_inputFieldComponent)
				{
					m_inputFieldComponent.set_onTextSelection(value);
				}
			}
		}

		[PublicAPI]
		public TextSelectionEvent onEndTextSelection
		{
			get
			{
				return m_onEndTextSelection;
			}
			set
			{
				m_onEndTextSelection = value;
				if (null != m_inputFieldComponent)
				{
					m_inputFieldComponent.set_onEndTextSelection(value);
				}
			}
		}

		[NotNull]
		[PublicAPI]
		public string GetText()
		{
			if (!(null != m_inputFieldComponent))
			{
				if (!(null != m_text))
				{
					return string.Empty;
				}
				return m_text.GetText();
			}
			return m_inputFieldComponent.get_text();
		}

		[PublicAPI]
		public void SetText([NotNull] string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (null != m_inputFieldComponent)
			{
				m_inputFieldComponent.set_text(value);
			}
			else if (null != m_text)
			{
				m_text.SetText(value);
			}
		}

		[PublicAPI]
		public string GetPlaceholderText()
		{
			if (!(null != m_placeholderText))
			{
				return string.Empty;
			}
			return m_placeholderText.GetText();
		}

		[PublicAPI]
		public void SetPlaceholderText(int textKeyId, IValueProvider valueProvider = null)
		{
			if (!(null == m_placeholderText))
			{
				m_placeholderText.SetText(textKeyId, valueProvider);
			}
		}

		[PublicAPI]
		public void SetPlaceholderText(string textKeyName, IValueProvider valueProvider = null)
		{
			if (!(null == m_placeholderText))
			{
				m_placeholderText.SetText(textKeyName, valueProvider);
			}
		}

		protected override void Start()
		{
			this.Start();
			if (null == m_inputFieldComponent)
			{
				CreateInputTextMeshProComponent();
			}
		}

		protected override void OnEnable()
		{
			this.OnEnable();
			if (!(null == m_inputFieldComponent))
			{
				m_inputFieldComponent.set_enabled(true);
			}
		}

		protected override void OnDisable()
		{
			if (null != m_inputFieldComponent)
			{
				m_inputFieldComponent.set_enabled(false);
			}
			this.OnDisable();
		}

		protected override void OnDestroy()
		{
			if (null != m_text)
			{
				m_text.TextComponentCreated -= OnTextComponentCreated;
			}
			if (null != m_placeholderText)
			{
				m_placeholderText.TextComponentCreated -= OnPlaceholderTextComponentCreated;
			}
			if (null != m_inputFieldComponent)
			{
				Object.Destroy(m_inputFieldComponent);
				m_inputFieldComponent = null;
			}
			this.OnDestroy();
		}

		private void CreateInputTextMeshProComponent()
		{
			TMP_InputField val = this.GetComponent<TMP_InputField>();
			if (null == val)
			{
				val = this.get_gameObject().AddComponent<TMP_InputField>();
			}
			m_inputFieldComponent = val;
			val.set_enabled(false);
			val.set_interactable(m_interactable);
			val.set_textViewport(m_viewport);
			val.set_transition(0);
			if (null != m_text)
			{
				TMP_Text textComponent = m_text.GetTextComponent();
				if (null == textComponent)
				{
					m_text.TextComponentCreated += OnTextComponentCreated;
					val.set_enabled(false);
				}
				else
				{
					OnTextComponentCreated(textComponent);
				}
			}
			if (null != m_placeholderText)
			{
				TMP_Text textComponent2 = m_placeholderText.GetTextComponent();
				if (null == textComponent2)
				{
					m_placeholderText.TextComponentCreated += OnPlaceholderTextComponentCreated;
				}
				else
				{
					OnPlaceholderTextComponentCreated(textComponent2);
				}
			}
			val.set_onValueChanged(m_onValueChanged);
			val.set_onEndEdit(m_onEndEdit);
			val.set_onSubmit(m_onSubmit);
			val.set_onSelect(m_onSelect);
			val.set_onDeselect(m_onDeselect);
			val.set_onTextSelection(m_onTextSelection);
			val.set_onEndTextSelection(m_onEndTextSelection);
		}

		private void OnTextComponentCreated(TMP_Text textComponent)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			TMP_InputField inputFieldComponent = m_inputFieldComponent;
			if (!(null == inputFieldComponent))
			{
				inputFieldComponent.set_textComponent(textComponent);
				inputFieldComponent.set_text((null != m_text) ? m_text.GetText() : string.Empty);
				inputFieldComponent.set_contentType(m_contentType);
				inputFieldComponent.set_lineType(m_lineType);
				inputFieldComponent.set_characterLimit(m_characterLimit);
				inputFieldComponent.set_selectionColor(m_selectionColor);
				inputFieldComponent.set_richText(false);
				inputFieldComponent.set_isRichTextEditingAllowed(false);
				inputFieldComponent.set_enabled(null != textComponent);
			}
		}

		private void OnPlaceholderTextComponentCreated(TMP_Text textComponent)
		{
			TMP_InputField inputFieldComponent = m_inputFieldComponent;
			if (!(null == inputFieldComponent))
			{
				inputFieldComponent.set_placeholder(textComponent);
				textComponent.set_enabled(string.IsNullOrEmpty(inputFieldComponent.get_text()));
			}
		}

		public InputTextField()
			: this()
		{
		}//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown

	}
}
