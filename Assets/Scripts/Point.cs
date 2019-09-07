using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {

	public Clock cl;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player"){
			AudioManager.AM.PlayPoint();
			Clock.points++;
			if (PlayerPrefs.GetInt("notVib")==0) AudioManager.AM.VibSuperLight();
			cl.MoveAround(this.transform);
		}
	}
}
