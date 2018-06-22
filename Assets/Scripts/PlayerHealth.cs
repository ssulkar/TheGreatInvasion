using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

	public float playerMaxHealth;
	private float currentHealth;

	private float invincibilityRate = 7f;
	private float invincibilityCooldown = 0f;

	void Start (){
		currentHealth = playerMaxHealth;
		//ic = GetComponent<IgnoreCollisions> ();
	}

	void Update() {
		/*if (!(invincibilityCooldown < Time.time)) {
			ic.enabled = true;
		}*/
	}

	void FixedUpdate() {
		if (transform.position.y < -10) {
			playerDeath ();
		}
	}

	public void addDamage(float damage){
		if(invincibilityCooldown > Time.time){
			invincibilityCooldown = Time.time + invincibilityRate;
			currentHealth -= damage;
			if (currentHealth <= 0) {
				playerDeath ();
			}
		}
		/*
		currentHealth -= damage;
		if (currentHealth <= 0) {
			playerDeath ();
		}*/
	}

	void playerDeath () {
		SceneManager.LoadScene("Main");
	}
}
