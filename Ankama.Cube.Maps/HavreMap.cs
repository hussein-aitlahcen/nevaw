using Ankama.Cube.Audio;
using Ankama.Cube.UI;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class HavreMap : MonoBehaviour
	{
		[Header("Map Information")]
		[SerializeField]
		private ZaapObject m_pvpZaap;

		[SerializeField]
		private ZaapGodObject m_godZaap;

		[SerializeField]
		private MapPathfindingActor m_mapCharacterObject;

		[SerializeField]
		private MapData m_mapData;

		[SerializeField]
		private Color m_ambientColor;

		[Header("Camera")]
		[SerializeField]
		private MapCameraHandler m_cameraHandler;

		[Header("Audio")]
		[SerializeField]
		private AudioEventGroup m_musicGroup;

		[SerializeField]
		private AudioEventGroup m_ambianceGroup;

		public Action onPvPTrigger;

		public Action onGodTrigger;

		private readonly MapAudioContext m_audioContext = new MapAudioContext();

		private AudioWorldMusicRequest m_worldMusicRequest;

		private List<Vector3> m_path = new List<Vector3>();

		private MapQuadTreePathfinding m_quadTreePathFinding = new MapQuadTreePathfinding();

		private bool m_interactable = true;

		public MapPathfindingActor character => m_mapCharacterObject;

		public ZaapGodObject godZaap => m_godZaap;

		private void Awake()
		{
			ZaapObject pvpZaap = m_pvpZaap;
			pvpZaap.onClick = (Action<ZaapObject>)Delegate.Combine(pvpZaap.onClick, new Action<ZaapObject>(OnZaapClick));
			ZaapObject pvpZaap2 = m_pvpZaap;
			pvpZaap2.onPortalBeginOpen = (Action<ZaapObject>)Delegate.Combine(pvpZaap2.onPortalBeginOpen, new Action<ZaapObject>(OnZaapBeginOpen));
			ZaapObject pvpZaap3 = m_pvpZaap;
			pvpZaap3.onPortalEndOpen = (Action<ZaapObject>)Delegate.Combine(pvpZaap3.onPortalEndOpen, new Action<ZaapObject>(OnZaapEndOpen));
			ZaapGodObject godZaap = m_godZaap;
			godZaap.onClick = (Action<ZaapObject>)Delegate.Combine(godZaap.onClick, new Action<ZaapObject>(OnZaapClick));
			ZaapGodObject godZaap2 = m_godZaap;
			godZaap2.onPortalBeginOpen = (Action<ZaapObject>)Delegate.Combine(godZaap2.onPortalBeginOpen, new Action<ZaapObject>(OnZaapBeginOpen));
			ZaapGodObject godZaap3 = m_godZaap;
			godZaap3.onPortalEndOpen = (Action<ZaapObject>)Delegate.Combine(godZaap3.onPortalEndOpen, new Action<ZaapObject>(OnZaapEndOpen));
		}

		public IEnumerator Initialize()
		{
			RenderSettings.set_ambientLight(m_ambientColor);
			m_cameraHandler.Initialize(m_mapData, m_mapCharacterObject.get_transform());
			if (AudioManager.isReady)
			{
				m_audioContext.Initialize();
				m_worldMusicRequest = AudioManager.LoadWorldMusic(m_musicGroup, m_ambianceGroup, m_audioContext);
				while (m_worldMusicRequest.state == AudioWorldMusicRequest.State.Loading)
				{
					yield return null;
				}
			}
		}

		public void InitEnterAnimFirstFrame()
		{
			m_cameraHandler.InitEnterAnimFirstFrame();
		}

		public IEnumerator PlayEnerAnim()
		{
			yield return m_cameraHandler.PlayEnterAnim();
		}

		public void Begin()
		{
			if (m_worldMusicRequest != null)
			{
				AudioManager.StartWorldMusic(m_worldMusicRequest);
			}
		}

		public void Release()
		{
			if (AudioManager.isReady)
			{
				m_audioContext.Release();
				if (m_worldMusicRequest != null)
				{
					AudioManager.StopWorldMusic(m_worldMusicRequest);
				}
			}
			m_worldMusicRequest = null;
		}

		public void SetInteractable(bool value)
		{
			m_interactable = value;
			m_pvpZaap.interactable = value;
			m_godZaap.interactable = value;
		}

		public void MoveCharacterOutsideZaap()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			ZaapObject zaapObject = m_pvpZaap;
			if (m_godZaap.state == ZaapObject.ZaapState.Open)
			{
				zaapObject = m_godZaap;
			}
			zaapObject.ClosePortal();
			MapData mapFromWorldPos = MapData.GetMapFromWorldPos(m_mapCharacterObject.get_transform().get_position());
			if (mapFromWorldPos == null)
			{
				Log.Error("Actor is not on MapData", 135, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\HavreMap\\HavreMap.cs");
			}
			else if (m_quadTreePathFinding.FindPath(mapFromWorldPos, m_mapCharacterObject.get_transform().get_position(), zaapObject.outsideDestination, m_path))
			{
				m_mapCharacterObject.FollowPath(m_path);
			}
		}

		private void OnZaapClick(ZaapObject zaap)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			if (!m_interactable)
			{
				return;
			}
			MapData mapFromWorldPos = MapData.GetMapFromWorldPos(m_mapCharacterObject.get_transform().get_position());
			if (mapFromWorldPos == null)
			{
				Log.Error("Actor is not on MapData", 153, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\HavreMap\\HavreMap.cs");
				return;
			}
			if (m_quadTreePathFinding.FindPath(mapFromWorldPos, m_mapCharacterObject.get_transform().get_position(), zaap.destination, m_path))
			{
				m_mapCharacterObject.FollowPath(m_path, zaap.destinationLookAt);
			}
			if (m_pvpZaap == zaap)
			{
				m_godZaap.OnClickOutside();
			}
			else
			{
				m_pvpZaap.OnClickOutside();
			}
		}

		private void OnZaapBeginOpen(ZaapObject zaap)
		{
			SetInteractable(value: false);
		}

		private void OnZaapEndOpen(ZaapObject zaap)
		{
			if (m_pvpZaap == zaap)
			{
				onPvPTrigger?.Invoke();
			}
			else
			{
				onGodTrigger?.Invoke();
			}
		}

		private unsafe void Update()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			m_pvpZaap.UpdateCharacterPos(m_mapCharacterObject.get_transform().get_position());
			m_godZaap.UpdateCharacterPos(m_mapCharacterObject.get_transform().get_position());
			if (Input.GetMouseButtonDown(0) && !InputUtility.IsMouseOverUI && m_interactable)
			{
				Ray val = Camera.get_main().ScreenPointToRay(Input.get_mousePosition());
				RaycastHit val2 = default(RaycastHit);
				if (Physics.Raycast(val, ref val2))
				{
					GameObject gameObject = val2.get_collider().get_gameObject();
					if (gameObject == m_pvpZaap.get_gameObject() || gameObject == m_godZaap.get_gameObject())
					{
						return;
					}
				}
				m_pvpZaap.OnClickOutside();
				m_godZaap.OnClickOutside();
				MapData mapFromWorldPos = MapData.GetMapFromWorldPos(m_mapCharacterObject.get_transform().get_position());
				if (mapFromWorldPos == null)
				{
					Log.Error("Actor is not on MapData", 210, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\HavreMap\\HavreMap.cs");
					return;
				}
				if (mapFromWorldPos.RayCast(val, out Vector3 hit) && m_quadTreePathFinding.FindPath(mapFromWorldPos, m_mapCharacterObject.get_transform().get_position(), hit, m_path))
				{
					m_mapCharacterObject.FollowPath(m_path);
				}
			}
			Camera camera = m_cameraHandler.camera;
			Vector3 pointerPosition = InputUtility.pointerPosition;
			Rect pixelRect = camera.get_pixelRect();
			if (pixelRect.Contains(pointerPosition) && !InputUtility.IsMouseOverUI)
			{
				float y = ((IntPtr)(void*)Input.get_mouseScrollDelta()).y;
				if (Math.Abs(y) > float.Epsilon)
				{
					m_cameraHandler.TweenZoom(y);
				}
			}
		}

		private void OnDrawGizmos()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			if (!Application.get_isPlaying() || m_path == null)
			{
				return;
			}
			Vector3 val = Vector3.get_up() * 0.01f;
			Gizmos.set_color(Color.get_blue());
			if (m_path != null && m_path.Count > 0)
			{
				Vector3 val2 = m_path[0];
				Gizmos.DrawSphere(val2 + val, 0.05f);
				for (int i = 1; i < m_path.Count; i++)
				{
					Gizmos.DrawSphere(m_path[i] + val, 0.05f);
					Gizmos.DrawLine(val2 + val, m_path[i] + val);
					val2 = m_path[i];
				}
			}
		}

		public HavreMap()
			: this()
		{
		}
	}
}
