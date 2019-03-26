using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Ankama.Cube.Demo.UI
{
	public class GameSelectionUIDemo : BaseFightSelectionUI
	{
		[SerializeField]
		private GameSelectionButton[] m_buttons;

		[SerializeField]
		private SlidingAnimUI m_slidingAnim;

		private GameSelectionButton m_hightlightedButton;

		public Action<int> onSelect;

		private unsafe void Start()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			for (int i = 0; i < m_buttons.Length; i++)
			{
				int index = i;
				GameSelectionButton obj = m_buttons[index];
				_003C_003Ec__DisplayClass4_0 _003C_003Ec__DisplayClass4_;
				obj.get_onClick().AddListener(new UnityAction((object)_003C_003Ec__DisplayClass4_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				obj.onPointerEnter = (Action<GameSelectionButton>)Delegate.Combine(obj.onPointerEnter, new Action<GameSelectionButton>(OnPointerEnter));
				obj.onPointerExit = (Action<GameSelectionButton>)Delegate.Combine(obj.onPointerExit, new Action<GameSelectionButton>(OnPointerExit));
			}
		}

		private void OnPointerEnter(GameSelectionButton button)
		{
			m_hightlightedButton = button;
			for (int i = 0; i < m_buttons.Length; i++)
			{
				GameSelectionButton obj = m_buttons[i];
				obj.anotherButtonIsHightlighted = (obj != m_hightlightedButton);
			}
		}

		private void OnPointerExit(GameSelectionButton button)
		{
			if (!(m_hightlightedButton != button))
			{
				for (int i = 0; i < m_buttons.Length; i++)
				{
					m_buttons[i].anotherButtonIsHightlighted = false;
				}
				m_hightlightedButton = null;
			}
		}

		private void OnButtonClick(int index)
		{
			onSelect?.Invoke(index);
		}

		public override IEnumerator OpenFrom(SlidingSide side)
		{
			Sequence elemSequence = m_slidingAnim.PlayAnim(open: true, side, side == SlidingSide.Left);
			m_openDirector.set_time(0.0);
			m_openDirector.Play();
			while (true)
			{
				if (!TweenExtensions.IsActive(elemSequence))
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
			Sequence elemSequence = m_slidingAnim.PlayAnim(open: false, side, side == SlidingSide.Right);
			m_closeDirector.set_time(0.0);
			m_closeDirector.Play();
			while (true)
			{
				if (!TweenExtensions.IsActive(elemSequence))
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
	}
}
