using Ankama.Cube.Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	[ExecuteInEditMode]
	public class CharacterUIContainer : MonoBehaviour
	{
		private enum TweenState
		{
			None,
			Showing,
			Hiding
		}

		private const float MapCellIndicatorTweenDuration = 0.2f;

		private const float PixelPerUnit = 100f;

		[SerializeField]
		private CharacterUIResources m_resources;

		[SerializeField]
		private Transform m_adjustableHeightContainer;

		[Header("Map Cell Indicators")]
		[SerializeField]
		private SpriteRenderer m_mapCellIndicatorRenderer;

		[Header("Layout")]
		[SerializeField]
		private CharacterUILayoutElement[] m_layoutElements = new CharacterUILayoutElement[0];

		[SerializeField]
		private int m_layoutSpacing = 1;

		private Color m_color;

		private int m_sortingOrder;

		private bool m_layoutIsDirty;

		private CharacterHeight m_height = CharacterHeight.Normal;

		private MapCellIndicator m_mapCellIndicator;

		private TweenState m_tweenState;

		private Tweener m_mapCellIndicatorTween;

		private float m_mapCellIndicatorAlpha;

		public Color color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_color;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				m_color = value;
				Color color = value;
				color.a *= m_mapCellIndicatorAlpha;
				m_mapCellIndicatorRenderer.set_color(color);
			}
		}

		public int sortingOrder
		{
			get
			{
				return m_sortingOrder;
			}
			set
			{
				m_sortingOrder = value;
				m_mapCellIndicatorRenderer.set_sortingOrder(sortingOrder);
			}
		}

		public void Setup()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			Color color = m_color;
			color.a = 0f;
			m_mapCellIndicatorAlpha = 0f;
			m_mapCellIndicatorRenderer.set_color(color);
			m_mapCellIndicatorRenderer.set_enabled(false);
			m_mapCellIndicatorRenderer.set_sprite(null);
		}

		public unsafe void SetCharacterHeight(CharacterHeight value)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			if (m_height != value)
			{
				float height = value.GetHeight();
				Vector3 localPosition = m_adjustableHeightContainer.get_localPosition();
				localPosition.y = height / ((IntPtr)(void*)this.get_transform().get_localScale()).y;
				m_adjustableHeightContainer.set_localPosition(localPosition);
				m_height = value;
			}
		}

		public unsafe void SetCellIndicator(MapCellIndicator cellIndicator)
		{
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Expected O, but got Unknown
			if (cellIndicator == m_mapCellIndicator)
			{
				return;
			}
			switch (m_tweenState)
			{
			case TweenState.None:
				if (m_mapCellIndicator == MapCellIndicator.None)
				{
					m_mapCellIndicatorRenderer.set_sprite(GetMapIndicatorSprite(cellIndicator));
					m_mapCellIndicatorRenderer.set_enabled(true);
					m_tweenState = TweenState.Showing;
					m_mapCellIndicatorTween = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 1f, 0.2f), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
					m_mapCellIndicator = cellIndicator;
					return;
				}
				break;
			case TweenState.Showing:
				TweenExtensions.Kill(m_mapCellIndicatorTween, false);
				break;
			case TweenState.Hiding:
				m_mapCellIndicator = cellIndicator;
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
			float num = Mathf.Lerp(0f, 0.2f, m_mapCellIndicatorAlpha);
			m_tweenState = TweenState.Hiding;
			m_mapCellIndicatorTween = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, num), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_mapCellIndicator = cellIndicator;
		}

		public void SetLayoutDirty()
		{
			m_layoutIsDirty = true;
		}

		private void OnEnable()
		{
			CameraHandler current = CameraHandler.current;
			if (null != current)
			{
				CameraHandler cameraHandler = current;
				cameraHandler.onZoomChanged = (Action<CameraHandler>)Delegate.Combine(cameraHandler.onZoomChanged, new Action<CameraHandler>(OnZoomChanged));
				OnZoomChanged(current);
			}
			Device.ScreenStateChanged += OnScreenStateChange;
			CharacterUILayoutElement[] layoutElements = m_layoutElements;
			int num = layoutElements.Length;
			for (int i = 0; i < num; i++)
			{
				layoutElements[i].SetContainer(this);
			}
			m_layoutIsDirty = true;
		}

		private void LateUpdate()
		{
			if (m_layoutIsDirty)
			{
				UpdateLayout();
				m_layoutIsDirty = false;
			}
		}

		private void OnDisable()
		{
			CameraHandler current = CameraHandler.current;
			if (null != current)
			{
				CameraHandler cameraHandler = current;
				cameraHandler.onZoomChanged = (Action<CameraHandler>)Delegate.Remove(cameraHandler.onZoomChanged, new Action<CameraHandler>(OnZoomChanged));
			}
			Device.ScreenStateChanged -= OnScreenStateChange;
		}

		private void OnZoomChanged(CameraHandler cameraHandler)
		{
			SetLocalScale(cameraHandler.camera.get_orthographicSize());
		}

		private void OnScreenStateChange()
		{
			CameraHandler current = CameraHandler.current;
			if (null != current)
			{
				SetLocalScale(current.camera.get_orthographicSize());
			}
		}

		private void SetLocalScale(float orthographicSize)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			int height = Screen.get_height();
			float num = (float)height / (2f * orthographicSize);
			float num2 = 100f / num * (float)height / 1080f;
			this.get_transform().set_localScale(new Vector3(num2, num2, num2));
			float height2 = m_height.GetHeight();
			Vector3 localPosition = m_adjustableHeightContainer.get_localPosition();
			localPosition.y = height2 / num2;
			m_adjustableHeightContainer.set_localPosition(localPosition);
		}

		private void UpdateLayout()
		{
			CharacterUILayoutElement[] layoutElements = m_layoutElements;
			int num = layoutElements.Length;
			if (num == 0)
			{
				return;
			}
			int layoutSpacing = m_layoutSpacing;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				CharacterUILayoutElement characterUILayoutElement = layoutElements[i];
				if (!(null == characterUILayoutElement) && characterUILayoutElement.get_enabled())
				{
					num2 += characterUILayoutElement.layoutWidth + layoutSpacing;
				}
			}
			int num3 = -(num2 - layoutSpacing >> 1);
			num2 = 0;
			for (int j = 0; j < num; j++)
			{
				CharacterUILayoutElement characterUILayoutElement2 = layoutElements[j];
				if (!(null == characterUILayoutElement2) && characterUILayoutElement2.get_enabled())
				{
					characterUILayoutElement2.SetLayoutPosition(num2 + num3);
					num2 += characterUILayoutElement2.layoutWidth + layoutSpacing;
				}
			}
		}

		private float MapCellIndicatorTweenGetter()
		{
			return m_mapCellIndicatorAlpha;
		}

		private void MapCellIndicatorTweenSetter(float value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			Color color = m_color;
			color.a *= value;
			m_mapCellIndicatorRenderer.set_color(color);
			m_mapCellIndicatorAlpha = value;
		}

		private unsafe void OnMapCellIndicatorHidingTweenComplete()
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			if (m_mapCellIndicator == MapCellIndicator.None)
			{
				m_mapCellIndicatorRenderer.set_enabled(false);
				m_tweenState = TweenState.None;
				m_mapCellIndicatorTween = null;
			}
			else
			{
				m_mapCellIndicatorRenderer.set_sprite(GetMapIndicatorSprite(m_mapCellIndicator));
				m_tweenState = TweenState.Showing;
				m_mapCellIndicatorTween = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 1f, 0.2f), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
		}

		private void OnMapCellIndicatorShowingTweenComplete()
		{
			m_tweenState = TweenState.None;
			m_mapCellIndicatorTween = null;
		}

		private Sprite GetMapIndicatorSprite(MapCellIndicator cellIndicator)
		{
			switch (cellIndicator)
			{
			case MapCellIndicator.None:
				return null;
			case MapCellIndicator.Death:
				return m_resources.mapCellIndicatorDeathIcon;
			default:
				throw new ArgumentOutOfRangeException("cellIndicator", cellIndicator, null);
			}
		}

		public CharacterUIContainer()
			: this()
		{
		}
	}
}
