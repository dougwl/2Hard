﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager GM = null;

    public GameState GameState { get; private set; }
    public GameMode GameMode { get; private set; }

    public BackState backButtonState;

    public bool isRandom = true;

    public GameObject gameStateObj;  // RENAME THIS THING
    public GameObject modeMenu;

    public EnemyManager EnemyManager;
    public ModeSwitch ModeSwitch;

    public bool Playing = false;

    public bool isLoaded = false;

    public float screenWidth { get; private set; }

    public float screenHeight { get; private set; } = 1920;

    public Vector2 ScreenBorder;

    public event Action OnModeChange;

    private void Awake()
    {
        if (GM == null) GM = this;
        else if (GM != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneLoad;
        ChangeGameMode(GameMode.None);
        ChangeState(GameState.MainMenu);
        screenWidth = Screen.width * screenHeight / Screen.height;
        ScreenBorder = new Vector2(screenWidth,screenHeight);
    }

    private void Start()
    {
        ModeSwitch.Invoke("Randomize", 0.05f); //WTF IS THIS - Find a solution to delay the call of reset start or a condition to avoid calling it on the start.
        isLoaded = true;
    }


    // Methods - Change of State

    public void GameOver()
    {
        Playing = false;
        gameStateObj.GetComponent<GameOver>().CallGameOver(); //THIS THING HERE.
        ChangeState(GameState.GameOver);
    }

    public void StartGame()
    {
        Playing = false;
        ChangeState(GameState.InGame);
        if (isRandom) ModeSwitch.Randomize();
        SceneManager.LoadScene("Game");
    }

    public void Play()
    {
        Playing = true;
        EnemyManager.StartMovement();
    }

    public void StartMenu()
    {
        ChangeState(GameState.MainMenu);
        if (isRandom) ModeSwitch.Randomize();
        SceneManager.LoadScene("Main_Menu");
    }

    public void ChangeState(GameState state)
    {
        GameState = state;
    }

    public void ChangeGameMode(GameMode mode)
    {
        GameMode = mode;
        if(OnModeChange != null) {
            OnModeChange();
            /* for (int i = 0; i < OnModeChange.GetInvocationList().Length; i++)
            {
                Debug.Log(OnModeChange.GetInvocationList()[i]);
            } */
            
        }

    }

    public void OnSceneLoad(Scene scene, Scene sceneMode)
    {

        if (sceneMode.name == "Main_Menu")
        {
            ChangeState(GameState.MainMenu);
        }
        else
        {
            ChangeState(GameState.InGame);
        }
    }
}
