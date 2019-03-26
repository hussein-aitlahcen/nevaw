using Ankama.Cube.UI.Components;

public class DeckEditDynamicList : DynamicList
{
	protected override void InitCellRenderer(CellRenderer cellRenderer, bool andUpdate)
	{
		base.InitCellRenderer(cellRenderer, andUpdate);
		cellRenderer.get_gameObject().AddComponent<DeckEditItemPointerListener>();
	}
}
