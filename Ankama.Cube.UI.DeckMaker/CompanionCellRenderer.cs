using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Fight;
using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.UI.DeckMaker
{
	public abstract class CompanionCellRenderer<T, U> : WithTooltipCellRenderer<T, U>, ICharacterTooltipDataProvider, ITooltipDataProvider where U : ICompanionCellRendererConfigurator
	{
		[SerializeField]
		protected FightUIFactory m_factory;

		[SerializeField]
		protected ImageLoader m_companionImage;

		[SerializeField]
		protected GaugeItemUI[] m_elements;

		[SerializeField]
		protected CanvasGroup m_canvasGroup;

		private CompanionDefinitionContext m_context;

		private CompanionDefinition m_definition;

		private int m_level;

		private static readonly IReadOnlyList<Cost> s_noCost = new List<Cost>();

		private bool m_usable;

		public override TooltipDataType tooltipDataType => TooltipDataType.Character;

		public override KeywordReference[] keywordReferences => m_definition.precomputedData.keywordReferences;

		protected abstract IReadOnlyList<Cost> GetCosts();

		protected abstract bool IsAvailable();

		protected void SetValue(CompanionDefinition definition, int level)
		{
			CompanionDefinition definition2 = m_definition;
			m_definition = definition;
			m_level = level;
			if (definition != definition2)
			{
				SetIllustration(definition);
				SetCost();
				SetUsable(IsAvailable(), instant: true, force: true);
			}
			if (definition != null)
			{
				m_valueProvider = new FightValueProvider(definition, level);
				m_context = new CompanionDefinitionContext(definition, level);
			}
			else
			{
				m_valueProvider = null;
				m_context = null;
			}
		}

		protected override void Clear()
		{
			m_definition = null;
			SetIllustration(null);
			SetCost();
			SetUsable(IsAvailable(), instant: true, force: true);
		}

		private void SetIllustration(CompanionDefinition definition)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			if (!(definition == null))
			{
				AssetReference illustrationReference = definition.illustrationReference;
				if (illustrationReference.get_hasValue())
				{
					m_companionImage.Setup(definition.illustrationReference, definition.illustrationBundleName);
					return;
				}
			}
			m_companionImage.Clear();
		}

		private void SetCost()
		{
			IReadOnlyList<Cost> elements = GetCosts() ?? s_noCost;
			SetElements(elements);
		}

		private void SetElements(IReadOnlyList<Cost> costs)
		{
			int num = 0;
			int count = costs.Count;
			for (int i = 0; i < count; i++)
			{
				ElementPointsCost elementPointsCost;
				if ((elementPointsCost = (costs[i] as ElementPointsCost)) != null)
				{
					elementPointsCost.value.GetValue(m_context, out int value);
					GaugeItemUI gaugeItemUI = m_elements[num];
					gaugeItemUI.SetValue(value);
					m_factory.Initialize(gaugeItemUI, elementPointsCost.element);
					gaugeItemUI.get_gameObject().SetActive(true);
					num++;
				}
			}
			int num2 = m_elements.Length;
			for (int j = num; j < num2; j++)
			{
				m_elements[j].get_gameObject().SetActive(false);
			}
		}

		private unsafe void SetUsable(bool usable, bool instant = false, bool force = false)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			if ((m_usable != usable) | force)
			{
				m_usable = usable;
				Color val = usable ? new Color(1f, 1f, 1f, 1f) : new Color(0.5f, 0.25f, 1f, 0.6f);
				if (instant)
				{
					m_companionImage.color = val;
				}
				else
				{
					DOTween.To(new DOGetter<Color>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Color>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), val, 0.2f);
				}
			}
		}

		public override void OnConfiguratorUpdate(bool instant)
		{
			base.OnConfiguratorUpdate(instant);
			SetCost();
			SetUsable(IsAvailable(), instant);
		}

		public unsafe override Sequence DestroySequence()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Sequence obj = DOTween.Sequence();
			TweenSettingsExtensions.Insert(obj, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.get_transform(), ((IntPtr)(void*)this.get_transform().get_localPosition()).y + 40f, 0.25f, false), 3));
			TweenSettingsExtensions.Insert(obj, 0f, DOTweenModuleUI.DOFade(m_canvasGroup, 0f, 0.25f));
			return obj;
		}

		public IEnumerator WaitForImage()
		{
			while (m_companionImage.loadState == UIResourceLoadState.Loading)
			{
				yield return null;
			}
		}

		public override int GetTitleKey()
		{
			return m_definition.i18nNameId;
		}

		public override int GetDescriptionKey()
		{
			return m_definition.i18nDescriptionId;
		}

		public override IFightValueProvider GetValueProvider()
		{
			return m_valueProvider;
		}

		public ActionType GetActionType()
		{
			return m_definition.actionType;
		}

		public TooltipActionIcon GetActionIcon()
		{
			return TooltipWindowUtility.GetActionIcon(m_definition);
		}

		public bool TryGetActionValue(out int val)
		{
			ILevelOnlyDependant actionValue = m_definition.actionValue;
			if (actionValue != null)
			{
				val = actionValue.GetValueWithLevel(m_level);
				return true;
			}
			val = 0;
			return false;
		}

		public int GetLifeValue()
		{
			return m_definition.life.GetValueWithLevel(m_level);
		}

		public int GetMovementValue()
		{
			return m_definition.movementPoints.GetValueWithLevel(m_level);
		}

		public override CellRenderer Clone()
		{
			CompanionCellRenderer<T, U> obj = (CompanionCellRenderer<T, U>)base.Clone();
			obj.m_definition = m_definition;
			obj.m_level = m_level;
			return obj;
		}
	}
}
