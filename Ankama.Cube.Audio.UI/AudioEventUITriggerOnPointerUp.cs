using UnityEngine.EventSystems;

namespace Ankama.Cube.Audio.UI
{
	public class AudioEventUITriggerOnPointerUp : AudioEventUITrigger, IPointerUpHandler, IEventSystemHandler
	{
		public void OnPointerUp(PointerEventData eventData)
		{
			PlaySound();
		}
	}
}
