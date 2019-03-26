using Ankama.Cube.Data;
using JetBrains.Annotations;

namespace Ankama.Cube.Maps.Objects
{
	public interface IObjectTargetableByAction : ICharacterObject, IMovableIsoObject, IIsoObject
	{
		[PublicAPI]
		void ShowActionTargetFeedback(ActionType actionType, bool isSelected);

		[PublicAPI]
		void HideActionTargetFeedback();
	}
}
