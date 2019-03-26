using System;

namespace Ankama.Cube.UI.DeckMaker
{
	public class DeckBuildingEventController
	{
		public event Action<EditModeSelection> OnEditRequest;

		public event Action OnSelectRequest;

		public event Action<int, int> OnCloneRequest;

		public event Action OnDeleteRequest;

		public event Action OnSaveRequest;

		public event Action OnCancelRequest;

		public event Action OnCloseRequest;

		public event Action<DeckSlot> OnDeckSlotSelectionChanged;

		public void OnClone(int title, int desc)
		{
			this.OnCloneRequest?.Invoke(title, desc);
		}

		public void OnEdit(EditModeSelection selection)
		{
			this.OnEditRequest?.Invoke(selection);
		}

		public void OnDelete()
		{
			this.OnDeleteRequest?.Invoke();
		}

		public void OnSave()
		{
			this.OnSaveRequest?.Invoke();
		}

		public void OnCancel()
		{
			this.OnCancelRequest?.Invoke();
		}

		public void OnClose()
		{
			this.OnCloseRequest?.Invoke();
		}

		public void OnSelect()
		{
			this.OnSelectRequest?.Invoke();
		}

		public void OnDeckSlotSelectionChange(DeckSlot slot)
		{
			this.OnDeckSlotSelectionChanged?.Invoke(slot);
		}
	}
}
