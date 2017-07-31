using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotController : MonoBehaviour {
	float turnSpeed = 6;
	float detectionRadius = 20;

	float cooldownDuration = 0.8f;
	bool canAttack = true;

	float stunDuration = 0.15f;
	bool canMove = true;

	[SerializeField] GameObject deadRobotPrefab;

	Movement mov;
	Health health;

	[SerializeField] LayerMask targetLayerMask;
	Transform target;

	bool isDancing = false;
	float danceAmount = 0;

	float scootDirection = 0;

	[SerializeField] AudioClip hurtSound;
	[SerializeField] float hurtVolume = 1f;

	[SerializeField] AudioClip deathSound;
	[SerializeField] float deathVolume = 1f;

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);
	}

	void Start () {
		mov = GetComponent<Movement>();
		health = GetComponent<Health>();

		health.OnDeath += () => {
			GameObject go = (GameObject)Instantiate(deadRobotPrefab, transform.position, transform.rotation);
			go.transform.Rotate(0, 0, 180);
			RobotSpawner.HandleDeath(this);
			AudioSource.PlayClipAtPoint(deathSound, transform.position, deathVolume * GameManager.sfxVolume);
		};

		health.OnChange += (DamageType dt) => {
			if (dt == DamageType.LASER) {
				StartCoroutine(Stun());
				target = GameManager.pc.transform;
				AudioSource.PlayClipAtPoint(hurtSound, transform.position, hurtVolume * GameManager.sfxVolume);
			}	
		};

		scootDirection = Random.Range(0, 2) == 0 ? -1 : 1;
	}
	
	void FindTarget() {
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectionRadius, transform.up, 0, targetLayerMask);
		if (hits.Length > 0) {
			target = hits.OrderBy(hit => Vector3.Distance(hit.transform.position, transform.position)).ToArray()[0].transform;
		}
	}

	void GoToTarget() {
		transform.up = Vector3.Lerp(
			transform.up,
			target.position - transform.position,
			turnSpeed * Time.deltaTime
		);

		if ((target.position - transform.position).magnitude > 0.2f) {
			mov.Move(0, 1, Space.Self);
		}
	}

	IEnumerator Stun() {
		canMove = false;
		yield return new WaitForSeconds(stunDuration);
		canMove = true;
		yield return null;
	}

	IEnumerator Cooldown() {
		canAttack = false;
		yield return new WaitForSeconds(cooldownDuration);
		canAttack = true;
		yield return null;
	}

	void TryAttack(Health h) {
		if (canAttack) {
			StartCoroutine(Cooldown());
			h.Damage(DamageType.ROBOT);
		}
	}

	void RandomizeDanceAmount() {
		danceAmount = Random.Range(270f, 540f) * (Random.Range(0, 2) == 0 ? 1 : -1);
	}

	void Dance() {
		transform.Rotate(0, 0, danceAmount * Time.deltaTime);
		mov.Move(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Space.Self, 0.15f);
	}

	void Update () {
		if (GameManager.isPaused) {
			return;
		}

		if ((GameManager.pc != null && GameManager.pc.isDead) || GameManager.isGameOver) {
			if (!isDancing || Random.Range(0, 50) < 1) {
				isDancing = true;
				RandomizeDanceAmount();	
			}
			Dance();
			return;
		}

		if (health.isDead) {
			return;
		}

		if (target == null) {
			FindTarget();
		} else {
			if (canMove) {
				GoToTarget();
			}
		}
	}

	void OnCollision(Collision2D col) {
		if ((GameManager.pc != null && GameManager.pc.isDead) || GameManager.isGameOver) {
			return;
		}

		RobotController rc = col.transform.GetComponent<RobotController>();
		Health h = col.transform.GetComponent<Health>();

		if (h != null && rc == null && !col.collider.CompareTag("Battery")) {
			TryAttack(h);
		}

		if (rc != null && Vector2.Dot((Vector2)(col.transform.position - transform.position).normalized, (Vector2)transform.up) > 0) {
			// if the dot product of those two vectors is > 0, sidestep because this robot is behind the other robot
			mov.Move(Random.Range(0f, 1f) * scootDirection, 0, Space.Self);	
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		OnCollision(col);
	}

	void OnCollisionStay2D(Collision2D col) {
		OnCollision(col);
	}
}
