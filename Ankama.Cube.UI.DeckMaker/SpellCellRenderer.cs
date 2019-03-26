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
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.DeckMaker
{
	public abstract class SpellCellRenderer<T, U> : WithTooltipCellRenderer<T, U>, ISpellTooltipDataProvider, ITooltipDataProvider where U : ISpellCellRendererConfigurator
	{
		[SerializeField]
		protected FightUIFactory m_factory;

		[SerializeField]
		protected ImageLoader m_spellImageLoader;

		[SerializeField]
		protected Image m_elementaryStateImage;

		[SerializeField]
		protected GameObject m_costGameObject;

		[SerializeField]
		protected UISpriteTextRenderer m_costValue;

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		[SerializeField]
		protected Image m_highlighted;

		private SpellDefinition m_definition;

		private bool m_usable;

		protected SpellDefinitionContext m_spellContext;

		public override TooltipDataType tooltipDataType => TooltipDataType.Spell;

		public override KeywordReference[] keywordReferences => m_definition.precomputedData.keywordReferences;

		protected abstract bool IsAvailable();

		protected abstract int? GetAPCost();

		protected abstract int? GetBaseAPCost();

		private void Awake()
		{
			if (Object.op_Implicit(m_highlighted))
			{
				m_highlighted.get_gameObject().SetActive(m_usable);
			}
		}

		protected void SetValue(SpellDefinition definition, int level)
		{
			SpellDefinition definition2 = m_definition;
			m_definition = definition;
			if (definition2 != definition)
			{
				SetIllustration(definition);
				SetAPCost(GetAPCost(), GetBaseAPCost());
				SetUsable(IsAvailable(), instant: true, force: true);
				if (m_highlighted != null)
				{
					Material spellSelectedMaterial = m_factory.GetSpellSelectedMaterial(definition);
					m_highlighted.set_material(spellSelectedMaterial);
				}
			}
			if (definition != null)
			{
				m_valueProvider = new FightValueProvider(definition, level);
				m_spellContext = new SpellDefinitionContext(definition, level);
			}
		}

		protected override void Clear()
		{
			m_definition = null;
			SetIllustration(null);
			SetAPCost(null, null);
			SetUsable(usable: false, instant: true, force: true);
		}

		private void SetIllustration(SpellDefinition definition)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			m_elementaryStateImage.set_enabled(false);
			if (definition != null)
			{
				AssetReference illustrationReference = definition.illustrationReference;
				if (illustrationReference.get_hasValue())
				{
					m_spellImageLoader.Setup(definition.illustrationReference, definition.illustrationBundleName);
					m_factory.Initialize(m_elementaryStateImage, definition.elementaryStates);
					return;
				}
			}
			m_spellImageLoader.Clear();
		}

		protected void SetAPCost(int? cost, int? baseCost)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			m_costGameObject.SetActive(cost.HasValue);
			if (cost.HasValue)
			{
				int value = cost.Value;
				m_costValue.text = value.ToString();
				Color white = default(Color);
				if (value == baseCost)
				{
					white = Color.get_white();
				}
				else if (value < baseCost)
				{
					white._002Ector(0.23f, 1f, 0.16f);
				}
				else
				{
					white._002Ector(1f, 0.25f, 0.18f);
				}
				m_costValue.tint = white;
			}
		}

		private unsafe void SetUsable(bool usable, bool instant = false, bool force = false)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			if ((m_usable != usable) | force)
			{
				m_usable = usable;
				Color val = usable ? new Color(1f, 1f, 1f, 1f) : new Color(0.5f, 0.25f, 1f, 0.6f);
				if (instant)
				{
					m_spellImageLoader.color = val;
				}
				else
				{
					DOTween.To(new DOGetter<Color>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Color>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), val, 0.2f);
				}
				if (Object.op_Implicit(m_highlighted))
				{
					m_highlighted.get_gameObject().SetActive(usable);
				}
			}
		}

		public override void OnConfiguratorUpdate(bool instant)
		{
			base.OnConfiguratorUpdate(instant);
			SetUsable(IsAvailable(), instant);
			SetAPCost(GetAPCost(), GetBaseAPCost());
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
			while (m_spellImageLoader.loadState == UIResourceLoadState.Loading)
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

		public TooltipElementValues GetGaugeModifications()
		{
			return TooltipWindowUtility.GetTooltipElementValues(m_definition, m_spellContext);
		}

		public override CellRenderer Clone()
		{
			SpellCellRenderer<T, U> obj = (SpellCellRenderer<T, U>)base.Clone();
			obj.m_definition = m_definition;
			return obj;
		}
	}
}
