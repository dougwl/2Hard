using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitFadeGame : MonoBehaviour {

	public CanvasGroup board;

	private void Awake() {
		board.alpha = 1;
	}

	private void Start() {
		StartCoroutine(Move());
	}

	private IEnumerator Move() {
		float decelerate = 0f;
		while(board.alpha!=0f){
			decelerate += Time.deltaTime;
					
			board.alpha = Mathf.Lerp(board.alpha, 0f, decelerate*2);

			if (board.alpha < 0.05f) {
				board.alpha = 0f;
			}
			yield return null;
		}
	}
}