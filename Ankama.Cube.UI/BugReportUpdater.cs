using System.Collections;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;

namespace Ankama.Cube.UI
{
	public class BugReportUpdater : MonoBehaviour
	{
		private UnityUserReportingUpdater m_unityUserReportingUpdater;

		private void Awake()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			m_unityUserReportingUpdater = new UnityUserReportingUpdater();
		}

		private void Update()
		{
			m_unityUserReportingUpdater.Reset();
			this.StartCoroutine((IEnumerator)m_unityUserReportingUpdater);
		}

		private void OnDestroy()
		{
			m_unityUserReportingUpdater = null;
		}

		public BugReportUpdater()
			: this()
		{
		}
	}
}
