using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryManager : MonoBehaviour {

    [SerializeField] private Button endSceneButton;
    [SerializeField] private Button backToMainMenuButton;

    // Use this for initialization
    void Start () {
        endSceneButton.onClick.AddListener(() => { GameManager.instance.LevelHasEnded(); });
        backToMainMenuButton.onClick.AddListener(() => { GameManager.instance.BackToMainMenu(); });
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
