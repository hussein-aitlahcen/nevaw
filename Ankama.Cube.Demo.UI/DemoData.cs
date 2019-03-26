using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class DemoData : ScriptableObject
	{
		[SerializeField]
		public bool resetSelection;

		[SerializeField]
		public int godNbElementLockedBefore;

		[SerializeField]
		public int godNbElementLockedAfter;

		[SerializeField]
		public GodFakeData[] gods;

		[SerializeField]
		public int squadNbElementLockedBefore;

		[SerializeField]
		public int squadNbElementLockedAfter;

		[SerializeField]
		public SquadFakeData[] squads;

		public DemoData()
			: this()
		{
		}
	}
}
