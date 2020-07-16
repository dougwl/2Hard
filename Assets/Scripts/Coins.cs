using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Coins : MonoBehaviour {

	[SerializeField] private Text CoinsText;

	// Use this for initialization
	void Start () {
		CoinsText.text = PlayerPrefs.GetInt("Coins").ToString();
	}
	
}
