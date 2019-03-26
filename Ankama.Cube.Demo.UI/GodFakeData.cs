using Ankama.Cube.Data;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class GodFakeData : ScriptableObject
	{
		[SerializeField]
		public God god;

		[SerializeField]
		[Multiline]
		public string title;

		[SerializeField]
		[Multiline]
		public string description;

		[SerializeField]
		public Sprite illu;

		[SerializeField]
		public bool locked;

		public GodFakeData()
			: this()
		{
		}
	}
}
