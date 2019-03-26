using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.SRP;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class FightMapCellPointerManager
	{
		private const float Shift = -0.01f;

		private const float Height = 0.51f;

		private const int InitialCapacity = 4;

		private readonly List<CellPointer> m_active = new List<CellPointer>(4);

		private DirectionAngle m_mapRotation;

		private GameObject m_prefab;

		private GameObjectPool m_pool;

		private bool m_displayPlayableCharactersHighlights;

		private CellPointer m_cursor;

		public void Initialize(MovementFeedbackResources resources, Material material, uint renderLayerMask)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			Sprite allyCursorSprite = resources.allyCursorSprite;
			GameObject val = new GameObject("", new Type[1]
			{
				typeof(CellPointer)
			});
			CellPointer component = val.GetComponent<CellPointer>();
			component.Initialize(material, renderLayerMask);
			component.SetSprite(allyCursorSprite);
			val.SetActive(false);
			Object.DontDestroyOnLoad(val);
			GameObject obj = Object.Instantiate<GameObject>(val);
			CellPointer component2 = obj.GetComponent<CellPointer>();
			component2.Initialize(material, renderLayerMask);
			obj.SetActive(true);
			m_pool = new GameObjectPool(val, 4);
			m_prefab = val;
			m_cursor = component2;
			CellPointer.Initialize();
		}

		public void SetMapRotation(DirectionAngle mapRotation)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			m_mapRotation = mapRotation;
			if (null != m_cursor)
			{
				m_cursor.get_transform().set_rotation(mapRotation.GetInverseRotation());
			}
		}

		public void Release()
		{
			CellPointer.Release();
			List<CellPointer> active = m_active;
			int count = active.Count;
			for (int i = 0; i < count; i++)
			{
				CellPointer cellPointer = active[i];
				if (null != cellPointer)
				{
					Object.Destroy(cellPointer.get_gameObject());
				}
			}
			active.Clear();
			if (null != m_cursor)
			{
				Object.Destroy(m_cursor.get_gameObject());
				m_cursor = null;
			}
			if (m_pool != null)
			{
				m_pool.Dispose();
				m_pool = null;
			}
			if (null != m_prefab)
			{
				Object.Destroy(m_prefab);
				m_prefab = null;
			}
		}

		public void SetCursorPosition([NotNull] CellObject parent)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			if (null == m_cursor)
			{
				return;
			}
			Transform transform = parent.get_transform();
			Transform transform2 = m_cursor.get_transform();
			transform2.SetParent(transform, true);
			Quaternion inverseRotation = m_mapRotation.GetInverseRotation();
			Vector3 val = transform.get_position() + inverseRotation * new Vector3(-0.01f, 0.51f, -0.01f);
			Quaternion val2 = m_mapRotation.GetInverseRotation() * Quaternion.Euler(90f, 0f, 0f);
			transform2.SetPositionAndRotation(val, val2);
			if (!m_displayPlayableCharactersHighlights)
			{
				return;
			}
			List<CellPointer> active = m_active;
			int count = active.Count;
			for (int i = 0; i < count; i++)
			{
				CellPointer cellPointer = active[i];
				if (!(null == cellPointer) && cellPointer.get_transform().get_parent() == transform)
				{
					m_cursor.get_gameObject().SetActive(false);
					return;
				}
			}
			m_cursor.get_gameObject().SetActive(true);
		}

		public void ShowCursor()
		{
			if (!(null == m_cursor))
			{
				m_cursor.Show();
			}
		}

		public void HideCursor()
		{
			if (!(null == m_cursor))
			{
				m_cursor.Hide();
			}
		}

		public void SetAnimatedCursor(bool value)
		{
			if (!(null == m_cursor))
			{
				m_cursor.SetAnimated(value);
			}
		}

		public void BeginHighlightingPlayableCharacters(IMap map, IMapEntityProvider entityProvider)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			if (!m_displayPlayableCharactersHighlights)
			{
				GameObjectPool pool = m_pool;
				if (pool != null)
				{
					List<CellPointer> active = m_active;
					Quaternion inverseRotation = m_mapRotation.GetInverseRotation();
					Vector3 val = inverseRotation * new Vector3(-0.01f, 0.51f, -0.01f);
					Quaternion val2 = inverseRotation * Quaternion.Euler(90f, 0f, 0f);
					Transform val3 = (null != m_cursor) ? m_cursor.get_transform().get_parent() : null;
					foreach (ICharacterEntity item in entityProvider.EnumeratePlayableCharacters())
					{
						Vector2Int refCoord = item.area.refCoord;
						Transform transform = map.GetCellObject(refCoord.get_x(), refCoord.get_y()).get_transform();
						Vector3 val4 = transform.get_position() + val;
						GameObject obj = pool.Instantiate(val4, val2, transform);
						CellPointer component = obj.GetComponent<CellPointer>();
						component.SetAnimated(value: true);
						component.Show();
						obj.SetActive(true);
						active.Add(component);
						if (transform == val3)
						{
							m_cursor.get_gameObject().SetActive(false);
						}
					}
					m_displayPlayableCharactersHighlights = true;
				}
			}
		}

		public void RefreshPlayableCharactersHighlights(IMap map, IMapEntityProvider entityProvider)
		{
			EndHighlightingPlayableCharacters();
			BeginHighlightingPlayableCharacters(map, entityProvider);
		}

		public void EndHighlightingPlayableCharacters()
		{
			if (!m_displayPlayableCharactersHighlights)
			{
				return;
			}
			GameObjectPool pool = m_pool;
			if (pool == null)
			{
				return;
			}
			List<CellPointer> active = m_active;
			int count = active.Count;
			for (int i = 0; i < count; i++)
			{
				CellPointer cellPointer = active[i];
				if (!(null == cellPointer))
				{
					pool.Release(cellPointer.get_gameObject());
				}
			}
			active.Clear();
			m_displayPlayableCharactersHighlights = false;
			if (null != m_cursor)
			{
				m_cursor.get_gameObject().SetActive(true);
			}
		}

		public void SetCharacterFocusLayer()
		{
			if (null != m_cursor)
			{
				m_cursor.get_gameObject().set_layer(LayerMaskNames.characterFocusLayer);
			}
		}

		public void SetDefaultLayer()
		{
			if (null != m_cursor)
			{
				m_cursor.get_gameObject().set_layer(LayerMaskNames.defaultLayer);
			}
		}
	}
}
