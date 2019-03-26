using Ankama.Cube.Protocols.PlayerProtocol;

namespace Ankama.Cube.Demo
{
	public class FightPlayerInfo
	{
		public long Uid;

		public PlayerPublicInfo Info = new PlayerPublicInfo();

		public int? WeaponId;

		public int Level = 1;
	}
}
