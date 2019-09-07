using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrailsPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(Input.GetAxis("Mouse Y"),Input.GetAxis("Mouse X"))*180/Mathf.PI - 90);
	}
}
