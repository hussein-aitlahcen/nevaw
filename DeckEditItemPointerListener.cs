using Ankama.Cube.Audio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckEditItemPointerListener : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler
{
	private string m_overSound = "event:/UI/Menu/UI_GEN_RollOver_Button";

	private bool m_destroying;

	private RectTransform m_effectTarget;

	private void Awake()
	{
		m_effectTarget = this.get_transform().GetChild(0).GetComponent<RectTransform>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!m_destroying)
		{
			ShortcutExtensions.DOLocalMoveY(m_effectTarget, 5f, 0.1f, false);
			AudioManager.PlayOneShot(m_overSound);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ShortcutExtensions.DOLocalMoveY(m_effectTarget, 0f, 0.1f, false);
	}

	public void RemoveComponent()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		m_destroying = true;
		Vector3 localPosition = m_effectTarget.get_localPosition();
		localPosition.y = 0f;
		m_effectTarget.set_localPosition(localPosition);
		Object.Destroy(this);
	}

	public DeckEditItemPointerListener()
		: this()
	{
	}
}
