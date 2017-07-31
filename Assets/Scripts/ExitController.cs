using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {
	[SerializeField] GameObject locked;
	[SerializeField] GameObject unlocked;

	[SerializeField] Transform[] path;

	public bool isUnlocked = false;

	Interactable interactable;
	SpriteRenderer sr;

	[SerializeField] Sprite[] sprites;
	float animSpeed = 0.15f;
	float walkSpeed = 1.4f;

	IEnumerator AnimateEnd() {
		foreach (Sprite s in sprites) {
			sr.sprite = s;
			yield return new WaitForSeconds(animSpeed);
		}
		Rigidbody2D rb = GameManager.pc.GetComponent<Rigidbody2D>();
		if (rb != null) {
			rb.simulated = false;
		}

		foreach (Transform target in path) {
			Vector3 targetPos = target.position;
			while (Vector3.Distance(GameManager.pc.transform.position, targetPos) > 0.1f) {
				GameManager.pc.transform.position = Vector3.Lerp(GameManager.pc.transform.position, targetPos, walkSpeed * Time.deltaTime);
				yield return null;
			}
		}

		Destroy(GameManager.pc.gameObject);
	}

	void Start() {
		interactable = GetComponent<Interactable>();
		interactable.OnInteract += TryOpen;

		sr = GetComponent<SpriteRenderer>();
	}

	public void Unlock() {
		isUnlocked = true;
		locked.SetActive(false);
		unlocked.SetActive(true);
	}

	public void TryOpen() {
		if (isUnlocked) {
			GameManager.Win();
			unlocked.SetActive(false);

			List<SpriteRenderer> pcSprites = new List<SpriteRenderer>();
			pcSprites.Add(GameManager.pc.GetComponent<SpriteRenderer>());
			foreach(SpriteRenderer sr in GameManager.pc.GetComponentsInChildren<SpriteRenderer>()) {
				pcSprites.Add(sr);
			}
			foreach(SpriteRenderer sr in pcSprites) {
				sr.sortingLayerName = "Background";
				sr.sortingOrder = -1;
			}

			foreach(Canvas c in GameManager.pc.GetComponentsInChildren<Canvas>()) {
				c.gameObject.SetActive(false);
			}

			StartCoroutine(AnimateEnd());
		}
	}
}
