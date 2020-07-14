using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using MoreMountains.NiceVibrations;

public class AudioManager : MonoBehaviour {

	public static AudioManager AM = null;

	[SerializeField] private AudioMixer Mixer;

	[SerializeField] private AudioSource PointSound;
	[SerializeField] private AudioSource GameOverSound;

    [SerializeField] private AudioMixerSnapshot MainMenuSnapshot;
	[SerializeField] private AudioMixerSnapshot StoreSnapshot;
	[SerializeField] private AudioMixerSnapshot BeforePlaySnapshot;
	[SerializeField] private AudioMixerSnapshot PlayingSnapshot;
	[SerializeField] private AudioMixerSnapshot GameOverSnapshot;
	
	private readonly float TransitionTime = 1.5f;

	private void Awake() {

		if (AM == null) AM = this;
        else if (AM != this) Destroy(gameObject);    
		
		DontDestroyOnLoad(gameObject);

		MusicVolume(PlayerPrefs.GetFloat("musicVolume"));
		EffectsVolume(PlayerPrefs.GetFloat("effectsVolume"));

	}

	public void MusicMainMenu(){
		MainMenuSnapshot.TransitionTo(TransitionTime);
	}

	public void MusicStore(){
		StoreSnapshot.TransitionTo(TransitionTime);
	}

	public void MusicBeforePlay(){
		BeforePlaySnapshot.TransitionTo(TransitionTime);
	}

	public void MusicPlaying(){
		PlayingSnapshot.TransitionTo(TransitionTime);
	}

	public void MusicGameOver(){
		GameOverSnapshot.TransitionTo(TransitionTime);
	}

	public void PlayPoint(){
		PointSound.Play();
	}

	public void GameOver(){
		GameOverSound.Play();
	}

	public void MusicVolume(float vol){
		Mixer.SetFloat ("musicVolume", Mathf.Log(vol) * 20);
		PlayerPrefs.SetFloat("musicVolume", vol);
	}

	public void EffectsVolume(float vol){
		Mixer.SetFloat ("effectsVolume", Mathf.Log(vol) * 20);
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
