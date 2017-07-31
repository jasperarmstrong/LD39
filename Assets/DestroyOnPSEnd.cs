﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnPSEnd : MonoBehaviour {
	ParticleSystem ps;
	
	void Start () {
		ps = GetComponent<ParticleSystem>();
	}

	void Update () {
		if (!ps.isPlaying) {
			Destroy(gameObject);
		}
	}
}
