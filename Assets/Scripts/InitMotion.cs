using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitMotion : MonoBehaviour {  //This name probably should be changed ! 

	public Transform top;
	public Transform topButtons;
	public Transform bottom;
	public CanvasGroup board;
	public CanvasGroup bg;
	public CanvasGroup fading;
	private bool isLoaded;
	public bool isFading = true;
	private TrailRenderer[] tr;
	private ParticleSystem[] ps;

	private void OnEnable() {
		//enemies.alpha = 0f;
		fading.alpha = 1;
		bottom.localPosition = new Vector3 (0,-1080);
		top.localPosition = new Vector3 (0, 1085);
		topButtons.localPosition = new Vector3 (topButtons.localPosition.x,top.localPosition.y + 175);
		if (GameManager.GM) isLoaded = GameManager.GM.isLoaded;
	}

	private void Start() {
		StartCoroutine(Move());
		
	}

	private IEnumerator Move() {
		//if (!isLoaded) yield return new WaitForSeconds(0.15f);
		float decelerate = 0;
		while(top.localPosition != new Vector3 (0,-175)){

			decelerate += Time.deltaTime;
					
			top.localPosition = Vector2.Lerp(top.localPosition, new Vector3 (0,-175), decelerate);
			bottom.localPosition = Vector2.Lerp(bottom.localPosition, new Vector3 (0,-780), decelerate);
			fading.alpha = Mathf.Lerp(fading.alpha, 0f, decelerate);
			topButtons.localPosition = new Vector3 (topButtons.localPosition.x,top.localPosition.y + 175);
			if (fading.alpha < 0.05f) {
				top.localPosition = new Vector3 (0,-175);
				bottom.localPosition = new Vector3 (0,-780);
				fading.alpha = 0f;
				topButtons.localPosition = new Vector3 (topButtons.localPosition.x,top.localPosition.y + 175);
			}
			yield return null;
		}
		isFading = false;
		//StartCoroutine(BallsFade());
	}

	public IEnumerator Moveout() {
		
		if (!isFading){
			isFading = true;
			tr = board.GetComponentsInChildren<TrailRenderer>();
			ps = board.GetComponentsInChildren<ParticleSystem>();
			float decelerate = 0;
			foreach (ParticleSystem par in ps){
				ParticleSystem.EmissionModule em = par.emission;
				em.enabled = false;
			}
			while(top.localPosition != new Vector3 (0, 125)){

				decelerate += Time.deltaTime;
						
				top.localPosition = Vector2.Lerp(top.localPosition, new Vector3 (0, 125), decelerate);
				bottom.localPosition = Vector2.Lerp(bottom.localPosition, new Vector3 (0,-1080), decelerate);
				fading.alpha = Mathf.Lerp(fading.alpha, 1f, decelerate);
				topButtons.localPosition = new Vector3 (topButtons.localPosition.x,top.localPosition.y + 175);

				if (fading.alpha > 0.95f) {
					top.localPosition = new Vector3 (0, 125);
					bottom.localPosition = new Vector3 (0,-1080);
					fading.alpha = 1f;
					topButtons.localPosition = new Vector3 (topButtons.localPosition.x,top.localPosition.y + 175);
					GameManager.GM.StartGame();
				}

				yield return null;
			}
		}
	}
}
