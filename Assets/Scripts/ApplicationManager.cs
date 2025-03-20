using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ApplicationManager : Foxthorne.FoxCore.Singleton<ApplicationManager>
{
	public GameObject menuReference;
	public GameObject uiReference;

	static GameObject menu;
	static GameObject ui;
	public static bool IsUIOpen { get { return ui.activeSelf; } }

	static readonly List<GameObject> tabs = new();

	private void Start()
	{
		menu = menuReference;
		ui = uiReference;

		// Get the tabs from the menu's children
		for (int i = 0; i < menu.transform.childCount; i++)
		{
			tabs.Add(menu.transform.GetChild(i).gameObject);
			tabs[i].SetActive(false);
		}

		SetCurrentTab(0);
	}

	public static void SetCurrentTab(int index)
	{
		for (int i = 0; i < tabs.Count; i++)
		{
			tabs[i].SetActive(i == index);
		}
	}

	public static void SetUIState(bool open)
	{
		ui.SetActive(open);
	}
	
	public static void ToggleMenuState()
	{
		SetUIState(!menu.activeSelf);
	}

	public static void CloseGame()
	{
		Application.Quit();
	}

	public static void OnSceneChanged(string sceneName)
	{
		if (sceneName == "Menu")
		{
			SetUIState(true);
		}
		else
		{
			SetUIState(false);
		}
	}
}
