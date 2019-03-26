using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace UnityEngine
{
	[Serializable]
	public struct SpriteFontCharacter
	{
		[SerializeField]
		private char m_char;

		[SerializeField]
		private Sprite m_sprite;

		[SerializeField]
		private float m_leftShift;

		[SerializeField]
		private float m_rightShift;

		private Vector3[] m_vertices;

		private int[] m_triangles;

		private Vector2[] m_uvs;

		private Vector4 m_rect;

		public char character
		{
			[Pure]
			get
			{
				return m_char;
			}
		}

		public Texture2D texture
		{
			[Pure]
			get
			{
				return m_sprite.get_texture();
			}
		}

		public Texture2D associatedAlphaSplitTexture
		{
			[Pure]
			get
			{
				return m_sprite.get_associatedAlphaSplitTexture();
			}
		}

		public float pixelsPerUnit
		{
			[Pure]
			get
			{
				return m_sprite.get_pixelsPerUnit();
			}
		}

		public int vertexCount
		{
			[Pure]
			get
			{
				return m_vertices.Length;
			}
		}

		public int triangleCount
		{
			[Pure]
			get
			{
				return m_triangles.Length;
			}
		}

		public float left
		{
			[Pure]
			get
			{
				return m_rect.x;
			}
		}

		public float right
		{
			[Pure]
			get
			{
				return m_rect.y;
			}
		}

		public float width
		{
			[Pure]
			get
			{
				return m_rect.z;
			}
		}

		public float height
		{
			[Pure]
			get
			{
				return m_rect.w;
			}
		}

		public float leftShift
		{
			[Pure]
			get
			{
				return m_leftShift;
			}
		}

		public float rightShift
		{
			[Pure]
			get
			{
				return m_rightShift;
			}
		}

		public unsafe bool Load()
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			if (null == m_sprite)
			{
				return false;
			}
			float num = 0f;
			float num2 = 0f;
			Vector2[] vertices = m_sprite.get_vertices();
			ushort[] triangles = m_sprite.get_triangles();
			int num3 = vertices.Length;
			int num4 = triangles.Length;
			m_vertices = (Vector3[])new Vector3[num3];
			m_triangles = new int[num4];
			for (int i = 0; i < num3; i++)
			{
				Vector2 val = vertices[i];
				num = Mathf.Min(num, ((IntPtr)(void*)val).x);
				num2 = Mathf.Max(num2, ((IntPtr)(void*)val).x);
				m_vertices[i].Set(((IntPtr)(void*)val).x, ((IntPtr)(void*)val).y, 0f);
			}
			for (int j = 0; j < num4; j++)
			{
				m_triangles[j] = triangles[j];
			}
			m_uvs = m_sprite.get_uv();
			num += m_leftShift / pixelsPerUnit;
			num2 += m_rightShift / pixelsPerUnit;
			m_rect.x = num;
			m_rect.y = num2;
			m_rect.z = num2 - num;
			ref Vector4 rect = ref m_rect;
			Rect rect2 = m_sprite.get_rect();
			rect.w = rect2.get_height();
			return true;
		}

		public void Unload()
		{
			m_vertices = null;
			m_triangles = null;
			m_uvs = null;
		}

		[Pure]
		public float Fill(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, float advance)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			float x = m_rect.x;
			int count = vertices.Count;
			int num = m_vertices.Length;
			for (int i = 0; i < num; i++)
			{
				Vector3 item = m_vertices[i];
				item.x += advance - x;
				vertices.Add(item);
				Vector2 item2 = m_uvs[i];
				uvs.Add(item2);
			}
			int num2 = m_triangles.Length;
			for (int j = 0; j < num2; j++)
			{
				triangles.Add(count + m_triangles[j]);
			}
			return advance + m_rect.z;
		}

		[Pure]
		public float FillUI(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, float advance)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			float pixelsPerUnit = this.pixelsPerUnit;
			float num = m_rect.x * pixelsPerUnit;
			int count = vertices.Count;
			int num2 = m_vertices.Length;
			for (int i = 0; i < num2; i++)
			{
				Vector3 item = m_vertices[i] * pixelsPerUnit;
				item.x += advance - num;
				vertices.Add(item);
				Vector2 item2 = m_uvs[i];
				uvs.Add(item2);
			}
			int num3 = m_triangles.Length;
			for (int j = 0; j < num3; j++)
			{
				triangles.Add(count + m_triangles[j]);
			}
			return advance + m_rect.z * pixelsPerUnit;
		}
	}
}
