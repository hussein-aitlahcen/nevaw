using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Cube.Maps.VisualEffects;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class MapCharacterObjectContext : VisualEffectContext
	{
		private readonly MapPathfindingActor m_mapCharacterObject;

		public override Transform transform => m_mapCharacterObject.get_transform();

		public MapCharacterObjectContext(MapPathfindingActor character)
		{
			m_mapCharacterObject = character;
		}

		public override void GetVisualEffectTransformation(out Quaternion rotation, out Vector3 scale)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			rotation = m_mapCharacterObject.direction.GetRotation();
			scale = Vector3.get_one();
		}

		protected override void InitializeEventInstance(EventInstance eventInstance)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			if (!(null == m_mapCharacterObject))
			{
				ATTRIBUTES_3D val = FMODUtility.To3DAttributes(m_mapCharacterObject.get_transform());
				eventInstance.set3DAttributes(val);
			}
		}

		public void UpdateDirection(Direction from, Direction to)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Quaternion rotation = from.DirectionAngleTo(to).GetRotation();
			int count = m_visualEffectInstances.Count;
			for (int i = 0; i < count; i++)
			{
				Transform transform = m_visualEffectInstances[i].get_transform();
				transform.set_rotation(transform.get_rotation() * rotation);
			}
		}
	}
}
