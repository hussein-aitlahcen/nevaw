using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Debug.FightAdminCommands;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Debug
{
	public class DebugFightUI : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_textPanel;

		[SerializeField]
		private Text m_text;

		[Header("Selectors")]
		[SerializeField]
		private DebugDropperSpell m_debugSpellDropper;

		[SerializeField]
		private DebugDropperCreature m_debugCreatureDropper;

		[SerializeField]
		private DebugSelectorProperty m_debugPropertySelector;

		[SerializeField]
		private DebugSelectorElementaryState m_debugElementaryStateSelector;

		[Header("Display info")]
		[SerializeField]
		private RawTextField m_turnCounterText;

		[SerializeField]
		private RawTextField m_timerText;

		private readonly List<AbstractFightAdminCommand> m_adminCommands = new List<AbstractFightAdminCommand>();

		private AbstractFightAdminCommand m_lastRunningCommand;

		private readonly Stopwatch m_stopwatch = new Stopwatch();

		private long m_lastTime;

		private bool m_commandsActivated;

		private void SetCommandsActivated(bool activated)
		{
			m_commandsActivated = activated;
			m_textPanel.SetActive(activated);
		}

		private void Awake()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			SetCommandsActivated(activated: false);
			InitializeAllCommands();
			string text = "";
			foreach (AbstractFightAdminCommand adminCommand in m_adminCommands)
			{
				string text2 = "<b><color=\"green\">" + adminCommand.key + "</color></b>: " + adminCommand.name;
				text = ((!string.IsNullOrEmpty(text)) ? (text + "\n" + text2) : text2);
			}
			m_text.set_text(text);
			m_stopwatch.Start();
			RectTransform component = this.GetComponent<RectTransform>();
			Vector2 zero;
			component.set_offsetMax(zero = Vector2.get_zero());
			component.set_offsetMin(zero);
		}

		private void InitializeAllCommands()
		{
			m_adminCommands.Add(new DrawSpellsCommand(115));
			m_adminCommands.Add(new DiscardSpellsCommand(113));
			m_adminCommands.Add(new GiveActionPointsCommand(97));
			m_adminCommands.Add(new GiveReservePointsCommand(114));
			m_adminCommands.Add(new GiveElementPointsCommand(101));
			m_adminCommands.Add(new KillEntityCommand(107));
			m_adminCommands.Add(new DealDamageCommand(100));
			m_adminCommands.Add(new HealCommand(104));
			m_adminCommands.Add(new TeleportCommand(116));
			m_adminCommands.Add(new PickSpellCommand(292, m_debugSpellDropper));
			m_adminCommands.Add(new InvokeCreatureCommand(105, m_debugCreatureDropper));
			m_adminCommands.Add(new SetPropertyCommand(112, m_debugPropertySelector));
			m_adminCommands.Add(new ApplyElementaryStateCommand(109, m_debugElementaryStateSelector));
			m_adminCommands.Sort((AbstractFightAdminCommand a, AbstractFightAdminCommand b) => string.CompareOrdinal(a.name, b.name));
		}

		private void Update()
		{
			RefreshTimer();
			if (Input.GetKeyDown(293))
			{
				SetCommandsActivated(!m_commandsActivated);
			}
			if (m_commandsActivated && (m_lastRunningCommand == null || !m_lastRunningCommand.Handle()))
			{
				m_lastRunningCommand = null;
				foreach (AbstractFightAdminCommand adminCommand in m_adminCommands)
				{
					if (adminCommand.Handle())
					{
						m_lastRunningCommand = adminCommand;
						break;
					}
				}
			}
		}

		private void RefreshTimer()
		{
			long elapsedMilliseconds = m_stopwatch.ElapsedMilliseconds;
			if (elapsedMilliseconds > m_lastTime + 1000)
			{
				m_lastTime = elapsedMilliseconds;
				m_timerText.SetText(m_stopwatch.Elapsed.ToString("mm\\:ss"));
			}
		}

		public void SetTurnIdex(int turnIndex)
		{
			int num = (turnIndex - 1) / 2 + 1;
			m_turnCounterText.SetText($"Turn: {num}");
		}

		public DebugFightUI()
			: this()
		{
		}
	}
}
