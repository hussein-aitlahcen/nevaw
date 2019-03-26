namespace UnityEngine.UI
{
	[AddComponentMenu("UI/UI Sprite Text Renderer", 12)]
	public sealed class UISpriteTextRenderer : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
	{
		private static Material s_etc1DefaultUIMaterial;

		[SerializeField]
		private string m_text;

		[SerializeField]
		private SpriteFont m_font;

		[SerializeField]
		private SpriteFontAlignment m_alignment = SpriteFontAlignment.Center;

		[SerializeField]
		private Color m_color = Color.get_white();

		[SerializeField]
		private Color m_tint = new Color(1f, 1f, 1f, 1f);

		private float m_computedWidth;

		public string text
		{
			get
			{
				return m_text;
			}
			set
			{
				if (!value.Equals(m_text))
				{
					m_text = value;
					if (null != m_font && m_font.isValid)
					{
						this.SetVerticesDirty();
					}
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
						if (!string.IsNullOrEmpty(m_text))
						{
							this.SetAllDirty();
						}
						else
						{
							this.SetMaterialDirty();
						}
					}
				}
				else if (!string.IsNullOrEmpty(m_text))
				{
					this.SetAllDirty();
				}
				else
				{
					this.SetMaterialDirty();
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
					if (!string.IsNullOrEmpty(m_text) && null != m_font && m_font.isValid)
					{
						this.SetVerticesDirty();
						this.SetLayoutDirty();
					}
				}
			}
		}

		public override Color color
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
						this.SetVerticesDirty();
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
						this.SetVerticesDirty();
					}
				}
			}
		}

		public static Material defaultETC1GraphicMaterial
		{
			get
			{
				if (s_etc1DefaultUIMaterial == null)
				{
					s_etc1DefaultUIMaterial = Canvas.GetETC1SupportedCanvasMaterial();
				}
				return s_etc1DefaultUIMaterial;
			}
		}

		public override Texture mainTexture
		{
			get
			{
				if (null == m_font)
				{
					if (this.get_material() != null && this.get_material().get_mainTexture() != null)
					{
						return this.get_material().get_mainTexture();
					}
					return Graphic.s_WhiteTexture;
				}
				return m_font.texture;
			}
		}

		public bool hasBorder => false;

		public float pixelsPerUnit
		{
			get
			{
				float num = 100f;
				if (null != m_font)
				{
					num = m_font.pixelsPerUnit;
				}
				float num2 = 100f;
				if (null != this.get_canvas())
				{
					num2 = this.get_canvas().get_referencePixelsPerUnit();
				}
				return num / num2;
			}
		}

		public override Material material
		{
			get
			{
				if (base.m_Material != null)
				{
					return base.m_Material;
				}
				if (null != m_font && null != m_font.associatedAlphaSplitTexture)
				{
					return defaultETC1GraphicMaterial;
				}
				return this.get_defaultMaterial();
			}
			set
			{
				this.set_material(value);
			}
		}

		public float minWidth => 0f;

		public float preferredWidth
		{
			get
			{
				if (null == m_font)
				{
					return 0f;
				}
				return m_computedWidth;
			}
		}

		public float flexibleWidth => -1f;

		public float minHeight => 0f;

		public float preferredHeight
		{
			get
			{
				if (null == m_font)
				{
					return 0f;
				}
				return m_font.height / pixelsPerUnit;
			}
		}

		public float flexibleHeight => -1f;

		public int layoutPriority => 0;

		protected override void OnEnable()
		{
			if (null != m_font)
			{
				m_font.Load();
			}
			this.OnEnable();
		}

		protected override void OnDisable()
		{
			if (null != m_font)
			{
				m_font.Unload();
			}
			this.OnDisable();
		}

		protected override void UpdateGeometry()
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			CanvasRenderer canvasRenderer = this.get_canvasRenderer();
			Mesh workerMesh = Graphic.get_workerMesh();
			if (null == m_font || !m_font.isValid || string.IsNullOrEmpty(m_text))
			{
				workerMesh.Clear();
				m_computedWidth = 0f;
			}
			else
			{
				if (base.m_Material == null && m_tint != Color.get_white())
				{
					Color val = m_color * m_tint;
					m_font.BuildUIMesh(workerMesh, m_text, Color32.op_Implicit(val), m_alignment);
					m_font.ChangeMeshColor(workerMesh, val, Color.op_Implicit(Color.get_white()));
				}
				else
				{
					m_font.BuildUIMesh(workerMesh, m_text, Color32.op_Implicit(m_color), m_alignment);
					m_font.ChangeMeshColor(workerMesh, m_color, Color.op_Implicit(m_tint));
				}
				m_computedWidth = m_font.lastComputedWidth;
			}
			canvasRenderer.SetMesh(workerMesh);
		}

		protected override void UpdateMaterial()
		{
			this.UpdateMaterial();
			if (null == m_font)
			{
				this.get_canvasRenderer().SetAlphaTexture(null);
				return;
			}
			Texture2D associatedAlphaSplitTexture = m_font.associatedAlphaSplitTexture;
			if (null != associatedAlphaSplitTexture)
			{
				this.get_canvasRenderer().SetAlphaTexture(associatedAlphaSplitTexture);
			}
		}

		public void CalculateLayoutInputHorizontal()
		{
		}

		public void CalculateLayoutInputVertical()
		{
		}

		public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			return true;
		}

		public UISpriteTextRenderer()
			: this()
		{
		}//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)

	}
}
