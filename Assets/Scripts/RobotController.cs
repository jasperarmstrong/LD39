using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotController : MonoBehaviour {
	float turnSpeed = 6;
	float detectionRadius = 10;

	float cooldownAmount = 0.75f;
	bool canAttack = true;

	Movement mov;
	Health health;

	[SerializeField] LayerMask targetLayerMask;
	Transform target;

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);
	}

	void Start () {
		mov = GetComponent<Movement>();
		health = GetComponent<Health>();

		health.OnDeath += () => {
			RobotSpawner.HandleDeath(this);
		};
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

	IEnumerator Cooldown() {
		canAttack = false;
		yield return new WaitForSeconds(cooldownAmount);
		canAttack = true;
		yield return null;
	}

	void TryAttack(Health h) {
		if (canAttack) {
			StartCoroutine(Cooldown());
			h.Damage(DamageType.ROBOT);
		}
	}

	void Update () {
		if (health.isDead || GameManager.isGameOver) {
			return;
		}

		if (target == null) {
			FindTarget();
		} else {
			GoToTarget();
		}

		foreach(Health h in mov.healthCollisions) {
			if (h != null && h.GetComponent<RobotController>() == null) {
				TryAttack(h);
			}
		}
	}
}
