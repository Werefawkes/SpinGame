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

	public void SetFillPercent(float percent)
	{
		healthbarFill.fillAmount = percent;
	}
}
