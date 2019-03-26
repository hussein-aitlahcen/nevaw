using Ankama.AssetManagement.StateManagement;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasGroup))]
	public class AbstractUI : MonoBehaviour
	{
		[Header("Animation")]
		[SerializeField]
		protected UIAnimationDirector m_animationDirector;

		private const float FullBlurAmount = 0.75f;

		[SerializeField]
		[HideInInspector]
		private Canvas m_canvas;

		[SerializeField]
		[HideInInspector]
		protected CanvasGroup m_canvasGroup;

		[SerializeField]
		[HideInInspector]
		protected bool m_useBlur;

		[SerializeField]
		[HideInInspector]
		[Range(0f, 1f)]
		private float m_blurAmount = 1f;

		private float m_lastFrameBlurAmount;

		public Action onBlurFactorIsFull;

		public Action onBlurFactorStartDecrease;

		public Canvas canvas => m_canvas;

		public CanvasGroup canvasGroup => m_canvasGroup;

		public float blurAmount
		{
			get
			{
				return m_blurAmount;
			}
			set
			{
				m_blurAmount = value;
			}
		}

		public bool useBlur
		{
			get
			{
				return m_useBlur;
			}
			set
			{
				if (m_useBlur != value)
				{
					m_useBlur = value;
					UIManager instance = UIManager.instance;
					if (instance != null)
					{
						instance.UseBlurChanged(this, value);
					}
				}
			}
		}

		public bool interactable
		{
			get
			{
				return canvasGroup.get_interactable();
			}
			set
			{
				canvasGroup.set_interactable(value);
			}
		}

		public void SetDepth(StateContext state, int index = -1)
		{
			UIManager instance = UIManager.instance;
			if (instance != null)
			{
				instance.NotifyUIDepthChanged(this, state, index);
			}
		}

		protected virtual void Awake()
		{
			Device.ScreenStateChanged += RescaleCanvas;
			RescaleCanvas();
		}

		private void RescaleCanvas()
		{
			CanvasScaler component = this.GetComponent<CanvasScaler>();
			if (!(component == null))
			{
				if ((float)Screen.get_width() / (float)Screen.get_height() > 1.77777779f)
				{
					component.set_matchWidthOrHeight(1f);
				}
				else
				{
					component.set_matchWidthOrHeight(0f);
				}
			}
		}

		public IEnumerator LoadAssets()
		{
			if (m_animationDirector != null)
			{
				yield return m_animationDirector.Load();
			}
		}

		public void UnloadAsset()
		{
			if (m_animationDirector != null)
			{
				m_animationDirector.Unload();
			}
		}

		protected virtual void OnEnable()
		{
			m_lastFrameBlurAmount = m_blurAmount;
			UIManager instance = UIManager.instance;
			if (instance != null)
			{
				instance.EnableStateChanged(this, enable: true);
			}
		}

		protected virtual void OnDisable()
		{
			UIManager instance = UIManager.instance;
			if (instance != null)
			{
				instance.EnableStateChanged(this, enable: false);
			}
		}

		protected virtual void OnDestroy()
		{
			Device.ScreenStateChanged -= RescaleCanvas;
			UIManager instance = UIManager.instance;
			if (instance != null)
			{
				instance.NotifyUIDestroyed(this);
			}
		}

		protected virtual void Update()
		{
			if (m_useBlur)
			{
				UpdateBlurAmount();
			}
		}

		private void UpdateBlurAmount()
		{
			if (m_blurAmount > 0.75f && m_lastFrameBlurAmount <= 0.75f)
			{
				onBlurFactorIsFull?.Invoke();
			}
			else if (m_blurAmount < 0.75f && m_lastFrameBlurAmount >= 0.75f)
			{
				onBlurFactorStartDecrease?.Invoke();
			}
			m_lastFrameBlurAmount = m_blurAmount;
		}

		protected IEnumerator PlayAnimation(TimelineAsset animation)
		{
			PlayableDirector director = m_animationDirector.GetDirector();
			director.Play(animation);
			PlayableGraph graph = director.get_playableGraph();
			while (graph.IsValid() && !graph.IsDone())
			{
				yield return null;
			}
		}

		public AbstractUI()
			: this()
		{
		}
	}
}
