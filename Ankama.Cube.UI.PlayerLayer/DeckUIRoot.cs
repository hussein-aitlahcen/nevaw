using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Demo.UI;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Components.Tooltip;
using Ankama.Cube.UI.DeckMaker;
using Ankama.Cube.UI.Fight.Windows;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using DataEditor;
using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.PlayerLayer
{
	public class DeckUIRoot : AbstractUI, ISpellDataCellRendererConfigurator, ISpellCellRendererConfigurator, IWithTooltipCellRendererConfigurator, ICellRendererConfigurator, IWeaponDataCellRendererConfigurator
	{
		private const int m_equipWeapondTextID = 40340;

		private const int m_equippedWeapondTextID = 38149;

		private const int m_spellNameID = 33557;

		[Header("DeckPanel")]
		[SerializeField]
		private DeckPresetPanel m_presetPanel;

		[Header("SafePanel")]
		[SerializeField]
		private Transform m_safeArea;

		[Header("Buttons")]
		[SerializeField]
		private Button m_validateButton;

		[SerializeField]
		private TextField m_validateButtonText;

		[SerializeField]
		private Button m_editButton;

		[SerializeField]
		private Button m_createButton;

		[Header("Pedestal")]
		[SerializeField]
		private GameObject m_animatedCharacterRoot;

		[SerializeField]
		private EquippedFXControler m_equippedFX;

		[SerializeField]
		private ParticleSystem m_equipFXBtn;

		[SerializeField]
		private GameObjectLoader m_characterloader;

		[SerializeField]
		private WeaponCellRenderer m_pedestalWeaponCellRenderer;

		[SerializeField]
		private SpellDataCellRenderer m_pedestalSpellCellRenderer;

		[SerializeField]
		private RawTextField m_strengthField;

		[SerializeField]
		private RawTextField m_lifeField;

		[SerializeField]
		private RawTextField m_moveField;

		[Header("Weapons")]
		[SerializeField]
		private WeaponRibbonItem m_weaponUiPrefab;

		[SerializeField]
		private RectTransform m_weaponRibbon;

		[SerializeField]
		private RectTransform m_weaponContent;

		[SerializeField]
		private ScrollRect m_weaponScrollRect;

		[SerializeField]
		private GameObject m_weaponLeftArrow;

		[SerializeField]
		private GameObject m_weaponRightArrow;

		[Header("Weapon Ability and Spell")]
		[SerializeField]
		private TextField m_weaponTextField;

		[SerializeField]
		private TextField m_weaponLevelField;

		[SerializeField]
		private WeaponCellRenderer m_weaponVisual;

		[SerializeField]
		private TextField m_weaponPassiveText;

		[SerializeField]
		private SpellDataCellRenderer m_spellRenderer;

		[SerializeField]
		private Image m_spellVisual;

		[SerializeField]
		private TextField m_spellDescription;

		[SerializeField]
		private TextField m_spellName;

		[Header("Panel")]
		[SerializeField]
		private CanvasGroup m_safePanelCanvas;

		[SerializeField]
		private CanvasGroup m_weaponCanvas;

		[SerializeField]
		private CanvasGroup m_presetCanvas;

		[SerializeField]
		private CanvasGroup m_weaponListCanvas;

		[SerializeField]
		private CanvasGroup m_validateCanvas;

		[SerializeField]
		private CanvasGroup m_pedestalSpellAbilityBGCanvasGroup;

		[SerializeField]
		private CanvasGroup m_pedestalSpellAbilityMainCanvasGroup;

		[Header("Tooltips")]
		[SerializeField]
		private FightTooltip m_fightTooltip;

		[SerializeField]
		private TooltipPosition m_tooltipPosition;

		[SerializeField]
		private GenericTooltipWindow m_genericTooltipWindow;

		private int m_level = 1;

		private ImagePositionToShader m_backgroundShader;

		private WeaponDefinition m_currentWeapon;

		private List<WeaponRibbonItem> m_ribbonItems;

		private RectTransform m_pedestalSpellAbilityBgRectTransform;

		private RectTransform m_pedestalSpellAbilityMainRectTransform;

		private WeaponAndDeckModifications m_modifications;

		private DeckSlot m_emptySlot;

		private bool m_enterAnimationFinished;

		private Sequence m_pedestalTweenSequence;

		public DeckSlot selectedSlot => m_presetPanel.GetSelectedDeck();

		public FightTooltip tooltip => m_fightTooltip;

		public TooltipPosition tooltipPosition => m_tooltipPosition;

		public event Action<DeckSlot> OnEditRequest;

		public event Action OnGotoEditAnimComplete;

		public event Action<int> OnEquipWeaponRequest;

		public event Action<DeckSlot> OnSelectDeckForWeaponRequest;

		protected unsafe override void Awake()
		{
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Expected O, but got Unknown
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Expected O, but got Unknown
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Expected O, but got Unknown
			base.Awake();
			m_equippedFX.get_gameObject().SetActive(false);
			m_pedestalWeaponCellRenderer.SetConfigurator(this);
			m_pedestalSpellCellRenderer.SetConfigurator(this);
			m_spellRenderer.SetConfigurator(this);
			m_weaponVisual.SetConfigurator(this);
			m_pedestalSpellAbilityMainCanvasGroup.set_alpha(0f);
			m_pedestalSpellAbilityMainCanvasGroup.get_gameObject().SetActive(false);
			m_pedestalSpellAbilityBGCanvasGroup.set_alpha(0f);
			m_pedestalSpellAbilityBGCanvasGroup.get_gameObject().SetActive(false);
			m_pedestalSpellAbilityBgRectTransform = m_pedestalSpellAbilityBGCanvasGroup.GetComponent<RectTransform>();
			m_pedestalSpellAbilityMainRectTransform = m_pedestalSpellAbilityMainCanvasGroup.GetComponent<RectTransform>();
			m_pedestalSpellAbilityBgRectTransform.set_anchoredPosition(new Vector2(0f, 30f));
			m_pedestalSpellAbilityMainRectTransform.set_anchoredPosition(new Vector2(0f, 30f));
			m_presetPanel.OnSelectionChange += SelectDeckForWeapon;
			m_validateButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_createButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_editButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public unsafe void Initialise(WeaponAndDeckModifications modifications)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			m_modifications = modifications;
			Vector2 sizeDelta = base.canvas.GetComponent<RectTransform>().get_sizeDelta();
			sizeDelta.x *= 0.7f;
			sizeDelta.y = ((IntPtr)(void*)m_weaponRibbon.get_sizeDelta()).y;
			m_weaponRibbon.set_sizeDelta(sizeDelta);
			m_ribbonItems = new List<WeaponRibbonItem>();
			GameObject val = GameObject.FindGameObjectWithTag("DeckSelection_BG");
			m_backgroundShader = val.GetComponent<ImagePositionToShader>();
			m_presetPanel.InitialiseUI(m_modifications, OpenDeckEditState);
			m_safePanelCanvas.set_alpha(0f);
			m_weaponCanvas.set_alpha(0f);
			m_presetCanvas.set_alpha(0f);
		}

		public IEnumerator BuildWeaponList(List<WeaponDefinition> weapons)
		{
			if (m_ribbonItems == null)
			{
				m_ribbonItems = new List<WeaponRibbonItem>();
			}
			yield return null;
			int selectedWeapon = m_modifications.GetSelectedWeapon();
			for (int i = 0; i < weapons.Count; i++)
			{
				WeaponDefinition weaponDefinition = weapons[i];
				WeaponRibbonItem weaponRibbonItem;
				if (m_ribbonItems.Count > i)
				{
					weaponRibbonItem = m_ribbonItems[i];
				}
				else
				{
					weaponRibbonItem = Object.Instantiate<WeaponRibbonItem>(m_weaponUiPrefab, m_weaponUiPrefab.get_transform().get_parent());
					m_ribbonItems.Add(weaponRibbonItem);
				}
				weaponRibbonItem.get_gameObject().SetActive(true);
				weaponRibbonItem.Initialise(this, weaponDefinition);
				if (selectedWeapon == weaponDefinition.get_id())
				{
					weaponRibbonItem.ForceSelect();
				}
				SetRibbonItemScale(weaponRibbonItem);
			}
			m_weaponUiPrefab.get_gameObject().SetActive(false);
		}

		private bool IsCurrentWeapon(int weaponId)
		{
			return m_modifications.GetSelectedWeapon() == weaponId;
		}

		public void DisplayWeapon(WeaponDefinition definition)
		{
			foreach (WeaponRibbonItem ribbonItem in m_ribbonItems)
			{
				ribbonItem.OnSelectionChange(definition);
			}
			this.StartCoroutine(DisplayWeaponEnumerator(definition));
		}

		private IEnumerator DisplayWeaponEnumerator(WeaponDefinition definition)
		{
			if (!(m_currentWeapon == definition))
			{
				m_currentWeapon = definition;
				bool flag = PlayerData.instance.weaponInventory.Contains(definition.get_id());
				bool flag2 = !IsCurrentWeapon(definition.get_id()) && flag;
				m_validateButton.set_interactable(flag2);
				m_validateButtonText.SetText(flag2 ? 40340 : 38149);
				PlayerData.instance.weaponInventory.TryGetLevel(m_currentWeapon.get_id(), out m_level);
				if (m_enterAnimationFinished)
				{
					yield return PlayFadeSequence(toOut: true);
				}
				yield return LoadWeaponInfos(definition);
				BuildDeckList();
				m_backgroundShader.TweenColor(definition.deckBuildingBackgroundColor, definition.deckBuildingBackgroundColor2, 0.2f);
				AssetReference uIAnimatedCharacterReference = definition.GetUIAnimatedCharacterReference();
				CanvasGroup characterGroup = m_characterloader.GetComponent<CanvasGroup>();
				characterGroup.set_alpha(0f);
				m_characterloader.Setup(uIAnimatedCharacterReference, AssetBundlesUtility.GetUIAnimatedCharacterResourcesBundleName());
				while (m_characterloader.loadState == UIResourceLoadState.Loading)
				{
					yield return null;
				}
				DOTweenModuleUI.DOFade(characterGroup, 1f, 0.3f);
				m_equippedFX.SetEquipped(IsCurrentWeapon(definition.get_id()));
				if (m_enterAnimationFinished)
				{
					yield return PlayFadeSequence(toOut: false);
				}
			}
		}

		private IEnumerator PlayFadeSequence(bool toOut)
		{
			if (toOut)
			{
				yield return PlayAnimation(m_animationDirector.GetAnimation("FadeOut"));
			}
			else
			{
				yield return PlayAnimation(m_animationDirector.GetAnimation("FadeIn"));
			}
		}

		public void BuildDeckList()
		{
			m_emptySlot = null;
			List<DeckSlot> list = CreateDeckSlots();
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].isAvailableEmptyDeckSlot)
				{
					m_emptySlot = list[i];
					break;
				}
			}
			m_createButton.set_interactable(m_emptySlot != null);
			m_presetPanel.Populate(list, m_currentWeapon);
		}

		private IEnumerator LoadWeaponInfos(WeaponDefinition definition)
		{
			AssetReference weaponIllustrationReference = definition.GetWeaponIllustrationReference();
			m_weaponTextField.SetText(definition.i18nNameId);
			m_weaponLevelField.SetText(68066, new IndexedValueProvider(m_level.ToString()));
			AssetLoadRequest<Sprite> assetReferenceRequest = weaponIllustrationReference.LoadFromAssetBundleAsync<Sprite>(AssetBundlesUtility.GetUICharacterResourcesBundleName());
			while (!assetReferenceRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(assetReferenceRequest.get_error()) != 0)
			{
				Log.Error($"Error while loading illustration for {((object)definition).GetType().Name} {definition.get_name()} error={assetReferenceRequest.get_error()}", 305, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Player\\DeckRoot\\DeckUIRoot.cs");
				yield break;
			}
			WeaponData value = new WeaponData(definition, m_level);
			m_weaponVisual.SetValue(value);
			m_weaponPassiveText.SetText(definition.i18nDescriptionId, new FightValueProvider(definition, m_level));
			m_pedestalWeaponCellRenderer.SetValue(value);
			if (definition != null)
			{
				List<Id<SpellDefinition>> list = definition.spells.ToList();
				if (list.Count != 0 && RuntimeData.spellDefinitions.TryGetValue(list[0].value, out SpellDefinition value2))
				{
					SpellData spellData = new SpellData(value2, m_level);
					m_pedestalSpellCellRenderer.SetValue(spellData);
					m_equippedFX.SetElement(value2.element);
					m_spellRenderer.SetValue(new SpellData(value2, m_level));
					string text = RuntimeData.FormattedText(spellData.definition.i18nNameId);
					m_spellName.SetText(33557, new IndexedValueProvider(text));
					m_spellDescription.SetText(spellData.definition.i18nDescriptionId, new FightValueProvider(spellData.definition, m_level));
					int valueWithLevel = definition.movementPoints.GetValueWithLevel(m_level);
					int valueWithLevel2 = definition.life.GetValueWithLevel(m_level);
					int valueWithLevel3 = definition.actionValue.GetValueWithLevel(m_level);
					m_strengthField.SetText(valueWithLevel3.ToString());
					m_lifeField.SetText(valueWithLevel2.ToString());
					m_moveField.SetText(valueWithLevel.ToString());
				}
			}
		}

		private void OnSpellDataReceived(Sprite sprite, string loadedBundleName)
		{
			m_spellVisual.set_sprite(sprite);
		}

		public unsafe IEnumerator PlayEnterAnimation(List<WeaponDefinition> weapons)
		{
			yield return BuildWeaponList(weapons);
			yield return (object)new WaitForEndOfFrame();
			if (((IntPtr)(void*)m_weaponContent.get_sizeDelta()).x < ((IntPtr)(void*)m_weaponRibbon.get_sizeDelta()).x)
			{
				Vector2 sizeDelta = m_weaponContent.get_sizeDelta();
				sizeDelta.x += 10f;
				sizeDelta.y = ((IntPtr)(void*)m_weaponRibbon.get_sizeDelta()).y;
				m_weaponRibbon.set_sizeDelta(sizeDelta);
				Vector3 localPosition = m_weaponContent.get_transform().get_localPosition();
				localPosition.x = 0f;
				m_weaponContent.get_transform().set_localPosition(localPosition);
				m_weaponScrollRect.set_enabled(false);
				m_weaponLeftArrow.SetActive(false);
				m_weaponRightArrow.SetActive(false);
			}
			yield return PlayAnimation(m_animationDirector.GetAnimation("Open"));
			m_enterAnimationFinished = true;
		}

		public unsafe IEnumerator CloseUI()
		{
			m_presetPanel.Unload();
			DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, 0.25f);
			yield return PlayAnimation(m_animationDirector.GetAnimation("Close"));
		}

		public void Close()
		{
			PlayerIconRoot.instance.ReducePanel();
		}

		private void OpenDeckEditState(DeckSlot slot)
		{
			this.OnEditRequest?.Invoke(slot);
		}

		public unsafe void GotoEditAnim()
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			this.StartCoroutine(PlayFadeSequence(toOut: true));
			DisplayPedestalInfo(display: true);
			Sequence obj = DOTween.Sequence();
			TweenSettingsExtensions.Append(obj, DOTweenModuleUI.DOFade(m_weaponListCanvas, 0f, 0.3f));
			TweenSettingsExtensions.Insert(obj, 0f, DOTweenModuleUI.DOFade(m_validateCanvas, 0f, 0.3f));
			TweenSettingsExtensions.OnKill<Sequence>(obj, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public void BackFromEditAnim()
		{
			base.interactable = true;
			this.StartCoroutine(PlayAnimation(m_animationDirector.GetAnimation("BackFromEdition")));
			DisplayPedestalInfo(display: false);
			BuildDeckList();
		}

		public void EditDeck()
		{
			DeckSlot selectedSlot = this.selectedSlot;
			OpenDeckEditState(selectedSlot);
		}

		private void CreateDeckForWeapon()
		{
			if (m_emptySlot != null)
			{
				OpenDeckEditState(m_emptySlot);
			}
		}

		private List<DeckSlot> CreateDeckSlots()
		{
			List<DeckSlot> list = new List<DeckSlot>();
			God god = PlayerData.instance.god;
			int id = m_currentWeapon.get_id();
			if (RuntimeData.squadDefinitions.TryGetValue(m_currentWeapon.defaultDeck.value, out SquadDefinition value))
			{
				DeckInfo deckInfo = value.ToDeckInfo();
				deckInfo.Id = -value.get_id();
				list.Add(new DeckSlot(deckInfo.FillEmptySlotsCopy(), preconstructed: true));
			}
			foreach (DeckInfo deck in PlayerData.instance.GetDecks())
			{
				if (deck.God == (int)god && deck.Weapon == id)
				{
					list.Add(new DeckSlot(deck.Clone().FillEmptySlotsCopy()));
					if (list.Count >= 4)
					{
						break;
					}
				}
			}
			int i = list.Count;
			for (int num = 4; i < num; i++)
			{
				DeckInfo deckInfo2 = new DeckInfo().FillEmptySlotsCopy();
				deckInfo2.Name = RuntimeData.FormattedText(92537);
				deckInfo2.God = (int)god;
				deckInfo2.Weapon = id;
				list.Add(new DeckSlot(deckInfo2));
			}
			return list;
		}

		public void ValidateSelection()
		{
			this.OnEquipWeaponRequest?.Invoke(m_currentWeapon.get_id());
			m_equipFXBtn.Play();
		}

		public void SelectDeckForWeapon(DeckSlot slot)
		{
			this.OnSelectDeckForWeaponRequest?.Invoke(slot);
		}

		public void OnEquippedDeckUpdate()
		{
			m_presetPanel.OnEquippedDeckUpdate();
		}

		public void OnSelectedWeaponUpdate()
		{
			foreach (WeaponRibbonItem ribbonItem in m_ribbonItems)
			{
				SetRibbonItemScale(ribbonItem);
			}
			bool flag = !IsCurrentWeapon(m_currentWeapon.get_id());
			m_validateButton.set_interactable(flag);
			m_validateButtonText.SetText(flag ? 40340 : 38149);
			m_equippedFX.SetEquipped(IsCurrentWeapon(m_currentWeapon.get_id()));
		}

		private void SetRibbonItemScale(WeaponRibbonItem ribbonItem)
		{
			ribbonItem.SetEquiped(m_currentWeapon == ribbonItem.GetWeapon());
		}

		public bool IsWeaponDataAvailable(WeaponData data)
		{
			return true;
		}

		public bool IsSpellDataAvailable(SpellData data)
		{
			return true;
		}

		private unsafe void DisplayPedestalInfo(bool display)
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Expected O, but got Unknown
			Sequence pedestalTweenSequence = m_pedestalTweenSequence;
			if (pedestalTweenSequence != null)
			{
				TweenExtensions.Kill(pedestalTweenSequence, false);
			}
			Sequence val = DOTween.Sequence();
			if (display)
			{
				m_pedestalSpellAbilityBGCanvasGroup.get_gameObject().SetActive(true);
				m_pedestalSpellAbilityMainCanvasGroup.get_gameObject().SetActive(true);
				TweenSettingsExtensions.Insert(val, 0f, DOTweenModuleUI.DOFade(m_pedestalSpellAbilityMainCanvasGroup, 1f, 0.2f));
				TweenSettingsExtensions.Insert(val, 0f, DOTweenModuleUI.DOFade(m_pedestalSpellAbilityBGCanvasGroup, 1f, 0.2f));
				TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos(m_pedestalSpellAbilityBgRectTransform, Vector2.get_zero(), 0.2f, false), 7));
				TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos(m_pedestalSpellAbilityMainRectTransform, Vector2.get_zero(), 0.2f, false), 7));
			}
			else
			{
				TweenSettingsExtensions.Insert(val, 0f, DOTweenModuleUI.DOFade(m_pedestalSpellAbilityMainCanvasGroup, 0f, 0.2f));
				TweenSettingsExtensions.Insert(val, 0f, DOTweenModuleUI.DOFade(m_pedestalSpellAbilityBGCanvasGroup, 0f, 0.2f));
				TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos(m_pedestalSpellAbilityBgRectTransform, new Vector2(0f, 30f), 0.2f, false), 7));
				TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos(m_pedestalSpellAbilityMainRectTransform, new Vector2(0f, 30f), 0.2f, false), 7));
				TweenSettingsExtensions.OnKill<Sequence>(val, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			m_pedestalTweenSequence = val;
		}

		private void HidePedestalInfos()
		{
			m_pedestalSpellAbilityBGCanvasGroup.get_gameObject().SetActive(false);
			m_pedestalSpellAbilityMainCanvasGroup.get_gameObject().SetActive(false);
			m_fightTooltip.Hide();
		}

		public int GetCurrentWeaponID()
		{
			return m_currentWeapon.get_id();
		}
	}
}
