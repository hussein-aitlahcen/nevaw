using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.Components;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Zaap
{
	public class UIZaapPVPSelection : AbstractUI
	{
		public const int PVP_1V1_FIGHT_DEFINITION_ID = 1;

		public const int PVM_1V1_FIGHT_DEFINITION_ID = 2;

		public const int PVBOSS_FIGHT_DEFINITION_ID = 3;

		public const int PVP_2V2_FIGHT_DEFINITION_ID = 4;

		public Action<int, int?> onPlayRequested;

		[Header("Button")]
		[SerializeField]
		private AnimatedGraphicButton m_closeButton;

		[SerializeField]
		private Button m_oneVOneBtn;

		[SerializeField]
		private Button m_trainingBtn;

		public Action onCloseClick;

		protected unsafe override void Awake()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			base.Awake();
			m_closeButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_oneVOneBtn.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_trainingBtn.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnPlayButtonClicked(int fightDefinitionId)
		{
			if (PlayerData.instance.weaponInventory.TryGetLevel(PlayerData.instance.GetCurrentWeapon(), out int level))
			{
				onPlayRequested?.Invoke(fightDefinitionId, level);
			}
		}

		private void OnCloseClick()
		{
			onCloseClick?.Invoke();
		}

		public IEnumerator PlayEnterAnimation()
		{
			yield return PlayAnimation(m_animationDirector.GetAnimation("Open"));
		}

		public IEnumerator PlayTransitionToVersusAnimation()
		{
			yield return PlayAnimation(m_animationDirector.GetAnimation("Transition"));
		}

		public IEnumerator PlayCloseAnimation()
		{
			yield return PlayAnimation(m_animationDirector.GetAnimation("Close"));
		}
	}
}
