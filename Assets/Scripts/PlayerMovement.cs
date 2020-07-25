using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private GameObject Player;
	private RectTransform PlayerBody;

	private Vector2 Orig;
	private Vector2 PlayerOrig;

	private float LeftLimit;
	private float RightLimit;
	private float TopLimit;
	private float BottomLimit;

	private float PlayerLimit;

	private bool IsInsideLimit = true;

	private Touch CurrentTouch;
    
	private GameManager GM;
	private Canvas ParentCanvas;

	private Vector2 TouchPosition;

    private void Awake() {
		GM = GameManager.GM;
		CurrentTouch = new Touch();
		PlayerBody = Player.GetComponent<RectTransform>();
		Input.multiTouchEnabled = false;
		//Getting the screen limits
		PlayerLimit = PlayerBody.sizeDelta.x/2;
		LeftLimit = -GM.screenWidth/2 + PlayerLimit; 
		RightLimit = GM.screenWidth/2 - PlayerLimit;
		TopLimit = GM.screenHeight/2 - PlayerLimit;
		BottomLimit = -GM.screenHeight/2 + PlayerLimit;

		GetComponent<BoxCollider2D>().size = new Vector2 (GM.screenWidth, GM.screenHeight);
		ParentCanvas = GetComponentInParent<Canvas>();
		

		//Player.GetComponent<RectTransform>().position = new Vector2(0,0);
	}
	
	private void Start() {
		if (GameManager.GM.GameMode == GameMode.NoWalls) PlayerLimit = 0f;
	}

	private Vector2 WorldToCanvasPosition(Vector2 position){
		Vector2 convertedPosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
        	ParentCanvas.transform as RectTransform,
        	position, ParentCanvas.worldCamera,
        out convertedPosition);
		return convertedPosition;
	}

	private void OnTouchDown() {
		#if !UNITY_EDITOR
			//Getting the original positions of touch and player
			Orig = WorldToCanvasPosition(currentTouch.position);
			playerOrig = PlayerBody.anchoredPosition;
		#endif
	}

	private void OnMouseDown() {
		#if UNITY_EDITOR
			//Getting the original positions of mouse and player
			Orig = WorldToCanvasPosition(Input.mousePosition);
			PlayerOrig = PlayerBody.anchoredPosition;;
		#endif
	}

	private void OnMouseUp() {
		#if UNITY_EDITOR
			IsInsideLimit = true;
		#endif
	}

	private void OnMouseDrag() {
		//Called when dragging the mouse
		#if UNITY_EDITOR
			CurrentTouch.position = Input.mousePosition;
			OnDrag();
		#endif
	}
	
	private void OnDrag() {
		
		if (GameManager.GM.Playing){

			//Don't go beyond the left
			if (Player.transform.position.x<=LeftLimit){
				IsInsideLimit = false;
				if (Orig.x < Camera.main.ScreenToWorldPoint(new Vector3(CurrentTouch.position.x,CurrentTouch.position.y,0)).x) {
					IsInsideLimit = true;
				}
				else {
					Player.transform.position = new Vector3(LeftLimit, Camera.main.ScreenToWorldPoint(CurrentTouch.position).y - Orig.y + PlayerOrig.y ,0);
					Orig = Camera.main.ScreenToWorldPoint(new Vector3(CurrentTouch.position.x,CurrentTouch.position.y,0));
					PlayerOrig =  Player.transform.position;
					IsInsideLimit = true;
				}
			}

			//Don't go beyond the right
			if (Player.transform.position.x>=RightLimit){
				IsInsideLimit = false;
				if (Orig.x > Camera.main.ScreenToWorldPoint(new Vector3(CurrentTouch.position.x,CurrentTouch.position.y,0)).x) {
					IsInsideLimit = true;
				}
				else {
					Player.transform.position = new Vector3(RightLimit, Camera.main.ScreenToWorldPoint(CurrentTouch.position).y - Orig.y + PlayerOrig.y ,0);
					Orig = Camera.main.ScreenToWorldPoint(new Vector3(CurrentTouch.position.x,CurrentTouch.position.y,0));
					PlayerOrig =  Player.transform.position;
					IsInsideLimit = true;
				}
			}

			//Don't go beyond the top
			if (Player.transform.position.y>=TopLimit){
				IsInsideLimit = false;
				if (Orig.y > Camera.main.ScreenToWorldPoint(new Vector3(CurrentTouch.position.x,CurrentTouch.position.y,0)).y) {
					IsInsideLimit = true;
					IsInsideLimit = true;
				}
				else {
					Player.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(CurrentTouch.position).x - Orig.x + PlayerOrig.x,TopLimit,0);
					Orig = Camera.main.ScreenToWorldPoint(new Vector3(CurrentTouch.position.x,CurrentTouch.position.y,0));
					PlayerOrig =  Player.transform.position;
				}
			}

			//Don't go beyond the bottom
			if (Player.transform.position.y<=BottomLimit){
				IsInsideLimit = false;
				if (Orig.y < Camera.main.ScreenToWorldPoint(new Vector3(CurrentTouch.position.x,CurrentTouch.position.y,0)).y) {
					IsInsideLimit = true;
				}
				else {
					Player.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(CurrentTouch.position).x - Orig.x + PlayerOrig.x, BottomLimit ,0);
					Orig = Camera.main.ScreenToWorldPoint(new Vector3(CurrentTouch.position.x,CurrentTouch.position.y,0));
					PlayerOrig =  Player.transform.position;
					IsInsideLimit = true;
				}
			}

			//Movement inside the limits
			if (IsInsideLimit) {
				PlayerBody.anchoredPosition = WorldToCanvasPosition(CurrentTouch.position) - Orig + PlayerOrig;
			}
		}	
	}

	private void Update() {


    	//Vector3 mousePos = ParentCanvas.transform.tra

		
		//prevent the ball from getting out of the screen
        if (PlayerBody.anchoredPosition.x<LeftLimit) 
		{
			PlayerBody.anchoredPosition = new Vector2(LeftLimit,PlayerBody.anchoredPosition.y);
		}
		if (PlayerBody.anchoredPosition.x>RightLimit) 
		{
			PlayerBody.anchoredPosition = new Vector2(RightLimit,PlayerBody.anchoredPosition.y);
		}
		if (PlayerBody.anchoredPosition.y<BottomLimit) 
		{
			PlayerBody.anchoredPosition = new Vector2(PlayerBody.anchoredPosition.x,BottomLimit);
		}
		if (PlayerBody.anchoredPosition.y>TopLimit) 
		{
			PlayerBody.anchoredPosition = new Vector2(PlayerBody.anchoredPosition.x,TopLimit);
		}
		
		

		#if !UNITY_EDITOR
					
			currentTouch = Input.GetTouch(0);

			if (currentTouch.phase == TouchPhase.Began){
				OnTouchDown();
			}
		
			OnDrag();

		#endif
		
		Debug.Log(TouchPosition);
	}

}
