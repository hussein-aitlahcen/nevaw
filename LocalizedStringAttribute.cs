using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class LocalizedStringAttribute : PropertyAttribute
{
	public readonly string keyFormat;

	public readonly string collection;

	public readonly int lines;

	public LocalizedStringAttribute(string keyFormat, string collection, int lines = 1)
		: this()
	{
		this.keyFormat = keyFormat;
		this.collection = collection;
		this.lines = lines;
	}
}
