using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour {

	public List<GameObject> enemies;
	private float origSize; 
	public Gradient normal;
	public Gradient ghost;

	private void Start(){
		origSize = enemies[1].GetComponent<RectTransform>().rect.width;
		GameManager.GM.enMan = this;
		ConfigMode();
	}

	private void ConfigMode(){
		if (GameManager.GM.gameMode == GameMode.Slow) SlowMode();
		else NormalSpeed();
		if (GameManager.GM.gameMode == GameMode.Ghost) SetGhost();	
		if (GameManager.GM.gameMode == GameMode.Duo) TwoBalls();	
		if (GameManager.GM.gameMode == GameMode.Survival) Survival();
		if (GameManager.GM.gameMode == GameMode.Pulse) Pulse();
		if (GameManager.GM.gameMode == GameMode.NoWalls) NoWalls();
	}

	public void startMovement(){

		foreach (GameObject ball in enemies)
		{
			if(ball.GetComponent<MovimentBalls>().isActiveAndEnabled){
				ball.GetComponent<MovimentBalls>().Move();
			}
		}
	}

	public void SetDefault(){
		foreach (GameObject obj in enemies)
		{
			Image sprite = obj.GetComponent<Image>();
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
			obj.GetComponent<CircleCollider2D>().isTrigger = false;
			obj.GetComponent<MovimentBalls>().directionRange = 1f;
			foreach (TrailRenderer tr in obj.GetComponentsInChildren<TrailRenderer>()){
				tr.colorGradient = normal;
			}
		}
	}

	public void SetGhost(){
		foreach (GameObject obj in enemies)
		{
			Image sprite = obj.GetComponent<Image>();
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
			obj.GetComponent<CircleCollider2D>().isTrigger = true;
			obj.GetComponent<MovimentBalls>().directionRange = 3f;
			foreach (TrailRenderer tr in obj.GetComponentsInChildren<TrailRenderer>()){
				tr.colorGradient = ghost;
			}
		}
	}

	public void SlowMode(){

		foreach (GameObject ball in enemies)
		{
			ball.GetComponent<MovimentBalls>().speed = 1.25f;
			ball.GetComponent<MovimentBalls>().accel = 1.5f;	
		}

	}
	
	public void NormalSpeed(){
		foreach (GameObject ball in enemies)
		{
			ball.GetComponent<MovimentBalls>().speed = 2.5f;
			ball.GetComponent<MovimentBalls>().accel = 3f;
		}
	}

	public void TwoBalls(){

		enemies[2].SetActive(false);
		enemies[3].SetActive(false);

		foreach (GameObject ball in enemies)
		{
			if(ball.activeSelf){
				ball.GetComponent<MovimentBalls>().start = false;
			}
			
		}
	}

	public void NoWalls(){
		foreach (GameObject ball in enemies)
		{
			ball.GetComponent<MovimentBalls>().enemyForce = 120f;
		}
	}

	public void ResetStart(){
		for (int i = 0; i < enemies.Count-1; i++)
		{
			enemies[i].SetActive(false);
			enemies[i].SetActive(true);
		}
		
		enemies[4].SetActive(false);

		foreach (GameObject obj in enemies)
		{
			obj.GetComponent<MovimentBalls>().start = false;
			obj.GetComponent<MovimentBalls>().enemyForce = 25f;
			obj.GetComponent<RectTransform>().sizeDelta = new Vector2 (origSize,origSize);
			obj.GetComponent<CircleCollider2D>().radius = origSize/2f;
			foreach(Transform tr in obj.transform){
				if (tr.tag == "shadow") {
					tr.localScale = new Vector2 (1,1);
					tr.localPosition = new Vector2 (6.6f,-6.6f);
				}
				if (tr.tag == "trails") {
					tr.localScale = new Vector2 (1,1);
				}
			}
		}

	}

	public void Survival(){
		for (int i = 0; i < enemies.Count-1; i++)
		{
			enemies[i].SetActive(false);
		}

		enemies[4].SetActive(true);
		SlowMode();
		if (GameManager.GM.menuScene) StartCoroutine(enemies[4].GetComponent<Survival>().Timer());
		}

	public void Pulse(){
		foreach (GameObject ball in enemies)
		{
			StartCoroutine(Pulsing(ball));
		}
	}

	private IEnumerator Pulsing(GameObject ball){
		
		float pp;
		float rand = Random.Range(0.1f,0.5f);
		Transform shadow = null;
		Transform trails = null;

		foreach(Transform tr in ball.transform){
			if (tr.tag == "shadow") shadow = tr;
			if (tr.tag == "trails") trails = tr;
		}
		
		while (GameManager.GM.gameMode == GameMode.Pulse){
			if (GameManager.GM.gameState != GameState.GameOver){
				pp = 0.5f + Mathf.PingPong(Time.time * rand, 1f);
				ball.GetComponent<RectTransform>().sizeDelta = new Vector2 (pp*origSize,pp*origSize);
				ball.GetComponent<CircleCollider2D>().radius = pp * origSize/2;
				shadow.localScale = new Vector2 (pp,pp);
				shadow.localPosition = new Vector2 (6.6f*pp,-6.6f*pp);
				trails.localScale = new Vector2 (pp,pp);
			}
			yield return null;
		}
	}
}
