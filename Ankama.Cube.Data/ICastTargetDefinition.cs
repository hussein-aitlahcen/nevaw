using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using DataEditor;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public interface ICastTargetDefinition : IEditableContent
	{
		int count
		{
			get;
		}

		CastTargetContext CreateCastTargetContext(FightStatus fightStatus, int playerId, DynamicValueHolderType type, int definitionId, int level, int instanceId);

		IEnumerable<Target> EnumerateTargets([NotNull] CastTargetContext castTargetContext);
	}
}
