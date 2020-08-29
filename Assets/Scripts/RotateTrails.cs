using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrails : MonoBehaviour {

	private Rigidbody2D rb;
	// Use this for initialization

	void Awake(){
		rb = GetComponentInParent<Rigidbody2D>();
		GetComponentInParent<EnemyMovement>().OnDirectionChange += UpdateRotation;
	}

	private void UpdateRotation(){
		Debug.Log("Updating trail rotation");
		transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(rb.velocity.normalized.y,rb.velocity.normalized.x)*180/Mathf.PI - 90);
	}
	// Update is called once per frame
}
