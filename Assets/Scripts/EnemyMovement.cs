using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EnemyMovement : MonoBehaviour {

	private Rigidbody2D Rigidbody;
	private RectTransform RectTransform;

	public float Speed = 2.5f;
	[SerializeField] private static float MaxSpeed = 10f;
	public float CollisionForce = 25;
	public float DirectionRange = 1; //range when enemy hit a wall
	public float Acceleration = 3f;
	public bool EnableTrail = true;
	public bool MovementStarted = false; 
	private float Radius;
    private GameManager GM;
	private ScreenLimit ScreenLimit;
	private ParticleSystem.EmissionModule Particle;



    void Awake() {
		GM = GameManager.GM;
		MovementStarted = false;
        Rigidbody = GetComponent<Rigidbody2D>();
		RectTransform = GetComponent<RectTransform>();
		Particle = GetComponentInChildren<ParticleSystem>().emission;
	}

	void OnEnable(){
		if (GM == null) GM = GameManager.GM;
	}

	void Start(){
		if (GM == null) GM = GameManager.GM;
		if (GM.GameState == GameState.InGame) GameOver.enemy.Add(this.gameObject);
		ScreenLimit = new ScreenLimit(GM.ScreenBorder);
		Move();
	}

	public void Move(){
		if 	(MovementStarted != true && GM.GameState == GameState.MainMenu ||
			(GM.GameState == GameState.InGame && GM.Playing) )
        {
			InitialMove();
			MovementStarted = true;
		}
		
	}

	void InitialMove(){
		//Set a random angle to the ball to start moving
		Rigidbody.AddRelativeForce (new Vector2 (Random.Range(-360,360), Random.Range(-360,360)));
	}

	public void UpdateRadius(){
		Radius = RectTransform.rect.height/2f;
	}
	void SetInsideLimits(float limit, bool isHorizontal = true){

		int signal = limit > 0 ? 1 : -1;
		
		Rigidbody.velocity = new Vector2(Rigidbody.velocity.x * (isHorizontal ? -1 : 1) , Rigidbody.velocity.y * (isHorizontal ? 1 : -1));
		this.transform.localPosition = new Vector2(	isHorizontal ? limit - signal * Radius : this.transform.localPosition.x,
													isHorizontal ? this.transform.localPosition.y : limit - signal * Radius);
	}

	void WarpIt(float limit, bool isHorizontal = true){ // Problematic Code

		int signal = limit > 0 ? 1 : -1;
	
		EnableTrail = false;
		Particle.enabled = false;

		this.transform.localPosition = new Vector2(	isHorizontal ? -(limit + signal * (Radius - 0.01f)) : this.transform.localPosition.x,
													isHorizontal ? this.transform.localPosition.y : -(limit + signal * (Radius - 0.01f)));

	}

	private void FixedUpdate(){
		
		if(MovementStarted && (GM.GameState == GameState.MainMenu || (GM.GameState != GameState.GameOver))){
			//set the ball movement and acceleration
			Rigidbody.velocity = Rigidbody.velocity.normalized * Speed;
			
			if (Speed < MaxSpeed) Speed += (Acceleration / 1000);
			
			if (GM.GameMode == GameMode.Pulse) Radius = RectTransform.rect.height/2f;

			if (GM.GameMode != GameMode.NoWalls){
			//prevent the enemy ball to get off screen (left)
				if (this.transform.localPosition.x < ScreenLimit.Left + Radius){
					SetInsideLimits(ScreenLimit.Left);
				}
				
				//prevent the enemy ball to get off screen (right)
				if (this.transform.localPosition.x > ScreenLimit.Right - Radius) {
					SetInsideLimits(ScreenLimit.Right);
				}
				
				//prevent the enemy ball to get off screen (bottom)
				if (this.transform.localPosition.y < ScreenLimit.Bottom + Radius) {
					SetInsideLimits(ScreenLimit.Bottom,isHorizontal:false);
				}
				
				//prevent the enemy ball to get off screen (top)
				if (this.transform.localPosition.y > ScreenLimit.Top - Radius) {
					SetInsideLimits(ScreenLimit.Top,isHorizontal:false);
				}

			}
			
			else // Problematic code 
			{
				if (this.transform.localPosition.x + Radius < ScreenLimit.Left){
					WarpIt(ScreenLimit.Left);					
				}
			
				else if (this.transform.localPosition.x - Radius > ScreenLimit.Right) {
					WarpIt(ScreenLimit.Right);
				}
				
				else if (this.transform.localPosition.y + Radius < ScreenLimit.Bottom) {
					WarpIt(ScreenLimit.Bottom, isHorizontal:false);
				}
				
				else if (this.transform.localPosition.y - Radius > ScreenLimit.Top) {
					WarpIt(ScreenLimit.Top, isHorizontal:false);
				}
				
				else if (!EnableTrail &&
						this.transform.localPosition.x > ScreenLimit.Left && 
						this.transform.localPosition.x < ScreenLimit.Right && 
						this.transform.localPosition.y > ScreenLimit.Bottom && 
						this.transform.localPosition.y < ScreenLimit.Top){
					EnableTrail = true;
					Particle.enabled = true;
				}
			} 
		}		
	}

	void OnCollisionEnter2D(Collision2D coll) {

		//Apply a relative force in the opposite direction when enemies collide to prevent movement with repetitive pattern
		if(coll.gameObject.tag == "Enemy"){
			Vector2 dir = coll.contacts[0].point - (new Vector2(transform.localPosition.x,transform.localPosition.y));
			dir = -dir.normalized;
			GetComponent<Rigidbody2D>().AddRelativeForce(dir * CollisionForce);
		}

	}
}
