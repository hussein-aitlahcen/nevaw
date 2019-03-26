using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum BundleCategory
	{
		[BundleName("others")]
		None = 0,
		[BundleName("others")]
		Others = 1,
		[BundleName("unknowns")]
		Unknown = 2,
		[BundleName("cras")]
		Cra = 10,
		[BundleName("ecaflips")]
		Ecaflip = 11,
		[BundleName("eniripsas")]
		Eniripsa = 12,
		[BundleName("enutrofs")]
		Enutrof = 13,
		[BundleName("fecas")]
		Feca = 14,
		[BundleName("iops")]
		Iop = 0xF,
		[BundleName("osamodas")]
		Osamodas = 0x10,
		[BundleName("pandawas")]
		Pandawa = 17,
		[BundleName("sacrieurs")]
		Sacrieur = 18,
		[BundleName("sadidas")]
		Sadida = 19,
		[BundleName("srams")]
		Sram = 20,
		[BundleName("xelors")]
		Xelor = 21,
		[BundleName("eliotropes")]
		Eliotrope = 0x200,
		[BundleName("huppermages")]
		Huppermage = 821,
		[BundleName("ouginaks")]
		Ouginak = 1521,
		[BundleName("srams")]
		Roublard = 1815,
		[BundleName("osamodas")]
		Steamer = 1920,
		[BundleName("sadidas")]
		Zobal = 2615,
		[BundleName("bouftous")]
		Bouftous = 20000,
		[BundleName("chachas")]
		Chachas = 30800,
		[BundleName("chafers")]
		Chafers = 30801,
		[BundleName("corbacs")]
		Corbacs = 31500,
		[BundleName("craqueleurs")]
		Craqueleurs = 31800,
		[BundleName("flaqueux")]
		Flaqueux = 61200,
		[BundleName("prespics")]
		Prespics = 161800,
		[BundleName("tofus")]
		Tofus = 201500
	}
}
