using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour {
	[System.Serializable]
	public class Cheat {
		public string name;
		public string code;
		public Action method;
	}
	
	[SerializeField] Cheat[] cheats;
	string inProgressCheat = "";

	float numSecondsReset = 0.5f;
	float resetTime = 1;

	void CheckCheat() {
		if (inProgressCheat != "") {
			bool foundCandidate = false;
			foreach (Cheat c in cheats) {
				if (c.code == inProgressCheat) {
					Debug.Log("found cheat!");
					inProgressCheat = "";
					c.method?.Invoke();
					GameManager.ImACheater();
					return;
				} else if (c.code.StartsWith(inProgressCheat)) {
					foundCandidate = true;
				}
			}
			if (!foundCandidate) {
				inProgressCheat = "";
			}
		}
	}

	void ResetTimer() {
		resetTime = numSecondsReset;
	}

	void Start() {
		foreach (Cheat c in cheats) {
			switch (c.name) {
				case "Invincible":
					c.method = () => {
						if (GameManager.pc != null) {
							GameManager.pc.GetComponent<Health>().isInvincible = true;
						}
						if (GameManager.emp != null) {
							GameManager.emp.GetComponent<Health>().isInvincible = true;
						}
						string msg = "Activated invincibility cheat!";
						Debug.Log(msg);
						GameManager.GiveTip(msg);
					};
					break;
				case "Win":
					c.method = () => {
						GameManager.emp.GetComponent<Charge>().GiveCharge(1000);
						GameManager.GiveTip("Activated win cheat!");
					};
					break;
				case "NoRobots":
					c.method = () => {
						RobotSpawner.rs.shouldSpawn = false;
						GameManager.GiveTip("Activated no more robots cheat!");
					};
					break;
				case "KillMe":
					c.method = () => {
						GameManager.pc.GetComponent<Health>().Damage(DamageType.INSTAKILL);
						GameManager.GiveTip("Killed player!");
					};
					break;
				case "BlowUp":
					c.method = () => {
						GameManager.emp.GetComponent<Health>().Damage(DamageType.INSTAKILL);
						GameManager.GiveTip("Killed emp!");
					};
					break;
				default:
					Debug.Log($"Unimplemented cheat: {c.name}");
					break;
			}
		}
	}

	void Update () {
		if (GameManager.isPaused) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			inProgressCheat += "UP";
			ResetTimer();
		} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			inProgressCheat += "RIGHT";
			ResetTimer();
		} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			inProgressCheat += "DOWN";
			ResetTimer();
		} else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			inProgressCheat += "LEFT";
			ResetTimer();
		} else if (Input.GetKeyDown(KeyCode.Return)) {
			inProgressCheat += "RETURN";
			ResetTimer();
		} else {
			bool hitAGoodKey = true;
			switch (Input.inputString) {
				case "":
					hitAGoodKey = false;
					break;
				default:
					inProgressCheat += Input.inputString.ToUpper();
					break;
			}
			if (hitAGoodKey) {
				ResetTimer();
			} else if (inProgressCheat != "") {
				resetTime -= Time.deltaTime;
			}
		}

		if (resetTime <= 0) {
			ResetTimer();
			inProgressCheat = "";
		}
		CheckCheat();
	}
}
