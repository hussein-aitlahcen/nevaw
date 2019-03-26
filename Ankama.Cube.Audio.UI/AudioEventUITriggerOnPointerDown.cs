using UnityEngine.EventSystems;

namespace Ankama.Cube.Audio.UI
{
	public class AudioEventUITriggerOnPointerDown : AudioEventUITrigger, IPointerDownHandler, IEventSystemHandler
	{
		public void OnPointerDown(PointerEventData eventData)
		{
			PlaySound();
		}
	}
}
