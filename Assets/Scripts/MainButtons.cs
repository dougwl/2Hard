using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtons : MonoBehaviour {

	public CanvasGroup board;
	public CanvasGroup darkfilter;
	public CanvasGroup fading;
	public InitMotion tween;
	public Transform settings;
	public Transform credits;
	public Transform progress;
	public bool lerp = false;
	public bool open = true;
	public bool openWait = true;

	private TrailRenderer[] tr;
	private ParticleSystem[] ps;
	private Transform page;
	private Transform pageWait;
	
	private void Start() {
		pageWait = null;
		if (GameManager.GM.menuScene){
			settings.localPosition = new Vector3 (GameManager.GM.screenWidth,0);
			credits.localPosition = new Vector3 (GameManager.GM.screenWidth,0);
			progress.localPosition = new Vector3 (GameManager.GM.screenWidth,0);
		}
	}

	public void PlayGame() {
		AudioManager.AM.MusicBeforePlay();
		StartCoroutine(tween.Moveout());
	}

	public void RestartGame(){
		AudioManager.AM.MusicBeforePlay();
		StartCoroutine(Game());
	}

	public void OpenMenu(){
		AudioManager.AM.MusicMainMenu();
		StartCoroutine(Menu());
	}

	public void OpenSettings(){
		if (!lerp) {
			page = settings;
			lerp = true;
			open = true;
			GameManager.GM.backButtonState = page.gameObject.name;
		}
		else if (open == false){
			pageWait = settings;
			openWait = true;
		}
	}

	public void ExitSettings(){
		if (!lerp || page == settings) {
			page = settings;
			lerp = true;
			open = false;
			GameManager.GM.backButtonState = "";
		}
		else if (open == false){
			pageWait = settings;
			openWait = false;
			GameManager.GM.backButtonState = "";
		}
	}

	public void OpenCredits(){
		if (!lerp) {
			page = credits;
			lerp = true;
			open = true;
			GameManager.GM.backButtonState = page.gameObject.name;
			print(page.gameObject.name);
		}
		else if (open == true){
			pageWait = credits;
			openWait = true;
		}
	}

	public void ExitCredits(){
		if (!lerp || page == credits) {
			page = credits;
			lerp = true;
			open = false;
			GameManager.GM.backButtonState = "Settings";
		}
	}

	public void OpenProgress(){
		if (!lerp || page == progress) {
			page = progress;
			lerp = true;
			open = true;
			GameManager.GM.backButtonState = page.gameObject.name;
		}
		else if (open == false){
			pageWait = progress;
			openWait = true;
		}
	}

	public void ExitProgress(){
		if (!lerp || page == progress) {
			page = progress;
			lerp = true;
			open = false;
			GameManager.GM.backButtonState = "";
		}
	}

	private IEnumerator Game() {
		board.blocksRaycasts=false;

		float decelerate = 0f;
		float aux = 1;
		if (GameManager.GM.gameScene) aux = 2;
		while(darkfilter.alpha!=1f){
			decelerate += Time.deltaTime;
					
			darkfilter.alpha = Mathf.Lerp(darkfilter.alpha, 1f, decelerate*aux);

			if (darkfilter.alpha > 0.95f) {
				darkfilter.alpha = 1;
				GameManager.GM.StartGame();
			}
			yield return null;
		}
	}

	private IEnumerator Menu() {
		board.blocksRaycasts=false;
		
		float decelerate = 0f;
		while(fading.alpha!=1f){
			decelerate += Time.deltaTime;
					
			fading.alpha = Mathf.Lerp(fading.alpha, 1f, decelerate);

			if (fading.alpha > 0.95f) {
				fading.alpha = 1f;
				GameManager.GM.StartMenu();
			}
			yield return null;
		}
	}

	private void Update() {
		if (GameManager.GM.menuScene){

			if (lerp && open) {
				float decelerate = Mathf.Min(10 * Time.deltaTime, 1f);

				page.localPosition = Vector2.Lerp(page.localPosition, Vector3.zero, decelerate);

				if (page.localPosition.x < 2f) {
					page.localPosition = Vector3.zero;
					lerp = false;
				}
			}
			if (lerp && !open){
				float decelerate = Mathf.Min(10 * Time.deltaTime, 1f);

				page.localPosition = Vector2.Lerp(page.localPosition, new Vector3 (GameManager.GM.screenWidth,0), decelerate);

				if (new Vector3 (GameManager.GM.screenWidth,0).x - page.localPosition.x < 25f) {
					page.localPosition = new Vector3 (GameManager.GM.screenWidth,0);
					if (pageWait != null){
						page = pageWait;
						open = openWait;
						if (open) GameManager.GM.backButtonState = page.gameObject.name;
						pageWait = null;
					}
					else lerp = false;
				}
			}	
		}
	}
}
