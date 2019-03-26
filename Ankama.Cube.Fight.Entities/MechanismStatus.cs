using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Maps.Objects;
using System.Collections.Generic;

namespace Ankama.Cube.Fight.Entities
{
	public abstract class MechanismStatus : EntityStatus, IEntityWithOwner, IEntityWithTeam, IEntity, IEntityWithBoardPresence, IEntityWithFamilies, IEntityWithLevel, IDynamicValueSource
	{
		public MechanismDefinition definition
		{
			get;
			protected set;
		}

		public int teamId
		{
			get;
		}

		public int teamIndex
		{
			get;
		}

		public int ownerId
		{
			get;
		}

		public IsoObject view
		{
			get;
			set;
		}

		public virtual Area area
		{
			get;
			protected set;
		}

		public abstract bool blocksMovement
		{
			get;
		}

		public IReadOnlyList<Family> families => definition.families;

		public int level
		{
			get;
		}

		public IReadOnlyList<ILevelOnlyDependant> dynamicValues => definition.precomputedData.dynamicValueReferences;

		protected MechanismStatus(int id, int ownerId, int teamId, int teamIndex, int level)
			: base(id)
		{
			this.ownerId = ownerId;
			this.teamId = teamId;
			this.teamIndex = teamIndex;
			this.level = level;
		}
	}
}
