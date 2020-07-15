using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour {

	public MainButtons MainButtons;
	public ScrollSnapVariate GameModeScroll;
	private GameManager GM;


#if  UNITY_ANDROID

	private void Start()
	{
		GM = GameManager.GM;
		Debug.Log(GM.GameState);
	}

	// Update is called once per frame
	void Update () {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				if (GM.GameState == GameState.MainMenu){
					if (GM.backButtonState == BackState.Mode){
						GameModeScroll.LerpDown();
					}
					if (GM.backButtonState == BackState.Settings){
						MainButtons.ExitSettings();
					}
					if (GM.backButtonState == BackState.Progress){
						MainButtons.ExitProgress();
					}
					if (GM.backButtonState == BackState.Credits){
						MainButtons.ExitCredits();
					}
				}
				if (GM.GameState == GameState.InGame){
					if (GM.backButtonState == BackState.Mode){
						GameModeScroll.LerpDown();
					}
					else if (!GM.Playing){
						MainButtons.OpenMenu();
					}
				}
			}
		}


#else

    private void Awake()
		{
			this.enabled = false;
		}

#endif

}
