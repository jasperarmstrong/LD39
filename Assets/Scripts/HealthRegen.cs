using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : MonoBehaviour {
	[SerializeField] float healRate = 0.15f;
	bool shouldHeal = false;

	[SerializeField] Color idleColor = Color.gray;
	[SerializeField] Color chargingColor = Color.green;

	[SerializeField] SpriteRenderer sr;

	public void Enter() {
		shouldHeal = true;
	}

	public void Exit() {
		shouldHeal = false;
	}

	void Update() {
		if (shouldHeal) {
			Health health = GameManager.pc?.GetComponent<Health>();
			if (health?.health < health?.maxHealth) {
				health?.Heal(healRate * Time.deltaTime);
				sr.color = chargingColor;
			} else {
				sr.color = idleColor;	
			}
		} else {
			sr.color = idleColor;
		}
	}
}
