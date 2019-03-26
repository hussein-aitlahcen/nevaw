using UnityEngine.EventSystems;

namespace Ankama.Cube.Audio.UI
{
	public sealed class AudioEventUITriggerOnClick : AudioEventUITrigger, IPointerClickHandler, IEventSystemHandler
	{
		public void OnPointerClick(PointerEventData eventData)
		{
			PlaySound();
		}
	}
}
