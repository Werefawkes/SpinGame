using UnityEngine;
using UnityEngine.UI;

public class UIHealthbar : MonoBehaviour
{
	public Image healthbarBackground;
	public Image healthbarFill;

	public void SetWidth(float width)
	{
		healthbarBackground.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
	}

	public void SetHeight(float height)
	{
		healthbarBackground.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
	}

	public void SetFillPercent(float percent)
	{
		healthbarFill.fillAmount = percent;
	}
}
