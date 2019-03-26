using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	public abstract class CharacterUILayoutElement : MonoBehaviour, ICharacterUI
	{
		private CharacterUIContainer m_container;

		protected Color m_color = Color.get_white();

		protected int m_sortingOrder;

		protected int m_layoutPosition;

		public abstract Color color
		{
			get;
			set;
		}

		public abstract int sortingOrder
		{
			get;
			set;
		}

		public int layoutWidth
		{
			get;
			protected set;
		}

		public abstract void SetLayoutPosition(int left);

		public void SetContainer(CharacterUIContainer container)
		{
			m_container = container;
		}

		protected virtual void Layout()
		{
			m_container.SetLayoutDirty();
		}

		protected unsafe static float LayoutSetTransform([NotNull] SpriteRenderer spriteRenderer, float position)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			Sprite sprite = spriteRenderer.get_sprite();
			float result;
			float num;
			if (null == sprite)
			{
				result = 0f;
				num = 0f;
			}
			else
			{
				float pixelsPerUnit = sprite.get_pixelsPerUnit();
				Rect rect = sprite.get_rect();
				result = rect.get_width() / pixelsPerUnit;
				num = ((IntPtr)(void*)sprite.get_pivot()).x / pixelsPerUnit;
			}
			Transform transform = spriteRenderer.get_transform();
			Vector3 localPosition = transform.get_localPosition();
			localPosition.x = position + num;
			transform.set_localPosition(localPosition);
			return result;
		}

		protected unsafe static float LayoutSetTransform(SpriteTextRenderer spriteRenderer, float position)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			Transform transform = spriteRenderer.get_transform();
			Vector3 localPosition = transform.get_localPosition();
			localPosition.x = position;
			transform.set_localPosition(localPosition);
			Bounds textBounds = spriteRenderer.textBounds;
			return Mathf.Ceil(100f * ((IntPtr)(void*)textBounds.get_max()).x) / 100f;
		}

		protected static void LayoutMoveTransform(Transform t, float delta)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Vector3 localPosition = t.get_localPosition();
			localPosition.x += delta;
			t.set_localPosition(localPosition);
		}

		protected CharacterUILayoutElement()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)

	}
}
