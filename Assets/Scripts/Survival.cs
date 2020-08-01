using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : MonoBehaviour {

	[SerializeField] private GameObject Enemy;
	[SerializeField] private Transform Board;

	private int aux = 0;
	private float clock = 0f;
	private GameManager GM;

    private void Awake()
    {
		GM = GameManager.GM;
	}
	private void Start() {
		if (GM.GameMode != GameMode.Survival) {
			gameObject.SetActive(false);
		}
		else {
			if (GM.GameState == GameState.MainMenu) StartCoroutine(Timer());
		}
	}
	
	private void Update() {
		if (GM.GameState == GameState.InGame) clock = Stopwatch.Clock;
		if (aux < (int)(clock/10) && aux <6){
			aux++;
			GameObject temp = Instantiate(Enemy,transform.position,transform.rotation,Board);
			temp.GetComponent<EnemyMovement>().speed = gameObject.GetComponent<EnemyMovement>().speed;
			temp.GetComponent<EnemyMovement>().accel = gameObject.GetComponent<EnemyMovement>().accel;
			if (GM.GameState == GameState.InGame) GameOver.enemy.Add(temp);
		}
	}

	public IEnumerator Timer(){
		while (GM.GameMode == GameMode.Survival) 
		{
			clock += Time.deltaTime;
			yield return null;
		}
		clock = 0f;
		aux = 0;
	}
}