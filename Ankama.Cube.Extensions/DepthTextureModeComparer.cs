using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Extensions
{
	public class DepthTextureModeComparer : IEqualityComparer<DepthTextureMode>
	{
		public static readonly DepthTextureModeComparer instance;

		static DepthTextureModeComparer()
		{
			instance = new DepthTextureModeComparer();
		}

		public bool Equals(DepthTextureMode x, DepthTextureMode y)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return x == y;
		}

		public int GetHashCode(DepthTextureMode obj)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Expected I4, but got Unknown
			return (int)obj;
		}
	}
}
