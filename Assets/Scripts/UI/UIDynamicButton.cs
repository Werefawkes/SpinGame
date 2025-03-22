using UnityEngine;
using UnityEngine.UI;
using CustomInspector;

[RequireComponent(typeof(Button))]
public class UIDynamicButton : MonoBehaviour
{
	[SelfFill(true)]
	public Button button;

	public void Start()
	{
		
	}
}