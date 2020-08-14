using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : MonoBehaviour {

	private int MaxClones;
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

	private void Start() {

		if (GM.GameMode != GameMode.Survival) {
			gameObject.SetActive(false);
		}
		
		InvokeRepeating("CreateClone", Timer, MaxClones);
	}

	private void CreateClone(){
		ActiveClones++;
		GameObject clone = Enemies[ActiveClones];
		clone.SetActive(true);
		clone.transform.position = transform.position;
		clone.GetComponent<EnemyMovement>().Speed = EnemyMovement.Speed;
		clone.GetComponent<EnemyMovement>().Acceleration = EnemyMovement.Acceleration;
	}
}