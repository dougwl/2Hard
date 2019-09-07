using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour {

	private void OnEnable() {
		this.GetComponent<RectTransform>().sizeDelta = new Vector2 (Screen.width*1920/Screen.height, 1920);
	}
}
