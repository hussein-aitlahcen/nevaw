using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class FightPathFinderFeedbackManager
	{
		private const float Height = 0.51f;

		private const int InitialCapacity = 4;

		private readonly List<SpriteRenderer> m_active = new List<SpriteRenderer>(4);

		private AbstractFightMap m_fightMap;

		private DirectionAngle m_mapRotation;

		private GameObject m_prefab;

		private GameObjectPool m_pool;

		public void Initialize(AbstractFightMap fightMap, Material material, uint renderLayerMask)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			m_fightMap = fightMap;
			GameObject val = new GameObject("", new Type[1]
			{
				typeof(SpriteRenderer)
			});
			SpriteRenderer component = val.GetComponent<SpriteRenderer>();
			component.set_sharedMaterial(material);
			component.set_renderingLayerMask(renderLayerMask);
			val.SetActive(false);
			Object.DontDestroyOnLoad(val);
			m_pool = new GameObjectPool(val, 4);
			m_prefab = val;
		}

		public void SetMapRotation(DirectionAngle mapRotation)
		{
			m_mapRotation = mapRotation;
		}

		public void Release()
		{
			List<SpriteRenderer> active = m_active;
			int count = active.Count;
			for (int i = 0; i < count; i++)
			{
				SpriteRenderer val = active[i];
				if (null != val)
				{
					Object.Destroy(val.get_gameObject());
				}
			}
			active.Clear();
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

		public void Setup(MovementFeedbackResources resources, List<Vector2Int> path, Vector2Int? target)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			int count = path.Count;
			if (count < 2)
			{
				Clear();
				return;
			}
			Sprite[] sprites = resources.sprites;
			DirectionAngle mapRotation = m_mapRotation;
			List<SpriteRenderer> active = m_active;
			int count2 = active.Count;
			Vector2Int val = path[0];
			Vector2Int val2 = path[1];
			Direction direction = val.GetDirectionTo(val2).Rotate(mapRotation);
			Direction direction2 = direction;
			if (count2 >= count)
			{
				for (int num = count2 - 1; num >= count; num--)
				{
					SpriteRenderer val3 = active[num];
					m_pool.Release(val3.get_gameObject());
					m_active.RemoveAt(num);
				}
				SetStartSprite(sprites, active[0], val, direction2);
				for (int i = 1; i < count - 1; i++)
				{
					val = val2;
					val2 = path[i + 1];
					direction2 = val.GetDirectionTo(val2).Rotate(mapRotation);
					SetSprite(sprites, active[i], val, direction2, direction);
					direction = direction2;
				}
				if (target.HasValue)
				{
					direction2 = val2.GetDirectionTo(target.Value).Rotate(mapRotation);
					SetSprite(sprites, active[count - 1], val2, direction2, direction);
				}
				else
				{
					SetEndSprite(sprites, active[count - 1], val2, direction2);
				}
			}
			else if (count2 > 0)
			{
				SetStartSprite(sprites, active[0], val, direction2);
				for (int j = 1; j < count2; j++)
				{
					val = val2;
					val2 = path[j + 1];
					direction2 = val.GetDirectionTo(val2).Rotate(mapRotation);
					SetSprite(sprites, active[j], path[j], direction2, direction);
					direction = direction2;
				}
				for (int k = count2; k < count - 1; k++)
				{
					val = val2;
					val2 = path[k + 1];
					direction2 = val.GetDirectionTo(val2).Rotate(mapRotation);
					SetSprite(sprites, null, path[k], direction2, direction);
					direction = direction2;
				}
				if (target.HasValue)
				{
					direction2 = val2.GetDirectionTo(target.Value).Rotate(mapRotation);
					SetSprite(sprites, null, val2, direction2, direction);
				}
				else
				{
					SetEndSprite(sprites, null, val2, direction2);
				}
			}
			else
			{
				SetStartSprite(sprites, null, val, direction2);
				for (int l = 1; l < count - 1; l++)
				{
					val = val2;
					val2 = path[l + 1];
					direction2 = val.GetDirectionTo(val2).Rotate(mapRotation);
					SetSprite(sprites, null, path[l], direction2, direction);
					direction = direction2;
				}
				if (target.HasValue)
				{
					direction2 = val2.GetDirectionTo(target.Value).Rotate(mapRotation);
					SetSprite(sprites, null, val2, direction2, direction);
				}
				else
				{
					SetEndSprite(sprites, null, val2, direction2);
				}
			}
		}

		public void Clear()
		{
			List<SpriteRenderer> active = m_active;
			int count = active.Count;
			if (count != 0)
			{
				for (int i = 0; i < count; i++)
				{
					SpriteRenderer val = active[i];
					m_pool.Release(val.get_gameObject());
				}
				active.Clear();
			}
		}

		private void SetStartSprite(Sprite[] sprites, SpriteRenderer instance, Vector2Int coord, Direction direction)
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			Transform transform = m_fightMap.GetCellObject(coord.get_x(), coord.get_y()).get_transform();
			Transform transform2;
			if (null == instance)
			{
				GameObject obj = m_pool.Instantiate(transform, true);
				instance = obj.GetComponent<SpriteRenderer>();
				transform2 = obj.get_transform();
				m_active.Add(instance);
			}
			else
			{
				transform2 = instance.get_transform();
				transform2.SetParent(transform, false);
			}
			Sprite sprite;
			bool flag;
			switch (direction)
			{
			case Direction.SouthEast:
				sprite = sprites[6];
				flag = true;
				break;
			case Direction.SouthWest:
				sprite = sprites[6];
				flag = false;
				break;
			case Direction.NorthWest:
				sprite = sprites[7];
				flag = false;
				break;
			case Direction.NorthEast:
				sprite = sprites[7];
				flag = true;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			Quaternion val = m_mapRotation.GetInverseRotation() * Quaternion.Euler(90f, flag ? 90f : 0f, 0f);
			instance.set_sprite(sprite);
			instance.set_flipX(flag);
			transform2.SetPositionAndRotation(transform.get_position() + new Vector3(0f, 0.51f, 0f), val);
			instance.get_gameObject().SetActive(true);
		}

		private void SetSprite(Sprite[] sprites, SpriteRenderer instance, Vector2Int coord, Direction direction, Direction previousDirection)
		{
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			Transform transform = m_fightMap.GetCellObject(coord.get_x(), coord.get_y()).get_transform();
			Transform transform2;
			if (null == instance)
			{
				GameObject obj = m_pool.Instantiate(transform, true);
				instance = obj.GetComponent<SpriteRenderer>();
				transform2 = obj.get_transform();
				m_active.Add(instance);
			}
			else
			{
				transform2 = instance.get_transform();
				transform2.SetParent(transform, false);
			}
			Sprite sprite;
			bool flag;
			if (direction == previousDirection)
			{
				switch (direction)
				{
				case Direction.SouthEast:
					sprite = sprites[8];
					flag = false;
					break;
				case Direction.SouthWest:
					sprite = sprites[8];
					flag = true;
					break;
				case Direction.NorthWest:
					sprite = sprites[9];
					flag = false;
					break;
				case Direction.NorthEast:
					sprite = sprites[9];
					flag = true;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				switch (previousDirection)
				{
				case Direction.SouthEast:
					switch (direction)
					{
					case Direction.SouthWest:
						sprite = sprites[0];
						flag = true;
						break;
					case Direction.NorthWest:
						sprite = sprites[9];
						flag = false;
						break;
					case Direction.NorthEast:
						sprite = sprites[1];
						flag = true;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
					break;
				case Direction.SouthWest:
					switch (direction)
					{
					case Direction.SouthEast:
						sprite = sprites[0];
						flag = false;
						break;
					case Direction.NorthWest:
						sprite = sprites[1];
						flag = false;
						break;
					case Direction.NorthEast:
						sprite = sprites[9];
						flag = true;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
					break;
				case Direction.NorthWest:
					switch (direction)
					{
					case Direction.SouthEast:
						sprite = sprites[8];
						flag = false;
						break;
					case Direction.SouthWest:
						sprite = sprites[2];
						flag = true;
						break;
					case Direction.NorthEast:
						sprite = sprites[3];
						flag = true;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
					break;
				case Direction.NorthEast:
					switch (direction)
					{
					case Direction.SouthEast:
						sprite = sprites[2];
						flag = false;
						break;
					case Direction.SouthWest:
						sprite = sprites[8];
						flag = true;
						break;
					case Direction.NorthWest:
						sprite = sprites[3];
						flag = false;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			Quaternion val = m_mapRotation.GetInverseRotation() * Quaternion.Euler(90f, flag ? 90f : 0f, 0f);
			instance.set_sprite(sprite);
			instance.set_flipX(flag);
			transform2.SetPositionAndRotation(transform.get_position() + new Vector3(0f, 0.51f, 0f), val);
			instance.get_gameObject().SetActive(true);
		}

		private void SetEndSprite(Sprite[] sprites, SpriteRenderer instance, Vector2Int coord, Direction direction)
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			Transform transform = m_fightMap.GetCellObject(coord.get_x(), coord.get_y()).get_transform();
			Transform transform2;
			if (null == instance)
			{
				GameObject obj = m_pool.Instantiate(transform, true);
				instance = obj.GetComponent<SpriteRenderer>();
				transform2 = obj.get_transform();
				m_active.Add(instance);
			}
			else
			{
				transform2 = instance.get_transform();
				transform2.SetParent(transform, false);
			}
			Sprite sprite;
			bool flag;
			switch (direction)
			{
			case Direction.SouthEast:
				sprite = sprites[4];
				flag = true;
				break;
			case Direction.SouthWest:
				sprite = sprites[4];
				flag = false;
				break;
			case Direction.NorthWest:
				sprite = sprites[5];
				flag = false;
				break;
			case Direction.NorthEast:
				sprite = sprites[5];
				flag = true;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			Quaternion val = m_mapRotation.GetInverseRotation() * Quaternion.Euler(90f, flag ? 90f : 0f, 0f);
			instance.set_sprite(sprite);
			instance.set_flipX(flag);
			transform2.SetPositionAndRotation(transform.get_position() + new Vector3(0f, 0.51f, 0f), val);
			instance.get_gameObject().SetActive(true);
		}
	}
}
