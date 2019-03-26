using Ankama.Cube.Data;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Utilities;
using JetBrains.Annotations;

namespace Ankama.Cube.Fight.Entities
{
	public class SpellStatus : ICastableStatus
	{
		public readonly PlayerStatus ownerPlayer;

		public readonly int instanceId;

		public SpellDefinition definition
		{
			get;
			private set;
		}

		public int? baseCost
		{
			get;
			private set;
		}

		public int level
		{
			get;
			private set;
		}

		[CanBeNull]
		public static SpellStatus TryCreate(SpellInfo spellInfo, PlayerStatus ownerStatus)
		{
			int? spellDefinitionId = spellInfo.SpellDefinitionId;
			if (spellDefinitionId.HasValue)
			{
				int value = spellDefinitionId.Value;
				if (!RuntimeData.spellDefinitions.TryGetValue(value, out SpellDefinition value2))
				{
					Log.Error($"Could not find spell definition with id {value}.", 26, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Entities\\Status\\SpellStatus.cs");
					return null;
				}
				int? spellLevel = spellInfo.SpellLevel;
				int level = spellLevel.HasValue ? spellLevel.Value : 0;
				return new SpellStatus(value2, level, ownerStatus, spellInfo.SpellInstanceId);
			}
			return new SpellStatus(ownerStatus, spellInfo.SpellInstanceId);
		}

		public SpellStatus(PlayerStatus ownerPlayer, int instanceId)
		{
			this.ownerPlayer = ownerPlayer;
			this.instanceId = instanceId;
			definition = null;
			level = 0;
			baseCost = null;
		}

		public SpellStatus([NotNull] SpellDefinition definition, int level, PlayerStatus ownerPlayer, int instanceId)
		{
			this.ownerPlayer = ownerPlayer;
			this.instanceId = instanceId;
			this.definition = definition;
			this.level = level;
			baseCost = definition.GetBaseCost(level);
		}

		public void Upgrade(SpellDefinition spellDefinition, int spellLevel)
		{
			definition = spellDefinition;
			level = spellLevel;
			baseCost = definition.GetBaseCost(level);
		}

		public ICastableDefinition GetDefinition()
		{
			return definition;
		}

		public CastTargetContext CreateCastTargetContext()
		{
			return definition.castTarget.CreateCastTargetContext(FightStatus.local, ownerPlayer.id, DynamicValueHolderType.Spell, definition.get_id(), level, instanceId);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}, ", "instanceId", instanceId) + "ownerPlayer: " + ownerPlayer.nickname + ", " + string.Format("{0}: {1}, ", "definition", definition.get_id()) + string.Format("{0}: {1}, ", "level", level);
		}
	}
}
