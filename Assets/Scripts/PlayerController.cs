using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	float turnSpeed = 3.5f;

	public Transform itemHolder;
	[SerializeField] GameObject gun;
	[SerializeField] Transform gunShootSpot;
	[SerializeField] GameObject laserPrefab;
	float gunShootCost = 0.01f;

	Movement mov;
	Health health;
	Charge charge;
	InteractionController ic;

	public bool isDead {
		get {
			return health?.isDead ?? false;
		}
	}

	void Awake() {
		GameManager.pc = this;
	}

	void Start() {
		mov = GetComponent<Movement>();
		
		health = GetComponent<Health>();
		health.OnDeath += () => {
			Debug.Log("the player died!");
			ic?.LetGo();
			Destroy(gameObject);
			GameManager.GameOver();
		};

		charge = GetComponent<Charge>();

		ic = GetComponentInChildren<InteractionController>();
		ic.OnGrab = (Grabbable gr) => {
			gun.SetActive(false);
		};
		ic.OnLetGo = (Grabbable gr) => {
			gun.SetActive(true);
		};
	}

	void Shoot() {
		if (!gun.activeInHierarchy) {
			return;
		}
		if (charge.charge > gunShootCost) {
			charge.TakeCharge(gunShootCost);
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
