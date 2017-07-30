using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	public static Vector3 MousePosInWorld() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		
		return mousePos;
	}
}
