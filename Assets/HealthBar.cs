using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
	public enum UIScaleVar {
		X, Y
	}
	
	[SerializeField] Health health;
	[SerializeField] UIScaleVar uiScaleVar;
	
	RectTransform rt;

	void Start () {
		rt = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();

		health.OnDamage += (DamageType dt) => {
			switch (uiScaleVar) {
				case UIScaleVar.X:
					rt.localScale = new Vector3(health.health, 1, 1);
					break;
				case UIScaleVar.Y:
					rt.localScale = new Vector3(1, health.health, 1);
					break;
				default:
					break;
			}
		};
	}
	
	void Update () {
		
	}
}
