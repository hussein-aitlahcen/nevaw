using Ankama.Utilities;
using JetBrains.Annotations;
using System;

namespace Ankama.Cube.Configuration
{
	public static class VersionChecker
	{
		public enum Result
		{
			None,
			Success,
			PatchAvailable,
			UpdateNeeded,
			VersionFileError,
			RuntimeError
		}

		public static Result ParseVersionFile([NotNull] string text)
		{
			ApplicationVersion other;
			try
			{
				other = new ApplicationVersion("0.1.0.6045");
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, 125, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\VersionChecker.cs");
				return Result.RuntimeError;
			}
			ApplicationVersion applicationVersion;
			try
			{
				applicationVersion = new ApplicationVersion(text);
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.Message, 136, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\VersionChecker.cs");
				return Result.VersionFileError;
			}
			if (applicationVersion.Matches(other, ApplicationVersion.MatchMask.None))
			{
				return Result.Success;
			}
			if (applicationVersion.Matches(other, ApplicationVersion.MatchMask.Patch))
			{
				return Result.PatchAvailable;
			}
			return Result.UpdateNeeded;
		}
	}
}
