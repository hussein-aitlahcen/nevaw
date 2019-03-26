using Ankama.Cube.Extensions;
using Ankama.Cube.Fight.Entities;
using Ankama.Utilities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class InLineInOneDirectionTargetFilter : IEditableContent, ICoordOrEntityFilter, ICoordFilter, ITargetFilter, IEntityFilter
	{
		private ISingleTargetSelector m_refDirectionTargetA;

		private ISingleTargetSelector m_refDirectionTargetB;

		private ITargetSelector m_applyStartTargets;

		private ValueFilter m_distance;

		public ISingleTargetSelector refDirectionTargetA => m_refDirectionTargetA;

		public ISingleTargetSelector refDirectionTargetB => m_refDirectionTargetB;

		public ITargetSelector applyStartTargets => m_applyStartTargets;

		public ValueFilter distance => m_distance;

		public override string ToString()
		{
			return "Vector from A to B.";
		}

		public static InLineInOneDirectionTargetFilter FromJsonToken(JToken token)
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
			InLineInOneDirectionTargetFilter inLineInOneDirectionTargetFilter = new InLineInOneDirectionTargetFilter();
			inLineInOneDirectionTargetFilter.PopulateFromJson(jsonObject);
			return inLineInOneDirectionTargetFilter;
		}

		public static InLineInOneDirectionTargetFilter FromJsonProperty(JObject jsonObject, string propertyName, InLineInOneDirectionTargetFilter defaultValue = null)
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
			m_refDirectionTargetA = ISingleTargetSelectorUtils.FromJsonProperty(jsonObject, "refDirectionTargetA");
			m_refDirectionTargetB = ISingleTargetSelectorUtils.FromJsonProperty(jsonObject, "refDirectionTargetB");
			m_applyStartTargets = ITargetSelectorUtils.FromJsonProperty(jsonObject, "applyStartTargets");
			m_distance = ValueFilter.FromJsonProperty(jsonObject, "distance");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			if (ZoneAreaFilterUtils.SingleTargetToCompareArea(refDirectionTargetA, context, out Area area) && ZoneAreaFilterUtils.SingleTargetToCompareArea(refDirectionTargetB, context, out Area area2))
			{
				Direction? dirOpt = area.refCoord.GetStrictDirection4To(area2.refCoord);
				if (dirOpt.HasValue)
				{
					List<Area> applyAreas = ListPool<Area>.Get();
					applyAreas.AddRange(ZoneAreaFilterUtils.TargetsToCompareAreaList(applyStartTargets, context));
					int applyAreaCount = applyAreas.Count;
					foreach (IEntity entity in entities)
					{
						IEntityWithBoardPresence entityWithBoardPresence = entity as IEntityWithBoardPresence;
						if (entityWithBoardPresence != null)
						{
							for (int i = 0; i < applyAreaCount; i++)
							{
								Area area3 = applyAreas[i];
								if (distance.Matches(entityWithBoardPresence.area.MinDistanceWith(area3), context) && entityWithBoardPresence.area.IsAlignedWith(area3) && area3.GetStrictDirection4To(entityWithBoardPresence.area) == dirOpt)
								{
									yield return entity;
									break;
								}
							}
						}
					}
					ListPool<Area>.Release(applyAreas);
				}
			}
		}

		public IEnumerable<Coord> Filter(IEnumerable<Coord> coords, DynamicValueContext context)
		{
			if (ZoneAreaFilterUtils.SingleTargetToCompareArea(refDirectionTargetA, context, out Area area) && ZoneAreaFilterUtils.SingleTargetToCompareArea(refDirectionTargetB, context, out Area area2))
			{
				Direction? dirOpt = area.refCoord.GetStrictDirection4To(area2.refCoord);
				if (dirOpt.HasValue)
				{
					List<Area> applyAreas = ListPool<Area>.Get();
					applyAreas.AddRange(ZoneAreaFilterUtils.TargetsToCompareAreaList(applyStartTargets, context));
					int applyAreaCount = applyAreas.Count;
					foreach (Coord coord in coords)
					{
						Vector2Int val = default(Vector2Int);
						val._002Ector(coord.x, coord.y);
						for (int i = 0; i < applyAreaCount; i++)
						{
							Area area3 = applyAreas[i];
							if (distance.Matches(area3.MinDistanceWith(val), context) && area3.IsAlignedWith(val) && area3.GetStrictDirection4To(val) == dirOpt)
							{
								yield return coord;
								break;
							}
						}
					}
					ListPool<Area>.Release(applyAreas);
				}
			}
		}
	}
}
