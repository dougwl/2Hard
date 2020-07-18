using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class MatchBehaviour : MonoBehaviour {

	public static float clock;
	public static int points;

	[SerializeField] private Text clockString;
	[SerializeField] private Text pointText;
	
	[SerializeField] private CanvasGroup startPage;
	[SerializeField] private CanvasGroup FadeStartGame;
	
	[SerializeField] private GameObject backButton;
	[SerializeField] private GameObject point;
	[SerializeField] private Transform board;
    public RectTransform player;

    [SerializeField] private GameObject enemies;
	[SerializeField] private BoxCollider2D mode1;
	[SerializeField] private BoxCollider2D mode2;

	private GameManager GM;

	
	private void Awake() {
        GM = GameManager.GM;
		FadeStartGame.alpha = 1f;
	}
	
	private void Start() {
     
		clock = 0.0f;
		points = 0;
	}
    
	//Set the match started
	private void OnMouseDown() {
		
		if (!GameManager.GM.Playing){
			AudioManager.AM.MusicPlaying();
			mode1.enabled = false;
			mode2.enabled = false;
			enemies.SetActive(true);
			backButton.SetActive(false);
            FadeStartGame.InterpolateCanvasAlpha(0.5f, 1, 0, CurveName.Linear);
			GameManager.GM.Play();
			startPage.alpha = 0;
			startPage.blocksRaycasts = false;
			SpawnPoint();
		}
	} 

	
	void Update () {        
		//Check and Set the Clock
		if (GM.Playing)
        {
			clock += Time.deltaTime;
			clockString.text = clock.ToString("F1", CultureInfo.InvariantCulture);
			if (points != 0) pointText.text = points.ToString();
		}
	}

	public void SpawnPoint(){
        GameObject temp = Instantiate(point, Vector3.zero, Quaternion.identity, board);
		temp.GetComponent<Point>().MatchBehaviour = this;
	}

	

}
