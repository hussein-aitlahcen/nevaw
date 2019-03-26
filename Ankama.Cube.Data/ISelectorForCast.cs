using Ankama.Cube.Fight.Entities;
using DataEditor;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public interface ISelectorForCast : ITargetSelector, IEditableContent
	{
		IEnumerable<Target> EnumerateTargets(DynamicValueContext context);
	}
}
