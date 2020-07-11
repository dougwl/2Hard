using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour {

	public static float clock;
	public static int points;

	[SerializeField] private Text clockString;
	[SerializeField] private Text pointText;
	
	[SerializeField] private CanvasGroup startPage;
	[SerializeField] private CanvasGroup FadeStartGame;
	
	[SerializeField] private GameObject backButton;
	[SerializeField] private GameObject point;
	[SerializeField] private Transform board;
    [SerializeField] private RectTransform player;


    [SerializeField] private GameObject enemies;
	[SerializeField] private BoxCollider2D mode1;
	[SerializeField] private BoxCollider2D mode2;

	public backButton androidBack;

	public int auxPoint = 0;

	public float minimum = 0.0f;
	public float speed = 100f;
	public float threshold = float.Epsilon;

	public bool start = false;

	private float widthLimit, heightLimit;

	private void Awake() {
		FadeStartGame.alpha = 1f;
	}
	
	private void Start() {
		widthLimit = (Screen.width*1920/Screen.height)/2 - 100; 
		heightLimit = 860;
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
            start = true;
			GameManager.GM.Play();
			startPage.alpha = 0;
			startPage.blocksRaycasts = false;
			SpawnPoint();
			//GameManager.GM.playing = true;
		}
	} 

	
	void Update () {        
		//Check and Set the Clock
		if (start){
			clock += Time.deltaTime;
			clockString.text = clock.ToString("F1", CultureInfo.InvariantCulture);
			if (points != 0) pointText.text = points.ToString();
		}
	}

	public void SpawnPoint(){
        GameObject temp = Instantiate(point, Vector3.zero, Quaternion.identity, board);
		MoveAround(temp.transform);
		temp.GetComponent<Point>().cl = this;
	}

	public void MoveAround(Transform obj){
		float xPos = Random.Range(-widthLimit,widthLimit);
		float yPos = Random.Range(-heightLimit,heightLimit);
        int offset = 2;
        while ((xPos < player.localPosition.x + offset * player.sizeDelta.x && xPos > player.localPosition.x - offset * player.sizeDelta.x) && 
               (yPos < player.localPosition.y + offset * player.sizeDelta.y && yPos > player.localPosition.y - offset * player.sizeDelta.y))
        {
            xPos = Random.Range(-widthLimit, widthLimit);
            yPos = Random.Range(-heightLimit, heightLimit);
        }
		obj.localPosition = new Vector3 (xPos,yPos);
	}

}
