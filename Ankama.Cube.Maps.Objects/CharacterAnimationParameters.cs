using Ankama.Cube.Data;

namespace Ankama.Cube.Maps.Objects
{
	public struct CharacterAnimationParameters
	{
		public readonly string animationName;

		public readonly string timelineKey;

		public readonly bool loops;

		public readonly Direction firstDirection;

		public readonly Direction secondDirection;

		public CharacterAnimationParameters(string animationName, string timelineKey, bool loops, Direction firstDirection, Direction secondDirection)
		{
			this.animationName = animationName;
			this.timelineKey = timelineKey;
			this.loops = loops;
			this.firstDirection = firstDirection;
			this.secondDirection = secondDirection;
		}
	}
}
