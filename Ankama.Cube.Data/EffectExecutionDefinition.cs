using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class EffectExecutionDefinition : AbstractEffectExecutionDefinition
	{
		protected EffectCondition m_condition;

		protected ITargetSelector m_executionTargetSelector;

		protected bool m_groupExecutionOnTargets;

		public EffectCondition condition => m_condition;

		public ITargetSelector executionTargetSelector => m_executionTargetSelector;

		public bool groupExecutionOnTargets => m_groupExecutionOnTargets;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static EffectExecutionDefinition FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class EffectExecutionDefinition");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			EffectExecutionDefinition effectExecutionDefinition;
			switch (text)
			{
			case "ExplosionEffectDefinition":
				effectExecutionDefinition = new ExplosionEffectDefinition();
				break;
			case "HealEffectDefinition":
				effectExecutionDefinition = new HealEffectDefinition();
				break;
			case "CaracChangedEffectDefinition":
				effectExecutionDefinition = new CaracChangedEffectDefinition();
				break;
			case "AddSpellInGameEffectDefinition":
				effectExecutionDefinition = new AddSpellInGameEffectDefinition();
				break;
			case "DrawSpellEffectDefinition":
				effectExecutionDefinition = new DrawSpellEffectDefinition();
				break;
			case "DiscardSpellEffectDefinition":
				effectExecutionDefinition = new DiscardSpellEffectDefinition();
				break;
			case "DiscardSpellAndDrawEffectDefinition":
				effectExecutionDefinition = new DiscardSpellAndDrawEffectDefinition();
				break;
			case "ThrowDiceEffectDefinition":
				effectExecutionDefinition = new ThrowDiceEffectDefinition();
				break;
			case "ReturnSpellToHandEffectDefinition":
				effectExecutionDefinition = new ReturnSpellToHandEffectDefinition();
				break;
			case "TeleportEffectDefinition":
				effectExecutionDefinition = new TeleportEffectDefinition();
				break;
			case "SwapPositionsEffectDefinition":
				effectExecutionDefinition = new SwapPositionsEffectDefinition();
				break;
			case "MoveInLineEffectDefinition":
				effectExecutionDefinition = new MoveInLineEffectDefinition();
				break;
			case "ChargeEffectDefinition":
				effectExecutionDefinition = new ChargeEffectDefinition();
				break;
			case "InvokeCreatureEffectDefinition":
				effectExecutionDefinition = new InvokeCreatureEffectDefinition();
				break;
			case "DuplicateSummoningEffectDefinition":
				effectExecutionDefinition = new DuplicateSummoningEffectDefinition();
				break;
			case "ResetActionEffectDefinition":
				effectExecutionDefinition = new ResetActionEffectDefinition();
				break;
			case "TransformationEffectDefinition":
				effectExecutionDefinition = new TransformationEffectDefinition();
				break;
			case "GrowEffectDefinition":
				effectExecutionDefinition = new GrowEffectDefinition();
				break;
			case "ElementStateChangeEffectDefinition":
				effectExecutionDefinition = new ElementStateChangeEffectDefinition();
				break;
			case "SetNonHealableLifeEffectDefinition":
				effectExecutionDefinition = new SetNonHealableLifeEffectDefinition();
				break;
			case "ToggleElementaryStateEffectDefinition":
				effectExecutionDefinition = new ToggleElementaryStateEffectDefinition();
				break;
			case "PlayEntityAnimationEffectDefinition":
				effectExecutionDefinition = new PlayEntityAnimationEffectDefinition();
				break;
			case "RemoveEntityEffectDefinition":
				effectExecutionDefinition = new RemoveEntityEffectDefinition();
				break;
			case "ReturnCompanionToHandEffectDefinition":
				effectExecutionDefinition = new ReturnCompanionToHandEffectDefinition();
				break;
			case "ActivateFloorMechanismEffectDefinition":
				effectExecutionDefinition = new ActivateFloorMechanismEffectDefinition();
				break;
			case "StealCaracEffectDefinition":
				effectExecutionDefinition = new StealCaracEffectDefinition();
				break;
			case "TriggerFightActionEffectDefinition":
				effectExecutionDefinition = new TriggerFightActionEffectDefinition();
				break;
			case "ResurrectCompanionsEffectDefinition":
				effectExecutionDefinition = new ResurrectCompanionsEffectDefinition();
				break;
			case "ThrowSpecificEventTrigger":
				effectExecutionDefinition = new ThrowSpecificEventTrigger();
				break;
			case "PhysicalDamageEffectDefinition":
				effectExecutionDefinition = new PhysicalDamageEffectDefinition();
				break;
			case "MagicalDamageEffectDefinition":
				effectExecutionDefinition = new MagicalDamageEffectDefinition();
				break;
			case "LifeLeechEffectDefinition":
				effectExecutionDefinition = new LifeLeechEffectDefinition();
				break;
			case "StoppableCaracChangedEffectDefinition":
				effectExecutionDefinition = new StoppableCaracChangedEffectDefinition();
				break;
			case "PropertyChangeEffectDefinition":
				effectExecutionDefinition = new PropertyChangeEffectDefinition();
				break;
			case "SpellCostModifierEffect":
				effectExecutionDefinition = new SpellCostModifierEffect();
				break;
			case "RegisterDamageProtectorEffectDefinition":
				effectExecutionDefinition = new RegisterDamageProtectorEffectDefinition();
				break;
			case "ChangeEntitySkinEffectDefinition":
				effectExecutionDefinition = new ChangeEntitySkinEffectDefinition();
				break;
			case "FloatingCounterModificationEffectDefinition":
				effectExecutionDefinition = new FloatingCounterModificationEffectDefinition();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			effectExecutionDefinition.PopulateFromJson(val);
			return effectExecutionDefinition;
		}

		public static EffectExecutionDefinition FromJsonProperty(JObject jsonObject, string propertyName, EffectExecutionDefinition defaultValue = null)
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
			m_executionTargetSelector = ITargetSelectorUtils.FromJsonProperty(jsonObject, "executionTargetSelector");
			m_groupExecutionOnTargets = Serialization.JsonTokenValue<bool>(jsonObject, "groupExecutionOnTargets", false);
		}
	}
}
