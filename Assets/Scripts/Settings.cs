using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

	public Slider eff;
	public Slider mus;
	public Toggle vib;

	private void Start() {
		if (PlayerPrefs.GetFloat("effectsVolume") == 0) PlayerPrefs.SetFloat("effectsVolume",1);
		if (PlayerPrefs.GetFloat("musicVolume") == 0) PlayerPrefs.SetFloat("musicVolume",1);
		eff.value = PlayerPrefs.GetFloat("effectsVolume");
		mus.value = PlayerPrefs.GetFloat("musicVolume");
		if (PlayerPrefs.GetInt("notVib")==1) vib.isOn = false;
		else  vib.isOn = true;
	}

	public void Music(float vol){
		AudioManager.AM.MusicVolume(vol);
	}

	public void Effects(float vol){
		AudioManager.AM.EffectsVolume(vol);
	}

	public void ToggleVib(){
		if(vib.isOn == false) PlayerPrefs.SetInt("notVib",1);
		else {
			PlayerPrefs.SetInt("notVib",0);
			AudioManager.AM.VibLight();
			}
	}
}
