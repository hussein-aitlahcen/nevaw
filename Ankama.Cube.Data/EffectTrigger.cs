using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class EffectTrigger : IEditableContent
	{
		[Serializable]
		public sealed class AfterEntityRemoval : EffectTrigger
		{
			private IEntityFilter m_removedValidator;

			private IEntityFilter m_removerValidator;

			private bool? m_onlyIfRemovedByDeath;

			private bool? m_removedByThisSpell;

			public IEntityFilter removedValidator => m_removedValidator;

			public IEntityFilter removerValidator => m_removerValidator;

			public bool? onlyIfRemovedByDeath => m_onlyIfRemovedByDeath;

			public bool? removedByThisSpell => m_removedByThisSpell;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static AfterEntityRemoval FromJsonToken(JToken token)
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
				AfterEntityRemoval afterEntityRemoval = new AfterEntityRemoval();
				afterEntityRemoval.PopulateFromJson(jsonObject);
				return afterEntityRemoval;
			}

			public static AfterEntityRemoval FromJsonProperty(JObject jsonObject, string propertyName, AfterEntityRemoval defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_removedValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "removedValidator");
				m_removerValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "removerValidator");
				m_onlyIfRemovedByDeath = Serialization.JsonTokenValue<bool?>(jsonObject, "onlyIfRemovedByDeath", (bool?)null);
				m_removedByThisSpell = Serialization.JsonTokenValue<bool?>(jsonObject, "removedByThisSpell", (bool?)null);
			}
		}

		[Serializable]
		public sealed class AfterFloorMechanismActivation : EffectTrigger
		{
			private IEntityFilter m_activatedValidator;

			private IEntityFilter m_activatorValidator;

			public IEntityFilter activatedValidator => m_activatedValidator;

			public IEntityFilter activatorValidator => m_activatorValidator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static AfterFloorMechanismActivation FromJsonToken(JToken token)
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
				AfterFloorMechanismActivation afterFloorMechanismActivation = new AfterFloorMechanismActivation();
				afterFloorMechanismActivation.PopulateFromJson(jsonObject);
				return afterFloorMechanismActivation;
			}

			public static AfterFloorMechanismActivation FromJsonProperty(JObject jsonObject, string propertyName, AfterFloorMechanismActivation defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_activatedValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "activatedValidator");
				m_activatorValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "activatorValidator");
			}
		}

		[Serializable]
		public sealed class AfterOtherSpellPlayed : EffectTrigger
		{
			private OwnerFilter m_playedBy;

			private List<SpellFilter> m_spellFilters;

			private ITargetFilter m_playedOn;

			public OwnerFilter playedBy => m_playedBy;

			public IReadOnlyList<SpellFilter> spellFilters => m_spellFilters;

			public ITargetFilter playedOn => m_playedOn;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static AfterOtherSpellPlayed FromJsonToken(JToken token)
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
				AfterOtherSpellPlayed afterOtherSpellPlayed = new AfterOtherSpellPlayed();
				afterOtherSpellPlayed.PopulateFromJson(jsonObject);
				return afterOtherSpellPlayed;
			}

			public static AfterOtherSpellPlayed FromJsonProperty(JObject jsonObject, string propertyName, AfterOtherSpellPlayed defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_playedBy = OwnerFilter.FromJsonProperty(jsonObject, "playedBy");
				JArray val = Serialization.JsonArray(jsonObject, "spellFilters");
				m_spellFilters = new List<SpellFilter>((val != null) ? val.get_Count() : 0);
				if (val != null)
				{
					foreach (JToken item in val)
					{
						m_spellFilters.Add(SpellFilter.FromJsonToken(item));
					}
				}
				m_playedOn = ITargetFilterUtils.FromJsonProperty(jsonObject, "playedOn");
			}
		}

		[Serializable]
		public sealed class AfterOtherSpellPlayedAndExecuted : EffectTrigger
		{
			private OwnerFilter m_playedBy;

			private List<SpellFilter> m_spellFilters;

			private ITargetFilter m_playedOn;

			public OwnerFilter playedBy => m_playedBy;

			public IReadOnlyList<SpellFilter> spellFilters => m_spellFilters;

			public ITargetFilter playedOn => m_playedOn;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static AfterOtherSpellPlayedAndExecuted FromJsonToken(JToken token)
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
				AfterOtherSpellPlayedAndExecuted afterOtherSpellPlayedAndExecuted = new AfterOtherSpellPlayedAndExecuted();
				afterOtherSpellPlayedAndExecuted.PopulateFromJson(jsonObject);
				return afterOtherSpellPlayedAndExecuted;
			}

			public static AfterOtherSpellPlayedAndExecuted FromJsonProperty(JObject jsonObject, string propertyName, AfterOtherSpellPlayedAndExecuted defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_playedBy = OwnerFilter.FromJsonProperty(jsonObject, "playedBy");
				JArray val = Serialization.JsonArray(jsonObject, "spellFilters");
				m_spellFilters = new List<SpellFilter>((val != null) ? val.get_Count() : 0);
				if (val != null)
				{
					foreach (JToken item in val)
					{
						m_spellFilters.Add(SpellFilter.FromJsonToken(item));
					}
				}
				m_playedOn = ITargetFilterUtils.FromJsonProperty(jsonObject, "playedOn");
			}
		}

		[Serializable]
		public sealed class AfterThisSpellPlayedAndExecuted : EffectTrigger
		{
			private ITargetFilter m_playedOn;

			public ITargetFilter playedOn => m_playedOn;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static AfterThisSpellPlayedAndExecuted FromJsonToken(JToken token)
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
				AfterThisSpellPlayedAndExecuted afterThisSpellPlayedAndExecuted = new AfterThisSpellPlayedAndExecuted();
				afterThisSpellPlayedAndExecuted.PopulateFromJson(jsonObject);
				return afterThisSpellPlayedAndExecuted;
			}

			public static AfterThisSpellPlayedAndExecuted FromJsonProperty(JObject jsonObject, string propertyName, AfterThisSpellPlayedAndExecuted defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_playedOn = ITargetFilterUtils.FromJsonProperty(jsonObject, "playedOn");
			}
		}

		[Serializable]
		public sealed class ConditionalTrigger : EffectTrigger
		{
			private EffectCondition m_condition;

			private List<EffectTrigger> m_triggers;

			public EffectCondition condition => m_condition;

			public IReadOnlyList<EffectTrigger> triggers => m_triggers;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static ConditionalTrigger FromJsonToken(JToken token)
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
				ConditionalTrigger conditionalTrigger = new ConditionalTrigger();
				conditionalTrigger.PopulateFromJson(jsonObject);
				return conditionalTrigger;
			}

			public static ConditionalTrigger FromJsonProperty(JObject jsonObject, string propertyName, ConditionalTrigger defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_condition = EffectCondition.FromJsonProperty(jsonObject, "condition");
				JArray val = Serialization.JsonArray(jsonObject, "triggers");
				m_triggers = new List<EffectTrigger>((val != null) ? val.get_Count() : 0);
				if (val != null)
				{
					foreach (JToken item in val)
					{
						m_triggers.Add(EffectTrigger.FromJsonToken(item));
					}
				}
			}
		}

		[Serializable]
		public sealed class EntityRemoval : EffectTrigger
		{
			private IEntityFilter m_removedValidator;

			private IEntityFilter m_removerValidator;

			private bool? m_onlyIfRemovedByDeath;

			private bool? m_removedByThisSpell;

			public IEntityFilter removedValidator => m_removedValidator;

			public IEntityFilter removerValidator => m_removerValidator;

			public bool? onlyIfRemovedByDeath => m_onlyIfRemovedByDeath;

			public bool? removedByThisSpell => m_removedByThisSpell;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static EntityRemoval FromJsonToken(JToken token)
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
				EntityRemoval entityRemoval = new EntityRemoval();
				entityRemoval.PopulateFromJson(jsonObject);
				return entityRemoval;
			}

			public static EntityRemoval FromJsonProperty(JObject jsonObject, string propertyName, EntityRemoval defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_removedValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "removedValidator");
				m_removerValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "removerValidator");
				m_onlyIfRemovedByDeath = Serialization.JsonTokenValue<bool?>(jsonObject, "onlyIfRemovedByDeath", (bool?)null);
				m_removedByThisSpell = Serialization.JsonTokenValue<bool?>(jsonObject, "removedByThisSpell", (bool?)null);
			}
		}

		[Serializable]
		public sealed class OnArmorChanged : EffectTrigger
		{
			private IEntityFilter m_targetValidator;

			private IEntityFilter m_sourceValidator;

			private ValueFilter m_modification;

			private ValueFilter m_newValue;

			public IEntityFilter targetValidator => m_targetValidator;

			public IEntityFilter sourceValidator => m_sourceValidator;

			public ValueFilter modification => m_modification;

			public ValueFilter newValue => m_newValue;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnArmorChanged FromJsonToken(JToken token)
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
				OnArmorChanged onArmorChanged = new OnArmorChanged();
				onArmorChanged.PopulateFromJson(jsonObject);
				return onArmorChanged;
			}

			public static OnArmorChanged FromJsonProperty(JObject jsonObject, string propertyName, OnArmorChanged defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_targetValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "targetValidator");
				m_sourceValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "sourceValidator");
				m_modification = ValueFilter.FromJsonProperty(jsonObject, "modification");
				m_newValue = ValueFilter.FromJsonProperty(jsonObject, "newValue");
			}
		}

		[Serializable]
		public sealed class OnAssemblingTrigger : EffectTrigger
		{
			private ITargetSelector m_triggeredBy;

			private ValueFilter m_assemblingSize;

			public ITargetSelector triggeredBy => m_triggeredBy;

			public ValueFilter assemblingSize => m_assemblingSize;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnAssemblingTrigger FromJsonToken(JToken token)
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
				OnAssemblingTrigger onAssemblingTrigger = new OnAssemblingTrigger();
				onAssemblingTrigger.PopulateFromJson(jsonObject);
				return onAssemblingTrigger;
			}

			public static OnAssemblingTrigger FromJsonProperty(JObject jsonObject, string propertyName, OnAssemblingTrigger defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_triggeredBy = ITargetSelectorUtils.FromJsonProperty(jsonObject, "triggeredBy");
				m_assemblingSize = ValueFilter.FromJsonProperty(jsonObject, "assemblingSize");
			}
		}

		[Serializable]
		public sealed class OnCaracTheft : EffectTrigger
		{
			private OwnerFilter m_thief;

			private OwnerFilter m_robbed;

			private ListComparison m_comparison;

			private List<CaracId> m_carac;

			private ValueFilter m_quantity;

			public OwnerFilter thief => m_thief;

			public OwnerFilter robbed => m_robbed;

			public ListComparison comparison => m_comparison;

			public IReadOnlyList<CaracId> carac => m_carac;

			public ValueFilter quantity => m_quantity;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnCaracTheft FromJsonToken(JToken token)
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
				OnCaracTheft onCaracTheft = new OnCaracTheft();
				onCaracTheft.PopulateFromJson(jsonObject);
				return onCaracTheft;
			}

			public static OnCaracTheft FromJsonProperty(JObject jsonObject, string propertyName, OnCaracTheft defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_thief = OwnerFilter.FromJsonProperty(jsonObject, "thief");
				m_robbed = OwnerFilter.FromJsonProperty(jsonObject, "robbed");
				m_comparison = (ListComparison)Serialization.JsonTokenValue<int>(jsonObject, "comparison", 2);
				m_carac = Serialization.JsonArrayAsList<CaracId>(jsonObject, "carac");
				m_quantity = ValueFilter.FromJsonProperty(jsonObject, "quantity");
			}
		}

		[Serializable]
		public sealed class OnCharacterAdded : EffectTrigger
		{
			private bool m_evenIfTransformation;

			private List<IEntityFilter> m_addedCharacterValidator;

			public bool evenIfTransformation => m_evenIfTransformation;

			public IReadOnlyList<IEntityFilter> addedCharacterValidator => m_addedCharacterValidator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnCharacterAdded FromJsonToken(JToken token)
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
				OnCharacterAdded onCharacterAdded = new OnCharacterAdded();
				onCharacterAdded.PopulateFromJson(jsonObject);
				return onCharacterAdded;
			}

			public static OnCharacterAdded FromJsonProperty(JObject jsonObject, string propertyName, OnCharacterAdded defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_evenIfTransformation = Serialization.JsonTokenValue<bool>(jsonObject, "evenIfTransformation", false);
				JArray val = Serialization.JsonArray(jsonObject, "addedCharacterValidator");
				m_addedCharacterValidator = new List<IEntityFilter>((val != null) ? val.get_Count() : 0);
				if (val != null)
				{
					foreach (JToken item in val)
					{
						m_addedCharacterValidator.Add(IEntityFilterUtils.FromJsonToken(item));
					}
				}
			}
		}

		[Serializable]
		public sealed class OnCompanionTransfered : EffectTrigger
		{
			private OwnerFilter m_player;

			public OwnerFilter player => m_player;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnCompanionTransfered FromJsonToken(JToken token)
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
				OnCompanionTransfered onCompanionTransfered = new OnCompanionTransfered();
				onCompanionTransfered.PopulateFromJson(jsonObject);
				return onCompanionTransfered;
			}

			public static OnCompanionTransfered FromJsonProperty(JObject jsonObject, string propertyName, OnCompanionTransfered defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_player = OwnerFilter.FromJsonProperty(jsonObject, "player");
			}
		}

		[Serializable]
		public sealed class OnDiceThrown : EffectTrigger
		{
			private ValueFilter m_valueCheck;

			private IEntityFilter m_throwerCheck;

			private bool? m_thrownByThisSpell;

			private bool? m_thrownByThisCharacterAction;

			public ValueFilter valueCheck => m_valueCheck;

			public IEntityFilter throwerCheck => m_throwerCheck;

			public bool? thrownByThisSpell => m_thrownByThisSpell;

			public bool? thrownByThisCharacterAction => m_thrownByThisCharacterAction;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnDiceThrown FromJsonToken(JToken token)
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
				OnDiceThrown onDiceThrown = new OnDiceThrown();
				onDiceThrown.PopulateFromJson(jsonObject);
				return onDiceThrown;
			}

			public static OnDiceThrown FromJsonProperty(JObject jsonObject, string propertyName, OnDiceThrown defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_valueCheck = ValueFilter.FromJsonProperty(jsonObject, "valueCheck");
				m_throwerCheck = IEntityFilterUtils.FromJsonProperty(jsonObject, "throwerCheck");
				m_thrownByThisSpell = Serialization.JsonTokenValue<bool?>(jsonObject, "thrownByThisSpell", (bool?)null);
				m_thrownByThisCharacterAction = Serialization.JsonTokenValue<bool?>(jsonObject, "thrownByThisCharacterAction", (bool?)null);
			}
		}

		[Serializable]
		public sealed class OnElementaryStateApplied : EffectTrigger
		{
			private IEntityFilter m_targetValidator;

			private IEntityFilter m_casterValidator;

			private ListComparison m_comparison;

			private List<ElementaryStates> m_states;

			public IEntityFilter targetValidator => m_targetValidator;

			public IEntityFilter casterValidator => m_casterValidator;

			public ListComparison comparison => m_comparison;

			public IReadOnlyList<ElementaryStates> states => m_states;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnElementaryStateApplied FromJsonToken(JToken token)
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
				OnElementaryStateApplied onElementaryStateApplied = new OnElementaryStateApplied();
				onElementaryStateApplied.PopulateFromJson(jsonObject);
				return onElementaryStateApplied;
			}

			public static OnElementaryStateApplied FromJsonProperty(JObject jsonObject, string propertyName, OnElementaryStateApplied defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_targetValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "targetValidator");
				m_casterValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "casterValidator");
				m_comparison = (ListComparison)Serialization.JsonTokenValue<int>(jsonObject, "comparison", 2);
				m_states = Serialization.JsonArrayAsList<ElementaryStates>(jsonObject, "states");
			}
		}

		[Serializable]
		public sealed class OnEntityAction : EffectTrigger
		{
			private bool m_afterAction;

			private IEntityFilter m_actionSrc;

			private IEntityFilter m_actionTarget;

			private bool? m_hasMovedBeforeAction;

			private ActionType? m_actionType;

			public bool afterAction => m_afterAction;

			public IEntityFilter actionSrc => m_actionSrc;

			public IEntityFilter actionTarget => m_actionTarget;

			public bool? hasMovedBeforeAction => m_hasMovedBeforeAction;

			public ActionType? actionType => m_actionType;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnEntityAction FromJsonToken(JToken token)
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
				OnEntityAction onEntityAction = new OnEntityAction();
				onEntityAction.PopulateFromJson(jsonObject);
				return onEntityAction;
			}

			public static OnEntityAction FromJsonProperty(JObject jsonObject, string propertyName, OnEntityAction defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_afterAction = Serialization.JsonTokenValue<bool>(jsonObject, "afterAction", true);
				m_actionSrc = IEntityFilterUtils.FromJsonProperty(jsonObject, "actionSrc");
				m_actionTarget = IEntityFilterUtils.FromJsonProperty(jsonObject, "actionTarget");
				m_hasMovedBeforeAction = Serialization.JsonTokenValue<bool?>(jsonObject, "hasMovedBeforeAction", (bool?)null);
				m_actionType = (ActionType?)Serialization.JsonTokenValue<int?>(jsonObject, "actionType", (int?)null);
			}
		}

		[Serializable]
		public sealed class OnEntityCharged : EffectTrigger
		{
			private IEntitySelector m_entityValidator;

			private ValueFilter m_chargeLengthValidator;

			public IEntitySelector entityValidator => m_entityValidator;

			public ValueFilter chargeLengthValidator => m_chargeLengthValidator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnEntityCharged FromJsonToken(JToken token)
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
				OnEntityCharged onEntityCharged = new OnEntityCharged();
				onEntityCharged.PopulateFromJson(jsonObject);
				return onEntityCharged;
			}

			public static OnEntityCharged FromJsonProperty(JObject jsonObject, string propertyName, OnEntityCharged defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_entityValidator = IEntitySelectorUtils.FromJsonProperty(jsonObject, "entityValidator");
				m_chargeLengthValidator = ValueFilter.FromJsonProperty(jsonObject, "chargeLengthValidator");
			}
		}

		[Serializable]
		public sealed class OnEntityCrossedOver : EffectTrigger
		{
			private IEntityFilter m_targetValidator;

			private IEntityFilter m_sourceValidator;

			public IEntityFilter targetValidator => m_targetValidator;

			public IEntityFilter sourceValidator => m_sourceValidator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnEntityCrossedOver FromJsonToken(JToken token)
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
				OnEntityCrossedOver onEntityCrossedOver = new OnEntityCrossedOver();
				onEntityCrossedOver.PopulateFromJson(jsonObject);
				return onEntityCrossedOver;
			}

			public static OnEntityCrossedOver FromJsonProperty(JObject jsonObject, string propertyName, OnEntityCrossedOver defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_targetValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "targetValidator");
				m_sourceValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "sourceValidator");
			}
		}

		[Serializable]
		public sealed class OnEntityFinishMoveIntoArea : EffectTrigger
		{
			private IEntityFilter m_movedEntityValidator;

			private ITargetSelector m_area;

			private int m_maxDistanceToArea;

			public IEntityFilter movedEntityValidator => m_movedEntityValidator;

			public ITargetSelector area => m_area;

			public int maxDistanceToArea => m_maxDistanceToArea;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnEntityFinishMoveIntoArea FromJsonToken(JToken token)
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
				OnEntityFinishMoveIntoArea onEntityFinishMoveIntoArea = new OnEntityFinishMoveIntoArea();
				onEntityFinishMoveIntoArea.PopulateFromJson(jsonObject);
				return onEntityFinishMoveIntoArea;
			}

			public static OnEntityFinishMoveIntoArea FromJsonProperty(JObject jsonObject, string propertyName, OnEntityFinishMoveIntoArea defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_movedEntityValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "movedEntityValidator");
				m_area = ITargetSelectorUtils.FromJsonProperty(jsonObject, "area");
				m_maxDistanceToArea = Serialization.JsonTokenValue<int>(jsonObject, "maxDistanceToArea", 0);
			}
		}

		[Serializable]
		public sealed class OnEntityFullMovementDoneTrigger : EffectTrigger
		{
			private IEntityFilter m_entityValidator;

			private List<MovementType> m_movementTypes;

			private ValueFilter m_movementSize;

			private YesNoWhatever m_requiresSpellCast;

			public IEntityFilter entityValidator => m_entityValidator;

			public IReadOnlyList<MovementType> movementTypes => m_movementTypes;

			public ValueFilter movementSize => m_movementSize;

			public YesNoWhatever requiresSpellCast => m_requiresSpellCast;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnEntityFullMovementDoneTrigger FromJsonToken(JToken token)
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
				OnEntityFullMovementDoneTrigger onEntityFullMovementDoneTrigger = new OnEntityFullMovementDoneTrigger();
				onEntityFullMovementDoneTrigger.PopulateFromJson(jsonObject);
				return onEntityFullMovementDoneTrigger;
			}

			public static OnEntityFullMovementDoneTrigger FromJsonProperty(JObject jsonObject, string propertyName, OnEntityFullMovementDoneTrigger defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_entityValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "entityValidator");
				m_movementTypes = Serialization.JsonArrayAsList<MovementType>(jsonObject, "movementTypes");
				m_movementSize = ValueFilter.FromJsonProperty(jsonObject, "movementSize");
				m_requiresSpellCast = (YesNoWhatever)Serialization.JsonTokenValue<int>(jsonObject, "requiresSpellCast", 0);
			}
		}

		[Serializable]
		public sealed class OnEntityHealed : EffectTrigger
		{
			private IEntityFilter m_targetValidator;

			private IEntityFilter m_sourceValidator;

			public IEntityFilter targetValidator => m_targetValidator;

			public IEntityFilter sourceValidator => m_sourceValidator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnEntityHealed FromJsonToken(JToken token)
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
				OnEntityHealed onEntityHealed = new OnEntityHealed();
				onEntityHealed.PopulateFromJson(jsonObject);
				return onEntityHealed;
			}

			public static OnEntityHealed FromJsonProperty(JObject jsonObject, string propertyName, OnEntityHealed defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_targetValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "targetValidator");
				m_sourceValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "sourceValidator");
			}
		}

		[Serializable]
		public sealed class OnEntityHurt : EffectTrigger
		{
			private IEntityFilter m_targetValidator;

			private IEntityFilter m_sourceValidator;

			public IEntityFilter targetValidator => m_targetValidator;

			public IEntityFilter sourceValidator => m_sourceValidator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnEntityHurt FromJsonToken(JToken token)
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
				OnEntityHurt onEntityHurt = new OnEntityHurt();
				onEntityHurt.PopulateFromJson(jsonObject);
				return onEntityHurt;
			}

			public static OnEntityHurt FromJsonProperty(JObject jsonObject, string propertyName, OnEntityHurt defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_targetValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "targetValidator");
				m_sourceValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "sourceValidator");
			}
		}

		[Serializable]
		public sealed class OnEntityRemainingLifeChanged : EffectTrigger
		{
			private ISingleEntitySelector m_entity;

			private ValueFilter m_remainingLifePercent;

			public ISingleEntitySelector entity => m_entity;

			public ValueFilter remainingLifePercent => m_remainingLifePercent;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnEntityRemainingLifeChanged FromJsonToken(JToken token)
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
				OnEntityRemainingLifeChanged onEntityRemainingLifeChanged = new OnEntityRemainingLifeChanged();
				onEntityRemainingLifeChanged.PopulateFromJson(jsonObject);
				return onEntityRemainingLifeChanged;
			}

			public static OnEntityRemainingLifeChanged FromJsonProperty(JObject jsonObject, string propertyName, OnEntityRemainingLifeChanged defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_entity = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "entity");
				m_remainingLifePercent = ValueFilter.FromJsonProperty(jsonObject, "remainingLifePercent");
			}
		}

		[Serializable]
		public sealed class OnEntityStartMoveFromArea : EffectTrigger
		{
			private IEntityFilter m_movedEntityValidator;

			private ITargetSelector m_area;

			private bool m_allowReenter;

			private int m_maxDistanceToArea;

			public IEntityFilter movedEntityValidator => m_movedEntityValidator;

			public ITargetSelector area => m_area;

			public bool allowReenter => m_allowReenter;

			public int maxDistanceToArea => m_maxDistanceToArea;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnEntityStartMoveFromArea FromJsonToken(JToken token)
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
				OnEntityStartMoveFromArea onEntityStartMoveFromArea = new OnEntityStartMoveFromArea();
				onEntityStartMoveFromArea.PopulateFromJson(jsonObject);
				return onEntityStartMoveFromArea;
			}

			public static OnEntityStartMoveFromArea FromJsonProperty(JObject jsonObject, string propertyName, OnEntityStartMoveFromArea defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_movedEntityValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "movedEntityValidator");
				m_area = ITargetSelectorUtils.FromJsonProperty(jsonObject, "area");
				m_allowReenter = Serialization.JsonTokenValue<bool>(jsonObject, "allowReenter", true);
				m_maxDistanceToArea = Serialization.JsonTokenValue<int>(jsonObject, "maxDistanceToArea", 0);
			}
		}

		[Serializable]
		public sealed class OnExplosion : EffectTrigger
		{
			private IEntityFilter m_from;

			private bool m_beforeDamage;

			public IEntityFilter from => m_from;

			public bool beforeDamage => m_beforeDamage;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnExplosion FromJsonToken(JToken token)
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
				OnExplosion onExplosion = new OnExplosion();
				onExplosion.PopulateFromJson(jsonObject);
				return onExplosion;
			}

			public static OnExplosion FromJsonProperty(JObject jsonObject, string propertyName, OnExplosion defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_from = IEntityFilterUtils.FromJsonProperty(jsonObject, "from");
				m_beforeDamage = Serialization.JsonTokenValue<bool>(jsonObject, "beforeDamage", true);
			}
		}

		[Serializable]
		public sealed class OnFloatingCounterOfEffectHolderTerminated : EffectTrigger
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnFloatingCounterOfEffectHolderTerminated FromJsonToken(JToken token)
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
				OnFloatingCounterOfEffectHolderTerminated onFloatingCounterOfEffectHolderTerminated = new OnFloatingCounterOfEffectHolderTerminated();
				onFloatingCounterOfEffectHolderTerminated.PopulateFromJson(jsonObject);
				return onFloatingCounterOfEffectHolderTerminated;
			}

			public static OnFloatingCounterOfEffectHolderTerminated FromJsonProperty(JObject jsonObject, string propertyName, OnFloatingCounterOfEffectHolderTerminated defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
			}
		}

		[Serializable]
		public sealed class OnFloorMechanismActivation : EffectTrigger
		{
			private IEntityFilter m_activatedValidator;

			private IEntityFilter m_activatorValidator;

			public IEntityFilter activatedValidator => m_activatedValidator;

			public IEntityFilter activatorValidator => m_activatorValidator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnFloorMechanismActivation FromJsonToken(JToken token)
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
				OnFloorMechanismActivation onFloorMechanismActivation = new OnFloorMechanismActivation();
				onFloorMechanismActivation.PopulateFromJson(jsonObject);
				return onFloorMechanismActivation;
			}

			public static OnFloorMechanismActivation FromJsonProperty(JObject jsonObject, string propertyName, OnFloorMechanismActivation defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_activatedValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "activatedValidator");
				m_activatorValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "activatorValidator");
			}
		}

		[Serializable]
		public sealed class OnMechanismAdded : EffectTrigger
		{
			private List<IEntityFilter> m_addedMechanismValidator;

			public IReadOnlyList<IEntityFilter> addedMechanismValidator => m_addedMechanismValidator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnMechanismAdded FromJsonToken(JToken token)
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
				OnMechanismAdded onMechanismAdded = new OnMechanismAdded();
				onMechanismAdded.PopulateFromJson(jsonObject);
				return onMechanismAdded;
			}

			public static OnMechanismAdded FromJsonProperty(JObject jsonObject, string propertyName, OnMechanismAdded defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				JArray val = Serialization.JsonArray(jsonObject, "addedMechanismValidator");
				m_addedMechanismValidator = new List<IEntityFilter>((val != null) ? val.get_Count() : 0);
				if (val != null)
				{
					foreach (JToken item in val)
					{
						m_addedMechanismValidator.Add(IEntityFilterUtils.FromJsonToken(item));
					}
				}
			}
		}

		[Serializable]
		public sealed class OnPropertyApplied : EffectTrigger
		{
			private IEntityFilter m_targetValidator;

			private IEntityFilter m_casterValidator;

			private ListComparison m_comparison;

			private List<PropertyId> m_properties;

			public IEntityFilter targetValidator => m_targetValidator;

			public IEntityFilter casterValidator => m_casterValidator;

			public ListComparison comparison => m_comparison;

			public IReadOnlyList<PropertyId> properties => m_properties;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnPropertyApplied FromJsonToken(JToken token)
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
				OnPropertyApplied onPropertyApplied = new OnPropertyApplied();
				onPropertyApplied.PopulateFromJson(jsonObject);
				return onPropertyApplied;
			}

			public static OnPropertyApplied FromJsonProperty(JObject jsonObject, string propertyName, OnPropertyApplied defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_targetValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "targetValidator");
				m_casterValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "casterValidator");
				m_comparison = (ListComparison)Serialization.JsonTokenValue<int>(jsonObject, "comparison", 2);
				m_properties = Serialization.JsonArrayAsList<PropertyId>(jsonObject, "properties");
			}
		}

		[Serializable]
		public sealed class OnReservePointChanged : EffectTrigger
		{
			private bool m_exceptFromSpellGaugeModification;

			private OwnerFilter m_player;

			private ValueFilter m_modification;

			private ValueFilter m_newValue;

			public bool exceptFromSpellGaugeModification => m_exceptFromSpellGaugeModification;

			public OwnerFilter player => m_player;

			public ValueFilter modification => m_modification;

			public ValueFilter newValue => m_newValue;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnReservePointChanged FromJsonToken(JToken token)
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
				OnReservePointChanged onReservePointChanged = new OnReservePointChanged();
				onReservePointChanged.PopulateFromJson(jsonObject);
				return onReservePointChanged;
			}

			public static OnReservePointChanged FromJsonProperty(JObject jsonObject, string propertyName, OnReservePointChanged defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_exceptFromSpellGaugeModification = Serialization.JsonTokenValue<bool>(jsonObject, "exceptFromSpellGaugeModification", false);
				m_player = OwnerFilter.FromJsonProperty(jsonObject, "player");
				m_modification = ValueFilter.FromJsonProperty(jsonObject, "modification");
				m_newValue = ValueFilter.FromJsonProperty(jsonObject, "newValue");
			}
		}

		[Serializable]
		public sealed class OnReserveUsed : EffectTrigger
		{
			private OwnerFilter m_player;

			public OwnerFilter player => m_player;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnReserveUsed FromJsonToken(JToken token)
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
				OnReserveUsed onReserveUsed = new OnReserveUsed();
				onReserveUsed.PopulateFromJson(jsonObject);
				return onReserveUsed;
			}

			public static OnReserveUsed FromJsonProperty(JObject jsonObject, string propertyName, OnReserveUsed defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_player = OwnerFilter.FromJsonProperty(jsonObject, "player");
			}
		}

		[Serializable]
		public sealed class OnSpecificEventTrigger : EffectTrigger
		{
			private SpecificEventTrigger m_trigger;

			private IEntityFilter m_effectHolderValidator;

			public SpecificEventTrigger trigger => m_trigger;

			public IEntityFilter effectHolderValidator => m_effectHolderValidator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnSpecificEventTrigger FromJsonToken(JToken token)
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
				OnSpecificEventTrigger onSpecificEventTrigger = new OnSpecificEventTrigger();
				onSpecificEventTrigger.PopulateFromJson(jsonObject);
				return onSpecificEventTrigger;
			}

			public static OnSpecificEventTrigger FromJsonProperty(JObject jsonObject, string propertyName, OnSpecificEventTrigger defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_trigger = (SpecificEventTrigger)Serialization.JsonTokenValue<int>(jsonObject, "trigger", 0);
				m_effectHolderValidator = IEntityFilterUtils.FromJsonProperty(jsonObject, "effectHolderValidator");
			}
		}

		[Serializable]
		public sealed class OnSpellDrawn : EffectTrigger
		{
			private OwnerFilter m_drawnBy;

			private List<SpellFilter> m_spellFilters;

			public OwnerFilter drawnBy => m_drawnBy;

			public IReadOnlyList<SpellFilter> spellFilters => m_spellFilters;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnSpellDrawn FromJsonToken(JToken token)
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
				OnSpellDrawn onSpellDrawn = new OnSpellDrawn();
				onSpellDrawn.PopulateFromJson(jsonObject);
				return onSpellDrawn;
			}

			public static OnSpellDrawn FromJsonProperty(JObject jsonObject, string propertyName, OnSpellDrawn defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_drawnBy = OwnerFilter.FromJsonProperty(jsonObject, "drawnBy");
				JArray val = Serialization.JsonArray(jsonObject, "spellFilters");
				m_spellFilters = new List<SpellFilter>((val != null) ? val.get_Count() : 0);
				if (val != null)
				{
					foreach (JToken item in val)
					{
						m_spellFilters.Add(SpellFilter.FromJsonToken(item));
					}
				}
			}
		}

		[Serializable]
		public sealed class OnSquadChanged : EffectTrigger
		{
			private List<SquadModification> m_modification;

			private IEntityFilter m_validator;

			public IReadOnlyList<SquadModification> modification => m_modification;

			public IEntityFilter validator => m_validator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnSquadChanged FromJsonToken(JToken token)
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
				OnSquadChanged onSquadChanged = new OnSquadChanged();
				onSquadChanged.PopulateFromJson(jsonObject);
				return onSquadChanged;
			}

			public static OnSquadChanged FromJsonProperty(JObject jsonObject, string propertyName, OnSquadChanged defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_modification = Serialization.JsonArrayAsList<SquadModification>(jsonObject, "modification");
				m_validator = IEntityFilterUtils.FromJsonProperty(jsonObject, "validator");
			}
		}

		[Serializable]
		public sealed class OnTeamsInitialized : EffectTrigger
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnTeamsInitialized FromJsonToken(JToken token)
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
				OnTeamsInitialized onTeamsInitialized = new OnTeamsInitialized();
				onTeamsInitialized.PopulateFromJson(jsonObject);
				return onTeamsInitialized;
			}

			public static OnTeamsInitialized FromJsonProperty(JObject jsonObject, string propertyName, OnTeamsInitialized defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
			}
		}

		[Serializable]
		public sealed class OnThisEffectStored : EffectTrigger
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnThisEffectStored FromJsonToken(JToken token)
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
				OnThisEffectStored onThisEffectStored = new OnThisEffectStored();
				onThisEffectStored.PopulateFromJson(jsonObject);
				return onThisEffectStored;
			}

			public static OnThisEffectStored FromJsonProperty(JObject jsonObject, string propertyName, OnThisEffectStored defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
			}
		}

		[Serializable]
		public sealed class OnTurnEnded : EffectTrigger
		{
			private IEntityFilter m_validator;

			public IEntityFilter validator => m_validator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnTurnEnded FromJsonToken(JToken token)
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
				OnTurnEnded onTurnEnded = new OnTurnEnded();
				onTurnEnded.PopulateFromJson(jsonObject);
				return onTurnEnded;
			}

			public static OnTurnEnded FromJsonProperty(JObject jsonObject, string propertyName, OnTurnEnded defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_validator = IEntityFilterUtils.FromJsonProperty(jsonObject, "validator");
			}
		}

		[Serializable]
		public sealed class OnTurnStarted : EffectTrigger
		{
			private IEntityFilter m_validator;

			public IEntityFilter validator => m_validator;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static OnTurnStarted FromJsonToken(JToken token)
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
				OnTurnStarted onTurnStarted = new OnTurnStarted();
				onTurnStarted.PopulateFromJson(jsonObject);
				return onTurnStarted;
			}

			public static OnTurnStarted FromJsonProperty(JObject jsonObject, string propertyName, OnTurnStarted defaultValue = null)
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

			public override void PopulateFromJson(JObject jsonObject)
			{
				base.PopulateFromJson(jsonObject);
				m_validator = IEntityFilterUtils.FromJsonProperty(jsonObject, "validator");
			}
		}

		public override string ToString()
		{
			return GetType().Name;
		}

		public static EffectTrigger FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject val = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			JToken val2 = default(JToken);
			if (!val.TryGetValue("type", ref val2))
			{
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class EffectTrigger");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			EffectTrigger effectTrigger;
			switch (text)
			{
			case "EntityRemoval":
				effectTrigger = new EntityRemoval();
				break;
			case "AfterEntityRemoval":
				effectTrigger = new AfterEntityRemoval();
				break;
			case "OnThisEffectStored":
				effectTrigger = new OnThisEffectStored();
				break;
			case "OnEntityAction":
				effectTrigger = new OnEntityAction();
				break;
			case "OnEntityFullMovementDoneTrigger":
				effectTrigger = new OnEntityFullMovementDoneTrigger();
				break;
			case "OnEntityFinishMoveIntoArea":
				effectTrigger = new OnEntityFinishMoveIntoArea();
				break;
			case "OnEntityStartMoveFromArea":
				effectTrigger = new OnEntityStartMoveFromArea();
				break;
			case "OnEntityCharged":
				effectTrigger = new OnEntityCharged();
				break;
			case "OnEntityHurt":
				effectTrigger = new OnEntityHurt();
				break;
			case "OnEntityHealed":
				effectTrigger = new OnEntityHealed();
				break;
			case "OnEntityRemainingLifeChanged":
				effectTrigger = new OnEntityRemainingLifeChanged();
				break;
			case "OnEntityCrossedOver":
				effectTrigger = new OnEntityCrossedOver();
				break;
			case "OnArmorChanged":
				effectTrigger = new OnArmorChanged();
				break;
			case "OnCharacterAdded":
				effectTrigger = new OnCharacterAdded();
				break;
			case "OnMechanismAdded":
				effectTrigger = new OnMechanismAdded();
				break;
			case "OnTeamsInitialized":
				effectTrigger = new OnTeamsInitialized();
				break;
			case "OnTurnStarted":
				effectTrigger = new OnTurnStarted();
				break;
			case "OnTurnEnded":
				effectTrigger = new OnTurnEnded();
				break;
			case "OnDiceThrown":
				effectTrigger = new OnDiceThrown();
				break;
			case "OnReserveUsed":
				effectTrigger = new OnReserveUsed();
				break;
			case "OnReservePointChanged":
				effectTrigger = new OnReservePointChanged();
				break;
			case "OnCaracTheft":
				effectTrigger = new OnCaracTheft();
				break;
			case "ConditionalTrigger":
				effectTrigger = new ConditionalTrigger();
				break;
			case "OnSquadChanged":
				effectTrigger = new OnSquadChanged();
				break;
			case "OnAssemblingTrigger":
				effectTrigger = new OnAssemblingTrigger();
				break;
			case "OnFloatingCounterOfEffectHolderTerminated":
				effectTrigger = new OnFloatingCounterOfEffectHolderTerminated();
				break;
			case "AfterOtherSpellPlayed":
				effectTrigger = new AfterOtherSpellPlayed();
				break;
			case "AfterOtherSpellPlayedAndExecuted":
				effectTrigger = new AfterOtherSpellPlayedAndExecuted();
				break;
			case "AfterThisSpellPlayedAndExecuted":
				effectTrigger = new AfterThisSpellPlayedAndExecuted();
				break;
			case "OnSpellDrawn":
				effectTrigger = new OnSpellDrawn();
				break;
			case "OnExplosion":
				effectTrigger = new OnExplosion();
				break;
			case "OnCompanionTransfered":
				effectTrigger = new OnCompanionTransfered();
				break;
			case "OnFloorMechanismActivation":
				effectTrigger = new OnFloorMechanismActivation();
				break;
			case "AfterFloorMechanismActivation":
				effectTrigger = new AfterFloorMechanismActivation();
				break;
			case "OnSpecificEventTrigger":
				effectTrigger = new OnSpecificEventTrigger();
				break;
			case "OnPropertyApplied":
				effectTrigger = new OnPropertyApplied();
				break;
			case "OnElementaryStateApplied":
				effectTrigger = new OnElementaryStateApplied();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			effectTrigger.PopulateFromJson(val);
			return effectTrigger;
		}

		public static EffectTrigger FromJsonProperty(JObject jsonObject, string propertyName, EffectTrigger defaultValue = null)
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

		public virtual void PopulateFromJson(JObject jsonObject)
		{
		}
	}
}
