namespace Ankama.Cube.Fight.Entities
{
	public struct TooltipElementValues
	{
		public readonly int air;

		public readonly int earth;

		public readonly int fire;

		public readonly int water;

		public readonly int reserve;

		public TooltipElementValues(int air, int earth, int fire, int water, int reserve)
		{
			this.air = air;
			this.earth = earth;
			this.fire = fire;
			this.water = water;
			this.reserve = reserve;
		}
	}
}
