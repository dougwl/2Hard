using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using MoreMountains.NiceVibrations;

public class AudioManager : MonoBehaviour {

	public AudioMixerSnapshot mainMenu;
	public AudioMixerSnapshot store;
	public AudioMixerSnapshot beforePlay;
	public AudioMixerSnapshot playing;
	public AudioMixerSnapshot gameOver;
	
	public AudioSource point;
	public AudioSource gameOverSound;

	public AudioMixer mixer;

	public static AudioManager AM = null;
	private float time = 1.5f;

	private void Awake() {

		if (AM == null) AM = this;
        else if (AM != this) Destroy(gameObject);    
		
		DontDestroyOnLoad(gameObject);

		MusicVolume(PlayerPrefs.GetFloat("musicVolume"));
		EffectsVolume(PlayerPrefs.GetFloat("effectsVolume"));

	}

	public void MusicMainMenu(){
		mainMenu.TransitionTo(time);
	}

	public void MusicStore(){
		store.TransitionTo(time);
	}

	public void MusicBeforePlay(){
		beforePlay.TransitionTo(time);
	}

	public void MusicPlaying(){
		playing.TransitionTo(time);
	}

	public void MusicGameOver(){
		gameOver.TransitionTo(time);
	}

	public void PlayPoint(){
		point.Play();
	}

	public void GameOver(){
		gameOverSound.Play();
	}

	public void MusicVolume(float vol){
		mixer.SetFloat ("musicVolume", Mathf.Log(vol) * 20);
		PlayerPrefs.SetFloat("musicVolume", vol);
	}

	public void EffectsVolume(float vol){
		mixer.SetFloat ("effectsVolume", Mathf.Log(vol) * 20);
		PlayerPrefs.SetFloat("effectsVolume", vol);
	}

	public void VibSuperLight(){
		MMVibrationManager.Haptic (HapticTypes.LightImpact);
	}

	public void VibLight(){
		MMVibrationManager.Haptic (HapticTypes.Success);
	}

	public void VibMedium(){
		MMVibrationManager.Haptic (HapticTypes.Failure);
	}
}
