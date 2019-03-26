using Ankama.AssetManagement.AssetReferences;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public interface ICastableDefinition : IDefinitionWithTooltip, IDefinitionWithDescription, IDefinitionWithPrecomputedData
	{
		string illustrationBundleName
		{
			get;
		}

		AssetReference illustrationReference
		{
			get;
		}

		IReadOnlyList<EventCategory> eventsInvalidatingCost
		{
			get;
		}

		IReadOnlyList<EventCategory> eventsInvalidatingCasting
		{
			get;
		}
	}
}
