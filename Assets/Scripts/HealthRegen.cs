using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : MonoBehaviour {
	[SerializeField] float healRate = 0.15f;
	bool shouldHeal = false;

	[SerializeField] Color idleColor = Color.gray;
	[SerializeField] Color chargingColor = Color.green;

	[SerializeField] SpriteRenderer sr;
	[SerializeField] AudioSource audioSource;

	public void Enter() {
		shouldHeal = true;
	}

	public void Exit() {
		shouldHeal = false;
	}

	void Update() {
		if (GameManager.isPaused) {
			return;
		}
		if (shouldHeal) {
			Health health = GameManager.pc?.GetComponent<Health>();
			if (health?.health < health?.maxHealth) {
				health?.Heal(healRate * Time.deltaTime);
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
