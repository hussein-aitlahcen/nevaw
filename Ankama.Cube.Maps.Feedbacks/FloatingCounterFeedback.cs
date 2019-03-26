using Ankama.Cube.Data;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Utilities;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps.Feedbacks
{
	public class FloatingCounterFeedback : MonoBehaviour
	{
		private readonly List<FloatingCounterFloatingObject> m_floatingObjects = new List<FloatingCounterFloatingObject>(3);

		private IObjectWithCounterEffects m_parent;

		private FloatingCounterEffect m_effectDefinition;

		private int m_objectsCount;

		public int objectsCount => m_objectsCount;

		public FloatingCounterEffect effect => m_effectDefinition;

		public IEnumerator Launch(IObjectWithCounterEffects parent, FloatingCounterEffect effect, int count)
		{
			m_parent = parent;
			m_effectDefinition = effect;
			m_objectsCount = count;
			return SpawnCoroutine();
		}

		public IEnumerator SetCount(int count)
		{
			m_objectsCount = Math.Max(0, count);
			int count2 = m_floatingObjects.Count;
			if (count2 != m_objectsCount)
			{
				if (m_objectsCount < count2)
				{
					yield return RemoveCoroutine();
				}
				else
				{
					yield return SpawnCoroutine();
				}
				if (count == 0)
				{
					m_parent.ClearFloatingCounterEffect();
				}
			}
		}

		public IEnumerator FadeOut()
		{
			List<FloatingCounterFloatingObject> floatingObjects = m_floatingObjects;
			int count = floatingObjects.Count;
			if (count != 0)
			{
				float clearAnimationDuration = m_effectDefinition.clearAnimationDuration;
				for (int i = 0; i < count; i++)
				{
					floatingObjects[i].FadeOut(clearAnimationDuration);
				}
				yield return (object)new WaitForTime(clearAnimationDuration);
			}
		}

		public void SetColorModifier(Color color)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			List<FloatingCounterFloatingObject> floatingObjects = m_floatingObjects;
			int count = floatingObjects.Count;
			for (int i = 0; i < count; i++)
			{
				floatingObjects[i].SetColorModifier(color);
			}
		}

		public void Clear()
		{
			DestroyAll();
			m_objectsCount = 0;
			m_parent = null;
			m_effectDefinition = null;
		}

		private void DestroyAll()
		{
			List<FloatingCounterFloatingObject> floatingObjects = m_floatingObjects;
			int count = floatingObjects.Count;
			for (int i = 0; i < count; i++)
			{
				Object.Destroy(floatingObjects[i].get_gameObject());
			}
			floatingObjects.Clear();
		}

		private void Update()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Vector3 rotation = m_effectDefinition.rotation;
			this.get_transform().Rotate(rotation * Time.get_deltaTime(), 0);
		}

		private IEnumerator SpawnCoroutine()
		{
			FloatingCounterEffect effectDefinition = m_effectDefinition;
			float radius = effectDefinition.radius;
			float height = effectDefinition.height;
			FloatingCounterFloatingObject floatingObject = effectDefinition.floatingObject;
			float startingAnimationDuration = effectDefinition.startingAnimationDuration;
			float num = MathF.PI * 2f / (float)m_objectsCount;
			Vector3 position = default(Vector3);
			position._002Ector(0f, 0f, 0f);
			Reposition();
			for (int i = m_floatingObjects.Count; i < m_objectsCount; i++)
			{
				float num2 = num * (float)i;
				position.x = Mathf.Cos(num2) * radius;
				position.z = Mathf.Sin(num2) * radius;
				position.y = height;
				FloatingCounterFloatingObject floatingCounterFloatingObject = Object.Instantiate<FloatingCounterFloatingObject>(floatingObject, this.get_transform());
				m_floatingObjects.Add(floatingCounterFloatingObject);
				floatingCounterFloatingObject.Spawn(position, startingAnimationDuration, num2, radius);
			}
			Object.Instantiate<VisualEffect>(effectDefinition.spawnFX, this.get_transform().get_position() + effectDefinition.spawnFXOffset, Quaternion.get_identity());
			effectDefinition.PlaySound(this.get_transform());
			yield return (object)new WaitForTime(startingAnimationDuration);
		}

		private IEnumerator RemoveCoroutine()
		{
			WaitForTime wait = new WaitForTime(m_effectDefinition.endAnimationDuration);
			List<FloatingCounterFloatingObject> floatingObjects = m_floatingObjects;
			int num;
			for (int i = floatingObjects.Count - 1; i >= m_objectsCount; i = num)
			{
				floatingObjects[i].Clear();
				floatingObjects.RemoveAt(i);
				yield return wait;
				wait.Reset();
				num = i - 1;
			}
			yield return null;
			Reposition();
		}

		private void Reposition()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			List<FloatingCounterFloatingObject> floatingObjects = m_floatingObjects;
			int count = floatingObjects.Count;
			if (count > 1)
			{
				float repositionDuration = m_effectDefinition.repositionDuration;
				Ease repositionEase = m_effectDefinition.repositionEase;
				float num = MathF.PI * 2f / (float)m_objectsCount;
				for (int i = 0; i < count; i++)
				{
					floatingObjects[i].Reposition(num * (float)i, repositionDuration, repositionEase);
				}
			}
		}

		public void ChangeVisual(FloatingCounterEffect effect)
		{
			if (!(effect == m_effectDefinition))
			{
				m_effectDefinition = effect;
				if (m_objectsCount > 0)
				{
					DestroyAll();
					this.StartCoroutine(SpawnCoroutine());
				}
			}
		}

		public FloatingCounterFeedback()
			: this()
		{
		}
	}
}
