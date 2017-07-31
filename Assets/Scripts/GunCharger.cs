using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCharger : MonoBehaviour {
	[SerializeField] float chargeRate = 0.25f;
	bool shouldChargeGun = false;

	[SerializeField] Color idleColor = Color.gray;
	[SerializeField] Color chargingColor = Color.green;

	[SerializeField] SpriteRenderer sr;

	AudioSource audioSource;

	void Start() {
		audioSource = GetComponent<AudioSource>();
	}

	public void Enter() {
		shouldChargeGun = true;
	}

	public void Exit() {
		shouldChargeGun = false;
	}

	void Update() {
		if (GameManager.isPaused) {
			return;
		}
		if (shouldChargeGun) {
			Charge charge = GameManager.pc?.GetComponent<Charge>();
			if (charge?.charge < charge?.maxCharge) {
				charge?.GiveCharge(chargeRate * Time.deltaTime);
				sr.color = chargingColor;
				if (!audioSource.isPlaying) {
					audioSource.Play();
				}
			} else {
				sr.color = idleColor;
				if (audioSource.isPlaying) {
					audioSource.Stop();
				}
			}
		} else {
			sr.color = idleColor;
			if (audioSource.isPlaying) {
				audioSource.Stop();
			}
		}
	}
}
