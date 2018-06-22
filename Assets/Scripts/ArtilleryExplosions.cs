using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryExplosions : MonoBehaviour {

	public GameObject explosion;
	public GameObject explosive;
	// Use this for initialization
	void Start () {
		Instantiate (explosion, transform.position, Quaternion.identity);
		Instantiate (explosion, transform.position, Quaternion.identity);
		Instantiate (explosion, transform.position, Quaternion.identity);
		Instantiate (explosion, transform.position, Quaternion.identity);


		Vector3 first = new Vector3(transform.position.x-3f, transform.position.y);
		Vector3 second = new Vector3(transform.position.x-2f, transform.position.y);
		Vector3 third = new Vector3(transform.position.x+2f, transform.position.y);
		Vector3 fourth = new Vector3(transform.position.x+3f, transform.position.y);

		Instantiate (explosive, transform.position, Quaternion.identity);
		Instantiate (explosive, transform.position, Quaternion.identity);
		Instantiate (explosive, transform.position, Quaternion.identity);
		Instantiate (explosive, transform.position, Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Enemy") {
			Instantiate (explosion, transform.position, Quaternion.identity);
		}
	}
}
