using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizeWidth : MonoBehaviour {

	private void Start() {
		this.GetComponent<RectTransform>().sizeDelta = new Vector2 (Screen.width*1920/Screen.height, this.GetComponent<RectTransform>().sizeDelta.y);
	}
}
