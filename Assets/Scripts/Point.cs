using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Point : MonoBehaviour {

	public MatchBehaviour MatchBehaviour; //Reference passed when point is spawned by MatchBehaviour.
	public GameManager GM;
    private float WidthLimit, HeightLimit;
	private RectTransform Player;


    private void Start()
    {
		GM = GameManager.GM;
		Player = MatchBehaviour.player;
		WidthLimit = GM.screenWidth / 2 - 100;
		HeightLimit = Screen.height / 2 - 100;
		MoveAround(this.transform);
    }

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player"){
			AudioManager.AM.PlayPoint();
			MatchBehaviour.points++;
			if (PlayerPrefs.GetInt("notVib")==0) AudioManager.AM.VibSuperLight();
			MoveAround(this.transform);
		}
	}

	public void MoveAround(Transform point)
	{
		float newXPosition = Random.Range(-WidthLimit, WidthLimit);
		float newYPosition = Random.Range(-HeightLimit, HeightLimit);
		int offset = 2;
		while ((newXPosition < Player.localPosition.x + offset * Player.sizeDelta.x && newXPosition > Player.localPosition.x - offset * Player.sizeDelta.x &&
			   (newYPosition < Player.localPosition.y + offset * Player.sizeDelta.y && newYPosition > Player.localPosition.y - offset * Player.sizeDelta.y)))
		{
			newXPosition = Random.Range(-WidthLimit, WidthLimit);
			newYPosition = Random.Range(-HeightLimit, HeightLimit);
		}
		point.localPosition = new Vector3(newXPosition, newYPosition);
	}
}
