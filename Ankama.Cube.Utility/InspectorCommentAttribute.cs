using System;
using UnityEngine;

namespace Ankama.Cube.Utility
{
	[AttributeUsage(AttributeTargets.Field)]
	public class InspectorCommentAttribute : PropertyAttribute
	{
		public readonly string text;

		public readonly float height;

		public InspectorCommentAttribute(string text, float height = 18f)
			: this()
		{
			this.height = height;
			this.text = text;
		}
	}
}
