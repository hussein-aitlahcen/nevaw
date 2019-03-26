using JetBrains.Annotations;

namespace Ankama.Cube.Audio
{
	[PublicAPI]
	public enum AudioBusIdentifier
	{
		[PublicAPI]
		Master,
		[PublicAPI]
		Music,
		[PublicAPI]
		SFX,
		[PublicAPI]
		UI
	}
}
