using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public static class AttackableObjectExtensions
	{
		public static IEnumerator Hit(this IObjectWithArmoredLife objectWithLife, Vector2Int position)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			CellObject cellObject = objectWithLife.cellObject;
			if (cellObject.coords != position)
			{
				Log.Warning($"Was not on the specified cell of hit sequence: {cellObject.coords} instead of {position} ({objectWithLife.gameObject.get_name()}).", 31, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\IObjectWithArmoredLife.cs");
				CellObject cellObject2 = cellObject.parentMap.GetCellObject(position.get_x(), position.get_y());
				objectWithLife.SetCellObject(cellObject2);
			}
			return objectWithLife.PlayHitAnimation();
		}

		public static void LethalHit(this IObjectWithArmoredLife objectWithLife, Vector2Int position)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			CellObject cellObject = objectWithLife.cellObject;
			if (cellObject.coords != position)
			{
				Log.Warning($"Was not on the specified cell of hit sequence: {cellObject.coords} instead of {position} ({objectWithLife.gameObject.get_name()}).", 49, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\IObjectWithArmoredLife.cs");
				CellObject cellObject2 = cellObject.parentMap.GetCellObject(position.get_x(), position.get_y());
				objectWithLife.SetCellObject(cellObject2);
			}
			MonoBehaviourExtensions.StartCoroutineImmediateSafe(objectWithLife, objectWithLife.PlayLethalHitAnimation(), null);
		}
	}
}
