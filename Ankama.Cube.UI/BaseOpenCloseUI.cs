using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Ankama.Cube.UI
{
	public class BaseOpenCloseUI : AbstractUI
	{
		[Header("OpenCloseUI")]
		[SerializeField]
		private PlayableDirector m_openDirector;

		[SerializeField]
		private PlayableDirector m_closeDirector;

		[Header("Sounds")]
		[SerializeField]
		private UnityEvent m_openSound;

		protected Coroutine m_playCoroutine;

		protected PlayableDirector m_currentPlayingDirector;

		public bool isPlaying => m_currentPlayingDirector != null;

		public void Open(Action completeCallback = null)
		{
			CancelCurrentAnim();
			m_playCoroutine = this.StartCoroutine(PlayDirectorCoroutine(m_openDirector, completeCallback));
			m_openSound.Invoke();
		}

		public void Close(Action completeCallback = null)
		{
			CancelCurrentAnim();
			m_playCoroutine = this.StartCoroutine(PlayDirectorCoroutine(m_closeDirector, completeCallback));
		}

		private void CancelCurrentAnim()
		{
			if (m_playCoroutine != null)
			{
				this.StopCoroutine(m_playCoroutine);
				m_playCoroutine = null;
			}
			if (m_currentPlayingDirector != null)
			{
				m_currentPlayingDirector.Stop();
				m_currentPlayingDirector = null;
			}
		}

		public virtual IEnumerator OpenCoroutine()
		{
			CancelCurrentAnim();
			m_openSound.Invoke();
			yield return PlayDirectorCoroutine(m_openDirector);
		}

		public IEnumerator CloseCoroutine()
		{
			CancelCurrentAnim();
			yield return PlayDirectorCoroutine(m_closeDirector);
		}

		private IEnumerator PlayDirectorCoroutine(PlayableDirector director, Action completeCallback = null)
		{
			if (null == director)
			{
				yield break;
			}
			m_currentPlayingDirector = director;
			director.set_time(0.0);
			director.Play();
			PlayableGraph playableGraph = director.get_playableGraph();
			while (playableGraph.IsValid() && !playableGraph.IsDone())
			{
				yield return null;
				if (null == director)
				{
					yield break;
				}
				playableGraph = director.get_playableGraph();
			}
			completeCallback?.Invoke();
			m_playCoroutine = null;
			m_currentPlayingDirector = null;
		}

		public static IEnumerator PlayDirector(PlayableDirector director)
		{
			director.set_time(0.0);
			director.Play();
			while (true)
			{
				PlayableGraph playableGraph = director.get_playableGraph();
				if (playableGraph.IsValid())
				{
					playableGraph = director.get_playableGraph();
					if (!playableGraph.IsDone())
					{
						yield return null;
						continue;
					}
					break;
				}
				break;
			}
		}
	}
}
