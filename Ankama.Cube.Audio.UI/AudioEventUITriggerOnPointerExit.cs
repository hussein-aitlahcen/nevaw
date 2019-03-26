using UnityEngine.EventSystems;

namespace Ankama.Cube.Audio.UI
{
	public class AudioEventUITriggerOnPointerExit : AudioEventUITrigger, IPointerExitHandler, IEventSystemHandler
	{
		public void OnPointerExit(PointerEventData eventData)
		{
			PlaySound();
		}
	}
}
