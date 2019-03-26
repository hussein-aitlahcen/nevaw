using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.UI.DeckMaker;
using Ankama.Utilities;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	public sealed class PlaySpellCompanionUI : MonoBehaviour
	{
		[SerializeField]
		private Canvas m_fightUICanvas;

		[SerializeField]
		private CastingModeParameters m_castingModeParameters;

		[SerializeField]
		private FightUIFactory m_factory;

		[SerializeField]
		private SpellStatusCellRenderer m_spellDummy;

		[SerializeField]
		private CompanionStatusCellRenderer m_companionDummy;

		private GameObjectPool m_spellDummyPool;

		private GameObjectPool m_companionDummyPool;

		private void Awake()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			GameObject gameObject = m_spellDummy.get_gameObject();
			GameObject gameObject2 = m_companionDummy.get_gameObject();
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
			m_spellDummyPool = new GameObjectPool(gameObject);
			m_companionDummyPool = new GameObjectPool(gameObject2);
		}

		public IEnumerator ShowPlaying(SpellStatus spell, CellObject cell)
		{
			if (m_spellDummyPool == null)
			{
				Log.Warning("PlaySpellCompanionUI is inactive.", 43, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\FightRework\\PlaySpellCompanionUI.cs");
				yield break;
			}
			GameObject dummy = m_spellDummyPool.Instantiate(this.get_transform(), false);
			dummy.get_transform().set_localScale(m_spellDummy.get_transform().get_localScale());
			SpellStatusCellRenderer itemUI = dummy.GetComponent<SpellStatusCellRenderer>();
			CastableDragNDropElement dnd = dummy.GetComponent<CastableDragNDropElement>();
			itemUI.SetValue(spell);
			yield return itemUI.WaitForImage();
			yield return ShowPlaying(spell, dnd, cell);
			itemUI.SetValue(null);
			m_spellDummyPool.Release(dummy);
		}

		public IEnumerator ShowPlaying(ReserveCompanionStatus companion, CellObject cell)
		{
			if (m_companionDummyPool == null)
			{
				Log.Warning("PlaySpellCompanionUI is inactive.", 64, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\FightRework\\PlaySpellCompanionUI.cs");
				yield break;
			}
			GameObject dummy = m_companionDummyPool.Instantiate(this.get_transform(), false);
			dummy.get_transform().set_localScale(m_companionDummy.get_transform().get_localScale());
			CompanionStatusCellRenderer itemUI = dummy.GetComponent<CompanionStatusCellRenderer>();
			CastableDragNDropElement dnd = dummy.GetComponent<CastableDragNDropElement>();
			itemUI.SetValue(companion);
			yield return itemUI.WaitForImage();
			yield return ShowPlaying(companion, dnd, cell);
			itemUI.SetValue(null);
			m_companionDummyPool.Release(dummy);
		}

		private IEnumerator ShowPlaying(ICastableStatus item, CastableDragNDropElement dnd, CellObject cell)
		{
			dnd.get_gameObject().SetActive(true);
			CastHighlight cellHighlight = m_factory.CreateCastHighlight(item, cell.highlight.get_transform());
			Vector3 worldPosition = FightUIRework.WorldToUIWorld(cell.highlight.get_transform().get_parent().get_position());
			Tween tween = dnd.PlayCastImmediate(worldPosition, m_fightUICanvas.get_transform());
			while (tween.get_active() && !TweenExtensions.IsComplete(tween))
			{
				yield return null;
			}
			yield return (object)new WaitForTime(m_castingModeParameters.opponentPlayingDuration);
			m_factory.DestroyCellHighlight(cellHighlight);
			yield return (object)new WaitForTime(m_castingModeParameters.opponentCastPlayingDuration);
			Tween endCastImmediate = dnd.EndCastImmediate();
			while (endCastImmediate.get_active() && !TweenExtensions.IsComplete(endCastImmediate))
			{
				yield return null;
			}
			dnd.get_gameObject().SetActive(false);
		}

		public PlaySpellCompanionUI()
			: this()
		{
		}
	}
}
