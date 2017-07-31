using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour {
	[SerializeField] AudioSource audioSource;
	[SerializeField] Slider slider;

	void Start () {
		slider.value = PlayerPrefs.GetFloat("musicVolume", audioSource.volume);
	}
	
	void Update () {
		if (Mathf.Abs(audioSource.volume - slider.value) > 0.01f) {
			audioSource.volume = slider.value;
			PlayerPrefs.SetFloat("musicVolume", slider.value);
		}
	}
}
