using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSize : MonoBehaviour {

	private float rightLimit, topLimit;

	private void OnEnable() {
		
		rightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1,0,0)).x;
		topLimit = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).y;
		
		this.transform.localScale  = new Vector3(2*rightLimit, 2*topLimit);
	}
}
