using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.States;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Fight
{
	public static class FightCastManager
	{
		public delegate void OnTargetChangeDelegate(bool hasTarget, CellObject cellObject);

		public delegate void OnUserActionEndDelegate(FightCastState state);

		public enum CurrentCastType
		{
			None,
			Spell,
			Companion
		}

		private static CurrentCastType s_currentCastType;

		private static PlayerStatus s_playerCasting;

		private static SpellStatus s_spellBeingCast;

		private static ReserveCompanionStatus s_companionBeingInvoked;

		private static ICastTargetDefinition s_castTargetDefinition;

		private static CastTargetContext s_castTargetContext;

		public static CurrentCastType currentCastType => s_currentCastType;

		public static event OnTargetChangeDelegate OnTargetChange;

		public static event OnUserActionEndDelegate OnUserActionEnd;

		public static bool StartCastingSpell(PlayerStatus casterStatus, SpellStatus spellStatus)
		{
			if (s_currentCastType != 0)
			{
				Log.Error($"Tried to start casting a spell while current cast type is {s_currentCastType}", 59, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightCastManager.cs");
				return false;
			}
			SpellDefinition definition = spellStatus.definition;
			if (null == definition)
			{
				Log.Error("Tried to start casting a spell without a loaded definition.", 66, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightCastManager.cs");
				return false;
			}
			ICastTargetDefinition castTarget = definition.castTarget;
			if (castTarget == null)
			{
				Log.Error("Tried to cast a spell that has no cast target definition.", 73, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightCastManager.cs");
				return false;
			}
			CastTargetContext castTargetContext = castTarget.CreateCastTargetContext(FightStatus.local, casterStatus.id, DynamicValueHolderType.Spell, definition.get_id(), spellStatus.level, spellStatus.instanceId);
			IReadOnlyList<Cost> costs = definition.costs;
			int count = costs.Count;
			for (int i = 0; i < count; i++)
			{
				if (costs[i].CheckValidity(casterStatus, castTargetContext) != 0)
				{
					Log.Error("Tried to cast a spell but one cost requirement is not met.", 86, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightCastManager.cs");
					return false;
				}
			}
			FightMap current = FightMap.current;
			if (null != current)
			{
				FightMap fightMap = current;
				fightMap.onTargetChanged = (Action<Target?, CellObject>)Delegate.Combine(fightMap.onTargetChanged, new Action<Target?, CellObject>(OnSpellTargetChanged));
				FightMap fightMap2 = current;
				fightMap2.onTargetSelected = (Action<Target?>)Delegate.Combine(fightMap2.onTargetSelected, new Action<Target?>(OnSpellTargetSelected));
				current.SetTargetingPhase(castTarget.EnumerateTargets(castTargetContext));
			}
			s_currentCastType = CurrentCastType.Spell;
			s_playerCasting = casterStatus;
			s_spellBeingCast = spellStatus;
			s_castTargetDefinition = castTarget;
			s_castTargetContext = castTargetContext;
			ShowSpellCostsPreview();
			return true;
		}

		private static void OnSpellTargetChanged(Target? target, [CanBeNull] CellObject cellObject)
		{
			FightCastManager.OnTargetChange?.Invoke(target.HasValue, cellObject);
		}

		private static void OnSpellTargetSelected(Target? targetOpt)
		{
			if (targetOpt.HasValue)
			{
				Target value = targetOpt.Value;
				s_castTargetContext.SelectTarget(value);
				int selectedTargetCount = s_castTargetContext.selectedTargetCount;
				if (selectedTargetCount == 1)
				{
					FightCastManager.OnUserActionEnd?.Invoke(FightCastState.Targeting);
				}
				if (selectedTargetCount >= s_castTargetContext.expectedTargetCount)
				{
					s_castTargetContext.SendCommand();
					FightCastManager.OnUserActionEnd?.Invoke(FightCastState.Casting);
				}
				else
				{
					FightMap.current.SetTargetingPhase(s_castTargetDefinition.EnumerateTargets(s_castTargetContext));
				}
			}
			else
			{
				StopCastingSpell(cancelled: true);
			}
		}

		public static void StopCastingSpell(bool cancelled)
		{
			if (s_currentCastType != CurrentCastType.Spell)
			{
				Log.Error($"Tried to stop casting a spell while current cast type is {s_currentCastType}", 155, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightCastManager.cs");
				return;
			}
			if (cancelled)
			{
				FightCastManager.OnUserActionEnd?.Invoke(FightCastState.Cancelled);
			}
			else
			{
				FightCastManager.OnUserActionEnd?.Invoke(FightCastState.DoneCasting);
			}
			HideSpellCostsPreview(cancelled);
			FightMap current = FightMap.current;
			if (null != current)
			{
				FightMap fightMap = current;
				fightMap.onTargetChanged = (Action<Target?, CellObject>)Delegate.Remove(fightMap.onTargetChanged, new Action<Target?, CellObject>(OnSpellTargetChanged));
				FightMap fightMap2 = current;
				fightMap2.onTargetSelected = (Action<Target?>)Delegate.Remove(fightMap2.onTargetSelected, new Action<Target?>(OnSpellTargetSelected));
				RevertFightMapTargetingPhase(current);
			}
			s_currentCastType = CurrentCastType.None;
			s_playerCasting = null;
			s_spellBeingCast = null;
			s_castTargetDefinition = null;
			s_castTargetContext = null;
		}

		public static void CheckSpellPlayed(int spellInstanceId)
		{
			if (s_currentCastType == CurrentCastType.Spell && s_spellBeingCast.instanceId == spellInstanceId)
			{
				StopCastingSpell(cancelled: false);
			}
		}

		public static bool StartInvokingCompanion(PlayerStatus casterStatus, ReserveCompanionStatus companionStatus)
		{
			if (s_currentCastType != 0)
			{
				Log.Error($"Tried to start invoking a companion while current cast type is {s_currentCastType}", 208, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightCastManager.cs");
				return false;
			}
			CompanionDefinition definition = companionStatus.definition;
			if (null == definition)
			{
				Log.Error("Tried to start invoking a companion without a loaded definition.", 215, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightCastManager.cs");
				return false;
			}
			OneCastTargetContext castTargetContext = new OneCastTargetContext(FightStatus.local, casterStatus.id, DynamicValueHolderType.Companion, definition.get_id(), companionStatus.level, 0);
			FightMap current = FightMap.current;
			if (null != current)
			{
				ICoordSelector spawnLocation = definition.spawnLocation;
				if (spawnLocation == null)
				{
					Log.Error("Tried to start invoking a companion that has no spawn location.", 227, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightCastManager.cs");
					return false;
				}
				FightMap fightMap = current;
				fightMap.onTargetChanged = (Action<Target?, CellObject>)Delegate.Combine(fightMap.onTargetChanged, new Action<Target?, CellObject>(OnCompanionInvocationLocationChanged));
				FightMap fightMap2 = current;
				fightMap2.onTargetSelected = (Action<Target?>)Delegate.Combine(fightMap2.onTargetSelected, new Action<Target?>(OnCompanionInvocationLocationSelected));
				current.SetTargetingPhase(EnumerateCompanionAvailableLocations(spawnLocation, castTargetContext));
			}
			s_currentCastType = CurrentCastType.Companion;
			s_playerCasting = casterStatus;
			s_companionBeingInvoked = companionStatus;
			s_castTargetContext = castTargetContext;
			ShowCompanionCostsPreview();
			return true;
		}

		private static void OnCompanionInvocationLocationChanged(Target? target, [CanBeNull] CellObject cellObject)
		{
			FightCastManager.OnTargetChange?.Invoke(target.HasValue, cellObject);
		}

		private static void OnCompanionInvocationLocationSelected(Target? targetOpt)
		{
			if (targetOpt.HasValue)
			{
				FightState instance = FightState.instance;
				bool flag = false;
				if (instance != null)
				{
					FightFrame frame = instance.frame;
					if (frame != null)
					{
						Target value = targetOpt.Value;
						frame.SendInvokeCompanion(s_companionBeingInvoked.definition.get_id(), value.coord);
						flag = true;
					}
				}
				if (flag)
				{
					FightCastManager.OnUserActionEnd?.Invoke(FightCastState.Casting);
				}
				else
				{
					StopInvokingCompanion(cancelled: true);
				}
			}
			else
			{
				StopInvokingCompanion(cancelled: true);
			}
		}

		public static void StopInvokingCompanion(bool cancelled)
		{
			if (s_currentCastType != CurrentCastType.Companion)
			{
				Log.Error($"Tried to stop casting a spell while current cast type is {s_currentCastType}", 285, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightCastManager.cs");
				return;
			}
			FightCastManager.OnUserActionEnd?.Invoke(cancelled ? FightCastState.Cancelled : FightCastState.DoneCasting);
			HideCompanionCostsPreview(cancelled);
			FightMap current = FightMap.current;
			if (null != current)
			{
				FightMap fightMap = current;
				fightMap.onTargetChanged = (Action<Target?, CellObject>)Delegate.Remove(fightMap.onTargetChanged, new Action<Target?, CellObject>(OnCompanionInvocationLocationChanged));
				FightMap fightMap2 = current;
				fightMap2.onTargetSelected = (Action<Target?>)Delegate.Remove(fightMap2.onTargetSelected, new Action<Target?>(OnCompanionInvocationLocationSelected));
				RevertFightMapTargetingPhase(current);
			}
			s_currentCastType = CurrentCastType.None;
			s_playerCasting = null;
			s_companionBeingInvoked = null;
			s_castTargetContext = null;
		}

		public static void CheckCompanionInvoked(int companionDefinitionId)
		{
			if (s_currentCastType == CurrentCastType.Companion && s_companionBeingInvoked.definition.get_id() == companionDefinitionId)
			{
				StopInvokingCompanion(cancelled: false);
			}
		}

		private static IEnumerable<Target> EnumerateCompanionAvailableLocations(ICoordSelector spawnLocation, CastTargetContext castTargetContext)
		{
			foreach (Coord item in spawnLocation.EnumerateCoords(castTargetContext))
			{
				yield return new Target(item);
			}
		}

		private static void RevertFightMapTargetingPhase(FightMap fightMap)
		{
			FightStatus local = FightStatus.local;
			if (local != null && local.currentTurnPlayerId == s_playerCasting.id)
			{
				if (fightMap.IsInTargetingPhase())
				{
					fightMap.SetMovementPhase();
				}
			}
			else
			{
				fightMap.EndCurrentPhase();
			}
		}

		private static void ShowSpellCostsPreview()
		{
			CastTargetContext context = s_spellBeingCast.CreateCastTargetContext();
			PreviewCosts(s_spellBeingCast.definition.costs, context);
		}

		private static void HideSpellCostsPreview(bool cancelled)
		{
			FightUIRework.instance.GetLocalPlayerUI(s_playerCasting).HideAllPreviews(cancelled);
		}

		private static void ShowCompanionCostsPreview()
		{
			ReserveCompanionValueContext context = s_companionBeingInvoked.CreateValueContext();
			PreviewCosts(s_companionBeingInvoked.definition.cost, context);
		}

		private static void HideCompanionCostsPreview(bool cancelled)
		{
			FightUIRework.instance.GetLocalPlayerUI(s_playerCasting).HideAllPreviews(cancelled);
		}

		private static void PreviewCosts(IReadOnlyList<Cost> costs, DynamicValueFightContext context)
		{
			LocalPlayerUIRework localPlayerUI = FightUIRework.instance.GetLocalPlayerUI(s_playerCasting);
			for (int i = 0; i < costs.Count; i++)
			{
				Cost cost = costs[i];
				if (cost == null)
				{
					continue;
				}
				ActionPointsCost actionPointsCost;
				int value3;
				if ((actionPointsCost = (cost as ActionPointsCost)) == null)
				{
					if (!(cost is DrainActionPointsCost))
					{
						ElementPointsCost elementPointsCost;
						if ((elementPointsCost = (cost as ElementPointsCost)) == null)
						{
							DrainElementsPoints drainElementsPoints;
							if ((drainElementsPoints = (cost as DrainElementsPoints)) == null)
							{
								ReservePointsCost reservePointsCost;
								int value;
								if ((reservePointsCost = (cost as ReservePointsCost)) == null)
								{
									if (cost is DrainReservePointsCost)
									{
										localPlayerUI.PreviewReservePoints(0, ValueModifier.Set);
									}
								}
								else if (reservePointsCost.value.GetValue(context, out value))
								{
									localPlayerUI.PreviewReservePoints(value, ValueModifier.Add);
								}
							}
							else
							{
								DrainElementsPoints drainElementsPoints2 = drainElementsPoints;
								int count = drainElementsPoints2.elements.Count;
								for (int j = 0; j < count; j++)
								{
									PreviewElementaryPoints(localPlayerUI, drainElementsPoints2.elements[j], 0, ValueModifier.Set);
								}
							}
						}
						else
						{
							ElementPointsCost elementPointsCost2 = elementPointsCost;
							if (elementPointsCost2.value.GetValue(context, out int value2))
							{
								PreviewElementaryPoints(localPlayerUI, elementPointsCost2.element, value2, ValueModifier.Add);
							}
						}
					}
					else
					{
						localPlayerUI.PreviewActionPoints(0, ValueModifier.Set);
					}
				}
				else if (actionPointsCost.value.GetValue(context, out value3))
				{
					localPlayerUI.PreviewActionPoints(value3, ValueModifier.Add);
				}
			}
		}

		private static void PreviewElementaryPoints(LocalPlayerUIRework localPlayerUI, CaracId element, int value, ValueModifier modifier)
		{
			switch (element)
			{
			case CaracId.FirePoints:
				localPlayerUI.ShowPreviewFire(value, modifier);
				break;
			case CaracId.WaterPoints:
				localPlayerUI.ShowPreviewWater(value, modifier);
				break;
			case CaracId.EarthPoints:
				localPlayerUI.ShowPreviewEarth(value, modifier);
				break;
			case CaracId.AirPoints:
				localPlayerUI.ShowPreviewAir(value, modifier);
				break;
			default:
				throw new ArgumentOutOfRangeException("element", element, null);
			}
		}
	}
}
