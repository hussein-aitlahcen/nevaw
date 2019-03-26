using System;

namespace Ankama.Cube.Data
{
	[AttributeUsage(AttributeTargets.Field)]
	public class BundleNameAttribute : Attribute
	{
		public readonly string bundleName;

		public BundleNameAttribute(string bundleName)
		{
			if (string.IsNullOrEmpty(bundleName))
			{
				throw new ArgumentException("bundleName");
			}
			this.bundleName = bundleName.Trim().ToLowerInvariant();
		}
	}
}
