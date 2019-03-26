using Ankama.Cube;
using Ankama.Cube.Data;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.Components;
using System.Collections;
using UnityEngine;

public class UIZaapPlayerPreview : MonoBehaviour
{
	[SerializeField]
	private ImageLoader m_imageLoader;

	[SerializeField]
	private RawTextField m_playerField;

	[SerializeField]
	private RawTextField m_playerLevelField;

	public IEnumerator SetPlayerData(PlayerData data)
	{
		WeaponDefinition weapon = RuntimeData.weaponDefinitions[data.GetCurrentWeapon()];
		m_playerField.SetText(data.nickName);
		yield return FillInformation(weapon, data.GetCurrentWeaponLevel().Value);
	}

	public IEnumerator SetPlayerData(FightInfo.Types.Player data)
	{
		WeaponDefinition weapon = RuntimeData.weaponDefinitions[data.WeaponId.Value];
		m_playerField.SetText(data.Name);
		yield return FillInformation(weapon, new int?(data.Level).Value);
	}

	private IEnumerator FillInformation(WeaponDefinition weapon, int level)
	{
		m_imageLoader.Setup(weapon.GetIlluMatchmakingReference(), "core/ui/matchmakingui");
		while (m_imageLoader.loadState == UIResourceLoadState.Loading)
		{
			yield return null;
		}
		m_playerLevelField.SetText($"Niveau {level}");
	}

	public UIZaapPlayerPreview()
		: this()
	{
	}
}
