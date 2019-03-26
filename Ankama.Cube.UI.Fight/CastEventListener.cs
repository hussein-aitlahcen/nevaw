using Ankama.Cube.UI.DeckMaker;
using System;

namespace Ankama.Cube.UI.Fight
{
	public class CastEventListener
	{
		public delegate void SpellCastBeginDelegate(SpellStatusCellRenderer spellStatusData, bool click);

		public delegate void CompanionCastBeginDelegate(CompanionStatusCellRenderer spellStatusData, bool click);

		public event SpellCastBeginDelegate OnCastSpellDragBegin;

		public event Action<SpellStatusCellRenderer, bool> OnCastSpellDragEnd;

		public event CompanionCastBeginDelegate OnCastCompanionDragBegin;

		public event Action<CompanionStatusCellRenderer, bool> OnCastCompanionDragEnd;

		public void CastSpellDragBegin(SpellStatusCellRenderer spellStatusData, bool click)
		{
			this.OnCastSpellDragBegin?.Invoke(spellStatusData, click);
		}

		public void CastSpellDragEnd(SpellStatusCellRenderer spellStatusData, bool onTarget)
		{
			this.OnCastSpellDragEnd?.Invoke(spellStatusData, onTarget);
		}

		public void CastCompanionDragBegin(CompanionStatusCellRenderer statusData, bool click)
		{
			this.OnCastCompanionDragBegin?.Invoke(statusData, click);
		}

		public void CastCompanionDragEnd(CompanionStatusCellRenderer statusData, bool onTarget)
		{
			this.OnCastCompanionDragEnd?.Invoke(statusData, onTarget);
		}
	}
}
