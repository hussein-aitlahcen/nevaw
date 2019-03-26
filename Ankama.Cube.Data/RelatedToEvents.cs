using System;

namespace Ankama.Cube.Data
{
	public class RelatedToEvents : Attribute
	{
		public readonly EventCategory[] categories;

		public RelatedToEvents(params EventCategory[] categories)
		{
			this.categories = categories;
		}
	}
}
