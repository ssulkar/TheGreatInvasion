using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public float muzzleVelocity;
	public float recoilPattern;

	private Rigidbody2D projectileRb;

	// Use this for initialization
	/*void Awake () {
		projectileRb = GetComponent<Rigidbody2D> ();

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane xy = new Plane (Vector3.forward, new Vector3(0,0,0));
		float distance;
		xy.Raycast (ray, out distance);
		Vector3 worldMousePos = ray.GetPoint (distance);
		Vector2 direction = (Vector2)(worldMousePos - transform.position);
		direction.Normalize ();
		projectileRb.AddForce (new Vector2 (direction.x, direction.y+Random.Range(-(recoilPattern/2), recoilPattern/2)) * muzzleVelocity, ForceMode2D.Impulse);
		transform.rotation = Quaternion.LookRotation(direction);
	}*/

	void Awake () {
		projectileRb = GetComponent<Rigidbody2D>();
		if (transform.localRotation.z > 0) {
			projectileRb.AddForce (new Vector2 (-1, Random.Range(-(recoilPattern/2), recoilPattern/2)) * muzzleVelocity, ForceMode2D.Impulse);
		} else {
			projectileRb.AddForce (new Vector2 (1, Random.Range(-(recoilPattern/2), recoilPattern/2)) * muzzleVelocity, ForceMode2D.Impulse);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void stopProjectile(){
		projectileRb.velocity = new Vector2(0, 0);
	}
}
