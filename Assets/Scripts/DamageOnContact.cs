using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour {

	public float damage, damageRate, pushBackMultiplier, criticalMultplier;
	//destroy if anything here is hit
	public bool destroyOnContact, ignorePlayer, ignoreEnemy, ignoreTouchable;
	private float coolDown;
	private float maxDamage;

	// Use this for initialization
	void Start () {
		coolDown = 0f;
		maxDamage = damage * criticalMultplier;
	}

	// Update is called once per frame
	void Update () {

	}

	public void applyCriticalMultiplier(){
		if (damage < maxDamage) {
			damage = damage * criticalMultplier;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("CriticalSpot")) {
			applyCriticalMultiplier ();
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		//print (damage);
		if (coolDown < Time.time) {
			if (other.tag == "Player" && !ignorePlayer) {
				PlayerController playerController = other.gameObject.GetComponent<PlayerController> ();
				playerController.addDamage (damage);
				coolDown = Time.time + damageRate;
				pushBack (other);
				if (destroyOnContact) {
					Destroy (this.gameObject);
				}
			} else if (other.tag == "Enemy" && !ignoreEnemy) {
				EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth> ();
				enemyHealth.addDamage (damage);
				coolDown = Time.time + damageRate;
				pushBack (other); 

				if (destroyOnContact) {
					Destroy (this.gameObject);
				}
			} else if (other.gameObject.layer == LayerMask.NameToLayer ("Touchable") && !ignoreTouchable) {
				if (destroyOnContact) {
					Destroy (this.gameObject);
				}
			}
		}
	}

	void pushBack(Collider2D other){
		other.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		Vector2 direction = (other.transform.position - gameObject.transform.position).normalized; 
		other.gameObject.GetComponent<Rigidbody2D> ().AddForce (direction * pushBackMultiplier, ForceMode2D.Impulse);
	}

}
