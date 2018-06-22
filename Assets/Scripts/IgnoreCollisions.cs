using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour {

	public bool ignorePlayer, 
	            ignoreEnemy;
	              

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(ignorePlayer == true && collision.gameObject.tag == "Player"){
			ignoreCollision (collision.gameObject);
		}
		else if(ignoreEnemy == true && collision.gameObject.tag == "Enemy"){
			ignoreCollision (collision.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(ignorePlayer == true && collision.gameObject.tag == "Player"){
			ignoreCollision (collision.gameObject);
		}
		else if(ignoreEnemy == true && collision.gameObject.tag == "Enemy"){
			ignoreCollision (collision.gameObject);
		}
	}

	void ignoreCollision(GameObject other){
		Collider2D[] colliders = GetComponents<Collider2D> ();
		Collider2D[] otherColliders = other.GetComponents<Collider2D> ();
		foreach (Collider2D c1 in colliders) {
			foreach (Collider2D c2 in otherColliders) {
				Physics2D.IgnoreCollision (c1, c2);
			}
		}
	}
}
