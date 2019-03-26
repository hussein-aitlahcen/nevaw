using UnityEngine;

namespace Ankama.Cube.UI
{
	[CreateAssetMenu(menuName = "Waven/UI/PopupInfoStyleData")]
	public class PopupInfoStyleData : ScriptableObject
	{
		[SerializeField]
		public Color titleColor = Color.get_white();

		[SerializeField]
		public Color textColor = Color.get_white();

		public PopupInfoStyleData()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)

	}
}
