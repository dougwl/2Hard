using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebButtons : MonoBehaviour {

	public void TwitterButton(){
		Application.OpenURL("https://twitter.com/evermadco");
	}

	public void InstagramButton(){
		Application.OpenURL("https://www.instagram.com/evermadco");
	}

	public void FacebookButton(){
		Application.OpenURL("https://www.facebook.com/evermadco");
	}
}
