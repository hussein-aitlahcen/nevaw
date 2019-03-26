using UnityEngine.EventSystems;

namespace Ankama.Cube.Audio.UI
{
	public class AudioEventUITriggerOnPointerEnter : AudioEventUITrigger, IPointerEnterHandler, IEventSystemHandler
	{
		public void OnPointerEnter(PointerEventData eventData)
		{
			PlaySound();
		}
	}
}
