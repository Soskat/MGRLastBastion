﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    
    [SerializeField] private string sceneName;
    [SerializeField] private Button endSceneButton;
    [SerializeField] private Button backToMainMenuButton;

    // Use this for initialization
    void Start () {
        endSceneButton.onClick.AddListener(() => { GameManager.instance.LevelHasEnded(); });
        backToMainMenuButton.onClick.AddListener(() => { GameManager.instance.BackToMainMenu(); });

        // save level info:
        if (GameManager.instance.AnalyticsEnabled)
        {
            DataManager.AddLevelInfo(sceneName, GameManager.instance.CurrentCalculationType, GameManager.instance.BBModule.AverageHrReading, GameManager.instance.BBModule.AverageGsrReading);
            GameManager.instance.SetTime();
            DataManager.AddGameEvent(EventType.GameStart, GameManager.instance.GetTime);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
