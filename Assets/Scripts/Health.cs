using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType {
    NONE, LASER, ROBOT, INSTAKILL
}

public class Health : MonoBehaviour {
	public float maxHealth {
		get {
			return 1;
		}
	}

	public float health = 1;
	public bool isDead = false;
	public bool isInvincible = false;

	public float laserDamage = 0.5f;
	public float robotDamage = 0.5f;

	public Action OnDeath;
	public Action<DamageType> OnChange;

	void CheckHealth() {
		if (health <= 0) {
			health = 0;
			isDead = true;
			OnDeath?.Invoke();
		}
	}

	public bool IsFull() {
		return Mathf.Abs(1 - health) < 0.01f;
	}

	public void Heal(float amount) {
		health += amount;
		if (health > 1) {
			health = 1;
		}
		OnChange?.Invoke(DamageType.NONE);
	}

	public void Damage(DamageType damageType) {
		if (isInvincible) {
			return;
		}

		switch (damageType) {
			case DamageType.LASER:
				health -= laserDamage;
				OnChange?.Invoke(damageType);
				CheckHealth();
				break;
			case DamageType.ROBOT:
				health -= robotDamage;
				OnChange?.Invoke(damageType);
				CheckHealth();
				break;
			case DamageType.INSTAKILL:
				health -= 1000;
				OnChange?.Invoke(damageType);
				CheckHealth();
				break;
			default:
				Debug.LogError($"Not sure how to deal with DamageType {damageType}");
				break;
		}
	}
}
