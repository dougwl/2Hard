using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Darkening : MonoBehaviour {

	private float rightLimit, topLimit, max;
	
	public IEnumerator FadeInSprite(){
		
		while(GetComponent<CanvasGroup>().alpha!=1f){
			float decelerate = Mathf.Min(7.5f * Time.deltaTime, 1f);
					
			GetComponent<CanvasGroup>().alpha = Mathf.Lerp(GetComponent<CanvasGroup>().alpha, 1f, decelerate);

			if (GetComponent<CanvasGroup>().alpha > 0.999f) {
				GetComponent<CanvasGroup>().alpha = 1f;
			}
			yield return null;
		}
	}
}
