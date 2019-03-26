using Ankama.Cube.Audio;
using Ankama.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Ankama.Cube.Maps
{
	public class MapCameraHandler : MonoBehaviour
	{
		public const float originalAspect = 1.77777779f;

		public const int ReferenceScreenHeight = 1080;

		private const float ReferenceMaxOrthoSize = 3.6f;

		private const float ReferenceMinOrthoSize = 10f;

		private const float AbsoluteMaxOrthoSize = 2.5f;

		private const float ReferenceScreenDpi = 96f;

		private const float ReferenceMaxUnitSize = 150f;

		private const float ReferenceMaxPhysicalSize = 1.5625f;

		[SerializeField]
		private Camera m_camera;

		[SerializeField]
		private Transform m_cameraContainer;

		[Header("Audio")]
		[SerializeField]
		private AudioListenerPosition m_audioListener;

		[Header("Zoom")]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_zoomLevel;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_zoomIncrement = 0.2f;

		[SerializeField]
		private float m_zoomTweenDuration = 0.4f;

		[SerializeField]
		private Ease m_zoomEase = 1;

		[Header("Move")]
		[SerializeField]
		private Vector3 m_targetOffset = Vector3.get_zero();

		[SerializeField]
		[Range(0f, 1f)]
		private float m_minZoomLerp = 0.1f;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_deadZoneWidth = 0.5f;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_deadZoneHeight = 0.5f;

		[SerializeField]
		private float m_damp = 0.5f;

		[SerializeField]
		private float m_unZoomDamp = 1f;

		[Header("Enter Anim")]
		[SerializeField]
		private PlayableDirector m_playableDirector;

		[SerializeField]
		private float m_animStartOrthoSize = 10f;

		[SerializeField]
		private float m_animEndZoomLevel;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_animZoomFactor;

		private bool m_allowInteraction;

		private float m_targetZoomLevel;

		private TweenerCore<float, float, FloatOptions> m_zoomTween;

		private float m_maxOrthoSize;

		private float m_minOrthoSize;

		private Transform m_target;

		private MapData m_mapData;

		private Vector3 m_characterCameraPosition;

		private bool m_isInitialized;

		private float m_originalOrthoSize;

		private Vector3 m_originalCenterPosition;

		private Rect m_clampRect;

		private Rect m_viewClampRect;

		private Rect m_viewMovableAreaRect;

		private Rect m_viewRect;

		private Vector3 m_dampVelocity = Vector3.get_zero();

		private bool m_dampUntilCenterReached;

		public Camera camera => m_camera;

		public Transform target => m_target;

		public Rect softRect => new Rect(0.5f - m_deadZoneWidth / 2f, 0.5f - m_deadZoneHeight / 2f, m_deadZoneWidth, m_deadZoneHeight);

		public void Initialize(MapData mapData, Transform targetCharacter)
		{
			m_mapData = mapData;
			m_target = targetCharacter;
			m_zoomLevel = 0f;
			m_targetZoomLevel = 0f;
			Setup();
		}

		public void InitEnterAnimFirstFrame()
		{
			if (!m_isInitialized)
			{
				Log.Error("Camera must be initialized before.", 116, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\HavreMap\\MapCameraHandler.cs");
			}
			m_allowInteraction = false;
			m_playableDirector.set_time(0.0);
			m_playableDirector.Evaluate();
		}

		public IEnumerator PlayEnterAnim()
		{
			m_allowInteraction = false;
			m_playableDirector.Play();
			PlayableGraph graph = m_playableDirector.get_playableGraph();
			while (graph.IsValid() && !graph.IsDone())
			{
				yield return null;
			}
			m_allowInteraction = true;
		}

		private void Setup()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			if (!m_isInitialized)
			{
				m_originalOrthoSize = m_camera.get_orthographicSize();
				m_originalCenterPosition = m_cameraContainer.get_position();
				m_isInitialized = true;
			}
			if (m_camera.get_aspect() >= 1.77777779f)
			{
				m_minOrthoSize = m_originalOrthoSize;
			}
			else
			{
				float num = 1.77777779f * m_originalOrthoSize;
				m_minOrthoSize = num / m_camera.get_aspect();
			}
			m_minOrthoSize = Mathf.Min(m_minOrthoSize, 10f);
			float dpi = Device.dpi;
			if (dpi <= 0f)
			{
				m_maxOrthoSize = 3.6f;
			}
			else
			{
				float num2 = 1.5625f * dpi;
				m_maxOrthoSize = Mathf.Clamp((float)m_camera.get_pixelHeight() / num2 / 2f, 2.5f, 10f);
			}
			m_maxOrthoSize = Mathf.Max(m_maxOrthoSize, 3.6f);
			float num3 = m_minOrthoSize * 2f;
			float num4 = num3 * m_camera.get_aspect();
			m_clampRect = new Rect((0f - num4) / 2f, (0f - num3) / 2f, num4, num3);
			m_camera.set_orthographicSize(Mathf.Lerp(m_minOrthoSize, m_maxOrthoSize, m_zoomLevel));
			m_characterCameraPosition = m_originalCenterPosition;
			if (m_target != null)
			{
				m_characterCameraPosition = m_target.get_position();
			}
			m_cameraContainer.set_position(Vector3.Lerp(m_originalCenterPosition, m_characterCameraPosition, m_zoomLevel));
			UpdateAudioListenerPosition();
		}

		private void Update()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			if (m_isInitialized && !(m_target == null))
			{
				float num = Mathf.Lerp(m_unZoomDamp, m_damp, m_zoomLevel);
				m_characterCameraPosition = Vector3.SmoothDamp(m_characterCameraPosition, m_target.get_position(), ref m_dampVelocity, num);
				Vector3 val = Vector3.Lerp(m_originalCenterPosition, m_characterCameraPosition, m_minZoomLerp);
				m_cameraContainer.set_position(Vector3.Lerp(val, m_characterCameraPosition, m_zoomLevel));
			}
		}

		private unsafe Vector3 ProjectOnPlane(Vector3 worldPos)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			float y = ((IntPtr)(void*)m_cameraContainer.get_transform().get_position()).y;
			Plane val = default(Plane);
			val._002Ector(Vector3.get_up(), y);
			Ray val2 = default(Ray);
			val2._002Ector(worldPos, m_camera.get_transform().get_forward());
			float num = default(float);
			if (!val.Raycast(val2, ref num))
			{
				return Vector3.get_zero();
			}
			return val2.GetPoint(num);
		}

		private unsafe Vector3 ClampWorldPositionToViewRect(Vector3 worldPos, Rect viewRect)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = m_camera.get_transform().InverseTransformPoint(worldPos);
			val.z = 0f;
			if (!Intersect(Vector2.op_Implicit(val), viewRect, out Vector2 intersection))
			{
				return worldPos;
			}
			Plane val2 = default(Plane);
			val2._002Ector(Vector3.get_up(), ((IntPtr)(void*)worldPos).y);
			Vector3 val3 = m_camera.get_transform().TransformPoint(new Vector3(((IntPtr)(void*)intersection).x, ((IntPtr)(void*)intersection).y, 0f));
			Ray val4 = default(Ray);
			val4._002Ector(val3, m_camera.get_transform().get_forward());
			float num = default(float);
			if (!val2.Raycast(val4, ref num))
			{
				return worldPos;
			}
			return val4.GetPoint(num);
		}

		private unsafe static bool Intersect(Vector2 a, Rect rect, out Vector2 intersection)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			intersection = Vector2.get_zero();
			if (rect.get_width() <= 0f || rect.get_height() <= 0f)
			{
				intersection = rect.get_center();
				return true;
			}
			if (((IntPtr)(void*)a).x > rect.get_xMin() && ((IntPtr)(void*)a).x < rect.get_xMax() && ((IntPtr)(void*)a).y > rect.get_yMin() && ((IntPtr)(void*)a).y < rect.get_yMax())
			{
				return false;
			}
			Vector2 center = rect.get_center();
			Vector2 val = default(Vector2);
			val._002Ector(rect.get_xMin(), rect.get_yMax());
			Vector2 val2 = default(Vector2);
			val2._002Ector(rect.get_xMax(), rect.get_yMax());
			Vector2 val3 = default(Vector2);
			val3._002Ector(rect.get_xMin(), rect.get_yMin());
			Vector2 val4 = default(Vector2);
			val4._002Ector(rect.get_xMax(), rect.get_yMin());
			if (IntersectsSegment(center, a, val, val2, out intersection))
			{
				return true;
			}
			if (IntersectsSegment(center, a, val2, val4, out intersection))
			{
				return true;
			}
			if (IntersectsSegment(center, a, val4, val3, out intersection))
			{
				return true;
			}
			if (IntersectsSegment(center, a, val3, val, out intersection))
			{
				return true;
			}
			return false;
		}

		private unsafe static bool IntersectsSegment(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			intersection = Vector2.get_zero();
			Vector2 val = a2 - a1;
			Vector2 val2 = b2 - b1;
			float num = ((IntPtr)(void*)val).x * ((IntPtr)(void*)val2).y - ((IntPtr)(void*)val).y * ((IntPtr)(void*)val2).x;
			if (num == 0f)
			{
				return false;
			}
			Vector2 val3 = b1 - a1;
			float num2 = (((IntPtr)(void*)val3).x * ((IntPtr)(void*)val2).y - ((IntPtr)(void*)val3).y * ((IntPtr)(void*)val2).x) / num;
			if (num2 < 0f || num2 > 1f)
			{
				return false;
			}
			float num3 = (((IntPtr)(void*)val3).x * ((IntPtr)(void*)val).y - ((IntPtr)(void*)val3).y * ((IntPtr)(void*)val).x) / num;
			if (num3 < 0f || num3 > 1f)
			{
				return false;
			}
			intersection = a1 + num2 * val;
			return true;
		}

		private unsafe void UpdateViewRects()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = m_camera.get_transform().InverseTransformPoint(m_originalCenterPosition);
			m_viewClampRect = m_clampRect;
			m_viewClampRect.set_x(m_viewClampRect.get_x() + ((IntPtr)(void*)val).x);
			m_viewClampRect.set_y(m_viewClampRect.get_y() + ((IntPtr)(void*)val).y);
			float num = m_camera.get_orthographicSize() * 2f;
			float num2 = num * m_camera.get_aspect();
			m_viewRect = new Rect((0f - num2) / 2f, (0f - num) / 2f, num2, num);
			Vector2 val2 = Vector2.Max(Vector2.get_zero(), m_viewClampRect.get_size() - m_viewRect.get_size());
			m_viewMovableAreaRect = new Rect(m_viewClampRect.get_center() - val2 / 2f, val2);
		}

		private unsafe bool IsOutsideRect(Vector3 pos, Rect clampViewRect, out Vector3 delta)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			bool result = false;
			delta = Vector3.get_zero();
			if (((IntPtr)(void*)pos).x < clampViewRect.get_xMin())
			{
				delta.x += ((IntPtr)(void*)pos).x - clampViewRect.get_xMin();
				result = true;
			}
			if (((IntPtr)(void*)pos).x > clampViewRect.get_xMax())
			{
				delta.x += ((IntPtr)(void*)pos).x - clampViewRect.get_xMax();
				result = true;
			}
			if (((IntPtr)(void*)pos).y < clampViewRect.get_yMin())
			{
				delta.y += ((IntPtr)(void*)pos).y - clampViewRect.get_yMin();
				result = true;
			}
			if (((IntPtr)(void*)pos).y > clampViewRect.get_yMax())
			{
				delta.y += ((IntPtr)(void*)pos).y - clampViewRect.get_yMax();
				result = true;
			}
			return result;
		}

		private Rect ViewportToViewSpace(Rect rect, float orthoSize, float aspect)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			Rect result = default(Rect);
			result.set_yMax(2f * orthoSize * (1f - rect.get_yMin() - 0.5f));
			result.set_yMin(2f * orthoSize * (1f - rect.get_yMax() - 0.5f));
			result.set_xMin(2f * orthoSize * aspect * (rect.get_xMin() - 0.5f));
			result.set_xMax(2f * orthoSize * aspect * (rect.get_xMax() - 0.5f));
			return result;
		}

		public unsafe void TweenZoom(float scrollDelta)
		{
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			if (!m_allowInteraction)
			{
				return;
			}
			float num = Mathf.Sign(scrollDelta) * m_zoomIncrement;
			float num2 = Mathf.Clamp01(m_targetZoomLevel + num);
			if (!(Mathf.Abs(num2 - m_targetZoomLevel) < float.Epsilon))
			{
				float num3 = Mathf.Lerp(0f, m_zoomTweenDuration, (num2 - m_zoomLevel) / num);
				m_targetZoomLevel = num2;
				if (m_zoomTween != null && TweenExtensions.IsPlaying(m_zoomTween))
				{
					m_zoomTween.ChangeEndValue((object)num2, num3, true);
				}
				else
				{
					m_zoomTween = TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), num2, num3), m_zoomEase);
				}
			}
		}

		private float ZoomTweenGetter()
		{
			return m_zoomLevel;
		}

		private void ZoomTweenSetter(float value)
		{
			m_zoomLevel = value;
			m_camera.set_orthographicSize(Mathf.LerpUnclamped(m_minOrthoSize, m_maxOrthoSize, m_zoomLevel));
			UpdateAudioListenerPosition();
		}

		private void OnDidApplyAnimationProperties()
		{
			float num = Mathf.Max(m_animStartOrthoSize, m_minOrthoSize);
			float num2 = Mathf.Lerp(m_minOrthoSize, m_maxOrthoSize, m_animEndZoomLevel);
			float num3 = Mathf.Lerp(num, num2, m_animZoomFactor);
			m_zoomLevel = Mathf.Clamp01(Mathf.InverseLerp(m_minOrthoSize, m_maxOrthoSize, num3));
			m_targetZoomLevel = m_zoomLevel;
			m_camera.set_orthographicSize(num3);
			UpdateAudioListenerPosition();
		}

		private void UpdateAudioListenerPosition()
		{
			m_audioListener.UpdatePosition(m_zoomLevel);
		}

		private void OnScreenStateChange()
		{
			Setup();
		}

		private void OnEnable()
		{
			m_targetZoomLevel = m_zoomLevel;
			Setup();
			Device.ScreenStateChanged += OnScreenStateChange;
		}

		private void OnDisable()
		{
			if (m_zoomTween != null)
			{
				TweenExtensions.Kill(m_zoomTween, false);
				m_zoomTween = null;
			}
			Device.ScreenStateChanged -= OnScreenStateChange;
		}

		public MapCameraHandler()
			: this()
		{
		}//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)

	}
}
