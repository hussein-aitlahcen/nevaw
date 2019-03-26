using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum AiThinkingMode
	{
		Play,
		SkipTurn,
		WaitEndOfTurn,
		PlayAsHuman
	}
}
