using JetBrains.Annotations;

namespace Ankama.Cube.Audio.UI
{
	public sealed class AudioEventUITriggerOnEvent : AudioEventUITrigger
	{
		[PublicAPI]
		public void Trigger()
		{
			PlaySound();
		}
	}
}
