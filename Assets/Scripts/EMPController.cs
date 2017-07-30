using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPController : MonoBehaviour {
	Health health;

	void Start () {
		health = GetComponent<Health>();
		health.OnDeath += () => {
			Debug.Log("emp destroyed!");
			Destroy(gameObject);
		};
	}
	
	void Update () {
		
	}
}
