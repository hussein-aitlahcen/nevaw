using Ankama.Cube.Animations;
using FMOD.Studio;
using System.Collections.Generic;

namespace Ankama.Cube.Audio
{
	public abstract class AudioContext : ITimelineContext
	{
		private readonly List<EventInstance> m_eventInstances = new List<EventInstance>();

		protected abstract void InitializeEventInstance(EventInstance eventInstance);

		public virtual void Initialize()
		{
			AudioManager.AddAudioContext(this);
		}

		public void AddEventInstance(EventInstance eventInstance)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (eventInstance.isValid())
			{
				InitializeEventInstance(eventInstance);
				m_eventInstances.Add(eventInstance);
			}
		}

		public void SetParameterValue(string name, float value)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			List<EventInstance> eventInstances = m_eventInstances;
			int count = eventInstances.Count;
			for (int i = 0; i < count; i++)
			{
				EventInstance val = eventInstances[i];
				val.setParameterValue(name, value);
			}
		}

		public void Cleanup()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			List<EventInstance> eventInstances = m_eventInstances;
			for (int num = eventInstances.Count - 1; num >= 0; num--)
			{
				EventInstance val = eventInstances[num];
				if (!val.isValid())
				{
					eventInstances.RemoveAt(num);
				}
			}
		}

		public void Release()
		{
			AudioManager.RemoveAudioContext(this);
			m_eventInstances.Clear();
		}
	}
}
