using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float enemyMaxHealth;
	public GameObject Blood;
	[HideInInspector]
	public float currentHealth;

	// Use this for initialization
	void Start () {
		currentHealth = enemyMaxHealth;
	}

	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		if (transform.position.y < -50) {
			enemyDeath ();
		}
	}

	public void addDamage(float damage){
		currentHealth -= damage;
		Instantiate (Blood, transform.position, transform.rotation);
		if (currentHealth <= 0) {
			enemyDeath ();
		}
	}

	void enemyDeath () {
		//Destroy (gameObject);
		//Instantiate (Blood, transform.position, transform.rotation);
		if (transform.parent != null) {
			Destroy (transform.parent.gameObject);
		} else {
			Destroy (gameObject);
		}
	}
}
