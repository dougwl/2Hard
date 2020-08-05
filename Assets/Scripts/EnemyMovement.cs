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
	public bool start = false; 
	private float Radius;

    private GameManager GM;
	
	private ScreenLimit ScreenLimit;

    void Awake() {
		GM = GameManager.GM;
		ScreenLimit = new ScreenLimit(GM.ScreenBorder);
		start = false;
        Rigidbody = GetComponent<Rigidbody2D>();
		RectTransform = GetComponent<RectTransform>();
		GM.OnModeChange += UpdateRadius;
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
		Rigidbody.AddRelativeForce (new Vector2 (Random.Range(-360,360), Random.Range(-360,360)));
	}

	private void UpdateRadius(){
		Radius = RectTransform.rect.height/2f;
		
	}



	
    private IEnumerator BallsControl()
    {
		void SetInsideLimits(float limit, bool isHorizontal = true){

			if (this.transform.localPosition.x < ScreenLimit.Left + Radius){
				Rigidbody.velocity = new Vector2(Rigidbody.velocity.x * -1, Rigidbody.velocity.y);
				this.transform.localPosition = new Vector3(ScreenLimit.Left + Radius, this.transform.localPosition.y,-1);
				Vector2 dir = -(new Vector2(transform.localPosition.x + (Random.Range(-DirectionRange,DirectionRange)),transform.localPosition.y).normalized);
			}
			

			// Rigidbody.velocity = new Vector2(Rigidbody.velocity.x*-1, Rigidbody.velocity.y);
			// this.transform.localPosition = new Vector3(ScreenLimit.Right - Radius, this.transform.localPosition.y,-1);
			// Vector2 dir = -(new Vector2(transform.localPosition.x+(Random.Range(-DirectionRange,DirectionRange)),transform.localPosition.y).normalized);
			
			// Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Rigidbody.velocity.y*-1);
			// this.transform.localPosition = new Vector3(this.transform.localPosition.x,ScreenLimit.Bottom + Radius,-1);
			// Vector2 dir = -(new Vector2(transform.localPosition.x,transform.localPosition.y+(Random.Range(-DirectionRange,DirectionRange))).normalized);
			
			// Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Rigidbody.velocity.y*-1);
			// this.transform.localPosition = new Vector3(this.transform.localPosition.x,ScreenLimit.Top - Radius,-1);
			// Vector2 dir = -(new Vector2(transform.localPosition.x,transform.localPosition.y+(Random.Range(-DirectionRange,DirectionRange))).normalized);

		}

		




















		while (GM.GameState == GameState.MainMenu || (GM.GameState != GameState.GameOver))
		{
			//set the ball movement and acceleration
			Rigidbody.velocity = Rigidbody.velocity.normalized * Speed;
			
			if (Speed < MaxSpeed) Speed += (Acceleration / 1000);
			
			if (GM.GameMode == GameMode.Pulse) Radius = RectTransform.rect.height/2f;

			if (GM.GameMode != GameMode.NoWalls){
			//prevent the enemy ball to get off screen (left)
			if (this.transform.localPosition.x< ScreenLimit.Left + Radius){
				Rigidbody.velocity = new Vector2(Rigidbody.velocity.x*-1, Rigidbody.velocity.y);
				this.transform.localPosition = new Vector3(ScreenLimit.Left + Radius,this.transform.localPosition.y,-1);
				Vector2 dir = -(new Vector2(transform.localPosition.x+(Random.Range(-DirectionRange,DirectionRange)),transform.localPosition.y).normalized);
			}
			
			//prevent the enemy ball to get off screen (right)
			if (this.transform.localPosition.x>ScreenLimit.Right - Radius) {
				Rigidbody.velocity = new Vector2(Rigidbody.velocity.x*-1, Rigidbody.velocity.y);
				this.transform.localPosition = new Vector3(ScreenLimit.Right - Radius,this.transform.localPosition.y,-1);
				Vector2 dir = -(new Vector2(transform.localPosition.x+(Random.Range(-DirectionRange,DirectionRange)),transform.localPosition.y).normalized);
			}
			
			//prevent the enemy ball to get off screen (bottom)
			if (this.transform.localPosition.y<ScreenLimit.Bottom + Radius) {
				Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Rigidbody.velocity.y*-1);
				this.transform.localPosition = new Vector3(this.transform.localPosition.x,ScreenLimit.Bottom + Radius,-1);
				Vector2 dir = -(new Vector2(transform.localPosition.x,transform.localPosition.y+(Random.Range(-DirectionRange,DirectionRange))).normalized);
			}
			
			//prevent the enemy ball to get off screen (top)
			if (this.transform.localPosition.y>ScreenLimit.Top - Radius) {
				Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Rigidbody.velocity.y*-1);
				this.transform.localPosition = new Vector3(this.transform.localPosition.x,ScreenLimit.Top - Radius,-1);
				Vector2 dir = -(new Vector2(transform.localPosition.x,transform.localPosition.y+(Random.Range(-DirectionRange,DirectionRange))).normalized);
			}

			}
			//WRAPING
			/* else
			{
				if (this.transform.localPosition.x + Radius < leftLimit){
					EnableTrail = false;
					ParticleSystem.EmissionModule a = GetComponentInChildren<ParticleSystem>().emission;
					a.enabled = false;
					this.transform.localPosition = new Vector3(rightLimit + Radius - 0.01f ,this.transform.localPosition.y,-1);					
				}
			
				//prevent the enemy ball to get off screen (right)
				else if (this.transform.localPosition.x - Radius > rightLimit) {
					EnableTrail = false;
					ParticleSystem.EmissionModule a = GetComponentInChildren<ParticleSystem>().emission;
					a.enabled = false;
					this.transform.localPosition = new Vector3(leftLimit - Radius + 0.01f,this.transform.localPosition.y,-1);
				}
				
				//prevent the enemy ball to get off screen (bottom)
				else if (this.transform.localPosition.y + Radius < bottomLimit) {
					EnableTrail = false;
					ParticleSystem.EmissionModule a = GetComponentInChildren<ParticleSystem>().emission;
					a.enabled = false;
					this.transform.localPosition = new Vector3(this.transform.localPosition.x,topLimit + Radius - 0.01f ,-1);
				}
				
				//prevent the enemy ball to get off screen (top)
				else if (this.transform.localPosition.y - Radius > topLimit) {
					EnableTrail = false;
					ParticleSystem.EmissionModule a = GetComponentInChildren<ParticleSystem>().emission;
					a.enabled = false;
					this.transform.localPosition = new Vector3(this.transform.localPosition.x,bottomLimit - Radius + 0.01f ,-1);
				}
				else if (this.transform.localPosition.x > leftLimit && this.transform.localPosition.x < rightLimit && this.transform.localPosition.y > bottomLimit && this.transform.localPosition.y < topLimit){
					EnableTrail = true;
					ParticleSystem.EmissionModule a = GetComponentInChildren<ParticleSystem>().emission;
					a.enabled = true;
					
				}
			} */
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
