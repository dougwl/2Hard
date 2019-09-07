using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailsSize : MonoBehaviour {

	private MovimentBalls mb;
	private float finalTime;
	public float pp;
	private float rand;

	// Use this for initialization
	void Start () {
		mb = GetComponentInParent<MovimentBalls>();
		finalTime = GetComponent<TrailRenderer>().time;
		rand = Random.Range(0.1f,0.7f);
	}
	
	// Update is called once per frame
	void Update () {
		pp = 0.8f + Mathf.PingPong(Time.time/3, 1f * rand);
		if (GameManager.GM.gameMode == "No Walls"){
			if (mb.enableTrail)	GetComponent<TrailRenderer>().time = pp*finalTime*10/mb.speed;
			if (!mb.enableTrail) GetComponent<TrailRenderer>().Clear();
		}
		else GetComponent<TrailRenderer>().time = pp*finalTime*10/mb.speed;
	}
}
