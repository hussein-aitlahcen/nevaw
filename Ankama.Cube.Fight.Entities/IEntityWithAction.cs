using Ankama.Cube.Data;

namespace Ankama.Cube.Fight.Entities
{
	public interface IEntityWithAction : IEntity
	{
		int? actionValue
		{
			get;
		}

		ActionType actionType
		{
			get;
		}

		bool hasRange
		{
			get;
		}

		int rangeMin
		{
			get;
		}

		int rangeMax
		{
			get;
		}

		int physicalDamageBoost
		{
			get;
		}

		int physicalHealBoost
		{
			get;
		}

		bool actionUsed
		{
			get;
			set;
		}

		bool canDoActionOnTarget
		{
			get;
		}

		IEntitySelector customActionTarget
		{
			get;
		}
	}
}
