using System;
using System.Reflection;

namespace Ankama.Cube.Extensions
{
	public static class AttributeExtensions
	{
		public static T GetCustomAttribute<T>(this MemberInfo member) where T : class
		{
			return Attribute.GetCustomAttribute(member, typeof(T)) as T;
		}

		public static T GetCustomAttribute<T>(this Enum e) where T : class
		{
			MemberInfo[] member = e.GetType().GetMember(e.ToString());
			if (member.Length == 0)
			{
				return null;
			}
			return member[0].GetCustomAttribute<T>();
		}
	}
}
