using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private GameObject Player;
    private RectTransform PlayerBody;
    private Vector2 TouchOrigin;
    private Vector2 PlayerOrigin;

    private float LeftLimit;
    private float RightLimit;
    private float TopLimit;
    private float BottomLimit;

    private float PlayerLimit;

    private bool IsInsideLimit = true;

    private Touch CurrentTouch;

    private GameManager GM;

    private Camera WorldCamera;

    private RectTransform Rect;

    private class ScreenLimit
    {
        public float Left, Right, Top, Bottom;
        
        private ScreenLimit(Vector2 screenBorder, float offset = 0){
            Left = -(screenBorder.x/2) + offset;
            Right = (screenBorder.x/2) - offset;
            Top = (screenBorder.y/2) - offset;
            Bottom = -(screenBorder.y/2) + offset;
        }
    }

    private void Awake()
    {
        GM = GameManager.GM;
        CurrentTouch = new Touch();
        PlayerBody = Player.GetComponent<RectTransform>();
        Input.multiTouchEnabled = false;
        //Getting the screen limits
        PlayerLimit = PlayerBody.sizeDelta.x / 2;
        LeftLimit = -GM.screenWidth / 2 + PlayerLimit;
        RightLimit = GM.screenWidth / 2 - PlayerLimit;
        TopLimit = GM.screenHeight / 2 - PlayerLimit;
        BottomLimit = -GM.screenHeight / 2 + PlayerLimit;

        GetComponent<BoxCollider2D>().size = new Vector2(GM.screenWidth, GM.screenHeight);
        WorldCamera = GetComponentInParent<Canvas>().worldCamera;
        Rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (GameManager.GM.GameMode == GameMode.NoWalls) PlayerLimit = 0f;
    }

    private Vector2 WorldToCanvasPosition(Vector2 position)
    {
        Vector2 convertedPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Rect, position, WorldCamera, out convertedPosition);
        return convertedPosition;
    }

    private void OnTouchDown()
    {
#if !UNITY_EDITOR
			//Getting the original positions of touch and player
			TouchOrigin = WorldToCanvasPosition(CurrentTouch.position);
			PlayerOrigin = PlayerBody.anchoredPosition;
#endif
    }

    private void OnMouseDown()
    {
#if UNITY_EDITOR
        //Getting the original positions of mouse and player
        TouchOrigin = WorldToCanvasPosition(Input.mousePosition);
        PlayerOrigin = PlayerBody.anchoredPosition; ;
#endif
    }

    private void OnMouseUp()
    {
#if UNITY_EDITOR
        IsInsideLimit = true;
#endif
    }

    private void OnMouseDrag()
    {
        //Called when dragging the mouse
#if UNITY_EDITOR
        CurrentTouch.position = Input.mousePosition;
        OnDrag();
#endif
    }

    private void OnDrag()
    {

        if (GameManager.GM.Playing)
        {

            //Don't go beyond the left
            if (PlayerBody.anchoredPosition.x <= LeftLimit)
            {
                SetPlayerInsideLimits(LeftLimit);
            }

            //Don't go beyond the right
            if (PlayerBody.anchoredPosition.x >= RightLimit)
            {
                SetPlayerInsideLimits(RightLimit);
            }

            //Don't go beyond the top
            if (PlayerBody.anchoredPosition.y >= TopLimit)
            {
                SetPlayerInsideLimits(TopLimit,isHorizontal:false);
            }

            //Don't go beyond the bottom
            if (PlayerBody.anchoredPosition.y <= BottomLimit)
            {
                SetPlayerInsideLimits(BottomLimit,isHorizontal:false);
            }

            //Movement inside the limits
            if (IsInsideLimit)
            {
                PlayerBody.anchoredPosition = WorldToCanvasPosition(CurrentTouch.position) - TouchOrigin + PlayerOrigin;
            }
        }
    }

    private void SetPlayerInsideLimits(float limit, bool isHorizontal = true)
    {
        IsInsideLimit = false;

        if (TouchOrigin.x < WorldToCanvasPosition(CurrentTouch.position).x && isHorizontal && limit < 0 || 
        	TouchOrigin.x > WorldToCanvasPosition(CurrentTouch.position).x && isHorizontal && limit > 0 || 
			TouchOrigin.y < WorldToCanvasPosition(CurrentTouch.position).y && !isHorizontal && limit < 0 ||
			TouchOrigin.y > WorldToCanvasPosition(CurrentTouch.position).y && !isHorizontal && limit > 0)
        {
            IsInsideLimit = true;
        }
        else
        {
            if (isHorizontal)
				PlayerBody.anchoredPosition = new Vector2(limit, WorldToCanvasPosition(CurrentTouch.position).y - TouchOrigin.y + PlayerOrigin.y);
            else
				PlayerBody.anchoredPosition = new Vector2(WorldToCanvasPosition(CurrentTouch.position).x - TouchOrigin.x + PlayerOrigin.x, limit);

			TouchOrigin = WorldToCanvasPosition(CurrentTouch.position);
            PlayerOrigin = PlayerBody.anchoredPosition;
            IsInsideLimit = true;
        }
    }

    private void Update()
    {
        //Vector3 mousePos = ParentCanvas.transform.tra

        //prevent the ball from getting out of the screen
        if (PlayerBody.anchoredPosition.x < LeftLimit)
        {
            PlayerBody.anchoredPosition = new Vector2(LeftLimit, PlayerBody.anchoredPosition.y);
        }
        if (PlayerBody.anchoredPosition.x > RightLimit)
        {
            PlayerBody.anchoredPosition = new Vector2(RightLimit, PlayerBody.anchoredPosition.y);
        }
        if (PlayerBody.anchoredPosition.y < BottomLimit)
        {
            PlayerBody.anchoredPosition = new Vector2(PlayerBody.anchoredPosition.x, BottomLimit);
        }
        if (PlayerBody.anchoredPosition.y > TopLimit)
        {
            PlayerBody.anchoredPosition = new Vector2(PlayerBody.anchoredPosition.x, TopLimit);
        }



#if !UNITY_EDITOR
					
			CurrentTouch = Input.GetTouch(0);

			if (CurrentTouch.phase == TouchPhase.Began){
				OnTouchDown();
			}
		
			OnDrag();

#endif

    }

}
