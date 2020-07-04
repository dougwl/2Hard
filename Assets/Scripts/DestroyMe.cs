using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour {

	void Update () {
		if (GameManager.GM.GameMode != GameMode.Survival) Destroy(this.gameObject);
	}

}
