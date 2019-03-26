using Ankama.Cube.UI.Components;
using System.Collections;
using UnityEngine;

public class GameObjectLoader : UIResourceLoader<GameObject>
{
	private GameObject m_asset;

	public GameObject GetAsset()
	{
		return m_asset;
	}

	protected override IEnumerator Apply(GameObject asset, UIResourceDisplayMode displayMode)
	{
		if (null != m_asset)
		{
			Object.Destroy(m_asset);
		}
		m_asset = Object.Instantiate<GameObject>(asset, this.get_transform());
		asset.get_transform().set_localPosition(Vector3.get_zero());
		yield break;
	}

	protected override IEnumerator Revert(UIResourceDisplayMode displayMode)
	{
		yield break;
	}
}
