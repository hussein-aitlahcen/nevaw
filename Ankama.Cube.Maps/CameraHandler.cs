using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	[RequireComponent(typeof(Camera))]
	public sealed class CameraHandler : MonoBehaviour
	{
		private struct CameraWorldRect
		{
			private const float OneOverSqrtTwo = 0.707106769f;

			public readonly float top;

			public readonly float left;

			public readonly float bottom;

			public readonly float right;

			public Vector3 center
			{
				[Pure]
				get
				{
					//IL_004a: Unknown result type (might be due to invalid IL or missing references)
					//IL_004f: Unknown result type (might be due to invalid IL or missing references)
					float num = left + 0.5f * (right - left);
					float num2 = bottom + 0.5f * (top - bottom);
					float num3 = num * 0.707106769f;
					float num4 = num2 * 0.707106769f;
					return Vector2.op_Implicit(new Vector2(num3 + num4, num4 - num3));
				}
			}

			public unsafe CameraWorldRect(IMapDefinition mapDefinition, Vector2 margin)
			{
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0081: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				//IL_009b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_0109: Unknown result type (might be due to invalid IL or missing references)
				//IL_0123: Unknown result type (might be due to invalid IL or missing references)
				//IL_012d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0137: Unknown result type (might be due to invalid IL or missing references)
				//IL_0141: Unknown result type (might be due to invalid IL or missing references)
				//IL_015b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0165: Unknown result type (might be due to invalid IL or missing references)
				//IL_016f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0179: Unknown result type (might be due to invalid IL or missing references)
				//IL_0197: Unknown result type (might be due to invalid IL or missing references)
				//IL_019c: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_020c: Unknown result type (might be due to invalid IL or missing references)
				//IL_022d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0253: Unknown result type (might be due to invalid IL or missing references)
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				int regionCount = mapDefinition.regionCount;
				Vector2Int val3 = default(Vector2Int);
				for (int i = 0; i < regionCount; i++)
				{
					FightMapRegionDefinition region = mapDefinition.GetRegion(i);
					Vector2Int sizeMin = region.sizeMin;
					Vector2Int val = region.sizeMax - Vector2Int.get_one();
					Vector2Int val2 = new Vector2Int(sizeMin.get_x(), val.get_y());
					val3._002Ector(val.get_x(), sizeMin.get_y());
					Vector2 val4 = Translate(Vector2Int.op_Implicit(sizeMin));
					Vector2 val5 = Translate(Vector2Int.op_Implicit(val));
					Vector2 val6 = Translate(Vector2Int.op_Implicit(val2));
					Vector2 val7 = Translate(Vector2Int.op_Implicit(val3));
					num = Mathf.Max(new float[5]
					{
						num,
						((IntPtr)(void*)val4).y,
						((IntPtr)(void*)val5).y,
						((IntPtr)(void*)val6).y,
						((IntPtr)(void*)val7).y
					});
					num3 = Mathf.Min(new float[5]
					{
						num3,
						((IntPtr)(void*)val4).y,
						((IntPtr)(void*)val5).y,
						((IntPtr)(void*)val6).y,
						((IntPtr)(void*)val7).y
					});
					num2 = Mathf.Min(new float[5]
					{
						num2,
						((IntPtr)(void*)val4).x,
						((IntPtr)(void*)val5).x,
						((IntPtr)(void*)val6).x,
						((IntPtr)(void*)val7).x
					});
					num4 = Mathf.Max(new float[5]
					{
						num4,
						((IntPtr)(void*)val4).x,
						((IntPtr)(void*)val5).x,
						((IntPtr)(void*)val6).x,
						((IntPtr)(void*)val7).x
					});
				}
				Vector3 val8 = Vector2.op_Implicit(Translate(Vector3Int.op_Implicit(mapDefinition.origin)));
				float num5 = (((IntPtr)(void*)margin).x + 1f) * 0.707106769f;
				float num6 = (((IntPtr)(void*)margin).y + 1f) * 0.707106769f;
				left = ((IntPtr)(void*)val8).x + num2 - num5;
				right = ((IntPtr)(void*)val8).x + num4 + num5;
				top = ((IntPtr)(void*)val8).z + num + num6;
				bottom = ((IntPtr)(void*)val8).z + num3 - num6;
				if (bottom > top)
				{
					top = (bottom = ((IntPtr)(void*)val8).z);
				}
				if (left > right)
				{
					left = (right = ((IntPtr)(void*)val8).x);
				}
			}

			private CameraWorldRect(float top, float left, float bottom, float right)
			{
				this.top = top;
				this.left = left;
				this.bottom = bottom;
				this.right = right;
			}

			[PublicAPI]
			[Pure]
			public unsafe static Vector2 Translate(Vector2 v)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				float num = ((IntPtr)(void*)v).x * 0.707106769f;
				float num2 = ((IntPtr)(void*)v).y * 0.707106769f;
				return new Vector2(num - num2, num + num2);
			}

			[PublicAPI]
			[Pure]
			public unsafe static Vector2 Translate(Vector3 v)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				float num = ((IntPtr)(void*)v).x * 0.707106769f;
				float num2 = ((IntPtr)(void*)v).z * 0.707106769f;
				return Vector2.op_Implicit(new Vector3(num - num2, ((IntPtr)(void*)v).y, num + num2));
			}

			[PublicAPI]
			[Pure]
			public unsafe static Vector2 TranslateInverse(Vector2 v)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				float num = ((IntPtr)(void*)v).x * 0.707106769f;
				float num2 = ((IntPtr)(void*)v).y * 0.707106769f;
				return new Vector2(num + num2, num2 - num);
			}

			[PublicAPI]
			[Pure]
			public unsafe static Vector3 TranslateInverse(Vector3 v)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				float num = ((IntPtr)(void*)v).x * 0.707106769f;
				float num2 = ((IntPtr)(void*)v).z * 0.707106769f;
				return new Vector3(num + num2, ((IntPtr)(void*)v).y, num2 - num);
			}

			[PublicAPI]
			[Pure]
			public unsafe bool ContainsPoint(Vector2 position)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				float num = ((IntPtr)(void*)position).x * 0.707106769f;
				float num2 = ((IntPtr)(void*)position).y * 0.707106769f;
				float num3 = num - num2;
				float num4 = num + num2;
				if (num3 >= left && num3 <= right && num4 >= bottom)
				{
					return num4 <= top;
				}
				return false;
			}

			[PublicAPI]
			[Pure]
			public unsafe bool ContainsPoint(Vector3 position)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				float num = ((IntPtr)(void*)position).x * 0.707106769f;
				float num2 = ((IntPtr)(void*)position).z * 0.707106769f;
				float num3 = num - num2;
				float num4 = num + num2;
				if (num3 >= left && num3 <= right && num4 >= bottom)
				{
					return num4 <= top;
				}
				return false;
			}

			[PublicAPI]
			[Pure]
			public unsafe Vector2 ClosestPoint(Vector2 position)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				float num = ((IntPtr)(void*)position).x * 0.707106769f;
				float num2 = ((IntPtr)(void*)position).y * 0.707106769f;
				float num3 = Mathf.Clamp(num - num2, left, right);
				float num4 = Mathf.Clamp(num + num2, bottom, top);
				float num5 = num3 * 0.707106769f;
				float num6 = num4 * 0.707106769f;
				return new Vector2(num5 + num6, num6 - num5);
			}

			[PublicAPI]
			[Pure]
			public unsafe Vector3 ClosestPoint(Vector3 position)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				float num = ((IntPtr)(void*)position).x * 0.707106769f;
				float num2 = ((IntPtr)(void*)position).z * 0.707106769f;
				float num3 = Mathf.Clamp(num - num2, left, right);
				float num4 = Mathf.Clamp(num + num2, bottom, top);
				float num5 = num3 * 0.707106769f;
				float num6 = num4 * 0.707106769f;
				return new Vector3(num5 + num6, ((IntPtr)(void*)position).y, num6 - num5);
			}

			[Pure]
			public unsafe CameraWorldRect RemoveMargin(Vector2 margin)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				float num = (((IntPtr)(void*)margin).x + 1f) * 0.707106769f;
				float num2 = (((IntPtr)(void*)margin).y + 1f) * 0.707106769f;
				float num3 = left + num;
				float num4 = right - num;
				float num5 = top - num2;
				float num6 = bottom + num2;
				if (num6 > num5)
				{
					num5 = (num6 = bottom + (top - bottom) * 0.5f);
				}
				if (left > right)
				{
					num3 = (num4 = left + (right - left) * 0.5f);
				}
				return new CameraWorldRect(num5, num3, num6, num4);
			}
		}

		public delegate void MapRotationChangedDelegate(DirectionAngle previousDirectionAngle, DirectionAngle newDirectionAngle);

		public const int ReferenceScreenHeight = 1080;

		private const float ReferenceMinOrthoSize = 6f;

		private const float AbsoluteMaxOrthoSize = 2.5f;

		private const float ReferenceScreenDpi = 96f;

		private const float ReferenceMaxOrthoSize = 3f;

		private const float ReferenceMaxUnitSize = 180f;

		private const float ReferenceMaxPhysicalSize = 1.875f;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_zoomLevel;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_zoomIncrement = 0.2f;

		[SerializeField]
		private float m_zoomTweenDuration = 0.4f;

		[SerializeField]
		private float m_panTweenDuration = 0.25f;

		[SerializeField]
		private Vector3 m_cameraPositionOffset = new Vector3(-0.5f, 0f, -0.5f);

		[SerializeField]
		private Vector2 m_cameraBoundsMargin = new Vector2(-5f, -5f);

		[SerializeField]
		private Vector2 m_regionFocusMargin = new Vector2(2f, 2f);

		[SerializeField]
		private CameraShakeParameters m_shakeParameters;

		[SerializeField]
		private CameraControlParameters m_defaultControlParameters;

		[SerializeField]
		private CameraControlParameters m_cinematicControlParameters;

		public Action<CameraHandler> onMoved;

		public Action<CameraHandler> onZoomChanged;

		private Transform m_cameraContainer;

		private Bounds m_mapWorldBounds;

		private CameraWorldRect m_cameraWorldRect;

		private float m_maxOrthoSize;

		private float m_targetZoomLevel;

		private Vector3 m_targetCameraPosition;

		private TweenerCore<Vector3, Vector3, VectorOptions> m_panTween;

		private TweenerCore<float, float, FloatOptions> m_zoomTween;

		private bool m_cameraIsShaking;

		private float m_cameraShakeStrength;

		private bool m_userHasControl;

		private Coroutine m_regionFocusCoroutine;

		public static CameraHandler current
		{
			get;
			private set;
		}

		[PublicAPI]
		public Camera camera
		{
			get;
			private set;
		}

		[PublicAPI]
		public DirectionAngle mapRotation
		{
			get;
			private set;
		}

		[PublicAPI]
		public float zoomLevel => m_zoomLevel;

		[PublicAPI]
		public float zoomScale => 6f / camera.get_orthographicSize();

		[PublicAPI]
		public bool hasZoomRange => m_maxOrthoSize < 6f;

		[PublicAPI]
		public bool userHasControl => m_userHasControl;

		[PublicAPI]
		public CameraControlParameters cinematicControlParameters => m_cinematicControlParameters;

		private static event MapRotationChangedDelegate MapRotationChanged;

		public void Initialize([NotNull] IMapDefinition mapDefinition, Bounds mapWorldBounds, bool giveUserControl)
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			Camera camera = this.camera;
			if (null == camera)
			{
				throw new NullReferenceException("A camera used by a CameraHandler was destroyed.");
			}
			Transform parent = camera.get_transform().get_parent();
			if (null == parent)
			{
				Log.Error("Camera doesn't have a pivot transform.", 151, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\CameraHandler.cs");
				return;
			}
			Transform parent2 = parent.get_parent();
			if (null == parent2)
			{
				Log.Error("Camera doesn't have a container transform.", 158, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\CameraHandler.cs");
				return;
			}
			CameraWorldRect cameraWorldRect = new CameraWorldRect(mapDefinition, m_cameraBoundsMargin);
			Vector3 val = cameraWorldRect.center + parent2.get_rotation() * m_cameraPositionOffset;
			parent2.set_position(val);
			float initializationOrthographicSize = GetInitializationOrthographicSize(cameraWorldRect, m_cameraBoundsMargin);
			float num = MathUtility.InverseLerpUnclamped(6f, m_maxOrthoSize, initializationOrthographicSize);
			camera.set_orthographicSize(initializationOrthographicSize);
			m_zoomLevel = num;
			m_targetZoomLevel = num;
			m_targetCameraPosition = val;
			m_cameraContainer = parent2;
			m_mapWorldBounds = mapWorldBounds;
			m_cameraWorldRect = cameraWorldRect;
			m_userHasControl = giveUserControl;
		}

		private float GetInitializationOrthographicSize(CameraWorldRect cameraWorldRect, Vector2 margin)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			CameraWorldRect cameraWorldRect2 = cameraWorldRect.RemoveMargin(margin);
			float num = 2f * m_cameraPositionOffset.get_magnitude();
			float num2 = cameraWorldRect2.top - cameraWorldRect2.bottom + m_regionFocusMargin.x + num;
			float num3 = cameraWorldRect2.right - cameraWorldRect2.left + m_regionFocusMargin.y + num;
			float num4 = num2 / 4f;
			float num5 = num3 / 2f / camera.get_aspect();
			return Mathf.Max(Mathf.Max(num4, num5), 6f);
		}

		public void StartFocusOnMapRegion([NotNull] IMapDefinition mapDefinition, int regionIndex, [CanBeNull] CameraControlParameters parameters = null)
		{
			m_regionFocusCoroutine = this.StartCoroutine(FocusOnMapRegion(mapDefinition, regionIndex, parameters));
		}

		public unsafe IEnumerator FocusOnMapRegion([NotNull] IMapDefinition mapDefinition, int regionIndex, [CanBeNull] CameraControlParameters parameters = null)
		{
			if (m_regionFocusCoroutine != null)
			{
				this.StopCoroutine(m_regionFocusCoroutine);
				m_regionFocusCoroutine = null;
			}
			Camera camera = this.camera;
			Transform cameraContainer = m_cameraContainer;
			if (null == camera || null == cameraContainer)
			{
				yield break;
			}
			float panTweenDuration;
			Ease val;
			float num;
			Ease val2;
			if (null == parameters)
			{
				CameraControlParameters defaultControlParameters = m_defaultControlParameters;
				if (null == defaultControlParameters)
				{
					panTweenDuration = m_panTweenDuration;
					val = 9;
					num = m_zoomTweenDuration;
					val2 = 9;
				}
				else
				{
					panTweenDuration = defaultControlParameters.panTweenDuration;
					val = defaultControlParameters.panTweenEase;
					num = defaultControlParameters.zoomTweenMaxDuration;
					val2 = defaultControlParameters.zoomTweenEase;
				}
			}
			else
			{
				panTweenDuration = parameters.panTweenDuration;
				val = parameters.panTweenEase;
				num = parameters.zoomTweenMaxDuration;
				val2 = parameters.zoomTweenEase;
			}
			FightMapRegionDefinition region = mapDefinition.GetRegion(regionIndex);
			Vector3 val3 = Vector3Int.op_Implicit(mapDefinition.origin);
			Vector2 val4 = Vector2Int.op_Implicit(region.sizeMin);
			Vector2 val5 = Vector2Int.op_Implicit(region.sizeMax);
			Vector3 val6 = cameraContainer.get_rotation() * m_cameraPositionOffset;
			Vector2 val7 = val4 + 0.5f * (val5 - Vector2.get_one() - val4);
			Vector3 targetCameraPosition = new Vector3(((IntPtr)(void*)val7).x, 0f, ((IntPtr)(void*)val7).y) + val3 + val6;
			Vector3 val8 = default(Vector3);
			val8._002Ector(((IntPtr)(void*)val4).x, 0f, ((IntPtr)(void*)val4).y);
			Vector3 val9 = default(Vector3);
			val9._002Ector(((IntPtr)(void*)val5).x, 0f, ((IntPtr)(void*)val5).y);
			Vector3 val10 = default(Vector3);
			val10._002Ector(((IntPtr)(void*)val8).x, 0f, ((IntPtr)(void*)val9).z);
			Vector3 val11 = default(Vector3);
			val11._002Ector(((IntPtr)(void*)val9).x, 0f, ((IntPtr)(void*)val8).z);
			Vector3 val12 = cameraContainer.get_rotation() * new Vector3(1f, 0f, 1f);
			Vector3 normalized = val12.get_normalized();
			Vector3 normalized2 = val6.get_normalized();
			val12 = val9 - val8;
			float num2 = Mathf.Abs(Vector3.Dot(val12.get_normalized(), normalized));
			val12 = val11 - val10;
			float num3 = Mathf.Abs(Vector3.Dot(val12.get_normalized(), normalized));
			float num4 = Mathf.Abs(Vector3.Dot(normalized2, normalized));
			float num5 = Vector3.Distance(val8, val9);
			float num6 = Vector3.Distance(val10, val11);
			float num7 = 2f * val6.get_magnitude();
			Vector2 val13 = 2.828427f * m_regionFocusMargin;
			float num8 = num2 * num5 + num3 * num6 + num4 * num7 + ((IntPtr)(void*)val13).y;
			float num9 = (1f - num2) * num5 + (1f - num3) * num6 + (1f - num4) * num7 + ((IntPtr)(void*)val13).x;
			float num10 = num8 / 4f;
			float num11 = num9 / 2f / camera.get_aspect();
			float num12 = Mathf.Max(num10, num11);
			float targetZoomLevel2 = Mathf.InverseLerp(6f, m_maxOrthoSize, num12);
			targetZoomLevel2 = Mathf.Floor(targetZoomLevel2 / m_zoomIncrement) * m_zoomIncrement;
			m_targetCameraPosition = targetCameraPosition;
			m_targetZoomLevel = targetZoomLevel2;
			m_userHasControl = false;
			if (m_panTween != null && TweenExtensions.IsPlaying(m_panTween))
			{
				TweenSettingsExtensions.SetEase<Tweener>(m_panTween.ChangeEndValue((object)targetCameraPosition, panTweenDuration, true), val);
			}
			else
			{
				m_panTween = TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(new DOGetter<Vector3>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Vector3>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), targetCameraPosition, panTweenDuration), val);
			}
			float num13 = Mathf.Lerp(0f, num, Mathf.Abs(targetZoomLevel2 - m_zoomLevel) / m_zoomIncrement);
			if (m_zoomTween != null && TweenExtensions.IsPlaying(m_zoomTween))
			{
				TweenSettingsExtensions.SetEase<Tweener>(m_zoomTween.ChangeEndValue((object)targetZoomLevel2, num13, true), val2);
			}
			else
			{
				m_zoomTween = TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), targetZoomLevel2, num13), val2);
			}
			while (TweenExtensions.IsPlaying(m_panTween) || TweenExtensions.IsPlaying(m_zoomTween))
			{
				yield return null;
				if (m_targetCameraPosition != targetCameraPosition || m_targetZoomLevel != targetZoomLevel2)
				{
					break;
				}
			}
			m_regionFocusCoroutine = null;
			m_userHasControl = true;
		}

		public unsafe void Pan(Vector2 screenPosition, Vector2 previousScreenPosition)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			Camera camera = this.camera;
			Transform cameraContainer = m_cameraContainer;
			if (!(null == camera) && !(null == cameraContainer))
			{
				Plane val = default(Plane);
				val._002Ector(Vector3.get_up(), m_mapWorldBounds.get_center());
				Ray val2 = camera.ScreenPointToRay(Vector2.op_Implicit(screenPosition));
				float num = default(float);
				val.Raycast(val2, ref num);
				Vector3 point = val2.GetPoint(num);
				Ray val3 = camera.ScreenPointToRay(Vector2.op_Implicit(previousScreenPosition));
				val.Raycast(val3, ref num);
				Vector3 val4 = val3.GetPoint(num) - point;
				Vector3 val5 = cameraContainer.get_rotation() * m_cameraPositionOffset;
				Vector3 position = m_targetCameraPosition + val4 - val5;
				position = (m_targetCameraPosition = val5 + m_cameraWorldRect.ClosestPoint(position));
				if (m_panTween != null && TweenExtensions.IsPlaying(m_panTween))
				{
					m_panTween.ChangeEndValue((object)position, m_panTweenDuration, true);
				}
				else
				{
					m_panTween = DOTween.To(new DOGetter<Vector3>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Vector3>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), position, m_panTweenDuration);
				}
			}
		}

		public unsafe void TweenZoom(float scrollDelta)
		{
			float num = Mathf.Sign(scrollDelta) * m_zoomIncrement;
			float num2 = Mathf.Clamp01(m_targetZoomLevel + num);
			if (!(Math.Abs(num2 - m_targetZoomLevel) < float.Epsilon))
			{
				float num3 = Mathf.Lerp(0f, m_zoomTweenDuration, (num2 - m_zoomLevel) / num);
				m_targetZoomLevel = num2;
				if (m_zoomTween != null && TweenExtensions.IsPlaying(m_zoomTween))
				{
					m_zoomTween.ChangeEndValue((object)num2, num3, true);
				}
				else
				{
					m_zoomTween = DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), num2, num3);
				}
			}
		}

		public static void AddMapRotationListener([NotNull] MapRotationChangedDelegate callback)
		{
			MapRotationChanged += callback;
			if (null == current)
			{
				callback(DirectionAngle.None, DirectionAngle.None);
			}
			else
			{
				callback(DirectionAngle.None, current.mapRotation);
			}
		}

		public static void RemoveMapRotationListener([NotNull] MapRotationChangedDelegate callback)
		{
			MapRotationChanged -= callback;
		}

		public void ChangeRotation(DirectionAngle directionAngle)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			if (this.mapRotation != directionAngle)
			{
				DirectionAngle mapRotation = this.mapRotation;
				this.mapRotation = directionAngle;
				Transform cameraContainer = m_cameraContainer;
				if (null != cameraContainer)
				{
					Quaternion inverseRotation = directionAngle.GetInverseRotation();
					Vector3 val = mapRotation.GetInverseRotation() * -m_cameraPositionOffset + inverseRotation * m_cameraPositionOffset;
					cameraContainer.SetPositionAndRotation(cameraContainer.get_position() + val, inverseRotation);
				}
				CameraHandler.MapRotationChanged?.Invoke(mapRotation, directionAngle);
			}
		}

		public void AddShake(float value)
		{
			m_cameraShakeStrength = Mathf.Clamp01(m_cameraShakeStrength + value);
		}

		private void Awake()
		{
			current = this;
			camera = this.GetComponent<Camera>();
			if (null == camera)
			{
				throw new NullReferenceException("A camera used by a CameraHandler was destroyed.");
			}
			m_targetZoomLevel = m_zoomLevel;
			Setup();
			Device.ScreenStateChanged += OnScreenStateChange;
		}

		private unsafe void LateUpdate()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			float cameraShakeStrength = m_cameraShakeStrength;
			if (cameraShakeStrength > 0f)
			{
				CameraShakeParameters shakeParameters = m_shakeParameters;
				if (null != shakeParameters)
				{
					Transform transform = this.get_transform();
					float time = Time.get_time();
					Vector2 translation = shakeParameters.GetTranslation(time, cameraShakeStrength);
					float angle = shakeParameters.GetAngle(time, cameraShakeStrength);
					Vector3 localPosition = default(Vector3);
					localPosition._002Ector(((IntPtr)(void*)translation).x, ((IntPtr)(void*)translation).y, ((IntPtr)(void*)transform.get_localPosition()).z);
					Quaternion localRotation = Quaternion.AngleAxis(angle, Vector3.get_forward());
					transform.set_localPosition(localPosition);
					transform.set_localRotation(localRotation);
					m_cameraIsShaking = true;
				}
			}
			else if (m_cameraIsShaking)
			{
				Transform transform2 = this.get_transform();
				Vector3 localPosition2 = transform2.get_localPosition();
				localPosition2.x = 0f;
				localPosition2.y = 0f;
				transform2.set_localPosition(localPosition2);
				transform2.set_localRotation(Quaternion.get_identity());
				m_cameraIsShaking = false;
			}
			m_cameraShakeStrength = 0f;
		}

		private void OnDestroy()
		{
			if (current == this)
			{
				current = null;
			}
			if (m_panTween != null)
			{
				TweenExtensions.Kill(m_panTween, false);
				m_panTween = null;
			}
			if (m_zoomTween != null)
			{
				TweenExtensions.Kill(m_zoomTween, false);
				m_zoomTween = null;
			}
			Device.ScreenStateChanged -= OnScreenStateChange;
		}

		private void Setup()
		{
			float dpi = Device.dpi;
			if (dpi <= 0f)
			{
				m_maxOrthoSize = 3f;
			}
			else
			{
				float num = 1.875f * dpi;
				m_maxOrthoSize = Mathf.Clamp((float)camera.get_pixelHeight() / num / 2f, 2.5f, 6f);
			}
			camera.set_orthographicSize(Mathf.Lerp(6f, m_maxOrthoSize, m_zoomLevel));
		}

		private void OnScreenStateChange()
		{
			Setup();
		}

		private Vector3 PanTweenGetter()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_cameraContainer.get_position();
		}

		private void PanTweenSetter(Vector3 value)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_cameraContainer.set_position(value);
			onMoved?.Invoke(this);
		}

		private float ZoomTweenGetter()
		{
			return m_zoomLevel;
		}

		private void ZoomTweenSetter(float value)
		{
			camera.set_orthographicSize(Mathf.LerpUnclamped(6f, m_maxOrthoSize, value));
			m_zoomLevel = value;
			onZoomChanged?.Invoke(this);
		}

		public CameraHandler()
			: this()
		{
		}//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)

	}
}
