using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Enemy"){
			GameManager.GM.GameOver();
		}
	}
}
