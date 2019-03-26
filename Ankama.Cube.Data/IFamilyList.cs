using DataEditor;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public interface IFamilyList : IEditableContent
	{
		IReadOnlyList<Family> families
		{
			get;
		}
	}
}
