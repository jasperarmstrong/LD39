using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager gm;

	public GameObject cheaterUI;
	[SerializeField] GameObject timeUI;
	[SerializeField] GameObject killsUI;
	[SerializeField] GameObject tipUI;
	[SerializeField] GameObject youWinUI;
	[SerializeField] GameObject gameOverUI;
	[SerializeField] GameObject menuUI;
	[SerializeField] GameObject startButtonUI;
	[SerializeField] GameObject restartButtonUI;
	[SerializeField] Text timeText;
	[SerializeField] Text bestTimeText;
	[SerializeField] Text killsText;
	[SerializeField] Text mostKillsText;
	public Text tipText;

	public static bool isPaused = true;
	static bool hasStarted = false;

	public static float time = 0;
	public static float bestTime;
	public static int kills = 0;
	public static int mostKills;
	public static bool isGameOver = false;
	public static bool isGameWon = false;
	public static bool areTipsLocked = false;
	public static PlayerController pc;
	public static EMPController emp;

	public static float sfxVolume;

	public static string interactKeyString = "\"E\" or Right-Click";

	void Awake() {
		if (gm == null) {
			gm = this;
			DontDestroyOnLoad(gameObject);
			
			Pause();

			bestTime = PlayerPrefs.GetFloat("bestTime", 9999.99f);
			mostKills = PlayerPrefs.GetInt("mostKills", 0);
			UpdateBestTimeUI();
			UpdateMostKillsUI();

			if (PlayerPrefs.GetInt("isCheater", 0) == 1) {
				cheaterUI.SetActive(true);
			}

			InitialTip();
		} else {
			DestroyImmediate(gameObject);
		}
	}

	public void Quit() {
		Application.Quit();
	}

	public void TogglePause() {
		if (isPaused) {
			Unpause();
		} else {
			Pause();
		}
	}

	static void Pause() {
		isPaused = true;
		Time.timeScale = 0;
		gm.menuUI.SetActive(true);
	}

	static void Unpause() {
		if (!hasStarted) {
			gm.timeUI.SetActive(true);
			gm.killsUI.SetActive(true);
			gm.tipUI.SetActive(true);
			gm.restartButtonUI.SetActive(true);
			gm.startButtonUI.GetComponentInChildren<Text>().text = "Resume Game";
		}
		isPaused = false;
		Time.timeScale = 1;
		gm.menuUI.SetActive(false);
	}

	void InitialTip() {
		GiveTip($"Robots are attacking and the escape door is locked! Charge up the EMP with the batteries in order to kill all the robots and unlock the door! Make sure not to let the robots do too much damage to you or the EMP by killing them with your laser gun. WASD or Arrow Keys to move, point and click to shoot, {interactKeyString} to interact with things.");
	}

	public void Reset() {
		time = 0;
		kills = 0;
		UpdateKillsUI();
		isGameOver = false;
		isGameWon = false;
		areTipsLocked = false;
		InitialTip();
		gm.gameOverUI.SetActive(false);
		gm.youWinUI.SetActive(false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void ImACheater() {
		GameManager.gm.cheaterUI.SetActive(true);
		PlayerPrefs.SetInt("isCheater", 1);
	}

	static void CheckMostKills() {
		if (kills > mostKills) {
			mostKills = kills;
			PlayerPrefs.SetInt("mostKills", mostKills);
			UpdateMostKillsUI();
		}
	}

	public static void GameOver() {
		isGameOver = true;
		gm.gameOverUI.SetActive(true);
		GiveTip("Press \"R\" to restart, or \"Escape\" for the menu.");
		CheckMostKills();
	}

	public static void Win() {
		isGameWon = true;
		gm.youWinUI.SetActive(true);
		if (time < bestTime) {
			bestTime = time;
			PlayerPrefs.SetFloat("bestTime", bestTime);
			UpdateBestTimeUI();
		}
		GiveTip("Press \"R\" to restart, or \"Escape\" for the menu.");
		CheckMostKills();
	}

	static void UpdateBestTimeUI() {
		gm.bestTimeText.text = $"Best Time: {bestTime.ToString("N2")}s";
	}

	static void UpdateMostKillsUI() {
		gm.mostKillsText.text = $"Most Kills: {mostKills}";
	}

	public static void UpdateKillsUI() {
		gm.killsText.text = $"Kills: {kills}";
	}

	public static void GiveTip(string tip, bool	shouldLock = false) {
		if (!areTipsLocked) {
			gm.tipText.text = tip;
		}
		if (shouldLock) {
			areTipsLocked = true;
		}
	}

	void Update () {
		if ((isGameOver || isGameWon) && Input.GetKeyDown(KeyCode.R)) {
			Reset();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			TogglePause();
		}

		if (isPaused) {
			return;
		}

		if (!isGameOver && !isGameWon) {
			time += Time.deltaTime;
			timeText.text = $"Time: {time.ToString("N2")}s";
		}
	}
}
