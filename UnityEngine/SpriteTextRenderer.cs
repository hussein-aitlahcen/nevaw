namespace UnityEngine
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
	public sealed class SpriteTextRenderer : MonoBehaviour
	{
		private static readonly int s_mainTextPropertyID = Shader.PropertyToID("_MainTex");

		[SerializeField]
		[HideInInspector]
		private MeshRenderer m_meshRenderer;

		[SerializeField]
		[HideInInspector]
		private MeshFilter m_meshFilter;

		[SerializeField]
		[HideInInspector]
		private Material m_material;

		[SerializeField]
		private string m_text;

		[SerializeField]
		private SpriteFont m_font;

		[SerializeField]
		private SpriteFontAlignment m_alignment = SpriteFontAlignment.Center;

		[SerializeField]
		private Color m_color = Color.get_white();

		[SerializeField]
		private Color m_tint = new Color(1f, 1f, 1f, 0f);

		private MaterialPropertyBlock m_matPropBlock;

		public string text
		{
			get
			{
				return m_text;
			}
			set
			{
				if (!(value != m_text))
				{
					return;
				}
				m_text = value;
				if (!string.IsNullOrEmpty(value))
				{
					if (null != m_font && m_font.isValid && m_font.isLoaded)
					{
						Rebuild();
					}
				}
				else
				{
					Clear();
				}
			}
		}

		public SpriteFont font
		{
			get
			{
				return m_font;
			}
			set
			{
				if (!(value != m_font))
				{
					return;
				}
				if (null != m_font)
				{
					m_font.Unload();
				}
				m_font = value;
				if (null != value)
				{
					if (value.Load())
					{
						SetTexture();
						if (!string.IsNullOrEmpty(m_text))
						{
							Rebuild();
						}
					}
				}
				else
				{
					Clear();
					ClearTexture();
				}
			}
		}

		public SpriteFontAlignment alignment
		{
			get
			{
				return m_alignment;
			}
			set
			{
				if (value != m_alignment)
				{
					m_alignment = value;
					if (!string.IsNullOrEmpty(m_text) && null != m_font && m_font.isValid && m_font.isLoaded)
					{
						Rebuild();
					}
				}
			}
		}

		public Color color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_color;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (value != m_color)
				{
					m_color = value;
					if (!string.IsNullOrEmpty(m_text) && null != m_font && m_font.isValid)
					{
						ApplyColorAndTint();
					}
				}
			}
		}

		public Color tint
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_tint;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (value != m_tint)
				{
					m_tint = value;
					if (!string.IsNullOrEmpty(m_text) && null != m_font && m_font.isValid)
					{
						ApplyColorAndTint();
					}
				}
			}
		}

		public int sortingLayerID
		{
			get
			{
				return m_meshRenderer.get_sortingLayerID();
			}
			set
			{
				m_meshRenderer.set_sortingLayerID(value);
			}
		}

		public string sortingLayerName
		{
			get
			{
				return m_meshRenderer.get_sortingLayerName();
			}
			set
			{
				m_meshRenderer.set_sortingLayerName(value);
			}
		}

		public int sortingOrder
		{
			get
			{
				return m_meshRenderer.get_sortingOrder();
			}
			set
			{
				m_meshRenderer.set_sortingOrder(value);
			}
		}

		public Bounds bounds => m_meshRenderer.get_bounds();

		public Bounds textBounds
		{
			get
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				Mesh sharedMesh = m_meshFilter.get_sharedMesh();
				if (!(null == sharedMesh))
				{
					return sharedMesh.get_bounds();
				}
				return default(Bounds);
			}
		}

		public void Set(string textValue, Color colorValue)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			if (textValue != m_text)
			{
				m_text = textValue;
				m_color = colorValue;
				if (!string.IsNullOrEmpty(textValue))
				{
					if (null != m_font && m_font.isValid && m_font.isLoaded)
					{
						Rebuild();
					}
				}
				else
				{
					Clear();
				}
			}
			else
			{
				color = colorValue;
			}
		}

		private void Rebuild()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			m_meshFilter.set_sharedMesh(m_font.BuildMesh(m_text, Color32.op_Implicit(m_color), Color.op_Implicit(m_tint), m_alignment));
		}

		private void Clear()
		{
			m_meshFilter.set_sharedMesh(null);
		}

		private void ApplyColorAndTint()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			m_font.ChangeMeshColor(m_meshFilter.get_sharedMesh(), m_color, Color.op_Implicit(m_tint));
		}

		private void SetTexture()
		{
			m_meshRenderer.GetPropertyBlock(m_matPropBlock);
			m_matPropBlock.SetTexture(s_mainTextPropertyID, m_font.texture);
			m_meshRenderer.SetPropertyBlock(m_matPropBlock);
		}

		private void ClearTexture()
		{
			m_meshRenderer.GetPropertyBlock(m_matPropBlock);
			m_matPropBlock.SetTexture(s_mainTextPropertyID, Texture2D.get_whiteTexture());
			m_meshRenderer.SetPropertyBlock(m_matPropBlock);
		}

		private void Awake()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			m_matPropBlock = new MaterialPropertyBlock();
			if (null != m_font && m_font.Load())
			{
				SetTexture();
				if (!string.IsNullOrEmpty(m_text))
				{
					Rebuild();
				}
				else
				{
					Clear();
				}
			}
			m_meshRenderer.set_enabled(this.get_enabled());
		}

		private void OnEnable()
		{
			m_meshRenderer.set_enabled(true);
		}

		private void OnDisable()
		{
			m_meshRenderer.set_enabled(false);
		}

		private void OnDestroy()
		{
			if (null != m_font)
			{
				ClearTexture();
				m_font.Unload();
			}
			m_matPropBlock = null;
		}

		private void OnDidApplyAnimationProperties()
		{
			if (null != m_font && m_font.isValid && !string.IsNullOrEmpty(m_text))
			{
				ApplyColorAndTint();
			}
		}

		public SpriteTextRenderer()
			: this()
		{
		}//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)

	}
}
