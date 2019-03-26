using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public static class ObjectWithActivationAnimationExtensions
	{
		public static IEnumerator AnimateActivation(this IObjectWithActivationAnimation objectWithActivationAnimation, Vector2Int position)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			CellObject cellObject = objectWithActivationAnimation.cellObject;
			if (cellObject.coords != position)
			{
				Log.Warning($"Was not on the specified cell of an attack sequence: {cellObject.coords} instead of {position} ({objectWithActivationAnimation.gameObject.get_name()}).", 23, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\IObjectWithActivationAnimation.cs");
				CellObject cellObject2 = cellObject.parentMap.GetCellObject(position.get_x(), position.get_y());
				objectWithActivationAnimation.SetCellObject(cellObject2);
			}
			yield return objectWithActivationAnimation.PlayActivationAnimation();
		}
	}
}
