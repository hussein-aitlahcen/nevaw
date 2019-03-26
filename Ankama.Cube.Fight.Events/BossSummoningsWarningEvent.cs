using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight.Events
{
	public class BossSummoningsWarningEvent : FightEvent
	{
		public bool addWarning
		{
			get;
			private set;
		}

		public IReadOnlyList<CellCoord> positions
		{
			get;
			private set;
		}

		public IReadOnlyList<int> directions
		{
			get;
			private set;
		}

		public BossSummoningsWarningEvent(int eventId, int? parentEventId, bool addWarning, IReadOnlyList<CellCoord> positions, IReadOnlyList<int> directions)
			: base(FightEventData.Types.EventType.BossSummoningsWarning, eventId, parentEventId)
		{
			this.addWarning = addWarning;
			this.positions = positions;
			this.directions = directions;
		}

		public BossSummoningsWarningEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.BossSummoningsWarning, proto)
		{
			addWarning = proto.Bool1;
			positions = (IReadOnlyList<CellCoord>)proto.CellCoordList1;
			directions = (IReadOnlyList<int>)proto.IntList1;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			int positionCount = positions.Count;
			FightMap current = FightMap.current;
			if (null != current)
			{
				IEnumerator[] array = new IEnumerator[positionCount];
				if (addWarning)
				{
					for (int i = 0; i < positionCount; i++)
					{
						CellCoord cellCoord = positions[i];
						Direction direction = (Direction)directions[i];
						array[i] = current.AddMonsterSpawnCell(cellCoord.X, cellCoord.Y, direction);
					}
				}
				else
				{
					for (int j = 0; j < positionCount; j++)
					{
						CellCoord cellCoord2 = positions[j];
						array[j] = current.RemoveMonsterSpawnCell(cellCoord2.X, cellCoord2.Y);
					}
				}
				yield return EnumeratorUtility.ParallelRecursiveImmediateSafeExecution(array);
			}
			for (int k = 0; k < positionCount; k++)
			{
				Vector2Int position = (Vector2Int)positions[k];
				ICharacterObject characterObject;
				if (fightStatus.TryGetEntityAt(position, out ICharacterEntity character) && (characterObject = (character.view as ICharacterObject)) != null)
				{
					characterObject.CheckParentCellIndicator();
				}
			}
		}
	}
}
