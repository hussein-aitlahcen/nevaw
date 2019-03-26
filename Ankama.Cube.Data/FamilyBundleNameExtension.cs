using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ankama.Cube.Data
{
	public static class FamilyBundleNameExtension
	{
		private static readonly Dictionary<BundleCategory, string> s_bundleSubCategoryToBundleName;

		private static readonly Dictionary<God, string> s_godToBundleName;

		static FamilyBundleNameExtension()
		{
			s_bundleSubCategoryToBundleName = new Dictionary<BundleCategory, string>();
			s_godToBundleName = new Dictionary<God, string>();
			CacheBundleNames(s_bundleSubCategoryToBundleName);
			CacheBundleNames(s_godToBundleName);
		}

		private static void CacheBundleNames<T>(Dictionary<T, string> data)
		{
			Type typeFromHandle = typeof(T);
			Array values = Enum.GetValues(typeof(T));
			int length = values.Length;
			for (int i = 0; i < length; i++)
			{
				T key = (T)values.GetValue(i);
				MemberInfo[] member = typeFromHandle.GetMember(key.ToString());
				BundleNameAttribute bundleNameAttribute = (member.Length == 0) ? null : ((BundleNameAttribute)Attribute.GetCustomAttribute(member[0], typeof(BundleNameAttribute)));
				string value = (bundleNameAttribute != null) ? bundleNameAttribute.bundleName : string.Empty;
				data.Add(key, value);
			}
		}

		[NotNull]
		public static string GetBundleName(this BundleCategory value)
		{
			return s_bundleSubCategoryToBundleName[value];
		}

		[NotNull]
		public static string GetBundleName(this God value)
		{
			return s_godToBundleName[value];
		}
	}
}
