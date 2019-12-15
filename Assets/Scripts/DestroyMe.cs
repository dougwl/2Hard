using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour {

	void Update () {
		if (GameManager.GM.gameMode != GameMode.Survival) Destroy(this.gameObject);
	}

}
