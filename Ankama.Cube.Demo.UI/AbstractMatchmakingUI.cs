using Ankama.Cube.Data;
using Ankama.Cube.UI;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Ankama.Cube.Demo.UI
{
	public abstract class AbstractMatchmakingUI : BaseFightSelectionUI
	{
		public const int DefaultPlayerLevel = 6;

		[SerializeField]
		protected PlayerInvitationUI m_playerInvitationPanel;

		[SerializeField]
		protected PlayableDirector m_matchmakingToVersus;

		[SerializeField]
		protected PlayableDirector m_versusToFight;

		[SerializeField]
		protected SlidingAnimUI m_slidingAnim;

		[SerializeField]
		protected DemoData m_fakeData;

		public Action onLaunchMatchmakingClick;

		public Action onCancelMatchmakingClick;

		public PlayerInvitationUI playerInvitation => m_playerInvitationPanel;

		public abstract void Init();

		public virtual IEnumerator GotoVersusAnim()
		{
			yield return BaseOpenCloseUI.PlayDirector(m_matchmakingToVersus);
		}

		public virtual IEnumerator GotoFightAnim()
		{
			yield return BaseOpenCloseUI.PlayDirector(m_versusToFight);
		}

		public override IEnumerator OpenFrom(SlidingSide side)
		{
			for (int i = 0; i < m_slidingAnim.elements.Count; i++)
			{
				m_slidingAnim.elements[i].get_transform().set_localPosition(Vector3.get_zero());
			}
			Sequence slidingSequence = m_slidingAnim.PlayAnim(open: true, side, side == SlidingSide.Left);
			m_openDirector.set_time(0.0);
			m_openDirector.Play();
			while (true)
			{
				if (!TweenExtensions.IsActive(slidingSequence))
				{
					PlayableGraph playableGraph = m_openDirector.get_playableGraph();
					if (!playableGraph.IsValid())
					{
						break;
					}
					playableGraph = m_openDirector.get_playableGraph();
					if (playableGraph.IsDone())
					{
						break;
					}
				}
				yield return null;
			}
		}

		public override IEnumerator CloseTo(SlidingSide side)
		{
			Sequence slidingSequence = m_slidingAnim.PlayAnim(open: false, side, side == SlidingSide.Right);
			m_closeDirector.set_time(0.0);
			m_closeDirector.Play();
			while (true)
			{
				if (!TweenExtensions.IsActive(slidingSequence))
				{
					PlayableGraph playableGraph = m_closeDirector.get_playableGraph();
					if (!playableGraph.IsValid())
					{
						break;
					}
					playableGraph = m_closeDirector.get_playableGraph();
					if (playableGraph.IsDone())
					{
						break;
					}
				}
				yield return null;
			}
		}

		protected Tuple<SquadDefinition, SquadFakeData> GetSquadDataByDeckId(int deckId)
		{
			for (int i = 0; i < m_fakeData.squads.Length; i++)
			{
				SquadFakeData squadFakeData = m_fakeData.squads[i];
				if (squadFakeData.id == deckId)
				{
					return new Tuple<SquadDefinition, SquadFakeData>(RuntimeData.squadDefinitions[deckId], squadFakeData);
				}
			}
			SquadFakeData squadFakeData2 = m_fakeData.squads[0];
			return new Tuple<SquadDefinition, SquadFakeData>(RuntimeData.squadDefinitions[squadFakeData2.id], squadFakeData2);
		}

		protected Tuple<SquadDefinition, SquadFakeData> GetSquadDataByWeaponId(int weaponId)
		{
			for (int i = 0; i < m_fakeData.squads.Length; i++)
			{
				SquadFakeData squadFakeData = m_fakeData.squads[i];
				SquadDefinition squadDefinition = RuntimeData.squadDefinitions[squadFakeData.id];
				if (squadDefinition.weapon.value == weaponId)
				{
					return new Tuple<SquadDefinition, SquadFakeData>(squadDefinition, squadFakeData);
				}
			}
			SquadFakeData squadFakeData2 = m_fakeData.squads[0];
			return new Tuple<SquadDefinition, SquadFakeData>(RuntimeData.squadDefinitions[squadFakeData2.id], squadFakeData2);
		}
	}
}
