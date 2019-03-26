using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class SquadFakeData : ScriptableObject
	{
		[SerializeField]
		public int id;

		[SerializeField]
		[Multiline]
		public string title;

		[SerializeField]
		[Multiline]
		public string description;

		[SerializeField]
		[Multiline]
		public string difficulty;

		[SerializeField]
		public Sprite illu;

		public SquadFakeData()
			: this()
		{
		}
	}
}
