using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class BugReportUI : AbstractUI
	{
		[SerializeField]
		private Image m_thumbnailImage;

		[SerializeField]
		private InputField m_summaryInput;

		[SerializeField]
		private InputField m_descriptionInput;

		[SerializeField]
		private GameObject m_progress;

		[SerializeField]
		private Text m_progressText;

		[SerializeField]
		private GameObject m_error;

		[SerializeField]
		private Button m_submitButton;

		[SerializeField]
		private Button m_closeButton;

		public Action<string, string> onSubmitClick;

		public Action onCloseClick;

		private bool m_submitting;

		protected unsafe override void Awake()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			base.Awake();
			ResetForm();
			m_submitButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_closeButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		protected unsafe override void OnDestroy()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			m_submitButton.get_onClick().RemoveListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_closeButton.get_onClick().RemoveListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			base.OnDestroy();
		}

		protected override void Update()
		{
			base.Update();
			if (!m_submitting)
			{
				m_submitButton.set_interactable(!string.IsNullOrWhiteSpace(m_summaryInput.get_text()) && !string.IsNullOrWhiteSpace(m_descriptionInput.get_text()));
			}
		}

		public void SetThumbnail(Sprite sprite)
		{
			if (null != m_thumbnailImage)
			{
				m_thumbnailImage.set_sprite(sprite);
				m_thumbnailImage.set_preserveAspect(true);
			}
		}

		public void SetProgress(float progress, int phaseIndex)
		{
			m_progressText.set_text($"Envoi du rapport en cours : {progress:P} ({phaseIndex} / 2)...");
			m_progress.get_gameObject().SetActive(true);
		}

		public void SetError()
		{
			m_error.get_gameObject().SetActive(true);
		}

		public void ResetForm()
		{
			m_submitting = false;
			m_summaryInput.set_interactable(true);
			m_descriptionInput.set_interactable(true);
			m_progress.get_gameObject().SetActive(false);
			m_error.get_gameObject().SetActive(false);
		}

		private void OnSubmitClick()
		{
			m_submitting = true;
			m_submitButton.set_interactable(false);
			m_summaryInput.set_interactable(false);
			m_descriptionInput.set_interactable(false);
			m_error.get_gameObject().SetActive(false);
			onSubmitClick?.Invoke(m_summaryInput.get_text(), m_descriptionInput.get_text());
		}

		private void OnCloseClick()
		{
			onCloseClick?.Invoke();
		}
	}
}
