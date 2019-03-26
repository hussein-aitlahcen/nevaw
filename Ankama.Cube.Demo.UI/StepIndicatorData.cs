using System;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class StepIndicatorData : ScriptableObject
	{
		[Serializable]
		public struct StateData
		{
			[SerializeField]
			public float alpha;

			[SerializeField]
			public float scale;
		}

		[SerializeField]
		public float transitionDuration;

		[SerializeField]
		public StateData enableState;

		[SerializeField]
		public StateData disableState;

		public StepIndicatorData()
			: this()
		{
		}
	}
}
