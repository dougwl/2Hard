using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreButtons : MonoBehaviour {

	[SerializeField] private List <Transform> Pages;
	[SerializeField] private List <Transform> Scrolls;
	[SerializeField] private CanvasGroup bonus;

	[SerializeField] private Text title;
	[SerializeField] private Text title2;

	private List <bool> lerp;

	private bool startLerp;
	private int targetPage;

	private void Start() {
		lerp = new List<bool>();
		for (int i=0; i<Pages.Count; i++){
			lerp.Add(false);
		}
	}

	public void ButtonPage(int pageId){
		for (int i=0; i < Pages.Count; i++){
			if (i==pageId) lerp[i] = true;
			else lerp[i] = false;
		}
		targetPage = pageId;
		startLerp = true;
	}

	public void ButtonExit(int pageId){
		lerp[pageId] = false;
		Scrolls[pageId].transform.localPosition = Vector3.zero;
		
	}

	public void ChangeTitle(string newTitle){
		title2.text = newTitle;
	}

	private void Update (){

		//make the specific store page go up
		if (startLerp) {
            float decelerate = Mathf.Min(10 * Time.deltaTime, 1f);
            for (int i=0;i<Pages.Count;i++){

				bonus.blocksRaycasts = false;
				
				if (lerp[i]) Pages[i].localPosition = Vector2.Lerp(Pages[i].localPosition, Vector3.zero, decelerate);
				
				if (Vector2.SqrMagnitude(Pages[i].localPosition - Vector3.zero) < 0.25f) {
					Pages[i].localPosition = Vector3.zero;
					startLerp = false;
            	}
			}
    	}
		
		//make the others store pages go down
		float decelerate2 = Mathf.Min(10f * Time.deltaTime, 1f);
		for (int i=0;i<Pages.Count;i++){
			
			if (!lerp[i]) Pages[i].localPosition = Vector2.Lerp(Pages[i].localPosition, new Vector3 (0, -1920, 0) , decelerate2);
			
			if (Vector2.SqrMagnitude(Pages[i].localPosition - new Vector3 (0, -1920, 0)) < 0.25f) {
				Pages[i].localPosition = new Vector3 (0, -1920, 0);
			}
		}

		bonus.alpha = -Pages[targetPage].localPosition.y/1920;
		title.GetComponent<CanvasGroup>().alpha = bonus.alpha;
		title2.GetComponent<CanvasGroup>().alpha = 1f - bonus.alpha;
	}

}