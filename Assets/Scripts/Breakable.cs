using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {

	public bool destroyedByRifle;
	public bool destroyedBySmg;
	public bool destroyedByShotgun;
	public bool destroyedBySlr;
	public float strength;
	private float currentStrength;
	// Use this for initialization
	void Start () {
		currentStrength = strength;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentStrength <= 0) {
			Destroy (this.gameObject);
		}
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("RifleBullet") && destroyedByRifle == true) {
			currentStrength--;
		}
		else if (other.CompareTag("SmgBullet") && destroyedBySmg == true) {
			currentStrength--;
		}
		else if (other.CompareTag("ShotgunBullet") && destroyedByShotgun == true) {
			currentStrength--;
		}
		else if (other.CompareTag("SlrBullet") && destroyedBySlr == true) {
			currentStrength--;
		}
	}
}
