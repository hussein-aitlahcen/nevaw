using Ankama.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ankama.Cube.Configuration
{
	public class Options
	{
		private interface ICommandArg
		{
			string usage
			{
				get;
			}

			bool Parse(string arg, ref int index);
		}

		private class CommandArgBool : ICommandArg
		{
			private readonly string m_prefix;

			private readonly string m_longPrefix;

			private readonly Action<bool> m_action;

			public string usage
			{
				get;
			}

			public CommandArgBool(string name, Action<bool> set)
			{
				m_prefix = "--" + name;
				m_longPrefix = "--" + name + "=";
				usage = m_prefix + " | " + m_longPrefix + "true|false";
				m_action = set;
			}

			public bool Parse(string arg, ref int index)
			{
				if (arg == m_prefix)
				{
					m_action(obj: true);
					return true;
				}
				if (StringExtensions.StartsWithFast(arg, m_longPrefix))
				{
					string value = arg.Substring(m_longPrefix.Length);
					m_action(bool.Parse(value));
					return true;
				}
				return false;
			}
		}

		private class CommandArgString : ICommandArg
		{
			private readonly string m_prefix;

			private readonly Action<string> m_action;

			public string usage
			{
				get;
			}

			public CommandArgString(string name, Action<string> set, string possibleValue)
			{
				m_prefix = "--" + name + "=";
				usage = m_prefix + possibleValue;
				m_action = set;
			}

			public bool Parse(string arg, ref int index)
			{
				if (StringExtensions.StartsWithFast(arg, m_prefix))
				{
					m_action(arg.Substring(m_prefix.Length));
					return true;
				}
				return false;
			}
		}

		private readonly List<ICommandArg> m_commands = new List<ICommandArg>();

		private readonly StringBuilder m_stringBuilder = new StringBuilder();

		public void Register(string name, Action<bool> set)
		{
			m_commands.Add(new CommandArgBool(name, set));
		}

		public void Register(string name, Action<string> set, string possibleValue)
		{
			m_commands.Add(new CommandArgString(name, set, possibleValue));
		}

		public bool ParseArgument(string arg, ref int index)
		{
			foreach (ICommandArg command in m_commands)
			{
				if (command.Parse(arg, ref index))
				{
					return true;
				}
			}
			if (arg.StartsWith("--"))
			{
				Log.Warning("Argument not handle " + arg + ".", 377, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationConfig.cs");
			}
			return false;
		}

		public string Usage()
		{
			m_stringBuilder.Clear();
			for (int i = 0; i < m_commands.Count; i++)
			{
				m_stringBuilder.AppendLine(m_commands[i].usage);
			}
			return m_stringBuilder.ToString();
		}
	}
}
