using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public sealed class AnimatedFloorMechanismData : AnimatedCharacterData
	{
		[SerializeField]
		private GameObject m_prefab;

		public GameObject prefab => m_prefab;

		protected override void GatherAdditionalResourcesLoadingRoutines(List<IEnumerator> routines)
		{
		}

		protected override void UnloadAdditionalResources()
		{
		}
	}
}
