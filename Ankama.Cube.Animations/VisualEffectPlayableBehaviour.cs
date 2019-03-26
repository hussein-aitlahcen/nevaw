using Ankama.Cube.Extensions;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Ankama.Cube.Animations
{
	public sealed class VisualEffectPlayableBehaviour : PlayableBehaviour
	{
		private readonly Transform m_ownerTransform;

		private readonly VisualEffectContext m_visualEffectContext;

		private readonly VisualEffect m_prefab;

		private readonly VisualEffectPlayableAsset.StopMode m_stopMode;

		private readonly VisualEffectPlayableAsset.ParentingMode m_parentingMode;

		private readonly VisualEffectPlayableAsset.OrientationMethod m_orientationMethod;

		private readonly Vector3 m_offset;

		private GameObject m_instance;

		private VisualEffect m_visualEffect;

		private PlayableGraph m_playableGraph;

		[UsedImplicitly]
		public VisualEffectPlayableBehaviour()
			: this()
		{
			throw new NotImplementedException();
		}

		public VisualEffectPlayableBehaviour(GameObject owner, VisualEffectContext context, VisualEffect prefab, VisualEffectPlayableAsset.StopMode stopMode, VisualEffectPlayableAsset.ParentingMode parentingMode, VisualEffectPlayableAsset.OrientationMethod orientationMethod, Vector3 offset)
			: this()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			m_ownerTransform = owner.get_transform();
			m_visualEffectContext = context;
			m_prefab = prefab;
			m_stopMode = stopMode;
			m_parentingMode = parentingMode;
			m_orientationMethod = orientationMethod;
			m_offset = offset;
		}

		public override void OnPlayableDestroy(Playable playable)
		{
			Stop();
		}

		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			Stop();
		}

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			if (!(null == m_prefab) && !(null != m_visualEffect) && !Start())
			{
				Log.Error("Failed to start a VisualEffect, this will leak GameObject every frame.", 141, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Animations\\VisualEffectPlayableBehaviour.cs");
			}
		}

		private bool Start()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			Quaternion rotation = Quaternion.get_identity();
			Vector3 scale = Vector3.get_one();
			Transform val;
			switch (m_parentingMode)
			{
			case VisualEffectPlayableAsset.ParentingMode.Owner:
				val = m_ownerTransform;
				break;
			case VisualEffectPlayableAsset.ParentingMode.Parent:
				val = m_ownerTransform.get_parent();
				break;
			case VisualEffectPlayableAsset.ParentingMode.ContextOwner:
				val = ((m_visualEffectContext != null) ? m_visualEffectContext.transform : m_ownerTransform);
				break;
			case VisualEffectPlayableAsset.ParentingMode.ContextParent:
				val = ((m_visualEffectContext != null) ? m_visualEffectContext.transform.get_parent() : m_ownerTransform.get_parent());
				break;
			case VisualEffectPlayableAsset.ParentingMode.World:
				val = null;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			Vector3 position = (!(null != val)) ? (m_ownerTransform.get_position() + m_offset) : (val.get_position() + m_offset);
			switch (m_orientationMethod)
			{
			case VisualEffectPlayableAsset.OrientationMethod.None:
			{
				CameraHandler current = CameraHandler.current;
				if (null != current)
				{
					rotation = current.mapRotation.GetInverseRotation();
				}
				break;
			}
			case VisualEffectPlayableAsset.OrientationMethod.Context:
				if (m_visualEffectContext != null)
				{
					m_visualEffectContext.GetVisualEffectTransformation(out rotation, out scale);
				}
				break;
			case VisualEffectPlayableAsset.OrientationMethod.Director:
				rotation = m_ownerTransform.get_rotation();
				break;
			case VisualEffectPlayableAsset.OrientationMethod.Transform:
				rotation = ((null != val) ? val.get_rotation() : m_ownerTransform.get_rotation());
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			VisualEffect visualEffect = VisualEffectFactory.Instantiate(m_prefab, position, rotation, scale, val);
			visualEffect.destructionOverride = OnVisualEffectInstanceDestructionRequest;
			if (m_visualEffectContext != null)
			{
				VisualEffectPlayableAsset.ParentingMode parentingMode = m_parentingMode;
				if ((uint)(parentingMode - 2) <= 1u)
				{
					m_visualEffectContext.AddVisualEffect(visualEffect);
				}
			}
			visualEffect.Play();
			m_instance = visualEffect.get_gameObject();
			m_visualEffect = visualEffect;
			return true;
		}

		private void Stop()
		{
			switch (m_stopMode)
			{
			case VisualEffectPlayableAsset.StopMode.None:
			{
				VisualEffect visualEffect = m_visualEffect;
				if (!(null != visualEffect))
				{
					break;
				}
				if (m_visualEffectContext != null)
				{
					VisualEffectPlayableAsset.ParentingMode parentingMode = m_parentingMode;
					if ((uint)(parentingMode - 2) <= 1u)
					{
						m_visualEffectContext.RemoveVisualEffect(visualEffect);
					}
				}
				m_visualEffect = null;
				break;
			}
			case VisualEffectPlayableAsset.StopMode.Stop:
			{
				VisualEffect visualEffect2 = m_visualEffect;
				if (!(null != visualEffect2))
				{
					break;
				}
				visualEffect2.Stop();
				if (m_visualEffectContext != null)
				{
					VisualEffectPlayableAsset.ParentingMode parentingMode = m_parentingMode;
					if ((uint)(parentingMode - 2) <= 1u)
					{
						m_visualEffectContext.RemoveVisualEffect(visualEffect2);
					}
				}
				m_visualEffect = null;
				break;
			}
			case VisualEffectPlayableAsset.StopMode.Destroy:
				if (!(null != m_instance))
				{
					break;
				}
				Object.Destroy(m_instance);
				if (m_visualEffectContext != null)
				{
					VisualEffectPlayableAsset.ParentingMode parentingMode = m_parentingMode;
					if ((uint)(parentingMode - 2) <= 1u)
					{
						m_visualEffectContext.RemoveVisualEffect(m_visualEffect);
					}
				}
				m_visualEffect = null;
				m_instance = null;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void OnVisualEffectInstanceDestructionRequest(VisualEffect instance)
		{
			VisualEffectFactory.Release(m_prefab, instance);
		}
	}
}
