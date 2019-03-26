using System;
using System.Collections.Generic;

namespace UnityEngine
{
	[CreateAssetMenu]
	public class SpriteFont : ScriptableObject
	{
		[SerializeField]
		protected float m_shiftScale = 1f;

		[SerializeField]
		protected SpriteFontCharacter[] m_characters;

		private int m_referenceCount;

		protected List<Vector3> m_vertexPool = new List<Vector3>(64);

		protected List<int> m_trianglePool = new List<int>(64);

		protected List<Vector2> m_uvPool = new List<Vector2>(64);

		protected List<Color32> m_colorPool = new List<Color32>(64);

		protected List<Vector4> m_tintPool = new List<Vector4>(64);

		protected List<SpriteFontCharacter> m_textCharacters = new List<SpriteFontCharacter>(1);

		public bool isValid
		{
			get;
			protected set;
		}

		public bool isLoaded
		{
			get;
			private set;
		}

		public Texture2D texture
		{
			get;
			private set;
		}

		public Texture2D associatedAlphaSplitTexture
		{
			get;
			private set;
		}

		public float height
		{
			get;
			private set;
		}

		public float pixelsPerUnit
		{
			get;
			private set;
		} = 100f;


		public float lastComputedWidth
		{
			get;
			private set;
		}

		public bool Load()
		{
			if (!isLoaded)
			{
				if (InternalLoad())
				{
					m_referenceCount++;
				}
			}
			else if (isValid)
			{
				m_referenceCount++;
			}
			return isValid;
		}

		private bool InternalLoad()
		{
			int num = m_characters.Length;
			if (num == 0)
			{
				isValid = false;
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (!m_characters[i].Load())
				{
					Debug.LogWarning((object)$"No sprite for character '{m_characters[i].character}' (index={i})");
					isValid = false;
					return false;
				}
			}
			SpriteFontCharacter spriteFontCharacter = m_characters[0];
			texture = spriteFontCharacter.texture;
			associatedAlphaSplitTexture = spriteFontCharacter.associatedAlphaSplitTexture;
			height = spriteFontCharacter.height;
			pixelsPerUnit = spriteFontCharacter.pixelsPerUnit;
			isValid = (null != texture);
			isLoaded = true;
			return true;
		}

		public void Unload()
		{
			if (!isLoaded || !isValid)
			{
				return;
			}
			m_referenceCount--;
			if (m_referenceCount == 0)
			{
				int num = m_characters.Length;
				for (int i = 0; i < num; i++)
				{
					m_characters[i].Unload();
				}
				texture = null;
				associatedAlphaSplitTexture = null;
				height = 0f;
				pixelsPerUnit = 100f;
				isLoaded = false;
			}
		}

		public Mesh BuildMesh(string text, Color32 color, Vector4 tint, SpriteFontAlignment alignment)
		{
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Expected O, but got Unknown
			int num = 0;
			int num2 = 0;
			float num3 = 0f;
			int length = text.Length;
			int num4 = m_characters.Length;
			m_textCharacters.Clear();
			if (m_textCharacters.Capacity < length)
			{
				m_textCharacters.Capacity = length;
			}
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				for (int j = 0; j < num4; j++)
				{
					SpriteFontCharacter item = m_characters[j];
					if (c == item.character)
					{
						m_textCharacters.Add(item);
						num += item.vertexCount;
						num2 += item.triangleCount;
						num3 += item.width;
						break;
					}
				}
			}
			lastComputedWidth = num3;
			int count = m_textCharacters.Count;
			if (count > 0)
			{
				m_vertexPool.Clear();
				if (m_vertexPool.Capacity < num)
				{
					m_vertexPool.Capacity = num;
				}
				m_trianglePool.Clear();
				if (m_trianglePool.Capacity < num2)
				{
					m_trianglePool.Capacity = num2;
				}
				m_uvPool.Clear();
				if (m_uvPool.Capacity < num)
				{
					m_uvPool.Capacity = num;
				}
				m_colorPool.Clear();
				if (m_colorPool.Capacity < num)
				{
					m_colorPool.Capacity = num;
				}
				m_tintPool.Clear();
				if (m_tintPool.Capacity < num)
				{
					m_tintPool.Capacity = num;
				}
				float advance;
				switch (alignment)
				{
				case SpriteFontAlignment.Left:
					advance = m_textCharacters[0].leftShift / pixelsPerUnit;
					break;
				case SpriteFontAlignment.Center:
				{
					float left = m_textCharacters[0].left;
					float right = m_textCharacters[count - 1].right;
					advance = -0.5f * (num3 - left - right);
					break;
				}
				case SpriteFontAlignment.Right:
					advance = 0f - num3 + m_textCharacters[count - 1].rightShift / pixelsPerUnit;
					break;
				case SpriteFontAlignment.LeftCharacter:
					advance = m_textCharacters[0].left;
					break;
				case SpriteFontAlignment.RightCharacter:
					advance = 0f - num3 + m_textCharacters[count - 1].right;
					break;
				default:
					throw new ArgumentOutOfRangeException("alignment", alignment, null);
				}
				for (int k = 0; k < count; k++)
				{
					advance = m_textCharacters[k].Fill(m_vertexPool, m_trianglePool, m_uvPool, advance);
				}
				for (int l = 0; l < num; l++)
				{
					m_colorPool.Add(color);
					m_tintPool.Add(tint);
				}
				Mesh val = new Mesh();
				val.MarkDynamic();
				val.SetVertices(m_vertexPool);
				val.SetTriangles(m_trianglePool, 0);
				val.SetUVs(0, m_uvPool);
				val.SetColors(m_colorPool);
				val.SetTangents(m_tintPool);
				val.set_hideFlags(20);
				return val;
			}
			return null;
		}

