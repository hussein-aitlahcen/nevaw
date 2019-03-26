using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.States;

namespace Ankama.Cube.Data
{
	public abstract class CastTargetContext : DynamicValueFightContext
	{
		public readonly int spellDefinitionId;

		public readonly int expectedTargetCount;

		private readonly Target[] m_targets;

		public readonly int instanceId;

		public int selectedTargetCount
		{
			get;
			private set;
		}

		protected CastTargetContext(FightStatus fightStatus, int playerId, DynamicValueHolderType type, int spellDefinitionId, int level, int instanceId, int expectedTargetCount)
			: base(fightStatus, playerId, type, level)
		{
			this.spellDefinitionId = spellDefinitionId;
			this.expectedTargetCount = expectedTargetCount;
			this.instanceId = instanceId;
			selectedTargetCount = 0;
			m_targets = new Target[expectedTargetCount];
		}

		public Target GetTarget(int index)
		{
			return m_targets[index];
		}

		public void SelectTarget(Target target)
		{
			m_targets[selectedTargetCount] = target;
			int num = ++selectedTargetCount;
		}

		public void SendCommand()
		{
			FightState.instance?.frame.SendSpell(instanceId, m_targets);
		}
	}
}
