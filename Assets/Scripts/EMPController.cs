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

	bool hasExploded = false;

	void Start () {
		energySprite = energyGraphic.GetComponent<SpriteRenderer>();

		health = GetComponent<Health>();
		health.OnDeath += () => {
			Debug.Log("emp destroyed!");
			Destroy(gameObject);
		};

		charge = GetComponent<Charge>();
	}

	void ResetGraphic() {
		energyGraphic.localScale = 0.01f * Vector3.one;
	}

	IEnumerator Explode() {
		hasExploded = true;
		Debug.Log("the emp went off!");

		GameManager.Win();
		RobotSpawner.KillAll();

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
		if (hasExploded) {
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
