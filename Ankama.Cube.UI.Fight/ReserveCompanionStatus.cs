using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightCommonProtocol;

namespace Ankama.Cube.UI.Fight
{
	public class ReserveCompanionStatus : ICastableStatus
	{
		public readonly CompanionDefinition definition;

		public readonly PlayerStatus ownerPlayer;

		private PlayerStatus m_currentPlayer;

		private CompanionReserveState m_state;

		public PlayerStatus currentPlayer => m_currentPlayer;

		public CompanionReserveState state => m_state;

		public int level
		{
			get;
		}

		public bool isGiven
		{
			get
			{
				if (ownerPlayer.id == m_currentPlayer.id)
				{
					return ownerPlayer.fightId != m_currentPlayer.fightId;
				}
				return true;
			}
		}

		public ReserveCompanionStatus(PlayerStatus ownerPlayer, CompanionDefinition definition, int level)
		{
			this.ownerPlayer = ownerPlayer;
			this.definition = definition;
			this.level = level;
			m_currentPlayer = ownerPlayer;
		}

		public void SetCurrentPlayer(PlayerStatus value)
		{
			m_currentPlayer = value;
		}

		public void SetState(CompanionReserveState value)
		{
			m_state = value;
		}

		public ReserveCompanionValueContext CreateValueContext()
		{
			return new ReserveCompanionValueContext(GameStatus.GetFightStatus(m_currentPlayer.fightId), m_currentPlayer.id, level);
		}

		public ICastableDefinition GetDefinition()
		{
			return definition;
		}
	}
}
