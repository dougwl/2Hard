﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backButton : MonoBehaviour {

	public MainButtons mb;
	public ScrollSnapVariate ssv;

	#if UNITY_EDITOR || UNITY_ANDROID

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (GameManager.GM.menuScene){
				if (GameManager.GM.backButtonState == "Mode"){
					ssv.LerpDown();
				}
				if (GameManager.GM.backButtonState == "Settings"){
					mb.ExitSettings();
				}
				if (GameManager.GM.backButtonState == "Progress"){
					mb.ExitProgress();
				}
				if (GameManager.GM.backButtonState == "Credits"){
					mb.ExitCredits();
				}
			}
			if (GameManager.GM.gameScene){
				if (GameManager.GM.backButtonState == "Mode"){
					ssv.LerpDown();
				}
				else if (!GameManager.GM.playing){
					mb.OpenMenu();
				}
			}
		}
	}

	#endif
}
