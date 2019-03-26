using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public static class InputUtility
	{
		private static readonly List<IPointerClickHandler> s_clickHandlerBuffer = new List<IPointerClickHandler>();

		[PublicAPI]
		public static Vector3 pointerPosition => Input.get_mousePosition();

		public static bool IsMouseOverUI
		{
			get
			{
				if (EventSystem.get_current() != null)
				{
					return EventSystem.get_current().IsPointerOverGameObject();
				}
				return false;
			}
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize()
		{
			Input.set_simulateMouseWithTouches(false);
		}

		[PublicAPI]
		public static bool GetPointerDown()
		{
			return Input.GetMouseButtonDown(0);
		}

		[PublicAPI]
		public static bool IsPointerDown()
		{
			return Input.GetMouseButton(0);
		}

		[PublicAPI]
		public static bool GetPointerUp()
		{
			return Input.GetMouseButtonUp(0);
		}

		[PublicAPI]
		public static bool GetSecondaryDown()
		{
			return Input.GetMouseButtonDown(1);
		}

		[PublicAPI]
		public static bool IsSecondaryDown()
		{
			return Input.GetMouseButton(1);
		}

		[PublicAPI]
		public static bool GetSecondaryUp()
		{
			return Input.GetMouseButtonUp(1);
		}

		[PublicAPI]
		public static bool GetTertiaryDown()
		{
			return Input.GetMouseButtonDown(2);
		}

		[PublicAPI]
		public static bool IsTertiaryDown()
		{
			return Input.GetMouseButton(2);
		}

		[PublicAPI]
		public static bool GetTertiaryUp()
		{
			return Input.GetMouseButtonUp(2);
		}

		public static void SimulateClickOn(Selectable button)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			if (!(null == button) && button.IsInteractable() && button.get_isActiveAndEnabled())
			{
				List<IPointerClickHandler> list = s_clickHandlerBuffer;
				button.GetComponents<IPointerClickHandler>(list);
				try
				{
					PointerEventData val = new PointerEventData(EventSystem.get_current());
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						ExecuteEvents.get_pointerClickHandler().Invoke(list[i], val);
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				finally
				{
					s_clickHandlerBuffer.Clear();
				}
			}
		}
	}
}
