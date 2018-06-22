using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnContact : MonoBehaviour {

	public GameObject explosion;
	public float maxDamage;
	public float pushBackMultiplier;
	public bool destroyOnContact;
	public bool hostile;
	public bool explodeInstant;
	public float damageRate;
	private float coolDown;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (explodeInstant) {
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (coolDown < Time.time) {
			if (!hostile && other.tag == "Enemy") {
				coolDown = Time.time + damageRate;
				EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth> ();
				enemyHealth.addDamage (maxDamage);
				pushBack (other);
				Instantiate (explosion, transform.position, Quaternion.identity);
				if (destroyOnContact) {
					Destroy (this.gameObject);
				}
			}
			else if (hostile && other.tag == "Player") {
				coolDown = Time.time + damageRate;
				PlayerController playerController = other.gameObject.GetComponent<PlayerController> ();
				//Health.addDamage (maxDamage);
				playerController.addDamage(maxDamage);
				pushBack (other);
				Instantiate (explosion, transform.position, Quaternion.identity);
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
