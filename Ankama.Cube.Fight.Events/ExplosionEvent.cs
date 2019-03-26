using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Fight.Events
{
	public class ExplosionEvent : FightEvent
	{
		public CellCoord center
		{
			get;
			private set;
		}

		public ExplosionEvent(int eventId, int? parentEventId, CellCoord center)
			: base(FightEventData.Types.EventType.Explosion, eventId, parentEventId)
		{
			this.center = center;
		}

		public ExplosionEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.Explosion, proto)
		{
			center = proto.CellCoord1;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			FightMap current = FightMap.current;
			if (!(null != current))
			{
				yield break;
			}
			if (current.TryGetCellObject(center.X, center.Y, out CellObject cellObject))
			{
				if (FightSpellEffectFactory.TryGetSpellEffect(SpellEffectKey.Explosion, fightStatus.fightId, parentEventId, out SpellEffect spellEffect))
				{
					CameraHandler current2 = CameraHandler.current;
					yield return FightSpellEffectFactory.PlaySpellEffect(rotation: (!(null != current2)) ? Quaternion.get_identity() : current2.mapRotation.GetInverseRotation(), spellEffect: spellEffect, transform: cellObject.get_transform(), scale: Vector3.get_one(), delay: 0f, fightContext: fightStatus.context, contextProvider: null);
				}
			}
			else
			{
				Log.Error(FightEventErrors.InvalidPosition(center), 38, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ExplosionEvent.cs");
			}
		}

		public override bool CanBeGroupedWith(FightEvent other)
		{
			return false;
		}
	}
}
