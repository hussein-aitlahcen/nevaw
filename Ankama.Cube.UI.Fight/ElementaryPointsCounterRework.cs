using Ankama.Cube.Data;
using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	public sealed class ElementaryPointsCounterRework : MonoBehaviour
	{
		[SerializeField]
		private PointCounterRework m_air;

		[SerializeField]
		private PointCounterRework m_earth;

		[SerializeField]
		private PointCounterRework m_fire;

		[SerializeField]
		private PointCounterRework m_water;

		public void SetValues(int air, int earth, int fire, int water)
		{
			m_air.SetValue(air);
			m_earth.SetValue(earth);
			m_fire.SetValue(fire);
			m_water.SetValue(water);
		}

		public void ChangeAirValue(int value)
		{
			if (null != m_air)
			{
				m_air.ChangeValue(value);
			}
		}

		public void ShowPreviewAir(int value, ValueModifier modifier)
		{
		}

		public void HidePreviewAir(bool cancelled)
		{
		}

		public void ChangeEarthValue(int value)
		{
			if (null != m_earth)
			{
				m_earth.ChangeValue(value);
			}
		}

		public void ShowPreviewEarth(int value, ValueModifier modifier)
		{
		}

		public void HidePreviewEarth(bool cancelled)
		{
		}

		public void ChangeFireValue(int value)
		{
			if (null != m_fire)
			{
				m_fire.ChangeValue(value);
			}
		}

		public void ShowPreviewFire(int value, ValueModifier modifier)
		{
		}

		public void HidePreviewFire(bool cancelled)
		{
		}

		public void ChangeWaterValue(int value)
		{
			if (null != m_water)
			{
				m_water.ChangeValue(value);
			}
		}

		public void ShowPreviewWater(int value, ValueModifier modifier)
		{
		}

		public void HidePreviewWater(bool cancelled)
		{
		}

		public ElementaryPointsCounterRework()
			: this()
		{
		}
	}
}
