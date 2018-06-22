using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzeDetonate : MonoBehaviour {

	public GameObject[] sticks;
	public GameObject explosion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			detonate ();
		}
	}

	void detonate(){
		for (int i = 0; i < sticks.Length; i++) {
			Instantiate (explosion, sticks[i].transform.position, Quaternion.identity);
		}
		if (transform.parent != null) {
			Destroy (transform.parent.gameObject);
		} else {
			Destroy (gameObject);
		}
	}
}
