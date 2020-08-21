using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : MonoBehaviour {

	private int MaxClones = 6;
	public List<GameObject> Enemies;

	private int ActiveClones = 0;
	private float Timer = 10f;
	private GameManager GM;

	private EnemyMovement EnemyMovement;

    private void Awake()
    {
		GM = GameManager.GM;
		EnemyMovement = GetComponent<EnemyMovement>();
	}

	private void OnEnable(){

		if (GM.GameMode != GameMode.Survival) {
			gameObject.SetActive(false);
		}

		ActiveClones = 0;
		InvokeRepeating("CreateClone", 2f, 3f);
	}

	private void CreateClone(){
		GameObject clone = Enemies[ActiveClones];
		clone.SetActive(true);
		clone.transform.position = transform.position;
		clone.GetComponent<EnemyMovement>().Speed = EnemyMovement.Speed;
		clone.GetComponent<EnemyMovement>().Acceleration = EnemyMovement.Acceleration;
		clone.GetComponent<EnemyMovement>().MovementStarted = true;
		clone.name = "SurvivalClone_" + ActiveClones;
		if(ActiveClones == MaxClones) CancelInvoke("CreateClone");
		ActiveClones++;
	}

	
	private void OnDisable(){
		CancelInvoke("CreateClone");
	}


}