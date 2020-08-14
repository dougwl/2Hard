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
	[SerializeField] private CanvasGroup HolderGameOver;

	[SerializeField] public static List<GameObject> enemy;
	
	[SerializeField] private CanvasGroup DarkFilter;

	[SerializeField] private Color accent;
	[SerializeField] private Color grey;

	[SerializeField] private Text pointsText;
	[SerializeField] private Text matchpoints;
	[SerializeField] private Image gift;
	

	public BackButton androidBack;
	
	public void CallGameOver(){
		
		AudioManager.AM.MusicGameOver();
		
		AudioManager.AM.GameOver();
		if (PlayerPrefs.GetInt("notVib")==0) AudioManager.AM.VibMedium();
			
		touch.SetActive(false);
		particles.SetActive(true);
		actualTime.gameObject.SetActive(true);

		//StartCoroutine(GameOverFadeIn(DarkFilter));
		//StartCoroutine(GameOverFadeIn(HolderGameOver));

		StartCoroutine(FadeInSprite(DarkFilter));
		StartCoroutine(FadeInSprite(HolderGameOver));

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
		
		Sleep();
			
		BestScore();

		PlayerPrefs.SetFloat("TimePlayed",PlayerPrefs.GetFloat("TimePlayed") + Stopwatch.Clock);
		PlayerPrefs.SetInt("MatchesPlayed",PlayerPrefs.GetInt("MatchesPlayed") + 1);

		HolderGameOver.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	public IEnumerator FadeInSprite(CanvasGroup filter)
	{
		while (filter.alpha != 1f)
		{
			float decelerate = Mathf.Min(7.5f * Time.deltaTime, 1f);

			filter.alpha = Mathf.Lerp(filter.alpha, 1f, decelerate);

			if (filter.alpha > 0.999f)
			{
				filter.alpha = 1f;
			}
			yield return null;
		}
	}

	public IEnumerator scoreLerp () {
		 
		 float lerp = 0f, duration = 1f, score = 0;

         while (score < Stopwatch.Clock){
			lerp += Time.deltaTime / duration;
			score = Mathf.Lerp (score, Stopwatch.Clock, lerp);
			actualTime.text = score.ToString("F2",CultureInfo.InvariantCulture);
			yield return null;
		}
		actualTime.text = Stopwatch.Clock.ToString("F2", CultureInfo.InvariantCulture);
     }

	// private IEnumerator GameOverFadeIn(CanvasGroup filter) {
		
	//	float decelerate = 0f;
	//	while(filter.alpha!=1f){
	//		decelerate += Time.deltaTime;
	//		filter.alpha = Mathf.Lerp(filter.alpha, 1f, decelerate);

	//		if (filter.alpha > 0.95f) {
	//			filter.alpha = 1;
	//		}
	//		yield return null;
	//	}
	//}

	private void Awake() {
		GameManager.GM.gameStateObj = this.gameObject;
		enemy = new List<GameObject>();
		HolderGameOver.GetComponent<CanvasGroup>().alpha = 0f;
	}

	private void BestScore(){

		if (GameManager.GM.GameMode == GameMode.Normal) {
			
			bestTime.text = PlayerPrefs.GetFloat("BestScoreNormal").ToString("F2", CultureInfo.InvariantCulture);
			if (Stopwatch.Clock>PlayerPrefs.GetFloat("BestScoreNormal")){
				PlayerPrefs.SetFloat("BestScoreNormal",Stopwatch.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}

		else if (GameManager.GM.GameMode == GameMode.Duo) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScoreDuo").ToString("F2", CultureInfo.InvariantCulture);
			if (Stopwatch.Clock > PlayerPrefs.GetFloat("BestScoreDuo")){
				PlayerPrefs.SetFloat("BestScoreDuo", Stopwatch.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
		
		else if (GameManager.GM.GameMode == GameMode.Slow) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScoreSlow").ToString("F2", CultureInfo.InvariantCulture);
			if (Stopwatch.Clock > PlayerPrefs.GetFloat("BestScoreSlow")){
				PlayerPrefs.SetFloat("BestScoreSlow", Stopwatch.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
		
		else if (GameManager.GM.GameMode == GameMode.NoWalls) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScoreNoWalls").ToString("F2", CultureInfo.InvariantCulture);
			if (Stopwatch.Clock > PlayerPrefs.GetFloat("BestScoreNoWalls")){
				PlayerPrefs.SetFloat("BestScoreNoWalls", Stopwatch.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
		
		else if (GameManager.GM.GameMode == GameMode.Survival) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScoreSurvival").ToString("F2", CultureInfo.InvariantCulture);
			if (Stopwatch.Clock > PlayerPrefs.GetFloat("BestScoreSurvival")){
				PlayerPrefs.SetFloat("BestScoreSurvival", Stopwatch.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
		
		else if (GameManager.GM.GameMode == GameMode.Ghost) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScoreGhost").ToString("F2", CultureInfo.InvariantCulture);
			if (Stopwatch.Clock > PlayerPrefs.GetFloat("BestScoreGhost")){
				PlayerPrefs.SetFloat("BestScoreGhost", Stopwatch.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
		
		else if (GameManager.GM.GameMode == GameMode.Pulse) {
		
			bestTime.text = PlayerPrefs.GetFloat("BestScorePulse").ToString("F2", CultureInfo.InvariantCulture);
			if (Stopwatch.Clock > PlayerPrefs.GetFloat("BestScorePulse")){
				PlayerPrefs.SetFloat("BestScorePulse", Stopwatch.Clock);
				actualTime.color = accent;
				bestTime.color = grey;
				best.SetActive(true);
			}
		
		}
	}

}
