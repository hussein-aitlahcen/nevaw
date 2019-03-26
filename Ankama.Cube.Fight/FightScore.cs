namespace Ankama.Cube.Fight
{
	public struct FightScore
	{
		public struct Score
		{
			public readonly int scoreBefore;

			public readonly int scoreAfter;

			public int delta => scoreAfter - scoreBefore;

			public bool changed => scoreAfter != scoreBefore;

			public Score(int scoreBefore, int scoreAfter)
			{
				this.scoreBefore = scoreBefore;
				this.scoreAfter = scoreAfter;
			}
		}

		public readonly Score myTeamScore;

		public readonly Score opponentTeamScore;

		public FightScore(Score myTeamScore, Score opponentTeamScore)
		{
			this.myTeamScore = myTeamScore;
			this.opponentTeamScore = opponentTeamScore;
		}
	}
}
