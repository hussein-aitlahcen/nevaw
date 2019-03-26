using UnityEngine;
using UnityEngine.EventSystems;

namespace Ankama.Cube.UI.Components
{
	public interface DragNDropClient
	{
		bool activeInHierarchy
		{
			get;
		}

		RectTransform rectTransform
		{
			get;
		}

		void OnDragOver(CellRenderer cellRenderer, PointerEventData evt);

		bool OnDropOut(CellRenderer cellRenderer, PointerEventData evt);

		bool OnDrop(CellRenderer cellRenderer, PointerEventData evt);
	}
}
