using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Audio;
using CustomInspector;

public class ApplicationManager : Foxthorne.FoxCore.Singleton<ApplicationManager>
{
	public GameObject menuReference;
	public GameObject uiReference;

	[HorizontalLine("Audio")]
	public AudioMixer mixer;
	[AsRange(-80, 20)]
	public Vector2 dbRange;

	static GameObject menu;
	static GameObject ui;
	static SpinNetworkManager netManager;
	public static bool IsUIOpen { get { return ui.activeSelf; } }

	static readonly List<GameObject> tabs = new();

	private void Start()
	{
		menu = menuReference;
		ui = uiReference;
		netManager = SpinNetworkManager.singleton;

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

	#region Settings
	public static void PrefSave()
	{

	}
	public static void PrefApply()
	{
		
	}
	public static void PrefLoad()
	{

	}
	#endregion

	#region Button Methods
	public static void PlaySingleplayer()
	{
		netManager.maxConnections = 1;
		netManager.StartHost();
	}

	public static void PlayMultiplayer()
	{

	}

	public static void CloseGame()
	{
#if UNITY_EDITOR
		EditorApplication.ExitPlaymode();
#endif
		Application.Quit();
	}

	public void UpdateAudioVolume(string paramName, float percent)
	{
		mixer.SetFloat(paramName, Mathf.Lerp(dbRange.x, dbRange.y, percent));
		
		PlayerPrefs.SetFloat(paramName, percent);
	}
	#endregion

}
