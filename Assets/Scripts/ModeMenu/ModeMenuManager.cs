using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeMenuManager : MonoBehaviour {
	
	[SerializeField] private Text gameMode;
	[SerializeField] private Text gameModeTitle;
	[SerializeField] private Transform highlight;
	public bool _lerp = false;
	public Vector3 _lerpTo;
	private GameManager GM;
	
	private void Start() {
        GM = GameManager.GM;

        GM.modeMenu = this.gameObject;
		gameMode.text = GM.GameMode.ToString();
		if (!GM.isRandom) {
			if (gameMode.text == "Normal") 	highlight.localPosition = new Vector3(0,500);
			if (gameMode.text == "Duo") 	highlight.localPosition = new Vector3(0,350);
			if (gameMode.text == "Slow") 	highlight.localPosition = new Vector3(0,200);
			if (gameMode.text == "NoWalls")highlight.localPosition = new Vector3(0,50);
			if (gameMode.text == "Survival")highlight.localPosition = new Vector3(0,-100);
			if (gameMode.text == "Pulse") 	highlight.localPosition = new Vector3(0,-250);
			if (gameMode.text == "Ghost") 	highlight.localPosition = new Vector3(0,-400);
		}
		else if (GM.GameState == GameState.InGame) gameModeTitle.text = "Random";
		else gameMode.text = "Random";
	}

	public void NormalMode(){SetMode("Normal"); _lerpTo = new Vector3(0,500); _lerp = true;}
	public void TwoBallsMode(){SetMode("Duo"); _lerpTo = new Vector3 (0,350); _lerp = true;}
	public void SlowBallsMode(){SetMode("Slow"); _lerpTo = new Vector3 (0,200); _lerp = true;}
	public void NoWallsMode(){SetMode("No Walls"); _lerpTo = new Vector3 (0,50); _lerp = true;}
	public void SurvivalMode(){SetMode("Survival"); _lerpTo = new Vector3 (0,-100); _lerp = true;}
	public void PulsingMode(){SetMode("Pulse"); _lerpTo = new Vector3 (0,-250); _lerp = true;}
	public void GhostBallsMode(){SetMode("Ghost"); _lerpTo = new Vector3 (0,-400); _lerp = true;}
	public void RandomMode(){SetRandom(); _lerpTo = new Vector3 (0,-550); _lerp = true;}

	public void SetMode(string mode){
		gameMode.text = mode;
		if (GM.GameState == GameState.InGame) gameModeTitle.text = "Game Mode";
	}

	public void SetRandom(){
		if (GM.GameState == GameState.MainMenu) gameMode.text = "Random";
		else {
			gameMode.text = GM.GameMode.ToString();
			gameModeTitle.text = "Random";
		}
	}

	private void FadeOutMe(GameObject cv){
		StartCoroutine(FadeOut(cv.GetComponent<CanvasGroup>()));
	}

	private void FadeInMe(GameObject cv){
		StartCoroutine(FadeIn(cv.GetComponent<CanvasGroup>()));
	}

	private IEnumerator FadeOut(CanvasGroup cg){
		
		while(cg.alpha != 0f){
			cg.alpha = Mathf.Lerp(cg.alpha, 0f, Time.deltaTime*15);
			if (cg.alpha < 0.05f){
				cg.alpha = 0f;	
			}
			yield return null;
		}
		cg.blocksRaycasts = false;
	}

	private IEnumerator FadeIn(CanvasGroup cg){

		while(cg.alpha != 1f){
			cg.alpha = Mathf.Lerp(cg.alpha, 1f, Time.deltaTime*15);
			if (cg.alpha > 0.95f){
				cg.alpha =  1f;	
			}
			yield return null;
		}
		cg.blocksRaycasts = true;
	}

	private void Update(){
		
        if (_lerp) {
			float decelerate = Mathf.Min(15f * Time.deltaTime, 1f);
			highlight.localPosition = Vector2.Lerp(highlight.localPosition, _lerpTo, decelerate);

			if (Vector2.SqrMagnitude(highlight.localPosition - _lerpTo) < 0.25f) {
				// snap to target and stop lerping
				highlight.localPosition = _lerpTo;
				_lerp = false;
			}
		}
	}
}