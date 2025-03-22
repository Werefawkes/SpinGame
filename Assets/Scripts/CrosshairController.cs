using UnityEngine;

public class CrosshairController : MonoBehaviour
{
	public Transform chCenter;
	public Transform[] chBars;

	public Material spriteMat;

	public void SetSize(float distance, float angle)
	{
		// Get the radius for the current distance
		float radius = Mathf.Tan(angle * Mathf.Deg2Rad) * distance;

		for (int i = 0; i < chBars.Length; i++)
		{
			chBars[i].localPosition = Shooter.RotateBy(Vector2.up * radius, 90 * i);
			chBars[i].localEulerAngles = new(0, 0, 90 * i);
		}
	}

	public void SetReload(float percent)
	{
		spriteMat.SetFloat("_Arc1", Mathf.Lerp(360, 0, percent));
	}
}
