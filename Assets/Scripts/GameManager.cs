using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	static GameManager gm;

	public static float time = 0;
	public static bool isGameOver = false;
	public static bool isGameWon = false;
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
		time = 0;
		isGameOver = false;
		isGameWon = false;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void GameOver() {
		isGameOver = true;
	}

	public static void Win() {
		isGameWon = true;
	}

	void OnGUI() {
		GUI.Label(new Rect(10, 10, 100, 50), time.ToString());
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			Reset();
		}

		if (!isGameOver || isGameWon) {
			time += Time.deltaTime;
		}
	}
}
