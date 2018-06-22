using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTimer : MonoBehaviour {

	public float lifeSpan;

	// Use this for initialization
	void Awake () {
		Destroy (gameObject, lifeSpan);
	}

	public void destroyNow(){
		Destroy (this.gameObject);
	}
}