		public void BuildUIMesh(Mesh mesh, string text, Color32 color, SpriteFontAlignment alignment)
		{
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			int num2 = 0;
			float num3 = 0f;
			int length = text.Length;
			int num4 = m_characters.Length;
			m_textCharacters.Clear();
			if (m_textCharacters.Capacity < length)
			{
				m_textCharacters.Capacity = length;
			}
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				for (int j = 0; j < num4; j++)
				{
					SpriteFontCharacter item = m_characters[j];
					if (c == item.character)
					{
						m_textCharacters.Add(item);
						num += item.vertexCount;
						num2 += item.triangleCount;
						num3 += item.width;
						break;
					}
				}
			}
			num3 = (lastComputedWidth = num3 * pixelsPerUnit);
			int count = m_textCharacters.Count;
			if (count > 0)
			{
				m_vertexPool.Clear();
				if (m_vertexPool.Capacity < num)
				{
					m_vertexPool.Capacity = num;
				}
				m_trianglePool.Clear();
				if (m_trianglePool.Capacity < num2)
				{
					m_trianglePool.Capacity = num2;
				}
				m_uvPool.Clear();
				if (m_uvPool.Capacity < num)
				{
					m_uvPool.Capacity = num;
				}
				m_colorPool.Clear();
				if (m_colorPool.Capacity < num)
				{
					m_colorPool.Capacity = num;
				}
				float advance;
				switch (alignment)
				{
				case SpriteFontAlignment.Left:
					advance = m_textCharacters[0].leftShift;
					break;
				case SpriteFontAlignment.Center:
				{
					float num6 = m_textCharacters[0].left * pixelsPerUnit;
					float num7 = m_textCharacters[count - 1].right * pixelsPerUnit;
					advance = -0.5f * (num3 - num6 - num7);
					break;
				}
				case SpriteFontAlignment.Right:
					advance = 0f - num3 + m_textCharacters[count - 1].rightShift;
					break;
				case SpriteFontAlignment.LeftCharacter:
					advance = m_textCharacters[0].left * pixelsPerUnit;
					break;
				case SpriteFontAlignment.RightCharacter:
					advance = 0f - num3 + m_textCharacters[count - 1].right * pixelsPerUnit;
					break;
				default:
					throw new ArgumentOutOfRangeException("alignment", alignment, null);
				}
				for (int k = 0; k < count; k++)
				{
					advance = m_textCharacters[k].FillUI(m_vertexPool, m_trianglePool, m_uvPool, advance);
				}
				for (int l = 0; l < num; l++)
				{
					m_colorPool.Add(color);
				}
				mesh.Clear();
				mesh.SetVertices(m_vertexPool);
				mesh.SetTriangles(m_trianglePool, 0);
				mesh.SetUVs(0, m_uvPool);
				mesh.SetColors(m_colorPool);
			}
			else
			{
				mesh.Clear();
			}
		}

		public void ChangeMeshColor(Mesh mesh, Color color, Vector4 tint)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			if (!(null == mesh))
			{
				m_colorPool.Clear();
				m_tintPool.Clear();
				int vertexCount = mesh.get_vertexCount();
				for (int i = 0; i < vertexCount; i++)
				{
					m_colorPool.Add(Color32.op_Implicit(color));
					m_tintPool.Add(tint);
				}
				mesh.SetColors(m_colorPool);
				mesh.SetTangents(m_tintPool);
			}
		}

		public SpriteFont()
			: this()
		{
		}
	}
}
