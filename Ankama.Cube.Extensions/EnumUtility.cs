using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ankama.Cube.Extensions
{
	public static class EnumUtility
	{
		[PublicAPI]
		public static T[] GetValues<T>()
		{
			Array values = Enum.GetValues(typeof(T));
			int length = values.Length;
			T[] array = new T[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = (T)values.GetValue(i);
			}
			return array;
		}

		[PublicAPI]
		public static IEnumerable<T> GetValuesNotObsolete<T>()
		{
			Array values = Enum.GetValues(typeof(T));
			int count = values.Length;
			Type enumType = typeof(T);
			int num;
			for (int i = 0; i < count; i = num)
			{
				T val = (T)values.GetValue(i);
				Enum @enum = val as Enum;
				if (@enum != null)
				{
					MemberInfo[] member = enumType.GetMember(@enum.ToString());
					if (member.Length != 0 && member[0].GetCustomAttributes(typeof(ObsoleteAttribute), inherit: false).Length == 0)
					{
						yield return val;
					}
				}
				num = i + 1;
			}
		}
	}
}
