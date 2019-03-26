using DataEditor;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ankama.Cube.Data
{
	public abstract class MetaDataExtractor
	{
		protected abstract void TryAdd(object elt);

		[PublicAPI]
		public void GetMemberRecursively(IEditableContent data, params string[] fieldNames)
		{
			GetMemberRecursively(data, (FieldInfo f) => fieldNames.Contains(f.Name));
		}

		[PublicAPI]
		public void GetMemberRecursively(IEditableContent data, Func<FieldInfo, bool> predicate = null)
		{
			FieldInfo[] array = ((object)data).GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
			if (predicate != null)
			{
				array = array.Where(predicate).ToArray();
			}
			GetRecursivelyInFields(data, array);
		}

		private void GetRecursivelyInFields(object src, IEnumerable<FieldInfo> fieldInfos)
		{
			if (src != null)
			{
				foreach (FieldInfo fieldInfo in fieldInfos)
				{
					object value = fieldInfo.GetValue(src);
					if (value != null)
					{
						IEnumerable enumerable = value as IEnumerable;
						if (enumerable != null)
						{
							foreach (object item in enumerable)
							{
								if (item != null)
								{
									Add(item);
								}
							}
						}
						else
						{
							Add(value);
						}
					}
				}
			}
		}

		private void Add(object elt)
		{
			TryAdd(elt);
			if (elt is IEditableContent)
			{
				FieldInfo[] fields = elt.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
				GetRecursivelyInFields(elt, fields);
			}
		}
	}
}
