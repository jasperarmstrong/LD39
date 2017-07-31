using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : MonoBehaviour {
	[SerializeField] Slider slider;

	void Start () {
		GameManager.sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1);
		slider.value = GameManager.sfxVolume;
	}
	
	void Update () {
		if (Mathf.Abs(GameManager.sfxVolume - slider.value) > 0.01f) {
			GameManager.sfxVolume = slider.value;
			PlayerPrefs.SetFloat("sfxVolume", slider.value);
		}
	}
}
