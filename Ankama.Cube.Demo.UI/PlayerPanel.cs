using Ankama.Cube.UI.Components;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class PlayerPanel : MonoBehaviour
	{
		public enum State
		{
			Empy,
			Player
		}

		[SerializeField]
		protected CanvasGroup m_canvasGroup;

		[SerializeField]
		protected Image m_illu;

		[SerializeField]
		private RawTextField m_name;

		[SerializeField]
		private RawTextField m_level;

		[SerializeField]
		private CanvasGroup[] m_texts;

		[SerializeField]
		private float m_openTransitionDuration = 1f;

		[SerializeField]
		private float m_closeTransitionDuration = 0.8f;

		private Tween m_tween;

		private float stateValue
		{
			get;
			set;
		}

		public long playerId
		{
			get;
			private set;
		}

		public bool isEmpty => playerId < 0;

		public void Set(FightPlayerInfo player, int level, SquadFakeData fakeData, bool tween = false)
		{
			playerId = player.Uid;
			Set(player.Info.Nickname, level, fakeData, tween);
		}

		public unsafe void Set(string nickname, int level, SquadFakeData fakeData, bool tween = false)
		{
			level = 6;
			m_illu.set_sprite(fakeData.illu);
			m_name.SetText(nickname);
			m_level.SetText($"Niveau {level}");
			if (m_tween != null && TweenExtensions.IsActive(m_tween))
			{
				TweenExtensions.Kill(m_tween, false);
			}
			if (tween)
			{
				m_tween = DOVirtual.Float(stateValue, 1f, m_openTransitionDuration, new TweenCallback<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			else
			{
				UpdateState(1f);
			}
		}

		public unsafe void SetEmpty(bool tween = false)
		{
			playerId = -1L;
			if (m_tween != null && TweenExtensions.IsActive(m_tween))
			{
				TweenExtensions.Kill(m_tween, false);
			}
			if (tween)
			{
				m_tween = DOVirtual.Float(stateValue, 0f, m_closeTransitionDuration, new TweenCallback<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			else
			{
				UpdateState(0f);
			}
		}

		private void UpdateState(float value)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			stateValue = value;
			for (int i = 0; i < m_texts.Length; i++)
			{
				CanvasGroup obj = m_texts[i];
				obj.set_alpha(value);
				obj.get_gameObject().SetActive(value > 0.0001f);
			}
			m_illu.set_color(new Color(value, 1f, 1f, 1f));
		}

		public PlayerPanel()
			: this()
		{
		}
	}
}
