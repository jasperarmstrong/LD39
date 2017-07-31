using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType {
    LASER, ROBOT
}

public class Health : MonoBehaviour {
	public float health = 1;
	public bool isDead = false;
	public bool isInvincible = false;

	public float laserDamage = 0.5f;
	public float robotDamage = 0.5f;

	public Action OnDeath;
	public Action<DamageType> OnDamage;

	void CheckHealth() {
		if (health <= 0) {
			health = 0;
			isDead = true;
			OnDeath?.Invoke();
		}
	}

	public void Damage(DamageType damageType) {
		if (isInvincible) {
			return;
		}

		switch (damageType) {
			case DamageType.LASER:
				health -= laserDamage;
				OnDamage?.Invoke(damageType);
				CheckHealth();
				break;
			case DamageType.ROBOT:
				health -= robotDamage;
				OnDamage?.Invoke(damageType);
				CheckHealth();
				break;
			default:
				Debug.LogError($"Not sure how to deal with DamageType {damageType}");
				break;
		}
	}
}
