using Ankama.Cube.Extensions;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using DataEditor;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SpellEffectInstantiationData : IEditableContent
	{
		[Serializable]
		public sealed class DelayOverDistance : IEditableContent
		{
			private ISingleTargetSelector m_origin;

			private float m_delay;

			public ISingleTargetSelector origin => m_origin;

			public float delay => m_delay;

			public override string ToString()
			{
				return $"{m_delay}s/m from {origin}";
			}

			public static DelayOverDistance FromJsonToken(JToken token)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Invalid comparison between Unknown and I4
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				if ((int)token.get_Type() != 1)
				{
					Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
					return null;
				}
				JObject jsonObject = Extensions.Value<JObject>((IEnumerable<JToken>)token);
				DelayOverDistance delayOverDistance = new DelayOverDistance();
				delayOverDistance.PopulateFromJson(jsonObject);
				return delayOverDistance;
			}

			public static DelayOverDistance FromJsonProperty(JObject jsonObject, string propertyName, DelayOverDistance defaultValue = null)
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Invalid comparison between Unknown and I4
				JProperty val = jsonObject.Property(propertyName);
				if (val == null || (int)val.get_Value().get_Type() == 10)
				{
					return defaultValue;
				}
				return FromJsonToken(val.get_Value());
			}

			public void PopulateFromJson(JObject jsonObject)
			{
				m_origin = ISingleTargetSelectorUtils.FromJsonProperty(jsonObject, "origin");
				m_delay = Serialization.JsonTokenValue<float>(jsonObject, "delay", 0.25f);
			}
		}

		private string m_spellEffect;

		private ITargetSelector m_originTarget;

		private ISingleTargetSelector m_orientation;

		private DelayOverDistance m_delayOverDistance;

		private Vector2Int m_delayOverDistanceOrigin;

		public string spellEffect => m_spellEffect;

		public ITargetSelector originTarget => m_originTarget;

		public ISingleTargetSelector orientation => m_orientation;

		public DelayOverDistance delayOverDistance => m_delayOverDistance;

		public override string ToString()
		{
			return string.Empty;
		}

		public static SpellEffectInstantiationData FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject jsonObject = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			SpellEffectInstantiationData spellEffectInstantiationData = new SpellEffectInstantiationData();
			spellEffectInstantiationData.PopulateFromJson(jsonObject);
			return spellEffectInstantiationData;
		}

		public static SpellEffectInstantiationData FromJsonProperty(JObject jsonObject, string propertyName, SpellEffectInstantiationData defaultValue = null)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			JProperty val = jsonObject.Property(propertyName);
			if (val == null || (int)val.get_Value().get_Type() == 10)
			{
				return defaultValue;
			}
			return FromJsonToken(val.get_Value());
		}

		public void PopulateFromJson(JObject jsonObject)
		{
			m_spellEffect = Serialization.JsonTokenValue<string>(jsonObject, "spellEffect", "");
			m_originTarget = ITargetSelectorUtils.FromJsonProperty(jsonObject, "originTarget");
			m_orientation = ISingleTargetSelectorUtils.FromJsonProperty(jsonObject, "orientation");
			m_delayOverDistance = DelayOverDistance.FromJsonProperty(jsonObject, "delayOverDistance");
		}

		public IEnumerable<Vector2Int> EnumerateInstantiationPositions([NotNull] DynamicValueContext castTargetContext)
		{
			ICoordSelector coordSelector = m_originTarget as ICoordSelector;
			if (coordSelector != null)
			{
				foreach (Coord item in coordSelector.EnumerateCoords(castTargetContext))
				{
					yield return new Vector2Int(item.x, item.y);
				}
			}
		}

		public IEnumerable<IsoObject> EnumerateInstantiationObjectTargets([NotNull] DynamicValueContext castTargetContext)
		{
			IEntitySelector entitySelector = m_originTarget as IEntitySelector;
			if (entitySelector != null)
			{
				foreach (IEntity item in entitySelector.EnumerateEntities(castTargetContext))
				{
					IEntityWithBoardPresence entityWithBoardPresence = item as IEntityWithBoardPresence;
					if (entityWithBoardPresence != null)
					{
						yield return entityWithBoardPresence.view;
					}
				}
			}
		}

		public Quaternion GetOrientation(Vector2Int origin, [NotNull] CastTargetContext castTargetContext)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			if (m_orientation != null)
			{
				ISingleCoordSelector singleCoordSelector = m_orientation as ISingleCoordSelector;
				if (singleCoordSelector != null && singleCoordSelector.TryGetCoord(castTargetContext, out Coord coord))
				{
					Vector2Int to = (Vector2Int)coord;
					return origin.GetDirectionTo(to).GetRotation();
				}
				ISingleEntitySelector singleEntitySelector = m_orientation as ISingleEntitySelector;
				if (singleEntitySelector != null && singleEntitySelector.TryGetEntity(castTargetContext, out IEntityWithBoardPresence entity))
				{
					Vector2Int coords = entity.view.cellObject.coords;
					return origin.GetDirectionTo(coords).GetRotation();
				}
			}
			else
			{
				Log.Warning($"Requested orientation but not orientation target is set (spell definition id: {castTargetContext.spellDefinitionId}).", 78, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Spells\\SpellEffectInstantiationData.cs");
			}
			return Quaternion.get_identity();
		}

		public void PreComputeDelayOverDistance([NotNull] DynamicValueContext castTargetContext)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			if (m_delayOverDistance == null)
			{
				return;
			}
			ISingleTargetSelector origin = m_delayOverDistance.origin;
			ISingleCoordSelector singleCoordSelector = origin as ISingleCoordSelector;
			if (singleCoordSelector != null && singleCoordSelector.TryGetCoord(castTargetContext, out Coord coord))
			{
				m_delayOverDistanceOrigin = (Vector2Int)coord;
				return;
			}
			ISingleEntitySelector singleEntitySelector = origin as ISingleEntitySelector;
			if (singleEntitySelector != null && singleEntitySelector.TryGetEntity(castTargetContext, out IEntityWithBoardPresence entity))
			{
				m_delayOverDistanceOrigin = entity.view.cellObject.coords;
			}
			else
			{
				Log.Warning("Could not find the origin for the delay over distance.", 115, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Spells\\SpellEffectInstantiationData.cs");
			}
		}

		public float GetDelayOverDistance(Vector2Int target)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (m_delayOverDistance == null)
			{
				return 0f;
			}
			return m_delayOverDistance.delay * (float)m_delayOverDistanceOrigin.DistanceTo(target);
		}
	}
}
