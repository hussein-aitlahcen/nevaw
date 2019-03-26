using Ankama.Cube.Data;
using System;

namespace Ankama.Cube.UI.Fight
{
	public struct GaugesModification
	{
		private int m_air;

		private int m_earth;

		private int m_fire;

		private int m_water;

		private int m_reserve;

		private int m_actionPoint;

		public int air => m_air;

		public int earth => m_earth;

		public int fire => m_fire;

		public int water => m_water;

		public int reserve => m_reserve;

		public int actionPoint => m_actionPoint;

		public void Increment(CaracId carac, int modification)
		{
			if (modification != 0)
			{
				switch (carac)
				{
				case CaracId.ActionPoints:
					m_actionPoint += modification;
					break;
				case CaracId.FirePoints:
					m_fire += modification;
					break;
				case CaracId.WaterPoints:
					m_water += modification;
					break;
				case CaracId.EarthPoints:
					m_earth += modification;
					break;
				case CaracId.AirPoints:
					m_air += modification;
					break;
				case CaracId.ReservePoints:
					m_reserve += modification;
					break;
				default:
					throw new ArgumentOutOfRangeException("carac", carac, null);
				}
			}
		}
	}
}
