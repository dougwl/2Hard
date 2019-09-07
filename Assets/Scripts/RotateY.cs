using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateY : MonoBehaviour {

	// Update is called once per frame

	Quaternion rot;
	public float smooth = 2.0F;
    public float tiltAngle = 360.0F;

	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime * 150);

	}
}
