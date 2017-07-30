using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotController : MonoBehaviour {
	float turnSpeed = 6;
	float detectionRadius = 10;

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
			Destroy(gameObject);	
		};
	}
	
	void FindTarget() {
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectionRadius, transform.up, 0, targetLayerMask);
		if (hits.Length > 0) {
			target = hits.OrderBy(hit => hit.distance).ToArray()[0].transform;
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

	void Update () {
		if (health.isDead || GameManager.isGameOver) {
			return;
		}

		if (target == null) {
			FindTarget();
		} else {
			GoToTarget();
		}
	}
}
