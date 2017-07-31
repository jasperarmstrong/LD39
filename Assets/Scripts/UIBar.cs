using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBar : MonoBehaviour {
	protected RectTransform rt;

	protected virtual void BaseStart () {
		rt = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
	}
}
