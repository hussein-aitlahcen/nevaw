using UnityEngine;

namespace Ankama.Cube.Data.Dictionaries
{
	public abstract class DataDictionaryItem : ScriptableObject
	{
		public abstract int id
		{
			get;
		}

		protected DataDictionaryItem()
			: this()
		{
		}
	}
}
