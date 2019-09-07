using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchColliderXTransform : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<BoxCollider2D>().size = new Vector2(Screen.width*1920/Screen.height, 1920);
	}
}
