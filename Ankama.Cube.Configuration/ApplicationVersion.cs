using System;

namespace Ankama.Cube.Configuration
{
	public struct ApplicationVersion
	{
		public enum MatchMask
		{
			None,
			Patch
		}

		public readonly int major;

		public readonly int minor;

		public readonly int patch;

		public readonly int build;

		public readonly string label;

		public string majorMinorPatch => $"{major}.{minor}.{patch}";

		public ApplicationVersion(string version)
		{
			string[] array = version.Split(new char[1]
			{
				'-'
			});
			if (array.Length == 0 || array.Length > 2)
			{
				throw new ArgumentException("'" + version + "' is not a valid version number.");
			}
			label = ((array.Length < 2) ? string.Empty : array[1]);
			string[] array2 = array[0].Split(new char[1]
			{
				'.'
			});
			if (array2.Length > 4 || array2.Length == 0)
			{
				throw new ArgumentException("'" + version + "' is not a valid version number.");
			}
			if (!int.TryParse(array2[0], out major))
			{
				throw new ArgumentException("'" + version + "' is not a valid version number.");
			}
			if (array2.Length > 1)
			{
				if (!int.TryParse(array2[1], out minor))
				{
					throw new ArgumentException("'" + version + "' is not a valid version number.");
				}
			}
			else
			{
				minor = 0;
			}
			if (array2.Length > 2)
			{
				if (!int.TryParse(array2[2], out patch))
				{
					throw new ArgumentException("'" + version + "' is not a valid version number.");
				}
			}
			else
			{
				patch = 0;
			}
			if (array2.Length > 3)
			{
				if (!int.TryParse(array2[3], out build))
				{
					throw new ArgumentException("'" + version + "' is not a valid version number.");
				}
			}
			else
			{
				build = 0;
			}
		}

		public bool Matches(ApplicationVersion other, MatchMask mask)
		{
			switch (mask)
			{
			case MatchMask.None:
				if (major == other.major && minor == other.minor)
				{
					return patch == other.patch;
				}
				return false;
			case MatchMask.Patch:
				if (major == other.major)
				{
					return minor == other.minor;
				}
				return false;
			default:
				throw new ArgumentOutOfRangeException("mask", mask, null);
			}
		}
	}
}
