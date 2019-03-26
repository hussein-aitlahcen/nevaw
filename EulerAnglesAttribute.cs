using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class EulerAnglesAttribute : PropertyAttribute
{
	public EulerAnglesAttribute()
		: this()
	{
	}
}
