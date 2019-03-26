using Ankama.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class GameModeButton : Button
	{
		[SerializeField]
		private Transform m_scaleDummy;

		[SerializeField]
		private Image m_image;

		[SerializeField]
		private GameModeButtonStyle m_style;

		private Sequence m_tweenSequence;

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected I4, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			if (!this.get_gameObject().get_activeInHierarchy())
			{
				return;
			}
			if (m_style == null)
			{
				Log.Error("AnimatedTextButton " + this.get_name() + " doesn't have a style defined !", 27, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\ZaapGameMode\\GameModeButton.cs");
				return;
			}
			GameModeButtonState gameModeButtonState = m_style.disable;
			switch ((int)state)
			{
			case 0:
				gameModeButtonState = m_style.normal;
				break;
			case 1:
				gameModeButtonState = m_style.highlight;
				break;
			case 2:
				gameModeButtonState = m_style.pressed;
				break;
			case 3:
				gameModeButtonState = m_style.disable;
				break;
			}
			Sequence tweenSequence = m_tweenSequence;
			if (tweenSequence != null)
			{
				TweenExtensions.Kill(tweenSequence, false);
			}
			if (instant)
			{
				m_scaleDummy.set_localScale(Vector3.get_one() * gameModeButtonState.scale);
				m_image.set_color(gameModeButtonState.imageColor);
			}
			else
			{
				m_tweenSequence = DOTween.Sequence();
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(m_scaleDummy, Vector3.get_one() * gameModeButtonState.scale, m_style.transitionDuration), m_style.ease));
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOColor(m_image, gameModeButtonState.imageColor, m_style.transitionDuration), m_style.ease));
			}
		}

		public GameModeButton()
			: this()
		{
		}
	}
}
