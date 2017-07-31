using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPController : MonoBehaviour {
	[SerializeField] Transform energyGraphic;
	SpriteRenderer energySprite;

	Health health;
	Charge charge;

	float explosionRadius = 64;
	float explosionTime = 0.4f;

	[SerializeField] GameObject explosionPrefab;

	[SerializeField] AudioClip explodeSound;
	[SerializeField] float explodeVolume = 1f;
	[SerializeField] AudioClip hurtSound;
	[SerializeField] float hurtVolume = 1f;
	[SerializeField] AudioClip explosionSound;
	[SerializeField] float explosionVolume = 1f;

	public bool hasExploded = false;

	void Start () {
		energySprite = energyGraphic.GetComponent<SpriteRenderer>();

		health = GetComponent<Health>();
		health.OnChange += (DamageType dt) => {
			if (dt != DamageType.NONE) {
				AudioSource.PlayClipAtPoint(hurtSound, transform.position, hurtVolume * GameManager.sfxVolume);
			}
		};
		health.OnDeath += () => {
			Debug.Log("emp destroyed!");
			GameManager.GameOver();
			Destroy(gameObject);
			AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionVolume * GameManager.sfxVolume);
			Instantiate(explosionPrefab, transform.position, transform.rotation);
		};

		charge = GetComponent<Charge>();
	}

	void Awake() {
		GameManager.emp = this;
	}

	void ResetGraphic() {
		energyGraphic.localScale = 0.01f * Vector3.one;
	}

	IEnumerator Explode() {
		hasExploded = true;
		GameObject.FindGameObjectWithTag("Exit")?.GetComponent<ExitController>().Unlock();
		Debug.Log("the emp went off!");

		GameManager.GiveTip("The door has been unlocked, all the robots have been killed, and no more robots will come! Go through the door to escape!");

		RobotSpawner.KillAll();
		RobotSpawner.rs.shouldSpawn = false;

		AudioSource.PlayClipAtPoint(explodeSound, transform.position, explodeVolume * GameManager.sfxVolume);

		float time = 0;
		while (time < explosionTime) {
			float timePercent = time / explosionTime;

			energyGraphic.localScale = Vector3.Lerp(Vector3.one, Vector3.one * explosionRadius, timePercent);

			Color newColor = energySprite.color;
			newColor.a = Mathf.Lerp(1, 0, timePercent);
			energySprite.color = newColor;

			time += Time.deltaTime;
			yield return null;
		}

		ResetGraphic();
	}
	
	void Update () {
		if (hasExploded || GameManager.isPaused) {
			return;
		}

		float chargePercent = charge.ChargePercent();

		if (chargePercent > 0.98f) {
			StartCoroutine(Explode());
		} else {
			energyGraphic.localScale = chargePercent * 0.91f * Vector3.one;

			Color newColor = energySprite.color;
			newColor.a = Mathf.Lerp(0.5f, 1f, chargePercent);
			energySprite.color = newColor;
		}
	}
}
