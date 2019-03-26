using DataEditor;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public interface IAreaDefinition : IEditableContent
	{
		Area ToArea(Vector2Int position);
	}
}
