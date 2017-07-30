using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public bool isDead;

	float moveSpeed = 7;
	float turnSpeed = 3.5f;

	float horizontal, vertical;

	[SerializeField] LayerMask moveLayerMask;

	[SerializeField] GameObject gun;
	[SerializeField] Transform gunShootSpot;
	[SerializeField] GameObject laserPrefab;
	float gunShootCost = 0.01f;
	float gunCharge = 1f;

	Rigidbody2D rb;
	CircleCollider2D col;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<CircleCollider2D>();
	}

	void Shoot() {
		if (gunCharge > 0) {
			gunCharge -= gunShootCost;
			Instantiate(laserPrefab, gunShootSpot.position, gunShootSpot.rotation);
		}
	}

	void Move() {
		float moveFactor = moveSpeed * Time.deltaTime;
		Vector3 moveVector = new Vector3(horizontal * moveFactor, vertical * moveFactor);
		
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, col.radius, moveVector.normalized, moveVector.magnitude, moveLayerMask);

		foreach(RaycastHit2D hit in hits) {
			if (hit && Vector3.Dot(hit.transform.position - transform.position, moveVector.normalized) > 0) {
				if (moveVector.magnitude > hit.distance) {
					moveVector = moveVector.normalized * hit.distance;
				}
			}
		}
		transform.position += moveVector;
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
		if (isDead) {
			return;
		}

		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");

		Move();
		FaceMouse();
		DoGunStuff();
	}
}
