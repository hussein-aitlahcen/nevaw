using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public static class ObjectWithActionExtensions
	{
		public static IEnumerator DoAction(this IObjectWithAction objectWithAction, Vector2Int position, Vector2Int target)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			CellObject cellObject = objectWithAction.cellObject;
			if (cellObject.coords != position)
			{
				Log.Warning($"Was not on the specified cell of an attack sequence: {cellObject.coords} instead of {position} ({objectWithAction.gameObject.get_name()}).", 51, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\IObjectWithAction.cs");
				CellObject cellObject2 = cellObject.parentMap.GetCellObject(position.get_x(), position.get_y());
				objectWithAction.SetCellObject(cellObject2);
			}
			Direction direction = position.GetDirectionTo(target);
			if (!direction.IsAxisAligned())
			{
				direction = direction.GetAxisAligned(objectWithAction.direction);
			}
			bool waitForAnimationEndOnMissingLabel = !position.IsAdjacentTo(target);
			yield return objectWithAction.PlayActionAnimation(direction, waitForAnimationEndOnMissingLabel);
			objectWithAction.TriggerActionEffect(target);
		}

		public static IEnumerator DoRangedAction(this IObjectWithAction objectWithAction, Vector2Int position, Vector2Int target)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			CellObject cellObject = objectWithAction.cellObject;
			if (cellObject.coords != position)
			{
				Log.Warning($"Was not on the specified cell of ranged attack sequence: {cellObject.coords} instead of {position} ({objectWithAction.gameObject.get_name()}).", 83, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\IObjectWithAction.cs");
				CellObject cellObject2 = cellObject.parentMap.GetCellObject(position.get_x(), position.get_y());
				objectWithAction.SetCellObject(cellObject2);
			}
			Direction direction = position.GetDirectionTo(target);
			if (!direction.IsAxisAligned())
			{
				direction = direction.GetAxisAligned(objectWithAction.direction);
			}
			yield return objectWithAction.PlayRangedActionAnimation(direction);
			objectWithAction.TriggerActionEffect(target);
		}
	}
}
