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
	
	[SerializeField] AudioClip laserShootSound;
	[SerializeField] float laserShootVolume = 1f;
	[SerializeField] AudioClip hurtSound;
	[SerializeField] float hurtVolume = 1f;
	[SerializeField] AudioClip deathSound;
	[SerializeField] float deathVolume = 1f;
	[SerializeField] AudioClip pickupSound;
	[SerializeField] float pickupVolume = 1f;
	[SerializeField] AudioClip dropSound;
	[SerializeField] float dropVolume = 1f;

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
			AudioSource.PlayClipAtPoint(deathSound, transform.position, deathVolume * GameManager.sfxVolume);
		};
		health.OnChange += (DamageType dt) => {
			if (dt == DamageType.NONE) {

			} else {
				AudioSource.PlayClipAtPoint(hurtSound, transform.position, hurtVolume * GameManager.sfxVolume);
			}
		};

		charge = GetComponent<Charge>();

		ic = GetComponentInChildren<InteractionController>();
		ic.OnGrab = (Grabbable gr) => {
			gun.SetActive(false);
			AudioSource.PlayClipAtPoint(pickupSound, transform.position, laserShootVolume * GameManager.sfxVolume);
		};
		ic.OnLetGo = (Grabbable gr) => {
			gun.SetActive(true);
			AudioSource.PlayClipAtPoint(dropSound, transform.position, laserShootVolume * GameManager.sfxVolume);
		};
	}

	void Shoot() {
		if (!gun.activeInHierarchy) {
			return;
		}
		if (charge.charge > gunShootCost) {
			charge.TakeCharge(gunShootCost);
			AudioSource.PlayClipAtPoint(laserShootSound, gunShootSpot.position, laserShootVolume * GameManager.sfxVolume);
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
		if (health.isDead || GameManager.isGameOver || GameManager.isPaused) {
			return;
		}

		mov.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		FaceMouse();
		DoGunStuff();
	}
}
