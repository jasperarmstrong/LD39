using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	static GameManager gm;

	public static bool isGameOver = false;
	public static PlayerController pc;

	void Awake() {
		if (gm == null) {
			gm = this;
			DontDestroyOnLoad(gameObject);
		} else {
			DestroyImmediate(gameObject);
		}
	}

	void Reset() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void GameOver() {
		isGameOver = true;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			Reset();
		}
	}
}
