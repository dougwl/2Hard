using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public Rigidbody2D rb;

	public float speed = 2.5f;
	[SerializeField] private static float maxspeed = 10f;
	public float CollisionForce = 25;
	public float directionRange = 1; //range when enemy hit a wall
	public float accel = 3f;

	private float leftLimit;
	private float rightLimit;
	private float topLimit;
	private float bottomLimit;

	private TrailRenderer[] tr;

	public bool enableTrail = true;

	public bool start = false; 

	public bool enable = true;

	public float ballLimit;
	public float ballOrigSize;

    private GameManager GM;

    void Awake() {
		GM = GameManager.GM;

		//Getting the screen limits
		leftLimit = -(Screen.width*1920/Screen.height)/2; 
		rightLimit = -leftLimit;
		topLimit = 960;
		bottomLimit = -960;
		start = false;
        rb = GetComponent<Rigidbody2D>();
		tr = this.GetComponentsInChildren<TrailRenderer>();
		ballOrigSize = this.GetComponent<RectTransform>().rect.height;
	}

	void Start(){
		if (GM == null) GM = GameManager.GM;
		if (GM.GameState == GameState.InGame) GameOver.enemy.Add(this.gameObject);
		Move();
	}

	public void Move(){
		if (start != true && GM.GameState == GameState.MainMenu)
        {
			InitialMove();
			start = true;
			StartCoroutine(BallsControl());
		}
		else if(GM.GameState == GameState.InGame && GM.Playing){
			InitialMove();
			start = true;
			StartCoroutine(BallsControl());
		}	
	}

	void InitialMove(){
		//Set a random angle to the ball to start moving
		rb.AddRelativeForce (new Vector2 (Random.Range(-360,360), Random.Range(-360,360)));
	}
	
    private IEnumerator BallsControl()
    {
		while (GM.GameState == GameState.MainMenu || (GM.GameState != GameState.GameOver))
		{
			//set the ball movement and acceleration
			rb.velocity = rb.velocity.normalized * speed;
			if (speed < maxspeed){
				speed += (accel / 1000);				
			}
			
			ballLimit = this.GetComponent<RectTransform>().rect.height/2f;

			//prevent the enemy ball to get off screen (left)
			if (this.transform.localPosition.x<leftLimit + ballLimit && GM.GameMode != GameMode.NoWalls){
				rb.velocity = new Vector2(rb.velocity.x*-1, rb.velocity.y);
				this.transform.localPosition = new Vector3(leftLimit + ballLimit,this.transform.localPosition.y,-1);
				Vector2 dir = -(new Vector2(transform.localPosition.x+(Random.Range(-directionRange,directionRange)),transform.localPosition.y).normalized);
			}
			
			//prevent the enemy ball to get off screen (right)
			if (this.transform.localPosition.x>rightLimit - ballLimit && GM.GameMode != GameMode.NoWalls) {
				rb.velocity = new Vector2(rb.velocity.x*-1, rb.velocity.y);
				this.transform.localPosition = new Vector3(rightLimit - ballLimit,this.transform.localPosition.y,-1);
				Vector2 dir = -(new Vector2(transform.localPosition.x+(Random.Range(-directionRange,directionRange)),transform.localPosition.y).normalized);
			}
			
			//prevent the enemy ball to get off screen (bottom)
			if (this.transform.localPosition.y<bottomLimit + ballLimit && GM.GameMode != GameMode.NoWalls) {
				rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*-1);
				this.transform.localPosition = new Vector3(this.transform.localPosition.x,bottomLimit + ballLimit,-1);
				Vector2 dir = -(new Vector2(transform.localPosition.x,transform.localPosition.y+(Random.Range(-directionRange,directionRange))).normalized);
			}
			
			//prevent the enemy ball to get off screen (top)
			if (this.transform.localPosition.y>topLimit - ballLimit && GM.GameMode != GameMode.NoWalls) {
				rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*-1);
				this.transform.localPosition = new Vector3(this.transform.localPosition.x,topLimit - ballLimit,-1);
				Vector2 dir = -(new Vector2(transform.localPosition.x,transform.localPosition.y+(Random.Range(-directionRange,directionRange))).normalized);
			}

			//WRAPING
			if (GM.GameMode == GameMode.NoWalls)
			{
				if (this.transform.localPosition.x + ballLimit < leftLimit){
					enableTrail = false;
					ParticleSystem.EmissionModule a = GetComponentInChildren<ParticleSystem>().emission;
					a.enabled = false;
					this.transform.localPosition = new Vector3(rightLimit + ballLimit - 0.01f ,this.transform.localPosition.y,-1);					
				}
			
				//prevent the enemy ball to get off screen (right)
				else if (this.transform.localPosition.x - ballLimit > rightLimit) {
					enableTrail = false;
					ParticleSystem.EmissionModule a = GetComponentInChildren<ParticleSystem>().emission;
					a.enabled = false;
					this.transform.localPosition = new Vector3(leftLimit - ballLimit + 0.01f,this.transform.localPosition.y,-1);
				}
				
				//prevent the enemy ball to get off screen (bottom)
				else if (this.transform.localPosition.y + ballLimit < bottomLimit) {
					enableTrail = false;
					ParticleSystem.EmissionModule a = GetComponentInChildren<ParticleSystem>().emission;
					a.enabled = false;
					this.transform.localPosition = new Vector3(this.transform.localPosition.x,topLimit + ballLimit - 0.01f ,-1);
				}
				
				//prevent the enemy ball to get off screen (top)
				else if (this.transform.localPosition.y - ballLimit > topLimit) {
					enableTrail = false;
					ParticleSystem.EmissionModule a = GetComponentInChildren<ParticleSystem>().emission;
					a.enabled = false;
					this.transform.localPosition = new Vector3(this.transform.localPosition.x,bottomLimit - ballLimit + 0.01f ,-1);
				}
				else if (this.transform.localPosition.x > leftLimit && this.transform.localPosition.x < rightLimit && this.transform.localPosition.y > bottomLimit && this.transform.localPosition.y < topLimit){
					enableTrail = true;
					ParticleSystem.EmissionModule a = GetComponentInChildren<ParticleSystem>().emission;
					a.enabled = true;
					
				}
			}
			//Warp the enemy ball from left to right
			
			yield return new WaitForFixedUpdate();
		}
    }

	void OnCollisionEnter2D(Collision2D coll) {

		//Apply a relative force in the opposite direction when enemies collide to prevent movement with repetitive pattern
		if(coll.gameObject.tag == "Enemy"){
			Vector2 dir = coll.contacts[0].point - (new Vector2(transform.localPosition.x,transform.localPosition.y));
			dir = -dir.normalized;
			GetComponent<Rigidbody2D>().AddRelativeForce(dir*CollisionForce);
		}

	}
}
