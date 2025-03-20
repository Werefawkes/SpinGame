using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class UIManager : MonoBehaviour
{
	[HorizontalLine("HUD")]
	public float healthbarWidthPerHP = 20;

	[HorizontalLine("References")]
	[ForceFill]
	public UIHealthbar healthBar;

	private void Update()
	{
		// Set healthbar
		if (GameManager.localPlayer)
		{
			// don't have to set width every frame, just when max health changes
			healthBar.SetWidth(GameManager.localPlayer.stats.maxHealth * healthbarWidthPerHP);
			healthBar.SetFillPercent(GameManager.localPlayer.currentHealth / GameManager.localPlayer.stats.maxHealth);
		}
	}
}
