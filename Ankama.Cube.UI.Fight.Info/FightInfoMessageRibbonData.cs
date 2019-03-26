using UnityEngine;

namespace Ankama.Cube.UI.Fight.Info
{
	[CreateAssetMenu(menuName = "Waven/UI/MessageInfo/RibbonData")]
	public class FightInfoMessageRibbonData : ScriptableObject
	{
		public MessageInfoIconData[] icons;

		public FightInfoMessageRibbonData()
			: this()
		{
		}
	}
}
