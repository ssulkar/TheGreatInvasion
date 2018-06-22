using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour {
	
	public float damage;
	public GameObject explosionParticle;
	public string message;

	private ProjectileController projectileController;
	private ProjectileDestroyer projectileDestroyer;
	//private bool hasCollided;


	void Awake () {
		//get parent controller
		projectileController = GetComponentInParent<ProjectileController> ();
		projectileDestroyer = GetComponentInParent<ProjectileDestroyer> ();
		print(message);
		//hasCollided = false;
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other){
		//when it collides with the touchable layer it does the following

		
		if (other.gameObject.layer == LayerMask.NameToLayer ("Touchable")) {
			projectileController.stopProjectile ();//stops the projectile in place
			Instantiate (explosionParticle, transform.position, transform.rotation);
			Destroy (gameObject); //destroys the actual disc
			projectileDestroyer.destroyNow (); //destroys the entire projectile object including particles
		
			if (other.tag == "Enemy") {
				EnemyHealth health = other.gameObject.GetComponent<EnemyHealth> ();
				health.addDamage (damage);
			}
		}
	}

	/*void OnTriggerExit2D (Collider2D other){
		//when it collides with the touchable layer it does the following
		if (hasCollided == false) {
			if (other.gameObject.layer == LayerMask.NameToLayer ("Touchable")) {
				projectileController.stopProjectile ();//stops the projectile in place
				Instantiate (explosionParticle, transform.position, transform.rotation);
				Destroy (gameObject); //destroys the actual disc
				projectileDestroyer.destroyNow (); //destroys the entire projectile object including particles

				if (other.tag == "Enemy") {
					EnemyHealth health = other.gameObject.GetComponent<EnemyHealth> ();
					health.addDamage (damage);
				}
				hasCollided = true;
			}
		}
	}*/

	/*if (other.gameObject.CompareTag ("CriticalSpot")) {
				damage = damage * 10;
				print (damage);
			}*/

	//just a safe guard in case the previous method doesn't work
	/*void OnTriggerStay2D (Collider2D other){
		//when it collides with the touchable layer it does the following
		if (other.gameObject.layer == LayerMask.NameToLayer ("Touchable")) {
			projectileController.stopProjectile ();//stops the projectile in place
			Instantiate (explosionParticle, transform.position, transform.rotation);
			Destroy (gameObject); //destroys the actual disc
			projectileDestroyer.destroyNow (); //destroys the entire projectile object including particles


			if (other.tag == "Enemy") {
				EnemyHealth health = other.gameObject.GetComponent<EnemyHealth> ();
				health.addDamage (damage);
			}
		}
	}*/
}
