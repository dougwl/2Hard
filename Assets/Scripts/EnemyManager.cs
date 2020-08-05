using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour {

	[SerializeField] private List<GameObject> Enemies;
	private float OriginalSize; 
	[SerializeField] private Gradient DefaultTrailGradient;
	[SerializeField] private Gradient GhostTrailGradient;
	private List<EnemyMovement> EnemiesMovement;
	private List<CircleCollider2D> CircleColliders;
	private List<Image> Images;
	private List<List<TrailRenderer>> ChildrenTrailRenderers;
	private List<RectTransform> RectTransforms;
	private GameManager GM;

	private Survival SurvivalEnemy;

	private void Start(){
		EnemiesMovement = new List<EnemyMovement>();
		CircleColliders = new List<CircleCollider2D>();
		Images = new List<Image>();
		ChildrenTrailRenderers = new List<List<TrailRenderer>>();
		RectTransforms = new List<RectTransform>();
		
		foreach (GameObject enemy in Enemies)
		{
			EnemiesMovement.Add(enemy.GetComponent<EnemyMovement>());
			CircleColliders.Add(enemy.GetComponent<CircleCollider2D>());
			Images.Add(enemy.GetComponent<Image>());
			List<TrailRenderer> chTrails = new List<TrailRenderer>();
			foreach (TrailRenderer trail in enemy.GetComponentsInChildren<TrailRenderer>())
			{
				chTrails.Add(trail);
			}
			ChildrenTrailRenderers.Add(chTrails);
			RectTransforms.Add(enemy.GetComponent<RectTransform>());
		}

		SurvivalEnemy = Enemies[4].GetComponent<Survival>();
		OriginalSize = RectTransforms[0].rect.width;
		GM = GameManager.GM;
		GM.EnemyManager = this;
		SetupMode();
	}

	private void SetupMode(){
		if (GM.GameMode == GameMode.Slow) SlowSpeedConfig();
		else DefaultSpeedConfig();
		if (GM.GameMode == GameMode.Ghost) GhostConfig();	
		if (GM.GameMode == GameMode.Duo) TwoBallsConfig();	
		if (GM.GameMode == GameMode.Survival) SurvivalConfig();
		if (GM.GameMode == GameMode.Pulse) PulseConfig();
		if (GM.GameMode == GameMode.NoWalls) NoWallsConfig();
	}

	public void StartMovement(){
		foreach (EnemyMovement enemy in EnemiesMovement)
		{
			if(enemy.isActiveAndEnabled) enemy.Move();
		}
	}

	public void DefaultConfig(){
		for (int i = 0; i < Enemies.Count; i++)
		{
			Images[i].color = new Color(Images[i].color.r, Images[i].color.g, Images[i].color.b, 1f);
			CircleColliders[i].isTrigger = false;
			EnemiesMovement[i].DirectionRange = 1f;
			foreach (TrailRenderer trail in ChildrenTrailRenderers[i])
			{
				trail.colorGradient = DefaultTrailGradient;
			}
		}
	}

	public void GhostConfig(){
		for (int i = 0; i < Enemies.Count; i++)
		{
			Images[i].color = new Color(Images[i].color.r, Images[i].color.g, Images[i].color.b, 0.5f);
			CircleColliders[i].isTrigger = true;
			EnemiesMovement[i].DirectionRange = 3f;
			foreach (TrailRenderer trail in ChildrenTrailRenderers[i])
			{
				trail.colorGradient = GhostTrailGradient;
			}
		}
	}

	public void SlowSpeedConfig(){
		foreach (EnemyMovement enemy in EnemiesMovement)
		{
			enemy.Speed = 1.25f;
			enemy.Acceleration = 1.5f;
		}
	}
	
	public void DefaultSpeedConfig(){
		foreach (EnemyMovement enemy in EnemiesMovement)
		{
			enemy.Speed = 2.5f;
			enemy.Acceleration = 3f;
		}
	}

	public void TwoBallsConfig(){

		for (int i = 0; i < Enemies.Count; i++)
		{
			if(Enemies[i].activeSelf){
				EnemiesMovement[i].start = false;
			}
		}

		Enemies[2].SetActive(false);
		Enemies[3].SetActive(false);

	}

	public void NoWallsConfig(){
		foreach (EnemyMovement enemy in EnemiesMovement)
		{
			enemy.CollisionForce = 120f;
		}
	}

	public void ResetStart(){

		for (int i = 0; i < Enemies.Count-1; i++) //WTF IS THIS, search for a new way to reset speed without turning the shit off.
		{
			Enemies[i].SetActive(false);
			Enemies[i].SetActive(true);
		}
		
		Enemies[4].SetActive(false); // Need to be changed, adding the survival script to all enemies.

		for (int i = 0; i < Enemies.Count; i++)
		{
			EnemiesMovement[i].start = false;
			EnemiesMovement[i].CollisionForce = 25f;
			RectTransforms[i].sizeDelta = new Vector2 (OriginalSize,OriginalSize);
			CircleColliders[i].radius = OriginalSize/2;
			foreach (Transform enemyChild in Enemies[i].transform)
			{
				if(enemyChild.CompareTag("shadow")){
					enemyChild.localScale = new Vector2 (1,1);
					enemyChild.localPosition = new Vector2 (6.6f,-6.6f);
				}
				if (enemyChild.CompareTag("trails")) {
					enemyChild.localScale = new Vector2 (1,1);
				}
			}
		}

	}

	public void SurvivalConfig(){
		for (int i = 0; i < Enemies.Count-1; i++)
		{
			Enemies[i].SetActive(false);
		}

		Enemies[4].SetActive(true);
		SlowSpeedConfig();
		if (GM.GameState == GameState.MainMenu) StartCoroutine(SurvivalEnemy.Timer());
		}

	public void PulseConfig(){
		for (int i = 0; i < Enemies.Count; i++)
		{
			StartCoroutine(Pulsing(Enemies[i],RectTransforms[i],CircleColliders[i]));
		}
	}

	private IEnumerator Pulsing(GameObject enemy, RectTransform enemyRect, CircleCollider2D enemyCollider){
		
		float timeMod = Random.Range(0.1f,0.5f);
		float sizePercent;
		float minimumSizePercent = 0.5f;
		float shadowOffset = 6.6f;
		Transform shadow = null;
		Transform trails = null;
		float newSize;

		foreach(Transform tr in enemy.transform){
			if (tr.tag == "shadow") shadow = tr;
			if (tr.tag == "trails") trails = tr;
		}

		while (GameManager.GM.GameMode == GameMode.Pulse){
			if (GameManager.GM.GameState != GameState.GameOver){
				sizePercent = minimumSizePercent + Mathf.PingPong(Time.time * timeMod, 1f);
				newSize = sizePercent * OriginalSize;
				enemyRect.sizeDelta = new Vector2 (newSize, newSize);
				enemyCollider.radius = newSize/2;
				shadow.localScale = new Vector2 (sizePercent,sizePercent);
				shadow.localPosition = new Vector2 (shadowOffset * sizePercent, -shadowOffset * sizePercent);
				trails.localScale = new Vector2 (sizePercent,sizePercent);
			}
			yield return null;
		}
	}
}
