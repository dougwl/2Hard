using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeMenuButtons : MonoBehaviour {

	public void Ghost(){
		GameManager.GM.ButtonGhostBalls();
	}

	public void Normal(){
		GameManager.GM.ButtonNormal();
	}

	public void NoWalls(){
		GameManager.GM.ButtonNoWalls();
	}

	public void Pulsing(){
		GameManager.GM.ButtonPulsing();
	}

	public void Random(){
		GameManager.GM.ButtonRandom();
	}

	public void Slow(){
		GameManager.GM.ButtonSlowBalls();
	}

	public void Survival(){
		GameManager.GM.ButtonSurvival();
	}
	
	public void TwoBalls(){
		GameManager.GM.ButtonTwoBalls();
	}

}
