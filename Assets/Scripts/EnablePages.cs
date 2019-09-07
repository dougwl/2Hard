using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePages : MonoBehaviour {

	[SerializeField] private List <GameObject> pages;
	void Start () {
		for (int i = 0; i < pages.Count; i++){
			pages[i].SetActive(true);
		}
	}
}
