using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.Demo.UI;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Components.Tooltip;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.GodSelection
{
	public class GodSelectionRoot : AbstractUI
	{
		private class GodComparer : Comparer<GodDefinition>
		{
			public override int Compare(GodDefinition x, GodDefinition y)
			{
				return x.Order - y.Order;
			}
		}

		public Action<God> onGodSelected;

		public Action onCloseClick;

		[Header("Button")]
		[SerializeField]
		private Button m_closeButton;

		[SerializeField]
		private Button m_validateButton;

		[Header("Visual")]
		[SerializeField]
		private CanvasGroup m_bgCanvas;

		[SerializeField]
		private ImageLoader m_illuLoader;

		[SerializeField]
		private ImageLoader m_statueLoader;

		[SerializeField]
		private ImagePositionToShader m_backgroundShader;

		[Header("Canvas")]
		[SerializeField]
		private CanvasGroup m_safePanelCanvas;

		[Header("Tooltips")]
		[SerializeField]
		private TooltipPosition m_tooltipPosition;

		[SerializeField]
		private GenericTooltipWindow m_genericTooltipWindow;

		[Header("Info")]
		[SerializeField]
		private TextField m_godName;

		[SerializeField]
		private TextField m_godDescription;

		[Header("BGColor")]
		[SerializeField]
		private Color m_BGColor;

		[SerializeField]
		private Color m_BGColorBorder;

		[Header("Visual")]
		[SerializeField]
		private ParticleSystem m_changeGodFX;

		[SerializeField]
		private ParticleSystem m_equipeGodFX;

		[Header("God")]
		[SerializeField]
		private GodSelectionRibbonItem m_godUiPrefab;

		[SerializeField]
		private RectTransform m_godRibbon;

		[SerializeField]
		private RectTransform m_godContent;

		[SerializeField]
		private ScrollRect m_godScrollRect;

		[SerializeField]
		private GameObject m_godLeftArrow;

		[SerializeField]
		private GameObject m_godRightArrow;

		[Header("Sounds")]
		[SerializeField]
		private UnityEvent m_onGodChange;

		private readonly List<GodDefinition> m_playableGods = new List<GodDefinition>();

		private List<GodSelectionRibbonItem> m_ribbonItems;

		private GodDefinition m_currentGod;

		protected override void Awake()
		{
			m_bgCanvas.set_alpha(0f);
			base.Awake();
			m_genericTooltipWindow.get_gameObject().SetActive(false);
		}

		public unsafe void Initialise()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			m_safePanelCanvas.set_alpha(0f);
			m_closeButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_validateButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_backgroundShader.SetColor(m_BGColor, m_BGColorBorder);
		}

		private void OnCloseClick()
		{
			onCloseClick?.Invoke();
		}

		public IEnumerator BuildGodList()
		{
			List<string> list = new List<string>();
			m_playableGods.Clear();
			God god = (PlayerData.instance == null) ? God.Iop : PlayerData.instance.god;
			int num = -1;
			foreach (GodDefinition value in RuntimeData.godDefinitions.Values)
			{
				if (value.playable)
				{
					m_playableGods.Add(value);
					list.Add(RuntimeData.FormattedText(value.i18nNameId));
				}
			}
			m_playableGods.Sort(new GodComparer());
			int num2 = 0;
			foreach (GodDefinition playableGod in m_playableGods)
			{
				if (playableGod.god == god)
				{
					num = num2;
				}
				num2++;
			}
			if (m_ribbonItems == null)
			{
				m_ribbonItems = new List<GodSelectionRibbonItem>();
			}
			IEnumerator[] array = new IEnumerator[m_playableGods.Count];
			for (int i = 0; i < m_playableGods.Count; i++)
			{
				GodDefinition definition = m_playableGods[i];
				GodSelectionRibbonItem godSelectionRibbonItem;
				if (m_ribbonItems.Count > i)
				{
					godSelectionRibbonItem = m_ribbonItems[i];
				}
				else
				{
					godSelectionRibbonItem = Object.Instantiate<GodSelectionRibbonItem>(m_godUiPrefab, m_godUiPrefab.get_transform().get_parent());
					m_ribbonItems.Add(godSelectionRibbonItem);
				}
				godSelectionRibbonItem.get_gameObject().SetActive(true);
				godSelectionRibbonItem.Initialise(this, definition);
				array[i] = godSelectionRibbonItem.GetLoadingRoutine();
				if (i == num)
				{
					godSelectionRibbonItem.ForceSelect();
				}
				SetRibbonItemScale(godSelectionRibbonItem);
			}
			yield return EnumeratorUtility.ParallelExecution(array);
			m_godUiPrefab.get_gameObject().SetActive(false);
		}

		public unsafe IEnumerator PlayEnterAnimation()
		{
			DOTweenModuleUI.DOFade(m_bgCanvas, 1f, 0.3f);
			yield return BuildGodList();
			yield return (object)new WaitForEndOfFrame();
			if (((IntPtr)(void*)m_godContent.get_sizeDelta()).x < ((IntPtr)(void*)m_godRibbon.get_sizeDelta()).x)
			{
				Vector2 sizeDelta = m_godContent.get_sizeDelta();
				sizeDelta.x += 10f;
				sizeDelta.y = ((IntPtr)(void*)m_godRibbon.get_sizeDelta()).y;
				m_godRibbon.set_sizeDelta(sizeDelta);
				Vector3 localPosition = m_godContent.get_transform().get_localPosition();
				localPosition.x = 0f;
				m_godContent.get_transform().set_localPosition(localPosition);
				m_godScrollRect.set_enabled(false);
				m_godLeftArrow.SetActive(false);
				m_godRightArrow.SetActive(false);
			}
			yield return PlayAnimation(m_animationDirector.GetAnimation("Open"));
		}

		private void SetRibbonItemScale(GodSelectionRibbonItem ribbonItem)
		{
			ribbonItem.SetEquiped(m_currentGod == ribbonItem.GetGod());
		}

		public void DisplayGod(GodDefinition definition)
		{
			if (!(m_currentGod == definition))
			{
				foreach (GodSelectionRibbonItem ribbonItem in m_ribbonItems)
				{
					ribbonItem.OnSelectionChange(definition);
				}
				this.StartCoroutine(AppearRoutine(definition));
				m_godName.SetText(definition.i18nNameId);
				m_godDescription.SetText(definition.i18nDescriptionId);
				m_validateButton.set_interactable(PlayerData.instance.god != definition.god);
				m_currentGod = definition;
			}
		}

		private unsafe IEnumerator AppearRoutine(GodDefinition definition)
		{
			m_statueLoader.color = new Color(1f, 1f, 1f, 0f);
			m_illuLoader.color = new Color(1f, 1f, 1f, 0f);
			AssetReference uIStatueReference = definition.GetUIStatueReference();
			m_statueLoader.Setup(uIStatueReference, AssetBundlesUtility.GetUIGodsResourcesBundleName());
			AssetReference uIBGReference = definition.GetUIBGReference();
			m_illuLoader.Setup(uIBGReference, AssetBundlesUtility.GetUIGodsResourcesBundleName());
			while (m_statueLoader.loadState == UIResourceLoadState.Loading && m_illuLoader.loadState == UIResourceLoadState.Loading)
			{
				yield return null;
			}
			DOTween.To(new DOGetter<Color>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Color>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), Color.get_white(), 0.25f);
			m_onGodChange.Invoke();
			yield return (object)new WaitForTime(0.1f);
			m_changeGodFX.Play();
			DOTween.To(new DOGetter<Color>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Color>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), Color.get_white(), 0.5f);
		}

		private void OnEquipGod()
		{
			onGodSelected(m_currentGod.god);
			m_validateButton.set_interactable(false);
			foreach (GodSelectionRibbonItem ribbonItem in m_ribbonItems)
			{
				if (ribbonItem.GetGod() == m_currentGod)
				{
					ribbonItem.SetEquiped(selected: true);
				}
				else
				{
					ribbonItem.SetEquiped(selected: false);
				}
			}
			m_equipeGodFX.Play();
			this.StartCoroutine(AutoCloseRoutine(0.3f));
		}

		private IEnumerator AutoCloseRoutine(float delay)
		{
			yield return (object)new WaitForTime(delay);
			SimulateCloseClick();
		}

		public unsafe IEnumerator CloseUI()
		{
			DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, 0.25f);
			yield return PlayAnimation(m_animationDirector.GetAnimation("Close"));
		}

		public void SimulateCloseClick()
		{
			InputUtility.SimulateClickOn(m_closeButton);
		}
	}
}
