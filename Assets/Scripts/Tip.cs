using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour {
	public string tip = "";
	
	public virtual string GetTip() {
		return tip;
	}
}
