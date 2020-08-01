using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModeSwitch : MonoBehaviour {

    private GameManager GM;
    public Dictionary<GameMode, Action<bool>> Modes;

    void Start(){
        GM = GameManager.GM;
        PopulateDictionary();
        GameManager.GM.ModeSwitch = this;
    }
    
    private void PopulateDictionary()
    {
        Modes = new Dictionary<GameMode, Action<bool>>();
        Modes.Add(GameMode.Normal,Normal);
        Modes.Add(GameMode.Duo,TwoBalls);
        Modes.Add(GameMode.Slow,Slow);
        Modes.Add(GameMode.NoWalls,NoWalls);
        Modes.Add(GameMode.Survival, Survival);
        Modes.Add(GameMode.Pulse, Pulsing);
        Modes.Add(GameMode.Ghost, Ghost);
    }

	public void Normal(bool isRandom = false){
        GM.isRandom = isRandom;
        if (GM.GameMode != GameMode.Normal)
        {
            GM.ChangeGameMode(GameMode.Normal);
            GM.EnemyManager.ResetStart();
            GM.EnemyManager.DefaultConfig();
            GM.EnemyManager.DefaultSpeedConfig();
            if (GM.GameState == GameState.MainMenu) GM.EnemyManager.StartMovement();
        }
        if (!isRandom) GM.modeMenu.GetComponent<ModeMenuAnimations>().NormalMode();
    }

    public void TwoBalls(bool isRandom = false)
    {
        GM.isRandom = isRandom;
        if (GM.GameMode != GameMode.Duo)
        {
            GM.ChangeGameMode(GameMode.Duo);
            GM.EnemyManager.ResetStart();
            GM.EnemyManager.TwoBallsConfig();
            GM.EnemyManager.DefaultConfig();
            GM.EnemyManager.DefaultSpeedConfig();
            if (GM.GameState == GameState.MainMenu) GM.EnemyManager.StartMovement();
        }
        if (!isRandom) GM.modeMenu.GetComponent<ModeMenuAnimations>().TwoBallsMode();
    }

    public void Slow(bool isRandom = false)
    {
        GM.isRandom = isRandom;
        if (GM.GameMode != GameMode.Slow)
        {
            GM.ChangeGameMode(GameMode.Slow);
            GM.EnemyManager.ResetStart();
            GM.EnemyManager.DefaultConfig();
            GM.EnemyManager.SlowSpeedConfig();
            if (GM.GameState == GameState.MainMenu) GM.EnemyManager.StartMovement();
        }
        if (!isRandom) GM.modeMenu.GetComponent<ModeMenuAnimations>().SlowBallsMode();
    }

    public void NoWalls(bool isRandom = false)
    {
        GM.isRandom = isRandom;
        if (GM.GameMode != GameMode.NoWalls)
		{
            GM.ChangeGameMode(GameMode.NoWalls);
            GM.EnemyManager.ResetStart();
            GM.EnemyManager.DefaultConfig();
            GM.EnemyManager.DefaultSpeedConfig();
            GM.EnemyManager.NoWallsConfig();
            if (GM.GameState == GameState.MainMenu) GM.EnemyManager.StartMovement();
        }
        if (!isRandom) GM.modeMenu.GetComponent<ModeMenuAnimations>().NoWallsMode();
	}

    public void Survival(bool isRandom = false)
    {
        GM.isRandom = isRandom;
        if (GM.GameMode != GameMode.Survival)
        {
            GM.ChangeGameMode(GameMode.Survival);
            GM.EnemyManager.ResetStart();
            GM.EnemyManager.DefaultConfig();
            GM.EnemyManager.SurvivalConfig();
            if (GM.GameState == GameState.MainMenu) GM.EnemyManager.StartMovement();
        }
        if (!isRandom) GM.modeMenu.GetComponent<ModeMenuAnimations>().SurvivalMode();
    }

    public void Pulsing(bool isRandom = false)
    {
        GM.isRandom = isRandom;
        if (GM.GameMode != GameMode.Pulse)
		{
            GM.ChangeGameMode(GameMode.Pulse);
            GM.EnemyManager.ResetStart();
            GM.EnemyManager.DefaultConfig();
            GM.EnemyManager.DefaultSpeedConfig();
            GM.EnemyManager.PulseConfig();
            if (GM.GameState == GameState.MainMenu) GM.EnemyManager.StartMovement();
        }
        if (!isRandom) GM.modeMenu.GetComponent<ModeMenuAnimations>().PulsingMode();
	}

    public void Ghost(bool isRandom = false)
    {
        GM.isRandom = isRandom;
        if (GM.GameMode != GameMode.Ghost)
        {
            GM.ChangeGameMode(GameMode.Ghost);
            GM.EnemyManager.GhostConfig();
            GM.EnemyManager.ResetStart();
            GM.EnemyManager.DefaultSpeedConfig();
            if (GM.GameState == GameState.MainMenu) GM.EnemyManager.StartMovement();
        }
        if (!isRandom) GM.modeMenu.GetComponent<ModeMenuAnimations>().GhostBallsMode();
    }

    public void Random(){
		Randomize();
		GM.isRandom = true;
		GM.modeMenu.GetComponent<ModeMenuAnimations>().RandomMode();
	}

    public void Randomize()
    {
        System.Random random = new System.Random();
        GameMode randomMode = (GameMode) random.Next(Enum.GetNames(typeof(GameMode)).Length); // Random positive int less than the total amount of GameModes;
        Modes[randomMode](true);
    }
}
