using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum Family
	{
		None = 0,
		[BundleName("iops")]
		Iop = 1,
		[BundleName("cras")]
		Cra = 2,
		[BundleName("eniripsas")]
		Eniripsa = 3,
		[BundleName("ecaflips")]
		Ecaflip = 4,
		[BundleName("enutrofs")]
		Enutrof = 5,
		[BundleName("srams")]
		Sram = 6,
		[BundleName("xelors")]
		Xelor = 7,
		[BundleName("sacrieurs")]
		Sacrieur = 8,
		[BundleName("fecas")]
		Feca = 9,
		[BundleName("sadidas")]
		Sadida = 10,
		[BundleName("osamodas")]
		Osamodas = 11,
		[BundleName("pandawas")]
		Pandawa = 12,
		Roublard = 13,
		Zobal = 14,
		Steamer = 0xF,
		Eliotrope = 0x10,
		Unknown = 17,
		Huppermage = 18,
		Ouginak = 19,
		Sinistro = 20,
		Chacha = 30,
		Momie = 0x1F,
		Hydruille = 0x20,
		Cadran = 33,
		Balise = 34,
		AmeSpectrale = 35,
		Bouftou = 36,
		Tofu = 37,
		Corbac = 38,
		Ombre = 39,
		Chafer = 40,
		Craqueleurs = 41,
		Flaqueux = 42,
		Prespics = 43,
		Nocturiens = 44,
		BeteOsamodas = 45
	}
}
