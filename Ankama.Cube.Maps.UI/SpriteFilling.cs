using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Sprite))]
	public sealed class SpriteFilling : MonoBehaviour
	{
		[SerializeField]
		[UsedImplicitly]
		private SpriteRenderer m_spriteRenderer;

		[SerializeField]
		[UsedImplicitly]
		private SpriteRenderer m_backgroundReference;

		[SerializeField]
		[UsedImplicitly]
		private Vector2 m_offset;

		[SerializeField]
		[UsedImplicitly]
		private Vector2 m_padding;

		[SerializeField]
		[UsedImplicitly]
		private TextAnchor m_anchor = 3;

		[SerializeField]
		[UsedImplicitly]
		[Range(0f, 1f)]
		private float m_value;

		[PublicAPI]
		public Sprite sprite
		{
			get
			{
				return m_spriteRenderer.get_sprite();
			}
			set
			{
				m_spriteRenderer.set_sprite(value);
			}
		}

		[PublicAPI]
		public SpriteRenderer backgroundReference
		{
			get
			{
				return m_backgroundReference;
			}
			set
			{
				if (m_backgroundReference != value)
				{
					m_backgroundReference = value;
					Refresh();
				}
			}
		}

		[PublicAPI]
		public TextAnchor anchor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_anchor;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				if (m_anchor != value)
				{
					m_anchor = value;
					Refresh();
				}
			}
		}

		[PublicAPI]
		public Vector2 offset
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_offset;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (m_offset != value)
				{
					m_offset = value;
					Refresh();
				}
			}
		}

		[PublicAPI]
		public Vector2 padding
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_padding;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (m_padding != value)
				{
					m_padding = value;
					Refresh();
				}
			}
		}

		[PublicAPI]
		public float value
		{
			get
			{
				return m_value;
			}
			set
			{
				value = Mathf.Clamp01(value);
				if (m_value != value)
				{
					m_value = value;
					Refresh();
				}
			}
		}

		private void OnEnable()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			if ((int)m_spriteRenderer.get_drawMode() != 1)
			{
				m_spriteRenderer.set_drawMode(1);
			}
			Refresh();
		}

		private unsafe void Refresh()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Invalid comparison between Unknown and I4
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected I4, but got Unknown
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			SpriteRenderer backgroundReference = m_backgroundReference;
			if (null == backgroundReference)
			{
				Log.Warning("Missing background reference on SpriteFilling named '" + this.get_name() + "'.", 136, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\UI\\SpriteFilling.cs");
				return;
			}
			if ((int)backgroundReference.get_drawMode() != 1)
			{
				Log.Warning(string.Format("Draw mode of background reference assigned for {0} named '{1}' is not {2}.", "SpriteFilling", this.get_name(), 1), 142, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\UI\\SpriteFilling.cs");
				return;
			}
			Transform transform = m_spriteRenderer.get_transform();
			Vector2 val = backgroundReference.get_size() - m_padding;
			TextAnchor anchor = m_anchor;
			Vector2 val2 = default(Vector2);
			Vector2 val3 = default(Vector2);
			switch ((int)anchor)
			{
			case 0:
				val2 = val * m_value;
				val3 = (val - val2) * new Vector2(-0.5f, 0.5f);
				break;
			case 1:
				val2._002Ector(((IntPtr)(void*)val).x, ((IntPtr)(void*)val).y * m_value);
				val3._002Ector(0f, (((IntPtr)(void*)val).y - ((IntPtr)(void*)val2).y) * 0.5f);
				break;
			case 2:
				val2 = val * m_value;
				val3 = (val - val2) * 0.5f;
				break;
			case 3:
				val2._002Ector(((IntPtr)(void*)val).x * m_value, ((IntPtr)(void*)val).y);
				val3._002Ector((((IntPtr)(void*)val).x - ((IntPtr)(void*)val2).x) * -0.5f, 0f);
				break;
			case 4:
				val2 = val * m_value;
				val3 = Vector2.get_zero();
				break;
			case 5:
				val2._002Ector(((IntPtr)(void*)val).x * m_value, ((IntPtr)(void*)val).y);
				val3._002Ector((((IntPtr)(void*)val).x - ((IntPtr)(void*)val2).x) * 0.5f, 0f);
				break;
			case 6:
				val2 = val * m_value;
				val3 = (val - val2) * new Vector2(-0.5f, -0.5f);
				break;
			case 7:
				val2 = val * m_value;
				val3 = -0.5f * val + 0.5f * val2;
				break;
			case 8:
				val2 = val * m_value;
				val3 = (val - val2) * new Vector2(0.5f, -0.5f);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			val3 += offset * val;
			m_spriteRenderer.set_size(val2);
			transform.set_localPosition(new Vector3(((IntPtr)(void*)val3).x, ((IntPtr)(void*)val3).y, ((IntPtr)(void*)transform.get_localPosition()).z));
		}

		public SpriteFilling()
			: this()
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)

	}
}
