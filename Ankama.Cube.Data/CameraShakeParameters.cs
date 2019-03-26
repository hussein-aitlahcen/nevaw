using System;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public sealed class CameraShakeParameters : ScriptableObject
	{
		[SerializeField]
		private Vector2 m_translationAmplitude;

		[SerializeField]
		private float m_rotationAmplitude;

		[SerializeField]
		private float m_translationXFrequency = 1f;

		[SerializeField]
		private Vector2 m_translationXPerlinStart;

		[SerializeField]
		private Vector2 m_translationXPerlinVector;

		[SerializeField]
		private float m_translationYFrequency = 1f;

		[SerializeField]
		private Vector2 m_translationYPerlinStart;

		[SerializeField]
		private Vector2 m_translationYPerlinVector;

		[SerializeField]
		private float m_rotationFrequency = 1f;

		[SerializeField]
		private Vector2 m_rotationPerlinStart;

		[SerializeField]
		private Vector2 m_rotationPerlinVector;

		public unsafe Vector2 GetTranslation(float time, float strength)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			float num = time * m_translationXFrequency;
			float num2 = time * m_translationYFrequency;
			Vector2 val = m_translationXPerlinStart + num * m_translationXPerlinVector;
			Vector2 val2 = m_translationYPerlinStart + num2 * m_translationYPerlinVector;
			Vector2 result = default(Vector2);
			result.x = -1f + 2f * Mathf.PerlinNoise(((IntPtr)(void*)val).x, ((IntPtr)(void*)val).y);
			result.y = -1f + 2f * Mathf.PerlinNoise(((IntPtr)(void*)val2).x, ((IntPtr)(void*)val2).y);
			result.Scale(strength * m_translationAmplitude);
			return result;
		}

		public unsafe float GetAngle(float time, float strength)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			float num = time * m_rotationFrequency;
			Vector2 val = m_rotationPerlinStart + num * m_rotationPerlinVector;
			return strength * m_rotationAmplitude * (-1f + 2f * Mathf.PerlinNoise(((IntPtr)(void*)val).x, ((IntPtr)(void*)val).y));
		}

		public CameraShakeParameters()
			: this()
		{
		}
	}
}
