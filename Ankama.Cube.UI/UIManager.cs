using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.SRP;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.UI
{
	public class UIManager : MonoBehaviour
	{
		public class Node
		{
			public List<Node> children = new List<Node>();

			public StateContext context;

			public Node parent;

			public bool renderBeforeParent;

			public int rootIndex = -1;

			public List<AbstractUI> uis = new List<AbstractUI>();
		}

		private class RootNodeComparer : IComparer<Node>
		{
			public int Compare(Node x, Node y)
			{
				return x.rootIndex - y.rootIndex;
			}
		}

		private struct UserInteractionLockSettings
		{
			public readonly float warningTimeout;

			public readonly float errorTimeout;

			public readonly float maxTimeout;

			public readonly Action warningTimeoutCallback;

			public readonly Action errorTimeoutCallback;

			public bool hasAnyCallback
			{
				get
				{
					if (warningTimeoutCallback != null)
					{
						return errorTimeoutCallback != null;
					}
					return false;
				}
			}

			public UserInteractionLockSettings(float warningTimeout, float errorTimeout, Action warningTimeoutCallback, Action errorTimeoutCallback)
			{
				this.warningTimeout = warningTimeout;
				this.errorTimeout = errorTimeout;
				maxTimeout = Mathf.Max(warningTimeout, errorTimeout);
				this.warningTimeoutCallback = warningTimeoutCallback;
				this.errorTimeoutCallback = errorTimeoutCallback;
			}
		}

		[SerializeField]
		private Camera m_cameraPrefab;

		[SerializeField]
		private int m_defaultUiDepth = 10;

		[SerializeField]
		private int m_defaultUiStep = 2;

		private readonly RootNodeComparer m_rootNodeComparer = new RootNodeComparer();

		private readonly List<Node> m_rootNodes = new List<Node>();

		private readonly Dictionary<StateContext, Node> m_stateContextToNode = new Dictionary<StateContext, Node>();

		private readonly Dictionary<AbstractUI, Node> m_uiToNode = new Dictionary<AbstractUI, Node>();

		private bool m_updateOrder;

		private UICameraPool m_cameraPool;

		private bool m_isBlurActive;

		private bool m_userInteractionLocked;

		private float m_userInteractionLockTimer;

		private Coroutine m_userInteractionLockCheckRoutine;

		private UserInteractionLockSettings m_userInteractionLockSettings;

		public static UIManager instance
		{
			get;
			private set;
		}

		public float lastDepth
		{
			get;
			private set;
		}

		public int lastSortingOrder
		{
			get;
			private set;
		}

		[PublicAPI]
		public bool userInteractionLocked => m_userInteractionLocked;

		private void Awake()
		{
			instance = this;
			Object.DontDestroyOnLoad(this.get_gameObject());
			m_cameraPool = new UICameraPool(m_cameraPrefab);
			m_cameraPrefab.get_gameObject().SetActive(false);
			UpdateBlurActiveState();
			QualityManager.onChanged = (Action<QualityAsset>)Delegate.Combine(QualityManager.onChanged, new Action<QualityAsset>(OnQualityChanged));
		}

		private void OnDisable()
		{
			m_cameraPool.ReleaseAll();
			m_userInteractionLockCheckRoutine = null;
		}

		private void OnDestroy()
		{
			QualityManager.onChanged = (Action<QualityAsset>)Delegate.Remove(QualityManager.onChanged, new Action<QualityAsset>(OnQualityChanged));
			if (instance == this)
			{
				instance = null;
			}
		}

		public void NotifyLayerIndexChange()
		{
			int count = m_rootNodes.Count;
			for (int i = 0; i < count; i++)
			{
				Node node = m_rootNodes[i];
				StateLayer layer = node.context.GetLayer();
				if (layer != null)
				{
					node.rootIndex = layer.get_index();
				}
			}
			m_rootNodes.Sort(m_rootNodeComparer);
			m_updateOrder = true;
		}

		public void NotifyUIDepthChanged(AbstractUI ui, StateContext state, int index = -1)
		{
			if (m_uiToNode.TryGetValue(ui, out Node value))
			{
				value.uis.Remove(ui);
				m_uiToNode.Remove(ui);
			}
			else
			{
				ui.canvas.set_worldCamera(m_cameraPrefab);
			}
			Node orCreateNode = GetOrCreateNode(state);
			if (index == -1)
			{
				orCreateNode.uis.Add(ui);
			}
			else
			{
				orCreateNode.uis.Insert(index, ui);
			}
			m_uiToNode.Add(ui, orCreateNode);
			if (ui.get_gameObject().get_activeInHierarchy())
			{
				m_updateOrder = true;
			}
		}

		[PublicAPI]
		public UICamera GetCamera()
		{
			return m_cameraPool.busyCameras[0];
		}

		public void EnableStateChanged(AbstractUI ui, bool enable)
		{
			if (m_uiToNode.ContainsKey(ui))
			{
				m_updateOrder = true;
			}
		}

		public void UseBlurChanged(AbstractUI ui, bool enable)
		{
			if (m_isBlurActive && ui.get_gameObject().get_activeInHierarchy() && m_uiToNode.ContainsKey(ui))
			{
				m_updateOrder = true;
			}
		}

		public void NotifyUIDestroyed(AbstractUI ui)
		{
			if (!m_uiToNode.TryGetValue(ui, out Node value))
			{
				return;
			}
			value.uis.Remove(ui);
			m_uiToNode.Remove(ui);
			if (CanBeRemoved(value))
			{
				Node parent = value.parent;
				while (parent != null && parent.children.Count == 1 && parent.uis.Count == 0)
				{
					value = parent;
					parent = value.parent;
				}
				RemoveNode(value);
			}
			m_updateOrder = true;
		}

		private Node GetOrCreateNode(StateContext state)
		{
			if (!m_stateContextToNode.TryGetValue(state, out Node value))
			{
				IStateUIChildPriority stateUIChildPriority;
				value = new Node
				{
					context = state,
					renderBeforeParent = ((stateUIChildPriority = (state as IStateUIChildPriority)) != null && stateUIChildPriority.uiChildPriority == UIPriority.Back)
				};
				m_stateContextToNode.Add(state, value);
				StateContext parent = state.get_parent();
				if (parent != null)
				{
					Node orCreateNode = GetOrCreateNode(parent);
					IStateUITransitionPriority stateUITransitionPriority;
					if ((stateUITransitionPriority = (state as IStateUITransitionPriority)) != null && stateUITransitionPriority.uiTransitionPriority == UIPriority.Back)
					{
						orCreateNode.children.Insert(0, value);
					}
					else
					{
						orCreateNode.children.Add(value);
					}
					value.parent = orCreateNode;
				}
				else
				{
					StateLayer val;
					if ((val = (state as StateLayer)) != null)
					{
						value.rootIndex = val.get_index();
					}
					m_rootNodes.Add(value);
					m_rootNodes.Sort(m_rootNodeComparer);
				}
			}
			return value;
		}

		private static bool CanBeRemoved(Node node)
		{
			bool flag = true;
			List<Node> children = node.children;
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				Node node2 = children[i];
				flag &= CanBeRemoved(node2);
			}
			if (flag)
			{
				return node.uis.Count == 0;
			}
			return false;
		}

		private void RemoveNode(Node node)
		{
			List<Node> children = node.children;
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				Node node2 = children[i];
				RemoveNode(node2);
			}
			m_stateContextToNode.Remove(node.context);
			Node parent = node.parent;
			if (parent == null)
			{
				m_rootNodes.Remove(node);
			}
			else
			{
				parent.children.Remove(node);
			}
		}

		private void LateUpdate()
		{
			if (m_updateOrder)
			{
				m_updateOrder = false;
				UpdateOrder();
			}
		}

		private void UpdateOrder()
		{
			m_cameraPool.ReleaseAll();
			if (m_uiToNode.Count == 0)
			{
				return;
			}
			int sortingOrder = 0;
			UICamera uiCamera = null;
			int count = m_rootNodes.Count;
			for (int i = 0; i < count; i++)
			{
				Node node = m_rootNodes[i];
				SetUINodeOrder(node, ref uiCamera, ref sortingOrder);
			}
			bool flag = false;
			float num = 0f;
			for (int num2 = m_cameraPool.busyCameras.Count - 1; num2 >= 0; num2--)
			{
				UICamera uICamera = m_cameraPool.busyCameras[num2];
				uICamera.camera.set_nearClipPlane(num);
				num += (float)m_defaultUiStep;
				for (int num3 = uICamera.uis.Count - 1; num3 >= 0; num3--)
				{
					AbstractUI abstractUI = uICamera.uis[num3];
					num += (float)m_defaultUiDepth;
					abstractUI.canvas.set_planeDistance(num);
				}
				num += (float)m_defaultUiStep;
				uICamera.camera.set_farClipPlane(num);
				if (uICamera.hasBlur)
				{
					if (flag)
					{
						uICamera.NeedToDisplayBlur(value: false);
					}
					else
					{
						uICamera.NeedToDisplayBlur(value: true);
						flag = uICamera.isFullBlur;
					}
				}
			}
			lastSortingOrder = sortingOrder;
			lastDepth = num;
		}

		private void SetUINodeOrder(Node node, ref UICamera uiCamera, ref int sortingOrder)
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			List<Node> children = node.children;
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				Node node2 = children[i];
				if (node2.renderBeforeParent)
				{
					SetUINodeOrder(node2, ref uiCamera, ref sortingOrder);
				}
			}
			List<AbstractUI> uis = node.uis;
			int count2 = uis.Count;
			for (int j = 0; j < count2; j++)
			{
				AbstractUI abstractUI = uis[j];
				if (!abstractUI.get_gameObject().get_activeInHierarchy())
				{
					continue;
				}
				if ((int)abstractUI.canvas.get_renderMode() == 0)
				{
					abstractUI.canvas.set_sortingOrder(sortingOrder);
					sortingOrder++;
					continue;
				}
				if (m_isBlurActive && abstractUI.useBlur)
				{
					UICamera child = uiCamera;
					uiCamera = m_cameraPool.Get();
					uiCamera.ActiveBlurFor(abstractUI);
					uiCamera.child = child;
				}
				if (uiCamera == null)
				{
					uiCamera = m_cameraPool.Get();
				}
				uiCamera.uis.Add(abstractUI);
				abstractUI.canvas.set_worldCamera(uiCamera.camera);
			}
			for (int k = 0; k < count; k++)
			{
				Node node3 = children[k];
				if (!node3.renderBeforeParent)
				{
					SetUINodeOrder(node3, ref uiCamera, ref sortingOrder);
				}
			}
		}

		private void OnQualityChanged(QualityAsset current)
		{
			UpdateBlurActiveState();
		}

		private void UpdateBlurActiveState()
		{
			bool activeBlurState = GetActiveBlurState();
			if (m_isBlurActive != activeBlurState)
			{
				m_isBlurActive = activeBlurState;
				if (m_uiToNode.Count > 0)
				{
					m_updateOrder = true;
				}
			}
		}

		private bool GetActiveBlurState()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (!SystemInfo.get_supportsImageEffects())
			{
				return false;
			}
			if ((int)QualityManager.get_current().get_uiBlurQuality() == 0)
			{
				return false;
			}
			return true;
		}

		public void LockUserInteraction(float warningTimeout = 1f, float errorTimeout = 8f, Action warningTimeoutCallback = null, Action errorTimeoutCallback = null)
		{
			if (m_userInteractionLocked)
			{
				Log.Warning("User Interaction is being locked but it was already acquired.", 489, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\UIManager.cs");
			}
			m_userInteractionLockSettings = new UserInteractionLockSettings(warningTimeout, errorTimeout, warningTimeoutCallback, errorTimeoutCallback);
			m_userInteractionLockTimer = 0f;
			m_userInteractionLocked = true;
			if (m_userInteractionLockCheckRoutine != null)
			{
				this.StopCoroutine(m_userInteractionLockCheckRoutine);
				m_userInteractionLockCheckRoutine = null;
			}
			if (m_userInteractionLockSettings.hasAnyCallback)
			{
				m_userInteractionLockCheckRoutine = this.StartCoroutine(UserInteractionTimeoutCheckRoutine());
			}
		}

		public void ReleaseUserInteractionLock()
		{
			if (!m_userInteractionLocked)
			{
				Log.Warning("User Interaction Lock is being released but it was not acquired.", 512, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\UIManager.cs");
			}
			m_userInteractionLockSettings = default(UserInteractionLockSettings);
			m_userInteractionLocked = false;
		}

		private IEnumerator UserInteractionTimeoutCheckRoutine()
		{
			UserInteractionLockSettings settings = m_userInteractionLockSettings;
			do
			{
				yield return null;
				float userInteractionLockTimer = m_userInteractionLockTimer;
				m_userInteractionLockTimer += Time.get_unscaledDeltaTime();
				Action warningTimeoutCallback = settings.warningTimeoutCallback;
				if (warningTimeoutCallback != null)
				{
					float warningTimeout = settings.warningTimeout;
					if (m_userInteractionLockTimer >= warningTimeout && userInteractionLockTimer < warningTimeout)
					{
						warningTimeoutCallback();
					}
				}
				Action errorTimeoutCallback = settings.errorTimeoutCallback;
				if (errorTimeoutCallback != null)
				{
					float errorTimeout = settings.errorTimeout;
					if (m_userInteractionLockTimer >= errorTimeout && userInteractionLockTimer < errorTimeout)
					{
						errorTimeoutCallback();
					}
				}
			}
			while (m_userInteractionLockTimer < settings.maxTimeout);
			m_userInteractionLocked = false;
			m_userInteractionLockCheckRoutine = null;
		}

		public UIManager()
			: this()
		{
		}
	}
}
