using Ankama.Cube.SRP;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Utility
{
	public class SceneEventListener : Singleton<SceneEventListener>
	{
		private List<Action> m_callbacks = new List<Action>();

		private SceneEventListenerBehaviour m_listenerGameObject;

		public void AddUpdateListener(Action callback)
		{
			AddListener(callback);
			SceneEventListenerBehaviour listenerGameObject = m_listenerGameObject;
			listenerGameObject.onUpdate = (Action)Delegate.Combine(listenerGameObject.onUpdate, callback);
		}

		public void RemoveUpdateListener(Action callback)
		{
			RemoveListener(callback);
			SceneEventListenerBehaviour listenerGameObject = m_listenerGameObject;
			listenerGameObject.onUpdate = (Action)Delegate.Remove(listenerGameObject.onUpdate, callback);
		}

		public void AddGizmosListener(Action callback)
		{
			AddListener(callback);
			SceneEventListenerBehaviour listenerGameObject = m_listenerGameObject;
			listenerGameObject.onDrawGizmos = (Action)Delegate.Combine(listenerGameObject.onDrawGizmos, callback);
		}

		public void RemoveGizmosListener(Action callback)
		{
			RemoveListener(callback);
			SceneEventListenerBehaviour listenerGameObject = m_listenerGameObject;
			listenerGameObject.onDrawGizmos = (Action)Delegate.Remove(listenerGameObject.onDrawGizmos, callback);
		}

		private void AddListener(Action callback)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			m_callbacks.Add(callback);
			if (m_callbacks.Count == 1)
			{
				m_listenerGameObject = new GameObject("SceneEventListener").AddComponent<SceneEventListenerBehaviour>();
				Object.DontDestroyOnLoad(m_listenerGameObject.get_gameObject());
			}
		}

		private void RemoveListener(Action callback)
		{
			m_callbacks.Remove(callback);
			if (m_callbacks.Count == 0 && m_listenerGameObject != null)
			{
				DestroyUtility.Destroy(m_listenerGameObject.get_gameObject());
			}
		}
	}
}
