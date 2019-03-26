using Ankama.Cube.Maps;
using System;
using UnityEngine;

namespace Ankama.Cube.Audio
{
	public sealed class AudioListenerPosition : MonoBehaviour
	{
		[SerializeField]
		private float m_minZoomDistance = -8f;

		[SerializeField]
		private float m_maxZoomDistance = 16f;

		private void Start()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			AudioManager.RegisterListenerPosition(this);
			CameraHandler current = CameraHandler.current;
			if (null != current && current.hasZoomRange)
			{
				CameraHandler cameraHandler = current;
				cameraHandler.onZoomChanged = (Action<CameraHandler>)Delegate.Combine(cameraHandler.onZoomChanged, new Action<CameraHandler>(OnZoomChanged));
				OnZoomChanged(current);
			}
			else
			{
				this.get_transform().set_localPosition(new Vector3(0f, 0f, m_minZoomDistance));
			}
		}

		private void Update()
		{
			if (this.get_transform().get_hasChanged())
			{
				AudioManager.UpdateListenerPosition(this);
				this.get_transform().set_hasChanged(false);
			}
		}

		private void OnZoomChanged(CameraHandler cameraHandler)
		{
			UpdatePosition(cameraHandler.zoomLevel);
		}

		public void UpdatePosition(float zoom)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			float num = Mathf.Lerp(m_minZoomDistance, m_maxZoomDistance, zoom);
			this.get_transform().set_localPosition(new Vector3(0f, 0f, num));
		}

		private void OnDestroy()
		{
			AudioManager.UnRegisterListenerPosition(this);
			CameraHandler current = CameraHandler.current;
			if (null != current)
			{
				CameraHandler cameraHandler = current;
				cameraHandler.onZoomChanged = (Action<CameraHandler>)Delegate.Remove(cameraHandler.onZoomChanged, new Action<CameraHandler>(OnZoomChanged));
			}
		}

		public AudioListenerPosition()
			: this()
		{
		}
	}
}
