using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
	public Action OnInteract;

	public void Interact() {
		OnInteract?.Invoke();
	}
}
