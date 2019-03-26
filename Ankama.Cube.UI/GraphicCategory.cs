using Ankama.Cube.Player;
using Ankama.Cube.SRP;
using Ankama.Cube.UI.Components;
using Ankama.ScreenManagement;
using Ankama.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class GraphicCategory : OptionCategory
	{
		private struct ResolutionData
		{
			public int defaultRatioDropDownIndex;

			public int defaultResolutionDropDownIndex;

			public int defaultRatioIndex;

			public Resolution defaultResolution;

			public List<int> ratioIndex;

			public List<List<Resolution>> resolutionsByRatio;
		}

		[SerializeField]
		protected Button m_defaultResolutionButton;

		[SerializeField]
		protected TextFieldDropdown m_fullScreenDropdown;

		[SerializeField]
		protected TextFieldDropdown m_displayDropdown;

		[SerializeField]
		protected TextFieldDropdown m_ratioDropdown;

		[SerializeField]
		protected TextFieldDropdown m_resolutionDropdown;

		[SerializeField]
		protected TextFieldDropdown m_graphicPresetDropdown;

		[SerializeField]
		protected Button m_applyButton;

		private const float MinimumSupportedRatio = 1.25f;

		private const float MaximumSupportedRatio = 2.33333325f;

		private const float SupportedRatioThreshold = 0.005f;

		private static readonly float[] SupportedRatios = new float[9]
		{
			2.33333325f,
			2f,
			1.77777779f,
			1.6f,
			1.33333337f,
			1.25f,
			1.66666663f,
			1.5f,
			1.4f
		};

		private static readonly string[] SupportedRatioNames = new string[9]
		{
			"21:9",
			"18:9",
			"16:9",
			"16:10",
			"4:3",
			"5:4",
			"5:3",
			"3:2",
			"7:5"
		};

		private List<ResolutionData> m_fullScreenResolutions = new List<ResolutionData>();

		private ResolutionData m_windowedResolutions;

		private int m_currentFullScreenDropDownIndex = -1;

		private int m_currentDisplayDropDownIndex = -1;

		private int m_currentResolutionDropDownIndex = -1;

		private int m_currentRatioDropDownIndex = -1;

		private int m_currentGraphicPresetDropDownIndex = -1;

		protected void Awake()
		{
			List<string> options = new List<string>
			{
				RuntimeData.FormattedText(9912),
				RuntimeData.FormattedText(68421)
			};
			m_fullScreenDropdown.ClearOptions();
			m_fullScreenDropdown.AddOptions(options);
			m_fullScreenDropdown.RefreshShownValue();
			List<string> list = new List<string>();
			List<QualityAsset> qualityPresets = QualityManager.GetQualityPresets();
			for (int i = 0; i < qualityPresets.Count; i++)
			{
				QualityAsset val = qualityPresets[i];
				string[] array = val.get_name().Split(new char[1]
				{
					'_'
				});
				string item = (array.Length != 0) ? array[array.Length - 1] : val.get_name();
				list.Add(item);
			}
			m_graphicPresetDropdown.ClearOptions();
			m_graphicPresetDropdown.AddOptions(list);
			m_graphicPresetDropdown.RefreshShownValue();
		}

		protected unsafe void OnEnable()
		{
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Expected O, but got Unknown
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Expected O, but got Unknown
			m_graphicPresetDropdown.value = QualityManager.GetQualityPresetIndex();
			ScreenManager.UpdateDisplayInformation();
			UpdateResolutionData();
			m_fullScreenDropdown.value = ((!Device.fullScreen) ? 1 : 0);
			m_fullScreenDropdown.RefreshShownValue();
			m_displayDropdown.get_transform().get_parent().get_parent()
				.get_gameObject()
				.SetActive(Device.fullScreen);
			m_defaultResolutionButton.get_transform().get_parent().get_gameObject()
				.SetActive(Device.fullScreen);
			UpdateDisplayList();
			UpdateRatioList();
			UpdateResolutionList();
			m_currentFullScreenDropDownIndex = m_fullScreenDropdown.value;
			m_currentDisplayDropDownIndex = m_displayDropdown.value;
			m_currentRatioDropDownIndex = m_ratioDropdown.value;
			m_currentResolutionDropDownIndex = m_resolutionDropdown.value;
			m_currentGraphicPresetDropDownIndex = m_graphicPresetDropdown.value;
			UpdateDefaultResolutionButton();
			m_fullScreenDropdown.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_displayDropdown.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_ratioDropdown.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_defaultResolutionButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_graphicPresetDropdown.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_applyButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			UpdateApplyButton();
		}

		protected unsafe void OnDisable()
		{
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			m_fullScreenDropdown.onValueChanged.RemoveListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_displayDropdown.onValueChanged.RemoveListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_ratioDropdown.onValueChanged.RemoveListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_resolutionDropdown.onValueChanged.RemoveListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_defaultResolutionButton.get_onClick().RemoveListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_graphicPresetDropdown.onValueChanged.RemoveListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_applyButton.get_onClick().RemoveListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private unsafe void UpdateResolutionData()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			m_fullScreenResolutions.Clear();
			int displayCount = ScreenManager.GetDisplayCount();
			if (displayCount == 0)
			{
				Log.Error("No display detected", 157, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Option\\Configuration\\GraphicCategory.cs");
			}
			int num = default(int);
			if (!ScreenManager.TryGetCurrentDisplayIndex(ref num))
			{
				Log.Error("Cannot get current Display", 163, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Option\\Configuration\\GraphicCategory.cs");
				return;
			}
			for (int i = 0; i < displayCount; i++)
			{
				DisplayInfo displayInfo = ScreenManager.GetDisplayInfo(i);
				Resolution systemResolution = ((IntPtr)(void*)displayInfo).systemResolution;
				int ratioIndex = GetRatioIndex(systemResolution.get_width(), systemResolution.get_height());
				ResolutionData resolutionData = default(ResolutionData);
				resolutionData.ratioIndex = new List<int>();
				resolutionData.resolutionsByRatio = new List<List<Resolution>>();
				resolutionData.defaultResolution = systemResolution;
				resolutionData.defaultRatioIndex = ratioIndex;
				int num2 = ((IntPtr)(void*)displayInfo).resolutions.Length;
				for (int j = 0; j < num2; j++)
				{
					Resolution item = ((IntPtr)(void*)displayInfo).resolutions[j];
					int ratioIndex2 = GetRatioIndex(item.get_width(), item.get_height());
					if (ratioIndex2 >= 0)
					{
						if (!resolutionData.ratioIndex.Contains(ratioIndex2))
						{
							resolutionData.ratioIndex.Add(ratioIndex2);
							resolutionData.resolutionsByRatio.Add(new List<Resolution>
							{
								item
							});
						}
						else
						{
							int index = resolutionData.ratioIndex.IndexOf(ratioIndex2);
							resolutionData.resolutionsByRatio[index].Add(item);
						}
						if (systemResolution.get_width() == item.get_width() && systemResolution.get_height() == item.get_height())
						{
							int index2 = resolutionData.defaultRatioDropDownIndex = resolutionData.ratioIndex.IndexOf(ratioIndex2);
							resolutionData.defaultResolutionDropDownIndex = resolutionData.resolutionsByRatio[index2].Count - 1;
						}
					}
				}
				m_fullScreenResolutions.Add(resolutionData);
			}
			m_windowedResolutions = default(ResolutionData);
			m_windowedResolutions.ratioIndex = new List<int>();
			m_windowedResolutions.resolutionsByRatio = new List<List<Resolution>>();
			int windowedResolutionCount = ScreenManager.GetWindowedResolutionCount();
			for (int k = 0; k < windowedResolutionCount; k++)
			{
				Resolution windowedResolution = ScreenManager.GetWindowedResolution(k);
				int ratioIndex3 = GetRatioIndex(windowedResolution.get_width(), windowedResolution.get_height());
				if (ratioIndex3 >= 0)
				{
					if (!m_windowedResolutions.ratioIndex.Contains(ratioIndex3))
					{
						m_windowedResolutions.ratioIndex.Add(ratioIndex3);
						m_windowedResolutions.resolutionsByRatio.Add(new List<Resolution>
						{
							windowedResolution
						});
					}
					else
					{
						int index3 = m_windowedResolutions.ratioIndex.IndexOf(ratioIndex3);
						m_windowedResolutions.resolutionsByRatio[index3].Add(windowedResolution);
					}
				}
			}
		}

		private unsafe void UpdateDisplayList()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			int displayCount = ScreenManager.GetDisplayCount();
			if (displayCount == 0)
			{
				Log.Error("No display detected", 240, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Option\\Configuration\\GraphicCategory.cs");
			}
			int value = default(int);
			if (!ScreenManager.TryGetCurrentDisplayIndex(ref value))
			{
				Log.Error("Cannot get current Display", 246, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Option\\Configuration\\GraphicCategory.cs");
				return;
			}
			List<string> list = new List<string>();
			for (int i = 0; i < displayCount; i++)
			{
				DisplayInfo displayInfo = ScreenManager.GetDisplayInfo(i);
				list.Add($"{((IntPtr)(void*)displayInfo).name} {i + 1}");
			}
			m_displayDropdown.ClearOptions();
			m_displayDropdown.AddOptions(list);
			m_displayDropdown.value = value;
			m_displayDropdown.RefreshShownValue();
		}

		private void UpdateRatioList()
		{
			int ratioIndex = GetRatioIndex(Screen.get_width(), Screen.get_height());
			int num = -1;
			List<string> list = new List<string>();
			if (m_displayDropdown.value >= 0)
			{
				ResolutionData resolutionData = (m_fullScreenDropdown.value != 0) ? m_windowedResolutions : m_fullScreenResolutions[m_displayDropdown.value];
				for (int i = 0; i < resolutionData.ratioIndex.Count; i++)
				{
					int num2 = resolutionData.ratioIndex[i];
					list.Add(SupportedRatioNames[num2]);
					if (ratioIndex == num2)
					{
						num = list.Count - 1;
					}
				}
			}
			if (num == -1)
			{
				num = list.Count - 1;
			}
			m_ratioDropdown.ClearOptions();
			m_ratioDropdown.AddOptions(list);
			m_ratioDropdown.value = num;
			m_ratioDropdown.RefreshShownValue();
		}

		private void UpdateResolutionList()
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			int num = -1;
			List<string> list = new List<string>();
			if (m_displayDropdown.value >= 0 && m_ratioDropdown.value >= 0)
			{
				List<Resolution> list2 = ((m_fullScreenDropdown.value != 0) ? m_windowedResolutions : m_fullScreenResolutions[m_displayDropdown.value]).resolutionsByRatio[m_ratioDropdown.value];
				for (int i = 0; i < list2.Count; i++)
				{
					Resolution val = list2[i];
					if (val.get_refreshRate() == 0)
					{
						list.Add($"{val.get_width()}x{val.get_height()}");
					}
					else
					{
						list.Add($"{val.get_width()}x{val.get_height()} {val.get_refreshRate()}Hz");
					}
					if (val.get_width() == Screen.get_width() && val.get_height() == Screen.get_height())
					{
						num = i;
					}
				}
			}
			if (num == -1)
			{
				num = list.Count - 1;
			}
			m_resolutionDropdown.ClearOptions();
			m_resolutionDropdown.AddOptions(list);
			m_resolutionDropdown.value = num;
			m_resolutionDropdown.RefreshShownValue();
		}

		protected void UpdateDefaultResolutionButton()
		{
			if (m_fullScreenDropdown.value != 0)
			{
				m_defaultResolutionButton.set_interactable(false);
				return;
			}
			ResolutionData resolutionData = m_fullScreenResolutions[m_displayDropdown.value];
			m_defaultResolutionButton.set_interactable(m_ratioDropdown.value != resolutionData.defaultRatioDropDownIndex || m_resolutionDropdown.value != resolutionData.defaultResolutionDropDownIndex);
		}

		protected void OnFullscreenDropdownChanged(int value)
		{
			bool active = value == 0;
			m_displayDropdown.get_transform().get_parent().get_parent()
				.get_gameObject()
				.SetActive(active);
			m_defaultResolutionButton.get_transform().get_parent().get_gameObject()
				.SetActive(active);
			UpdateRatioList();
			UpdateResolutionList();
			UpdateDefaultResolutionButton();
			UpdateApplyButton();
		}

		private void OnDisplayDropdownChanged(int value)
		{
			UpdateRatioList();
			UpdateResolutionList();
			UpdateDefaultResolutionButton();
			UpdateApplyButton();
		}

		protected void OnRatioDropdownChanged(int value)
		{
			UpdateResolutionList();
			UpdateDefaultResolutionButton();
			UpdateApplyButton();
		}

		protected void OnResolutionDropdownChanged(int value)
		{
			UpdateDefaultResolutionButton();
			UpdateApplyButton();
		}

		protected void OnDefaultResolutionButtonClicked()
		{
			if (m_fullScreenDropdown.value == 0)
			{
				ResolutionData resolutionData = m_fullScreenResolutions[m_displayDropdown.value];
				m_ratioDropdown.value = resolutionData.defaultRatioDropDownIndex;
				m_resolutionDropdown.value = resolutionData.defaultResolutionDropDownIndex;
			}
		}

		protected void ApplyScreenResolution()
		{
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			if (m_displayDropdown.value == -1)
			{
				Log.Error("No display index selected", 408, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Option\\Configuration\\GraphicCategory.cs");
				return;
			}
			if (m_ratioDropdown.value == -1)
			{
				Log.Error("No ratio index selected", 414, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Option\\Configuration\\GraphicCategory.cs");
				return;
			}
			if (m_resolutionDropdown.value == -1)
			{
				Log.Error("No resolution index selected", 420, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Option\\Configuration\\GraphicCategory.cs");
				return;
			}
			m_currentFullScreenDropDownIndex = m_fullScreenDropdown.value;
			m_currentDisplayDropDownIndex = m_displayDropdown.value;
			m_currentRatioDropDownIndex = m_ratioDropdown.value;
			m_currentResolutionDropDownIndex = m_resolutionDropdown.value;
			Resolution val = ((m_currentFullScreenDropDownIndex != 0) ? m_windowedResolutions : m_fullScreenResolutions[m_displayDropdown.value]).resolutionsByRatio[m_currentRatioDropDownIndex][m_currentResolutionDropDownIndex];
			if (m_currentFullScreenDropDownIndex == 0)
			{
				this.StartCoroutine(ScreenManager.SetFullScreenMode(m_currentDisplayDropDownIndex, val));
			}
			else
			{
				this.StartCoroutine(ScreenManager.SetWindowedMode(val, 1));
			}
			UpdateDefaultResolutionButton();
		}

		private static int GetRatioIndex(int width, int height)
		{
			float num = (float)width / (float)height;
			if (num - 1.25f < -0.005f)
			{
				return -1;
			}
			if (2.33333325f - num < -0.005f)
			{
				return -1;
			}
			int num2 = SupportedRatios.Length;
			float num3 = float.MaxValue;
			int result = 0;
			for (int i = 0; i < num2; i++)
			{
				float num4 = Mathf.Abs(num - SupportedRatios[i]);
				if (num4 < float.Epsilon)
				{
					return i;
				}
				if (num4 < num3)
				{
					num3 = num4;
					result = i;
				}
			}
			return result;
		}

		protected void UpdateApplyButton()
		{
			m_applyButton.set_interactable(m_fullScreenDropdown.value != m_currentFullScreenDropDownIndex || m_displayDropdown.value != m_currentDisplayDropDownIndex || m_ratioDropdown.value != m_currentRatioDropDownIndex || m_resolutionDropdown.value != m_currentResolutionDropDownIndex || m_graphicPresetDropdown.value != m_currentGraphicPresetDropDownIndex);
		}

		protected void OnApplyButtonClicked()
		{
			ApplyScreenResolution();
			ApplyGraphicQuality();
			UpdateApplyButton();
		}

		protected void ApplyGraphicQuality()
		{
			m_currentGraphicPresetDropDownIndex = m_graphicPresetDropdown.value;
			QualityManager.SetQualityPresetIndex(m_currentGraphicPresetDropDownIndex);
			PlayerPreferences.graphicPresetIndex = m_currentGraphicPresetDropDownIndex;
		}

		protected void OnQualityDropdownChanged(int value)
		{
			UpdateApplyButton();
		}
	}
}
