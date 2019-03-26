using UnityEngine;

public class CursorManager : MonoBehaviour
{
	[SerializeField]
	protected Texture2D m_cursorTexture;

	[SerializeField]
	protected Vector2 m_cursorHotspot;

	protected Texture2D m_currentTexture;

	protected Vector2 m_currentHotspot;

	public static CursorManager instance
	{
		get;
		private set;
	}

	public static bool hasInstance => null != instance;

	public void SetCursor(Texture2D texture, Vector2 hotspot)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Cursor.SetCursor(texture, hotspot, 0);
	}

	public void ResetCursor()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		Cursor.SetCursor(m_cursorTexture, m_cursorHotspot, 0);
	}

	private void Awake()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		instance = this;
		if ((int)SystemInfo.get_deviceType() == 1)
		{
			Cursor.set_visible(false);
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (focus)
		{
			Cursor.SetCursor(m_cursorTexture, m_cursorHotspot, 0);
		}
		else
		{
			Cursor.SetCursor(null, Vector2.get_zero(), 0);
		}
	}

	public CursorManager()
		: this()
	{
	}
}
