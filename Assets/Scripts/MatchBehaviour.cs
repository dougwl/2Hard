using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class MatchBehaviour : MonoBehaviour {

	public static float Clock;
	public static int Points;

	[SerializeField] private Text ClockString;
	[SerializeField] private Text PointText;
	
	[SerializeField] private CanvasGroup StartPage;
	[SerializeField] private CanvasGroup FadeStartGame;
	
	[SerializeField] private GameObject BackButton;
	[SerializeField] private GameObject Point;
	[SerializeField] private Transform Board;

    [SerializeField] private GameObject Enemies;
	[SerializeField] private BoxCollider2D GameModePage;
	[SerializeField] private BoxCollider2D GameModeTopper;

	public RectTransform Player;
	private GameManager GM;

	
	private void Awake() {
        GM = GameManager.GM;
		FadeStartGame.alpha = 1f;
	}
	
	private void Start() {
     
		Clock = 0.0f;
		Points = 0;
	}
    
	//Set the match started
	private void OnMouseDown() {
		
		if (!GameManager.GM.Playing){
			AudioManager.AM.MusicPlaying();
			GameModePage.enabled = false;
			GameModeTopper.enabled = false;
			Enemies.SetActive(true);
			BackButton.SetActive(false);
            FadeStartGame.InterpolateCanvasAlpha(0.5f, 1, 0, CurveName.Linear);
			GameManager.GM.Play();
			StartPage.alpha = 0;
			StartPage.blocksRaycasts = false;
			SpawnPoint();
		}
	} 

	
	void Update () {        
		//Check and Set the Clock
		if (GM.Playing)
        {
			Clock += Time.deltaTime;
			ClockString.text = Clock.ToString("F1", CultureInfo.InvariantCulture);
			if (Points != 0) PointText.text = Points.ToString();
		}
	}

	public void SpawnPoint(){
        GameObject temp = Instantiate(Point, Vector3.zero, Quaternion.identity, Board);
		temp.GetComponent<Point>().MatchBehaviour = this;
	}

	

}
