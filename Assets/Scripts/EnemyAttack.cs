using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

	public float damage;
	public float coolDown;
	public float pushBackForce;

	private float currentCoolDown;

	// Use this for initialization
	void Start () {
		currentCoolDown = 0f;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.tag == "Player" && currentCoolDown < Time.time) {
			PlayerController health = other.gameObject.GetComponent<PlayerController> ();
			health.addDamage (damage);
			currentCoolDown = Time.time + coolDown;

			pushBack(other);

		}
	}

	void pushBack(Collider2D other){
		other.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		Vector2 direction = (other.transform.position - gameObject.transform.position).normalized; 
		other.gameObject.GetComponent<Rigidbody2D> ().AddForce (direction * pushBackForce, ForceMode2D.Impulse);
	}
}
