using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrails : MonoBehaviour {

	private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		rb = GetComponentInParent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(rb.velocity.normalized.y,rb.velocity.normalized.x)*180/Mathf.PI - 90);
	}
}
