using Ankama.Cube.UI.Fight.Windows;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	public abstract class AbstractTooltipWindow : MonoBehaviour
	{
		public struct RectOffset
		{
			public float left;

			public float right;

			public float top;

			public float bottom;

			public RectOffset(float left, float right, float top, float bottom)
			{
				this.left = left;
				this.right = right;
				this.top = top;
				this.bottom = bottom;
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003C_003Ec__DisplayClass22_0
		{
			public Rect targetRect;

			public Vector2 tooltipSize;

			public Rect screenRect;
		}

		private const float ScreenMargin = 10f;

		[SerializeField]
		protected TooltipWindowParameters m_parameters;

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		private Tweener m_tweenAlpha;

		private Tweener m_tweenPosition;

		private bool m_opened;

		private RectOffset m_borderDistanceToScreen;

		public RectOffset borderDistanceToScreen => m_borderDistanceToScreen;

		public float alpha
		{
			get
			{
				return m_canvasGroup.get_alpha();
			}
			set
			{
				if (value <= 0f && this.get_gameObject().get_activeSelf())
				{
					this.get_gameObject().SetActive(false);
				}
				else if (value > 0f && !this.get_gameObject().get_activeSelf())
				{
					this.get_gameObject().SetActive(true);
				}
				m_canvasGroup.set_alpha(value);
			}
		}

		public virtual void Awake()
		{
			alpha = 0f;
			this.get_gameObject().SetActive(false);
		}

		public unsafe void ShowAt(TooltipPosition location, RectTransform targetRect)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			_003F val = this.get_transform();
			Vector3 lossyScale = val.get_lossyScale();
			Rect screenRect = RectTransformToScreenSpace(val.get_root());
			Rect rect = RectTransformToScreenSpace(targetRect);
			Enlarge(ref rect, 10f * ((IntPtr)(void*)lossyScale).x, 10f * ((IntPtr)(void*)lossyScale).y);
			Rect rect2 = val.get_rect();
			location = BestLocation(Vector2.Scale(rect2.get_size(), Vector2.op_Implicit(lossyScale)), screenRect, rect, location);
			Vector3 position = GetPosition(rect, location);
			position.z = ((IntPtr)(void*)this.get_transform().get_position()).z;
			ShowAt(location, position);
		}

		public unsafe void ShowAt(TooltipPosition position, Vector3 worldPosition)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			_003F val = this.get_transform();
			Vector3 lossyScale = val.get_lossyScale();
			val.set_pivot(GetPivotFor(position));
			RectTransform transform = val.get_root();
			Rect val2 = RectTransformToScreenSpace(val, worldPosition);
			Rect rect = RectTransformToScreenSpace(transform);
			Enlarge(ref rect, -10f * ((IntPtr)(void*)lossyScale).x, -10f * ((IntPtr)(void*)lossyScale).y);
			float num = rect.get_xMax() - val2.get_xMax();
			float num2 = val2.get_xMin() - rect.get_xMin();
			float num3 = rect.get_yMax() - val2.get_yMax();
			float num4 = val2.get_yMin() - rect.get_yMin();
			m_borderDistanceToScreen = new RectOffset(num2, num, num3, num4);
			if (num < 0f)
			{
				worldPosition.x += num;
			}
			if (num2 < 0f)
			{
				worldPosition.x -= num2;
			}
			if (num3 < 0f)
			{
				worldPosition.y += num3;
			}
			if (num4 < 0f)
			{
				worldPosition.y -= num4;
			}
			DisplayTooltip(worldPosition);
		}

		protected unsafe virtual void DisplayTooltip(Vector3 worldPosition)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			Vector3 position = this.get_transform().get_position();
			worldPosition.z = ((IntPtr)(void*)position).z;
			TooltipWindowParameters parameters = m_parameters;
			Tweener tweenPosition = m_tweenPosition;
			if (tweenPosition != null)
			{
				TweenExtensions.Kill(tweenPosition, false);
			}
			Tweener tweenAlpha = m_tweenAlpha;
			if (tweenAlpha != null)
			{
				TweenExtensions.Kill(tweenAlpha, false);
			}
			this.get_gameObject().SetActive(true);
			m_tweenAlpha = TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 1f, (1f - alpha) * parameters.openDuration), parameters.openEase);
			Transform transform = this.get_transform();
			if (!m_opened)
			{
				transform.set_position(worldPosition);
				TweenSettingsExtensions.SetDelay<Tweener>(m_tweenAlpha, parameters.openDelay);
			}
			else
			{
				m_tweenPosition = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMove(transform, worldPosition, parameters.moveDuration, false), parameters.moveEase);
			}
			m_opened = true;
		}

		public unsafe virtual void Hide()
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			Tweener tweenAlpha = m_tweenAlpha;
			if (tweenAlpha != null)
			{
				TweenExtensions.Kill(tweenAlpha, false);
			}
			m_tweenAlpha = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, alpha * m_parameters.closeDuration), m_parameters.closeEase), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private float TweenAlphaGetter()
		{
			return alpha;
		}

		private void TweenAlphaSetter(float value)
		{
			alpha = value;
		}

		private void TweenCompleteCallback()
		{
			this.get_gameObject().SetActive(false);
			m_opened = false;
		}

		private static Vector2 GetPivotFor(TooltipPosition position)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			switch (position)
			{
			case TooltipPosition.Top:
				return new Vector2(0.5f, 0f);
			case TooltipPosition.Bottom:
				return new Vector2(0.5f, 1f);
			case TooltipPosition.Left:
				return new Vector2(1f, 0.5f);
			case TooltipPosition.Right:
				return new Vector2(0f, 0.5f);
			default:
				throw new ArgumentOutOfRangeException("position", position, null);
			}
		}

		private static TooltipPosition BestLocation(Vector2 tooltipSize, Rect screenRect, Rect targetRect, TooltipPosition position)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			_003C_003Ec__DisplayClass22_0 _003C_003Ec__DisplayClass22_ = default(_003C_003Ec__DisplayClass22_0);
			_003C_003Ec__DisplayClass22_.targetRect = targetRect;
			_003C_003Ec__DisplayClass22_.tooltipSize = tooltipSize;
			_003C_003Ec__DisplayClass22_.screenRect = screenRect;
			switch (position)
			{
			case TooltipPosition.Top:
				if (_003CBestLocation_003Eg__InsideTop_007C22_0(ref _003C_003Ec__DisplayClass22_))
				{
					return TooltipPosition.Top;
				}
				if (!_003CBestLocation_003Eg__InsideBottom_007C22_1(ref _003C_003Ec__DisplayClass22_))
				{
					return TooltipPosition.Top;
				}
				return TooltipPosition.Bottom;
			case TooltipPosition.Bottom:
				if (_003CBestLocation_003Eg__InsideBottom_007C22_1(ref _003C_003Ec__DisplayClass22_))
				{
					return TooltipPosition.Bottom;
				}
				if (!_003CBestLocation_003Eg__InsideTop_007C22_0(ref _003C_003Ec__DisplayClass22_))
				{
					return TooltipPosition.Bottom;
				}
				return TooltipPosition.Top;
			case TooltipPosition.Left:
				if (_003CBestLocation_003Eg__InsideLeft_007C22_2(ref _003C_003Ec__DisplayClass22_))
				{
					return TooltipPosition.Left;
				}
				if (!_003CBestLocation_003Eg__InsideRight_007C22_3(ref _003C_003Ec__DisplayClass22_))
				{
					return TooltipPosition.Left;
				}
				return TooltipPosition.Right;
			case TooltipPosition.Right:
				if (_003CBestLocation_003Eg__InsideRight_007C22_3(ref _003C_003Ec__DisplayClass22_))
				{
					return TooltipPosition.Right;
				}
				if (!_003CBestLocation_003Eg__InsideLeft_007C22_2(ref _003C_003Ec__DisplayClass22_))
				{
					return TooltipPosition.Right;
				}
				return TooltipPosition.Left;
			default:
				throw new ArgumentOutOfRangeException("position", position, null);
			}
		}

		private static Rect RectTransformToScreenSpace(RectTransform transform)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return RectTransformToScreenSpace(transform, transform.get_position());
		}

		private unsafe static Rect RectTransformToScreenSpace(RectTransform transform, Vector3 worldPosition)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			Vector2 pivot = transform.get_pivot();
			Rect rect = transform.get_rect();
			Vector2 val = Vector2.Scale(rect.get_size(), Vector2.op_Implicit(transform.get_lossyScale()));
			float num = ((IntPtr)(void*)worldPosition).x - ((IntPtr)(void*)pivot).x * ((IntPtr)(void*)val).x;
			float num2 = ((IntPtr)(void*)worldPosition).y - ((IntPtr)(void*)pivot).y * ((IntPtr)(void*)val).y;
			return new Rect(num, num2, ((IntPtr)(void*)val).x, ((IntPtr)(void*)val).y);
		}

		private static Vector3 GetPosition(Rect target, TooltipPosition position)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			switch (position)
			{
			case TooltipPosition.Top:
				return Vector2.op_Implicit(new Vector2(target.get_x() + target.get_width() * 0.5f, target.get_yMax()));
			case TooltipPosition.Bottom:
				return Vector2.op_Implicit(new Vector2(target.get_x() + target.get_width() * 0.5f, target.get_yMin()));
			case TooltipPosition.Left:
				return Vector2.op_Implicit(new Vector2(target.get_xMin(), target.get_y() + target.get_height() * 0.5f));
			case TooltipPosition.Right:
				return Vector2.op_Implicit(new Vector2(target.get_xMax(), target.get_y() + target.get_height() * 0.5f));
			default:
				throw new ArgumentOutOfRangeException("position", position, null);
			}
		}

		private static void Enlarge(ref Rect rect, float marginX, float marginY)
		{
			rect.set_x(rect.get_x() - marginX);
			rect.set_width(rect.get_width() + 2f * marginX);
			rect.set_y(rect.get_y() - marginY);
			rect.set_height(rect.get_height() + 2f * marginY);
		}

		protected AbstractTooltipWindow()
			: this()
		{
		}

		[CompilerGenerated]
		private static bool _003CBestLocation_003Eg__InsideTop_007C22_0(ref _003C_003Ec__DisplayClass22_0 P_0)
		{
			return P_0.targetRect.get_yMax() + P_0.tooltipSize.y <= P_0.screenRect.get_yMax();
		}

		[CompilerGenerated]
		private static bool _003CBestLocation_003Eg__InsideBottom_007C22_1(ref _003C_003Ec__DisplayClass22_0 P_0)
		{
			return P_0.targetRect.get_yMin() - P_0.tooltipSize.y >= P_0.screenRect.get_yMin();
		}

		[CompilerGenerated]
		private static bool _003CBestLocation_003Eg__InsideLeft_007C22_2(ref _003C_003Ec__DisplayClass22_0 P_0)
		{
			return P_0.targetRect.get_xMin() + P_0.tooltipSize.x >= P_0.screenRect.get_xMin();
		}

		[CompilerGenerated]
		private static bool _003CBestLocation_003Eg__InsideRight_007C22_3(ref _003C_003Ec__DisplayClass22_0 P_0)
		{
			return P_0.targetRect.get_xMax() + P_0.tooltipSize.x <= P_0.screenRect.get_xMax();
		}
	}
}
