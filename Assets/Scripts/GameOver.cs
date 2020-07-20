using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
	
	[SerializeField] private Text clockString;
	[SerializeField] private Text actualTime;
	[SerializeField] private Text bestTime;
	[SerializeField] private GameObject best;
	
	[SerializeField] private GameObject touch;
	[SerializeField] private GameObject particles;
	[SerializeField] private GameObject HolderGameOver;

	[SerializeField] public static List<GameObject> enemy;
	
	[SerializeField] private GameObject darkFilter;

	[SerializeField] private Color accent;
	[SerializeField] private Color grey;

	[SerializeField] private Text pointsText;
	[SerializeField] private Text matchpoints;
	[SerializeField] private Image gift;

	public PhysicalBackButton androidBack;
	
	public void CallGameOver(){
		
		AudioManager.AM.MusicGameOver();
		
		AudioManager.AM.GameOver();
		if (PlayerPrefs.GetInt("notVib")==0) AudioManager.AM.VibMedium();
			
		touch.SetActive(false);
		particles.SetActive(true);
		actualTime.gameObject.SetActive(true);

		StartCoroutine(darkFilter.GetComponent<Darkening>().FadeInSprite());

		StartCoroutine(GameOverFadeIn());

		PlayerPrefs.SetInt("Points",PlayerPrefs.GetInt("Points") + MatchBehaviour.Points);

		int MAX = 20;

		int aux = PlayerPrefs.GetInt("Points");

		
		while (aux>20) {
			PlayerPrefs.SetInt("Points",PlayerPrefs.GetInt("Points")-20);
			aux = PlayerPrefs.GetInt("Points");
			PlayerPrefs.SetInt("Level",PlayerPrefs.GetInt("Level")+1);
		}

		pointsText.text = aux.ToString() + "/20";
		gift.fillAmount = (float)PlayerPrefs.GetInt("Points")/(float)MAX; 

        matchpoints.text = MatchBehaviour.Points.ToString();

		clockString.text = "";
		StartCoroutine(scoreLerp());
		
		foreach (GameObject enm in enemy){
			enm.GetComponent<Rigidbody2D>().Sleep();
		}
			
		BestScore();

		PlayerPrefs.SetFloat("TimePlayed",PlayerPrefs.GetFloat("TimePlayed") + MatchBehaviour.Clock);
		PlayerPrefs.SetInt("MatchesPlayed",PlayerPrefs.GetInt("MatchesPlayed") + 1);

		HolderGameOver.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
 
     public IEnumerator scoreLerp () {
		 
		 float lerp = 0f, duration = 1f, score = 0;

         while (score<MatchBehaviour.Clock){
			lerp += Time.deltaTime / duration;
			score = Mathf.Lerp (score, MatchBehaviour.Clock, lerp);
			actualTime.text = score.ToString("F2",CultureInfo.InvariantCulture);
			yield return null;
		}
		actualTime.text = MatchBehaviour.Clock.ToString("F2", CultureInfo.InvariantCulture);
     }

	 private IEnumerator GameOverFadeIn() {
		
		float decelerate = 0f;
		while(HolderGameOver.GetComponent<CanvasGroup>().alpha!=1f){
			decelerate += Time.deltaTime;
			HolderGameOver.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(HolderGameOver.GetComponent<CanvasGroup>().alpha, 1f, decelerate);

			if (HolderGameOver.GetComponent<CanvasGroup>().alpha > 0.95f) {
				HolderGameOver.GetComponent<CanvasGroup>().alpha = 1;
			}
			yield return null;
		}
	}

	private void Awake() {
		GameManager.GM.gameStateObj = this.gameObject;
		enemy = new List<GameObject>();
		HolderGameOver.GetComponent<CanvasGroup>().alpha = 0f;
	}

	private void BestScore(){

		if (GameManager.GM.GameMode == GameMode.Normal) {
			
			bestTime.text = PlayerPrefs.GetFloat("BestScoreNormal").ToString("F2", CultureInfo.InvariantCulture);
			if (MatchBehaviour.Clock>PlayerPrefs.GetFloat("BestScoreNormal")){
				PlayerPrefs.SetFloat("BestScoreNormal",MatchBehaviour.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}

		else if (GameManager.GM.GameMode == GameMode.Duo) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScoreDuo").ToString("F2", CultureInfo.InvariantCulture);
			if (MatchBehaviour.Clock>PlayerPrefs.GetFloat("BestScoreDuo")){
				PlayerPrefs.SetFloat("BestScoreDuo",MatchBehaviour.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
		
		else if (GameManager.GM.GameMode == GameMode.Slow) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScoreSlow").ToString("F2", CultureInfo.InvariantCulture);
			if (MatchBehaviour.Clock>PlayerPrefs.GetFloat("BestScoreSlow")){
				PlayerPrefs.SetFloat("BestScoreSlow",MatchBehaviour.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
		
		else if (GameManager.GM.GameMode == GameMode.NoWalls) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScoreNoWalls").ToString("F2", CultureInfo.InvariantCulture);
			if (MatchBehaviour.Clock>PlayerPrefs.GetFloat("BestScoreNoWalls")){
				PlayerPrefs.SetFloat("BestScoreNoWalls",MatchBehaviour.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
		
		else if (GameManager.GM.GameMode == GameMode.Survival) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScoreSurvival").ToString("F2", CultureInfo.InvariantCulture);
			if (MatchBehaviour.Clock>PlayerPrefs.GetFloat("BestScoreSurvival")){
				PlayerPrefs.SetFloat("BestScoreSurvival",MatchBehaviour.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
		
		else if (GameManager.GM.GameMode == GameMode.Ghost) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScoreGhost").ToString("F2", CultureInfo.InvariantCulture);
			if (MatchBehaviour.Clock>PlayerPrefs.GetFloat("BestScoreGhost")){
				PlayerPrefs.SetFloat("BestScoreGhost",MatchBehaviour.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
		
		else if (GameManager.GM.GameMode == GameMode.Pulse) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScorePulse").ToString("F2", CultureInfo.InvariantCulture);
			if (MatchBehaviour.Clock>PlayerPrefs.GetFloat("BestScorePulse")){
				PlayerPrefs.SetFloat("BestScorePulse",MatchBehaviour.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
	}

}
