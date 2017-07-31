using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipDoor : Tip {
	ExitController exit;

	void Start() {
		exit = GetComponent<ExitController>();
	}

	public override string GetTip() {
		if (exit.isUnlocked) {
			return $"The door is unlocked! Press {GameManager.interactKeyString} to escape!";
		} else {
			return "The door is locked. To open it, charge the EMP with the batteries.";
		}
	}
}
