using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	float turnSpeed = 3.5f;

	[SerializeField] GameObject gun;
	[SerializeField] Transform gunShootSpot;
	[SerializeField] GameObject laserPrefab;
	float gunShootCost = 0.01f;
	float gunCharge = 1f;

	Movement mov;
	Health health;

	void Start() {
		mov = GetComponent<Movement>();
		health = GetComponent<Health>();

		health.OnDeath += () => {
			Debug.Log("the player died!");
			Destroy(gameObject);
		};
	}

	void Shoot() {
		if (gunCharge > 0) {
			gunCharge -= gunShootCost;
			Instantiate(laserPrefab, gunShootSpot.position, gunShootSpot.rotation);
		}
	}

	void FaceMouse() {
		Vector3 mousePos = Utils.MousePosInWorld();

		transform.up = Vector3.Lerp(
			transform.up,
			mousePos - transform.position,
			turnSpeed * Time.deltaTime
		);

		if (gun != null) {
			gun.transform.up = mousePos - gun.transform.position;
		}
	}

	void DoGunStuff() {
		if (Input.GetMouseButtonDown(0)) {
			Shoot();
		}
	}

	void Update () {
		if (health.isDead || GameManager.isGameOver) {
			return;
		}

		mov.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		FaceMouse();
		DoGunStuff();
	}
}
