using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchBehaviour : MonoBehaviour {

	
	public static int Points;

	[SerializeField] private CanvasGroup StartPage;
	[SerializeField] private CanvasGroup FadeStartGame;
	
	[SerializeField] private GameObject BackButton;
	[SerializeField] private GameObject Point;
    [SerializeField] private Text PointText;
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
	
	private void Start() => Points = 0;
    
	//Set the match started
	private void OnMouseDown() {	
		if (!GM.Playing){
			AudioManager.AM.MusicPlaying();
			GameModePage.enabled = false;
			GameModeTopper.enabled = false;
			Enemies.SetActive(true);
			BackButton.SetActive(false);
            FadeStartGame.InterpolateCanvasAlpha(0.5f, 1, 0, CurveName.Linear);
			GM.Play();
			StartPage.alpha = 0;
			StartPage.blocksRaycasts = false;
			SpawnPoint();
		}
	} 

	public void SpawnPoint(){
        GameObject temp = Instantiate(Point, Vector3.zero, Quaternion.identity, Board);
		temp.GetComponent<Point>().MatchBehaviour = this;
	}

	public void UpdatePoints()
    {
		Points++;
        PointText.text = Points.ToString();
    }

}
