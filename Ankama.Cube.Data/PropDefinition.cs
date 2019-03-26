using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class PropDefinition : IsoObjectDefinition
	{
		[SerializeField]
		private FightCellState m_impliedCellState;

		public FightCellState impliedCellState => m_impliedCellState;

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
		}
	}
}
