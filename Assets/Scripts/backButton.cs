using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backButton : MonoBehaviour {

	public MainButtons mb;
	public ScrollSnapVariate ssv;
	private GameManager GM;

	#if UNITY_EDITOR || UNITY_ANDROID

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (GM.GameState == GameState.MainMenu){
				if (GM.backButtonState == BackState.Mode){
					ssv.LerpDown();
				}
				if (GM.backButtonState == BackState.Settings){
					mb.ExitSettings();
				}
				if (GM.backButtonState == BackState.Progress){
					mb.ExitProgress();
				}
				if (GM.backButtonState == BackState.Credits){
					mb.ExitCredits();
				}
			}
			if (GM.GameState == GameState.InGame){
				if (GM.backButtonState == BackState.Mode){
					ssv.LerpDown();
				}
				else if (!GM.playing){
					mb.OpenMenu();
				}
			}
		}
	}

	#endif
}
