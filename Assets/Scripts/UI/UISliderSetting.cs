using UnityEngine;
using UnityEngine.UI;

public class UISliderSetting : MonoBehaviour
{
	public string paramName;
	public float defaultValue = 0.8f;
	public Slider slider;
	public TMPro.TMP_Text displayText;
	private void Start()
	{
		slider.value = PlayerPrefs.GetFloat(paramName, defaultValue);
		SliderUpdate(slider.value);
	}

	public void SliderUpdate(float percent)
	{
		ApplicationManager.Instance.UpdateAudioVolume(paramName, percent);
		displayText.text = Mathf.RoundToInt(percent * 100) + "%";
	}
}
