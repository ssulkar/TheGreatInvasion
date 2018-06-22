using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammerMovement : MonoBehaviour {
	
	public Transform[] positions;
	public Transform[] barrels;
	public float speed;
	public GameObject bullet;

	private GameObject player;
	private Vector3 playerPosition;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		StartCoroutine ("ramPattern");
	}

	IEnumerator ramPattern(){
		while (true) {
			//move
			while (transform.position.x != positions [0].position.x) {
				Vector2 nextPosition = new Vector2 (positions [0].position.x, transform.position.y);
				transform.position = Vector2.MoveTowards (transform.position, nextPosition, speed);
				yield return null;
			}
			transform.localScale = new Vector2 (-1, 1);
			yield return new WaitForSeconds (1f);


			//shoot
			int i = 0;
			while (i < 4) {
				GameObject liveBullet = (GameObject)Instantiate (bullet, barrels [Random.Range (0, barrels.Length)].position, Quaternion.identity);
				liveBullet.GetComponent<Rigidbody2D> ().velocity = Vector2.left * 7;
				i++;
				yield return new WaitForSeconds (0.5f);
			}
			yield return new WaitForSeconds (1.5f);
			//smash
			GetComponent<Rigidbody2D> ().isKinematic = true;
			while (transform.position.x != positions [1].position.x) {
				transform.position = Vector2.MoveTowards (transform.position, positions [1].position, speed);
				yield return null;
			}
			playerPosition = player.transform.position;
			if (Vector2.Distance (playerPosition, transform.position) > 5) {
				playerPosition = positions [2].position;
			}

			yield return new WaitForSeconds (.5f);
			GetComponent<Rigidbody2D> ().isKinematic = false;
			while (transform.position.x != playerPosition.x) {
				transform.position = Vector2.MoveTowards (transform.position, playerPosition, speed);
				yield return null;
			}
			yield return new WaitForSeconds (2f);
			//ram
			Transform ram;
			if (transform.position.x > player.transform.position.x) {
				ram = positions [2];
			} else {
				ram = positions [0];
			}

			while (transform.position.x != ram.position.x) {
				Vector2 resetPosition = new Vector2 (ram.position.x, transform.position.y);
				transform.position = Vector2.MoveTowards (transform.position, resetPosition, speed);
				yield return null;
			}
		}
	}
}
