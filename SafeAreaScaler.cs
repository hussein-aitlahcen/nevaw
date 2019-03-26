using UnityEngine;

public class SafeAreaScaler : MonoBehaviour
{
	public RectTransform canvas;

	public RectTransform panel;

	[Header("Anchors")]
	[Range(0f, 1f)]
	public float TopPercent = 1f;

	[Range(0f, 1f)]
	public float BottomPercent = 1f;

	[Range(0f, 1f)]
	public float LeftPercent = 1f;

	[Range(0f, 1f)]
	public float RightPercent = 1f;

	private Rect lastSafeArea = new Rect(0f, 0f, 0f, 0f);

	private Rect GetSafeArea()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		Rect safeArea = Screen.get_safeArea();
		float num4 = safeArea.get_x() * LeftPercent;
		num2 = (float)Screen.get_width() - ((float)Screen.get_width() - (safeArea.get_width() + safeArea.get_x())) * RightPercent - safeArea.get_x() * LeftPercent;
		num = safeArea.get_y() * BottomPercent;
		num3 = (float)Screen.get_height() - ((float)Screen.get_height() - (safeArea.get_height() + safeArea.get_y())) * TopPercent - safeArea.get_y() * BottomPercent;
		return new Rect(num4, num, num2, num3);
	}

	private void ApplySafeArea(Rect area)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 position = area.get_position();
		Vector2 anchorMax = area.get_position() + area.get_size();
		position.x /= Screen.get_width();
		position.y /= Screen.get_height();
		anchorMax.x /= Screen.get_width();
		anchorMax.y /= Screen.get_height();
		panel.set_anchorMin(position);
		panel.set_anchorMax(anchorMax);
		lastSafeArea = area;
	}

	private void Update()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Rect safeArea = GetSafeArea();
		if (safeArea != lastSafeArea)
		{
			ApplySafeArea(safeArea);
		}
	}

	public SafeAreaScaler()
		: this()
	{
	}//IL_0041: Unknown result type (might be due to invalid IL or missing references)
	//IL_0046: Unknown result type (might be due to invalid IL or missing references)

}
