using Ankama.Cube.Extensions;
using FMODUnity;
using System;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[CreateAssetMenu(menuName = "Waven/Animations/CellObject Animation Parameters", fileName = "New CellObject Animation Parameters")]
	public class CellObjectAnimationParameters : ScriptableObject
	{
		public enum Shape
		{
			Disc,
			Donut,
			Line,
			Rectangle
		}

		public const string AudioStrengthParameterName = "Strength";

		[SerializeField]
		private AnimationCurve m_curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private float m_duration = 1f;

		[SerializeField]
		private Shape m_shape;

		[SerializeField]
		private Vector2 m_size = Vector2.get_one();

		[SerializeField]
		private Quaternion m_direction = Quaternion.get_identity();

		[SerializeField]
		private float m_propagationSpeed;

		[SerializeField]
		private float m_propagationDistance = 2f;

		[SerializeField]
		private AudioReferenceWithParameters m_sound;

		public bool isValid => m_duration > 0f;

		public AudioReferenceWithParameters sound => m_sound;

		public float totalDuration
		{
			get
			{
				if (m_propagationSpeed > 0f && m_propagationDistance > 0f)
				{
					return m_duration * (1f + m_propagationDistance / m_propagationSpeed);
				}
				return m_duration;
			}
		}

		public void GetBounds(Vector2Int origin, Quaternion rotation, float time, out Vector2Int min, out Vector2Int max)
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			float propagationSpeed = m_propagationSpeed;
			float propagationDistance = m_propagationDistance;
			bool flag = propagationSpeed > 0f && propagationDistance > 0f;
			switch (m_shape)
			{
			case Shape.Disc:
			case Shape.Donut:
			{
				int num5 = (!flag) ? Mathf.CeilToInt(m_size.y) : Mathf.CeilToInt(m_size.y * (1f + Mathf.Max(propagationDistance, propagationSpeed * time)));
				Vector2Int val6 = default(Vector2Int);
				val6._002Ector(num5, num5);
				min = origin - val6;
				max = origin + val6;
				break;
			}
			case Shape.Line:
			{
				Quaternion rotation3 = rotation * m_direction;
				int num4;
				if (flag)
				{
					float num3 = Mathf.Max(propagationDistance, propagationSpeed * time);
					num4 = Mathf.CeilToInt(m_size.x + num3);
				}
				else
				{
					num4 = Mathf.CeilToInt(m_size.x);
				}
				Vector2Int value = default(Vector2Int);
				value._002Ector(num4, 0);
				Vector2Int val5 = origin + value.Rotate(rotation3);
				min = new Vector2Int(Mathf.Min(origin.get_x(), val5.get_x()), Mathf.Min(origin.get_y(), val5.get_y()));
				max = new Vector2Int(Mathf.Max(origin.get_x(), val5.get_x()), Mathf.Max(origin.get_y(), val5.get_y()));
				break;
			}
			case Shape.Rectangle:
			{
				Quaternion rotation2 = rotation * m_direction;
				int num2;
				if (flag)
				{
					float num = Mathf.Max(propagationDistance, propagationSpeed * time);
					num2 = Mathf.CeilToInt(m_size.x + num);
				}
				else
				{
					num2 = Mathf.CeilToInt(m_size.x);
				}
				Vector2Int val = default(Vector2Int);
				val._002Ector(num2, 0);
				Vector2Int val2 = default(Vector2Int);
				val2._002Ector(0, Mathf.CeilToInt(0.5f * m_size.y));
				val = val.Rotate(rotation2);
				val2 = val2.Rotate(rotation2);
				Vector2Int val3 = origin - val2;
				Vector2Int val4 = origin + val + val2;
				min = new Vector2Int(Mathf.Min(val3.get_x(), val4.get_x()), Mathf.Min(val3.get_y(), val4.get_y()));
				max = new Vector2Int(Mathf.Max(val3.get_x(), val4.get_x()), Mathf.Max(val3.get_y(), val4.get_y()));
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public float Compute(Vector2Int coords, Vector2Int origin, Quaternion rotation, float time)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			float propagationSpeed = m_propagationSpeed;
			float propagationDistance = m_propagationDistance;
			bool flag = propagationSpeed > 0f && propagationDistance > 0f;
			switch (m_shape)
			{
			case Shape.Disc:
			{
				float y2 = m_size.y;
				float num17 = Vector2Int.Distance(coords, origin);
				if (flag)
				{
					float num18 = Mathf.Max(0f, num17 - y2);
					float num19 = num18 / propagationSpeed;
					float num20 = time * propagationSpeed;
					float num21 = Mathf.Min(1f, num18 / propagationDistance);
					float num22 = Mathf.Clamp01(num17 - (y2 + num20));
					return (1f - num22) * (1f - num21) * m_curve.Evaluate((time - num19) / m_duration);
				}
				float num23 = Mathf.Clamp01(num17 - y2);
				return (1f - num23) * m_curve.Evaluate(time / m_duration);
			}
			case Shape.Donut:
			{
				float x = m_size.x;
				float y = m_size.y;
				float num6 = Vector2Int.Distance(coords, origin);
				if (flag)
				{
					float num7 = Mathf.Max(0f, num6 - y);
					float num8 = num7 / propagationSpeed;
					float num9 = time * propagationSpeed;
					float num10 = Mathf.Min(1f, num7 / propagationDistance);
					float num11 = Mathf.Clamp01(x - num6);
					float num12 = Mathf.Clamp01(num6 - (y + num9));
					float num13 = Mathf.Max(num11, num12);
					return (1f - num13) * (1f - num10) * m_curve.Evaluate((time - num8) / m_duration);
				}
				float num14 = Mathf.Clamp01(x - num6);
				float num15 = Mathf.Clamp01(num6 - y);
				float num16 = Mathf.Max(num14, num15);
				return (1f - num16) * m_curve.Evaluate(time / m_duration);
			}
			case Shape.Line:
				if (flag)
				{
					Vector2Int val6 = coords - origin;
					int num24 = Math.Abs(val6.get_x() + val6.get_y());
					float num25 = Mathf.Max(0f, (float)num24 - m_size.x);
					float num26 = num25 / propagationSpeed;
					float num27 = Mathf.Min(1f, num25 / propagationDistance);
					return (1f - num27) * m_curve.Evaluate((time - num26) / m_duration);
				}
				return m_curve.Evaluate(time / m_duration);
			case Shape.Rectangle:
			{
				float num = 0.5f * m_size.y;
				Quaternion rotation2 = rotation * m_direction;
				Vector2Int val = Vector2Int.get_right().Rotate(rotation2);
				Vector2Int val2 = default(Vector2Int);
				val2._002Ector(val.get_y(), val.get_x());
				Vector2Int val3 = coords - origin;
				Vector2Int val4 = val3;
				val4.Scale(val2);
				float num2 = Mathf.Clamp01((float)Mathf.Abs(val4.get_x() + val4.get_y()) - num);
				if (flag)
				{
					Vector2Int val5 = val3;
					val5.Scale(val);
					int num3 = Mathf.Abs(val5.get_x() + val5.get_y());
					float num4 = (float)num3 / propagationSpeed;
					float num5 = Mathf.Min(1f, (float)num3 / propagationDistance);
					return (1f - num5) * (1f - num2) * m_curve.Evaluate((time - num4) / m_duration);
				}
				return (1f - num2) * m_curve.Evaluate(time / m_duration);
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public CellObjectAnimationParameters()
			: this()
		{
		}//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)

	}
}
