using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class UIManager : Foxthorne.FoxCore.Singleton<UIManager>
{
	[HorizontalLine("HUD")]
	public float healthbarHeightPerHP = 20;

	[HorizontalLine("References")]
	[ForceFill]	public UIHealthbar healthBar;
	[ForceFill] public TMPro.TMP_Text ammoText;

	private void Update()
	{
		// Set healthbar
		if (GameManager.localPlayer)
		{
			// don't have to set width every frame, just when max health changes
			healthBar.SetHeight(GameManager.localPlayer.stats.maxHealth * healthbarHeightPerHP);
			healthBar.SetFillPercent(GameManager.localPlayer.currentHealth / GameManager.localPlayer.stats.maxHealth);
		}
	}

	public void SetAmmoText(int magazine, int reserve)
	{
		ammoText.text = magazine + " / " + reserve;
	}
}
