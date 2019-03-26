using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class AbstractEffectExecutionDefinition : IEditableContent
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public static AbstractEffectExecutionDefinition FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class AbstractEffectExecutionDefinition");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			AbstractEffectExecutionDefinition abstractEffectExecutionDefinition;
			switch (text)
			{
			case "MultipleEffectExecutionDefinition":
				abstractEffectExecutionDefinition = new MultipleEffectExecutionDefinition();
				break;
			case "PhysicalDamageEffectDefinition":
				abstractEffectExecutionDefinition = new PhysicalDamageEffectDefinition();
				break;
			case "MagicalDamageEffectDefinition":
				abstractEffectExecutionDefinition = new MagicalDamageEffectDefinition();
				break;
			case "LifeLeechEffectDefinition":
				abstractEffectExecutionDefinition = new LifeLeechEffectDefinition();
				break;
			case "ExplosionEffectDefinition":
				abstractEffectExecutionDefinition = new ExplosionEffectDefinition();
				break;
			case "HealEffectDefinition":
				abstractEffectExecutionDefinition = new HealEffectDefinition();
				break;
			case "CaracChangedEffectDefinition":
				abstractEffectExecutionDefinition = new CaracChangedEffectDefinition();
				break;
			case "StoppableCaracChangedEffectDefinition":
				abstractEffectExecutionDefinition = new StoppableCaracChangedEffectDefinition();
				break;
			case "AddSpellInGameEffectDefinition":
				abstractEffectExecutionDefinition = new AddSpellInGameEffectDefinition();
				break;
			case "DrawSpellEffectDefinition":
				abstractEffectExecutionDefinition = new DrawSpellEffectDefinition();
				break;
			case "DiscardSpellEffectDefinition":
				abstractEffectExecutionDefinition = new DiscardSpellEffectDefinition();
				break;
			case "DiscardSpellAndDrawEffectDefinition":
				abstractEffectExecutionDefinition = new DiscardSpellAndDrawEffectDefinition();
				break;
			case "ThrowDiceEffectDefinition":
				abstractEffectExecutionDefinition = new ThrowDiceEffectDefinition();
				break;
			case "ReturnSpellToHandEffectDefinition":
				abstractEffectExecutionDefinition = new ReturnSpellToHandEffectDefinition();
				break;
			case "TeleportEffectDefinition":
				abstractEffectExecutionDefinition = new TeleportEffectDefinition();
				break;
			case "SwapPositionsEffectDefinition":
				abstractEffectExecutionDefinition = new SwapPositionsEffectDefinition();
				break;
			case "MoveInLineEffectDefinition":
				abstractEffectExecutionDefinition = new MoveInLineEffectDefinition();
				break;
			case "ChargeEffectDefinition":
				abstractEffectExecutionDefinition = new ChargeEffectDefinition();
				break;
			case "InvokeCreatureEffectDefinition":
				abstractEffectExecutionDefinition = new InvokeCreatureEffectDefinition();
				break;
			case "DuplicateSummoningEffectDefinition":
				abstractEffectExecutionDefinition = new DuplicateSummoningEffectDefinition();
				break;
			case "ResetActionEffectDefinition":
				abstractEffectExecutionDefinition = new ResetActionEffectDefinition();
				break;
			case "PropertyChangeEffectDefinition":
				abstractEffectExecutionDefinition = new PropertyChangeEffectDefinition();
				break;
			case "SpellCostModifierEffect":
				abstractEffectExecutionDefinition = new SpellCostModifierEffect();
				break;
			case "TransformationEffectDefinition":
				abstractEffectExecutionDefinition = new TransformationEffectDefinition();
				break;
			case "GrowEffectDefinition":
				abstractEffectExecutionDefinition = new GrowEffectDefinition();
				break;
			case "ElementStateChangeEffectDefinition":
				abstractEffectExecutionDefinition = new ElementStateChangeEffectDefinition();
				break;
			case "SetNonHealableLifeEffectDefinition":
				abstractEffectExecutionDefinition = new SetNonHealableLifeEffectDefinition();
				break;
			case "RegisterDamageProtectorEffectDefinition":
				abstractEffectExecutionDefinition = new RegisterDamageProtectorEffectDefinition();
				break;
			case "ToggleElementaryStateEffectDefinition":
				abstractEffectExecutionDefinition = new ToggleElementaryStateEffectDefinition();
				break;
			case "PlayEntityAnimationEffectDefinition":
				abstractEffectExecutionDefinition = new PlayEntityAnimationEffectDefinition();
				break;
			case "ChangeEntitySkinEffectDefinition":
				abstractEffectExecutionDefinition = new ChangeEntitySkinEffectDefinition();
				break;
			case "RemoveEntityEffectDefinition":
				abstractEffectExecutionDefinition = new RemoveEntityEffectDefinition();
				break;
			case "ReturnCompanionToHandEffectDefinition":
				abstractEffectExecutionDefinition = new ReturnCompanionToHandEffectDefinition();
				break;
			case "FloatingCounterModificationEffectDefinition":
				abstractEffectExecutionDefinition = new FloatingCounterModificationEffectDefinition();
				break;
			case "ActivateFloorMechanismEffectDefinition":
				abstractEffectExecutionDefinition = new ActivateFloorMechanismEffectDefinition();
				break;
			case "StealCaracEffectDefinition":
				abstractEffectExecutionDefinition = new StealCaracEffectDefinition();
				break;
			case "TriggerFightActionEffectDefinition":
				abstractEffectExecutionDefinition = new TriggerFightActionEffectDefinition();
				break;
			case "ResurrectCompanionsEffectDefinition":
				abstractEffectExecutionDefinition = new ResurrectCompanionsEffectDefinition();
				break;
			case "ThrowSpecificEventTrigger":
				abstractEffectExecutionDefinition = new ThrowSpecificEventTrigger();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			abstractEffectExecutionDefinition.PopulateFromJson(val);
			return abstractEffectExecutionDefinition;
		}

		public static AbstractEffectExecutionDefinition FromJsonProperty(JObject jsonObject, string propertyName, AbstractEffectExecutionDefinition defaultValue = null)
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
