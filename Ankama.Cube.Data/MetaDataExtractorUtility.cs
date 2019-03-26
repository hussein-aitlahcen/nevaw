using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ankama.Cube.Data
{
	public static class MetaDataExtractorUtility
	{
		public static IEnumerable<T> GetCustomAttributes<T>(this object obj) where T : class
		{
			Enum @enum = obj as Enum;
			if (@enum != null)
			{
				return @enum.GetAttributes<T>();
			}
			return obj.GetType().GetCustomAttributesIncludingBaseInterfaces<T>();
		}

		public static IEnumerable<T> GetAttributes<T>(this Enum e) where T : class
		{
			return e.GetType().GetMember(e.ToString())[0].GetAttributes<T>();
		}

		public static IEnumerable<T> GetAttributes<T>(this MemberInfo member) where T : class
		{
			return Attribute.GetCustomAttributes(member, typeof(T)).Cast<T>();
		}

		private static IEnumerable<T> GetCustomAttributesIncludingBaseInterfaces<T>(this Type type)
		{
			Type attributeType = typeof(T);
			return type.GetCustomAttributes(attributeType, inherit: true).Union(type.GetInterfaces().SelectMany((Type interfaceType) => interfaceType.GetCustomAttributes(attributeType, inherit: true))).Distinct()
				.Cast<T>();
		}
	}
}
