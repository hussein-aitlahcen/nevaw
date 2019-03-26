using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class InspectorButtonAttribute : PropertyAttribute
{
	public readonly string methodName;

	public readonly object[] args;

	private float? m_buttonWidth;

	public float? buttonWidth
	{
		get
		{
			return m_buttonWidth;
		}
		set
		{
			m_buttonWidth = value;
		}
	}

	public InspectorButtonAttribute(string methodName, params object[] args)
		: this()
	{
		this.methodName = methodName;
		this.args = args;
	}
}
