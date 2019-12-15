using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager GM = null;

	public GameState gameState;
	public GameMode gameMode = GameMode.Random;

	public BackState backButtonState;

	public bool isRandom = true;

	public GameObject gameStateObj;
	public GameObject modeMenu;

	public EnemyManager enMan;

	public Color ballNewColor;
	public Color ballNewColorP;

	public bool menuScene = false;
	public bool gameScene = false;
	public bool playing = false;

	public List <SpriteRenderer> BallsTexE;
	public List <SpriteRenderer> BallsTexP;

	public bool isLoaded = false;

	public float screenWidth;

	private void Awake() {
		if (GM == null) GM = this;
        else if (GM != this) Destroy(gameObject);    
		DontDestroyOnLoad(gameObject);
		SceneManager.activeSceneChanged += onSceneLoad;	
		PlayerPrefs.SetInt("Random",1);
		ChangeState(GameState.MainMenu);
		screenWidth = Screen.width*1920/Screen.height;
	}

	public int GetCoinTime(){
		if (gameMode == GameMode.Normal) return 5;
		else if (gameMode == GameMode.Duo) return 10;
		else if (gameMode == GameMode.Slow) return 10;
		else if (gameMode == GameMode.NoWalls) return 7;
		else if (gameMode == GameMode.Survival) return 10;
		else if (gameMode == GameMode.Ghost) return 7;
		else if (gameMode == GameMode.Pulse) return 5;
		else return 0;
	}

	private void Start() {
		Invoke("Randomize", 0.05f);
		isLoaded = true;
	}

	public void GameOver(){
		playing = false;
		gameStateObj.GetComponent<GameOver>().CallGameOver();
		ChangeState(GameState.GameOver);
	}

	public void StartGame(){
		playing = false;
		ChangeState(GameState.InGame);
		if (isRandom) Randomize();
		UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
	}

	public void Play(){
		playing = true;
		enMan.startMovement();
	}

	public void StartMenu(){
		ChangeState(GameState.MainMenu);
		if (isRandom) Randomize();
		UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
	}

	public void ButtonNormal(){
		isRandom = false;
		if (gameMode != GameMode.Normal){
			ChangeGameMode(GameMode.Normal);
			enMan.ResetStart();
			enMan.SetDefault();
			enMan.startMovement();
			enMan.NormalSpeed();
		}
		modeMenu.GetComponent<ModeMenuManager>().NormalMode();
	}

	public void ButtonTwoBalls(){
		isRandom = false;
		if (gameMode != GameMode.Duo){
			ChangeGameMode(GameMode.Duo);
			enMan.ResetStart();
			enMan.TwoBalls();
			enMan.SetDefault();
			enMan.startMovement();
			enMan.NormalSpeed();
		}
		modeMenu.GetComponent<ModeMenuManager>().TwoBallsMode();
	}

	public void ButtonSlowBalls(){
		isRandom = false;
		if (gameMode != GameMode.Slow){
			ChangeGameMode(GameMode.Slow);
			enMan.ResetStart();
			enMan.SetDefault();
			enMan.startMovement();
			enMan.SlowMode();
		}
		modeMenu.GetComponent<ModeMenuManager>().SlowBallsMode();
	}

	public void ButtonNoWalls(){
		isRandom = false;
		if (gameMode != GameMode.NoWalls){
			ChangeGameMode(GameMode.NoWalls);
			enMan.ResetStart();
			enMan.SetDefault();
			enMan.startMovement();
			enMan.NormalSpeed();
			enMan.NoWalls();
		}
		modeMenu.GetComponent<ModeMenuManager>().NoWallsMode();
	}

	public void ButtonSurvival(){
		isRandom = false;
		if (gameMode != GameMode.Survival)
		{
			ChangeGameMode(GameMode.Survival);
			enMan.ResetStart();
			enMan.SetDefault();
			enMan.Survival();
			enMan.startMovement();
		}
		modeMenu.GetComponent<ModeMenuManager>().SurvivalMode();
	}

	public void ButtonGhostBalls(){
		isRandom = false;
		if (gameMode != GameMode.Ghost){
			ChangeGameMode(GameMode.Ghost);
			enMan.ResetStart();
			enMan.startMovement();
			enMan.NormalSpeed();
			enMan.SetGhost();
		}
		modeMenu.GetComponent<ModeMenuManager>().GhostBallsMode();
	}

	public void ButtonPulsing(){
		isRandom = false;
		if (gameMode != GameMode.Pulse){
			ChangeGameMode(GameMode.Pulse);
			enMan.ResetStart();
			enMan.SetDefault();
			enMan.NormalSpeed();
			enMan.Pulse();
			enMan.startMovement();
		}
		modeMenu.GetComponent<ModeMenuManager>().PulsingMode();
	}

	public void ButtonRandom(){
		Randomize();
		isRandom = true;
		modeMenu.GetComponent<ModeMenuManager>().RandomMode();
	}

	public void Randomize(){
		int rand;
		rand = Random.Range(0,7);
		switch (rand){
			case 0:
				if (gameMode != GameMode.Normal){
					ChangeGameMode(GameMode.Normal);
					enMan.ResetStart();
					enMan.SetDefault();
					enMan.NormalSpeed();
					if (menuScene) enMan.startMovement();
				}
				break;
			case 1:
				if (gameMode != GameMode.Duo){
					ChangeGameMode(GameMode.Duo);
					enMan.ResetStart();
					enMan.SetDefault();
					enMan.TwoBalls();
					enMan.NormalSpeed();
					if (menuScene) enMan.startMovement();
				}
				break;
			case 2:
				if (gameMode != GameMode.Slow){
					ChangeGameMode(GameMode.Slow);
					enMan.ResetStart();
					enMan.SetDefault();
					enMan.SlowMode();
					if (menuScene) enMan.startMovement();
				}
				break;
			case 3:
				if (gameMode != GameMode.NoWalls){
					ChangeGameMode(GameMode.NoWalls);
					enMan.ResetStart();
					enMan.SetDefault();
					enMan.NormalSpeed();
					enMan.NoWalls();
					if (menuScene) enMan.startMovement();
				}
				break;
			case 4:
				if (gameMode != GameMode.Survival){
					ChangeGameMode(GameMode.Survival);
					enMan.ResetStart();
					enMan.SetDefault();
					enMan.Survival();
					if (menuScene) enMan.startMovement();
				}
				break;
			case 5:
				if (gameMode != GameMode.Ghost){
					ChangeGameMode(GameMode.Ghost);
					enMan.SetGhost();
					enMan.ResetStart();
					enMan.NormalSpeed();
					if (menuScene) enMan.startMovement();
				}
				break;
			case 6:
				if (gameMode != GameMode.Pulse){
					ChangeGameMode(GameMode.Pulse);
					enMan.ResetStart();
					enMan.SetDefault();
					enMan.NormalSpeed();
					enMan.Pulse();
					if (menuScene) enMan.startMovement();
				}
				break;
		}
	}

	public void ChangeState(GameState state){
		gameState = state; 
	}

	public void ChangeGameMode(GameMode mode){
		gameMode = mode;
	}

	public void onSceneLoad(Scene scene, Scene sceneMode){
		
		if(sceneMode.name == "Main_Menu"){
			menuScene = true;
			gameScene = false;
		}
		else{
			gameScene = true;
			menuScene = false;
		}
	}
}
