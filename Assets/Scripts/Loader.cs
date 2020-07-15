using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	[SerializeField] private GameObject GameManagerObject;        
        
    void Awake (){

		if (GameManager.GM == null) Instantiate(GameManagerObject);		
	
	}
}
