using Ankama.Cube.Data;
using Ankama.Cube.Protocols.FightCommonProtocol;
using System.Collections.Generic;

namespace Ankama.Cube.UI.DeckMaker
{
	public struct CompanionStatusData
	{
		public bool hasResources;

		public IReadOnlyList<Cost> cost;

		public CompanionReserveState state;

		public bool isGiven;
	}
}
