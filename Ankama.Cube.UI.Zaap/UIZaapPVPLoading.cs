using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.Components;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Ankama.Cube.UI.Zaap
{
	public class UIZaapPVPLoading : AbstractUI
	{
		public Action onForceAiRequested;

		public Action onCancelRequested;

		public Action<int, int?> onEnterAnimationFinished;

		[Header("Canvas")]
		[SerializeField]
		private CanvasGroup m_searchOpponentGroup;

		[Header("Button")]
		[SerializeField]
		private AnimatedTextButton m_aiButton;

		[SerializeField]
		private AnimatedTextButton m_cancelButton;

		[Header("Preview")]
		[SerializeField]
		private UIZaapPlayerPreview m_playerPreview;

		[SerializeField]
		private UIZaapPlayerPreview m_opponentPreview;

		[SerializeField]
		private RectTransform m_playerRoot;

		[SerializeField]
		private RectTransform m_opponentRoot;

		private int m_fightDefinitionId;

		public unsafe void Init(int fightDefinitionId)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			m_fightDefinitionId = fightDefinitionId;
			m_searchOpponentGroup.set_alpha(0f);
			m_playerRoot.set_anchoredPosition(Vector2.op_Implicit(new Vector3(-2000f, 0f, 0f)));
			m_opponentRoot.set_anchoredPosition(Vector2.op_Implicit(new Vector3(2000f, 0f, 0f)));
			m_aiButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_cancelButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public void OnEnterFinished()
		{
			onEnterAnimationFinished(m_fightDefinitionId, PlayerData.instance.GetCurrentWeaponLevel());
		}

		private void OnCancel()
		{
			onCancelRequested?.Invoke();
		}

		private void OnAiClic()
		{
			onForceAiRequested?.Invoke();
		}

		public IEnumerator LoadUI()
		{
			yield return PlayAnimation(m_animationDirector.GetAnimation("Init"));
			yield return m_playerPreview.SetPlayerData(PlayerData.instance);
			yield return PlayAnimation(m_animationDirector.GetAnimation("Open"));
		}

		public IEnumerator CloseUI()
		{
			yield return PlayAnimation(m_animationDirector.GetAnimation("Close"));
		}

		public IEnumerator SetOpponent(FightInfo.Types.Player opponent)
		{
			yield return m_opponentPreview.SetPlayerData(opponent);
		}

		public IEnumerator GotoVersusAnim()
		{
			yield return PlayAnimation(m_animationDirector.GetAnimation("OpponentFound"));
		}
	}
}
