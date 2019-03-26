using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class PlayerAvatarDemo : MonoBehaviour
	{
		private Coroutine m_loadingCoroutine;

		private WeaponDefinition m_weaponDefinition;

		[SerializeField]
		private Image m_illu;

		[SerializeField]
		private RawTextField m_name;

		public string nickname
		{
			set
			{
				m_name.SetText(value);
			}
		}

		public int weaponId
		{
			set
			{
				if (RuntimeData.weaponDefinitions.TryGetValue(value, out WeaponDefinition value2))
				{
					weaponDefinition = value2;
				}
				else
				{
					Log.Error($"Cannot find weapon definition with id {value}", 29, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Demo\\Code\\UI\\PlayerAvatarDemo.cs");
				}
			}
		}

		public WeaponDefinition weaponDefinition
		{
			set
			{
				m_weaponDefinition = value;
				UpdateSprite();
			}
		}

		private void OnEnable()
		{
			UpdateSprite();
		}

		private void OnDisable()
		{
			if (m_loadingCoroutine != null)
			{
				this.StopCoroutine(m_loadingCoroutine);
				m_loadingCoroutine = null;
			}
		}

		private void UpdateSprite()
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (!this.get_isActiveAndEnabled())
			{
				return;
			}
			if (m_weaponDefinition == null)
			{
				m_illu.set_sprite(null);
				return;
			}
			if (m_loadingCoroutine != null)
			{
				this.StopCoroutine(m_loadingCoroutine);
				m_illu.set_sprite(null);
			}
			m_loadingCoroutine = this.StartCoroutine(m_weaponDefinition.LoadIllustrationAsync<Sprite>(AssetBundlesUtility.GetUICharacterResourcesBundleName(), m_weaponDefinition.GetIllustrationReference(), (Action<Sprite, string>)UpdateSpriteCallback));
		}

		private void UpdateSpriteCallback(Sprite sprite, string loadedBundleName)
		{
			if (null != m_illu)
			{
				m_illu.set_sprite(sprite);
			}
			m_loadingCoroutine = null;
		}

		public PlayerAvatarDemo()
			: this()
		{
		}
	}
}
