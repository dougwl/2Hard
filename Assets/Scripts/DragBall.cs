using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragBall : MonoBehaviour {

	[SerializeField] private GameObject player;

	private Vector3 Orig;
	private Vector3 playerOrig;

	private float leftLimit;
	private float rightLimit;
	private float topLimit;
	private float bottomLimit;

	private float playerLimit = 0.45f;

	private bool isInsideLimit = true;

	private Touch currentTouch;

	private void Awake() {
		currentTouch = new Touch();
		Input.multiTouchEnabled = false;
		//Getting the screen limits
		leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).x + playerLimit; 
		rightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1,0,0)).x - playerLimit;
		topLimit = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).y - playerLimit;
		bottomLimit = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).y + playerLimit;

		GetComponent<BoxCollider2D>().size = new Vector2 (Screen.width*1920/Screen.height, 1920);
	}
	
	private void Start() {
		if (GameManager.GM.gameMode == "No Walls") playerLimit = 0f;
	}

	private void OnMouseDown() {
		#if UNITY_EDITOR
			//Getting the original positions of mouse and player
			Orig = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0));
			playerOrig =  player.transform.position;
		#endif
	}

	private void OnMouseUp() {
		#if UNITY_EDITOR
			isInsideLimit = true;
		#endif
	}

	private void OnMouseDrag() {
		//Called when dragging the mouse
		#if UNITY_EDITOR
			currentTouch.position = Input.mousePosition;
			OnDrag();
		#endif
	}

	private void OnTouchDown() {
		#if !UNITY_EDITOR
			//Getting the original positions of touch and player
			Orig = Camera.main.ScreenToWorldPoint(new Vector3(currentTouch.position.x ,currentTouch.position.y ,0));
			playerOrig =  player.transform.position;
		#endif
	}

	private void OnDrag() {
		
		if (GameManager.GM.playing){

			//Don't go beyond the left
			if (player.transform.position.x<=leftLimit){
				isInsideLimit = false;
				if (Orig.x < Camera.main.ScreenToWorldPoint(new Vector3(currentTouch.position.x,currentTouch.position.y,0)).x) {
					isInsideLimit = true;
				}
				else {
					player.transform.position = new Vector3(leftLimit, Camera.main.ScreenToWorldPoint(currentTouch.position).y - Orig.y + playerOrig.y ,0);
					Orig = Camera.main.ScreenToWorldPoint(new Vector3(currentTouch.position.x,currentTouch.position.y,0));
					playerOrig =  player.transform.position;
					isInsideLimit = true;
				}
			}

			//Don't go beyond the right
			if (player.transform.position.x>=rightLimit){
				isInsideLimit = false;
				if (Orig.x > Camera.main.ScreenToWorldPoint(new Vector3(currentTouch.position.x,currentTouch.position.y,0)).x) {
					isInsideLimit = true;
				}
				else {
					player.transform.position = new Vector3(rightLimit, Camera.main.ScreenToWorldPoint(currentTouch.position).y - Orig.y + playerOrig.y ,0);
					Orig = Camera.main.ScreenToWorldPoint(new Vector3(currentTouch.position.x,currentTouch.position.y,0));
					playerOrig =  player.transform.position;
					isInsideLimit = true;
				}
			}

			//Don't go beyond the top
			if (player.transform.position.y>=topLimit){
				isInsideLimit = false;
				if (Orig.y > Camera.main.ScreenToWorldPoint(new Vector3(currentTouch.position.x,currentTouch.position.y,0)).y) {
					isInsideLimit = true;
					isInsideLimit = true;
				}
				else {
					player.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(currentTouch.position).x - Orig.x + playerOrig.x,topLimit,0);
					Orig = Camera.main.ScreenToWorldPoint(new Vector3(currentTouch.position.x,currentTouch.position.y,0));
					playerOrig =  player.transform.position;
				}
			}

			//Don't go beyond the bottom
			if (player.transform.position.y<=bottomLimit){
				isInsideLimit = false;
				if (Orig.y < Camera.main.ScreenToWorldPoint(new Vector3(currentTouch.position.x,currentTouch.position.y,0)).y) {
					isInsideLimit = true;
				}
				else {
					player.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(currentTouch.position).x - Orig.x + playerOrig.x, bottomLimit ,0);
					Orig = Camera.main.ScreenToWorldPoint(new Vector3(currentTouch.position.x,currentTouch.position.y,0));
					playerOrig =  player.transform.position;
					isInsideLimit = true;
				}
			}

			//Movement inside the limits
			if (isInsideLimit) {
				player.transform.position = (Camera.main.ScreenToWorldPoint(new Vector3(currentTouch.position.x,currentTouch.position.y,0)) - Orig + playerOrig);
			}
		}	
	}

	private void Update() {

		//prevent the ball from getting out of the screen
		if (player.transform.position.x<leftLimit) player.transform.position = new Vector3(leftLimit,player.transform.position.y);
		if (player.transform.position.x>rightLimit) player.transform.position = new Vector3(rightLimit,player.transform.position.y);
		if (player.transform.position.y<bottomLimit) player.transform.position = new Vector3(player.transform.position.x,bottomLimit);
		if (player.transform.position.y>topLimit) player.transform.position = new Vector3(player.transform.position.x,topLimit);
		

		#if !UNITY_EDITOR
					
			currentTouch = Input.GetTouch(0);

			if (currentTouch.phase == TouchPhase.Began){
				OnTouchDown();
			}
		
			OnDrag();

		#endif
	}

}
