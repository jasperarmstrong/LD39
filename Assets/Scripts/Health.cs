using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType {
    LASER
}

public class Health : MonoBehaviour {
	public float health = 1;
	public bool isDead = false;

	public float laserDamage = 0.5f;

	public Action OnDeath;

	void CheckHealth() {
		if (health <= 0) {
			health = 0;
			isDead = true;
			OnDeath?.Invoke();
		}
	}

	public void Damage(DamageType damageType) {
		switch (damageType) {
			case DamageType.LASER:
				health -= laserDamage;
				CheckHealth();
				break;
			default:
				Debug.LogError($"Not sure how to deal with DamageType {damageType}");
				break;
		}
	}
}
