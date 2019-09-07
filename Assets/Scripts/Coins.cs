using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Coins : MonoBehaviour {

	[SerializeField] private Text coins;

	// Use this for initialization
	void Start () {
		coins.text = PlayerPrefs.GetInt("Coins").ToString();
	}
	
}
