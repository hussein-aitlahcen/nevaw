using Ankama.Cube.Data;

namespace Ankama.Cube.Fight.Entities
{
	public interface ICharacterTooltipDataProvider : ITooltipDataProvider
	{
		ActionType GetActionType();

		TooltipActionIcon GetActionIcon();

		bool TryGetActionValue(out int value);

		int GetLifeValue();

		int GetMovementValue();
	}
}
