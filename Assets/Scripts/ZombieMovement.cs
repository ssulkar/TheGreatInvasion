using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour {

	public Transform[] positions;
	public Transform sightPosition;
	public float speed;
	public float idleTime;
	public float sight;
	public float chaseSpeed;
	public float chaseCoolDown;

	private Rigidbody2D rb;
	private int currentPosition;


	private bool hitPlayer = false;
	private bool foundPlayer = false;
	//private bool idle = false;



	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		StartCoroutine ("Patrol");
	}

	void Update(){
		RaycastHit2D hit = Physics2D.Raycast (sightPosition.position, transform.localScale.x * Vector2.right, sight);

		if (hitPlayer == false && hit.collider != null && hit.collider.CompareTag ("Player")) {
			foundPlayer = true;
			transform.position = Vector2.MoveTowards (transform.position, new Vector2 (hit.collider.transform.position.x, transform.position.y), chaseSpeed);
		} else {
			foundPlayer = false;
		}
	}

	IEnumerator Patrol() {
		//int currentPosition = 0;
		currentPosition = 0;
		while (true) {
			if (transform.position.x == positions [currentPosition].position.x) {
				currentPosition++;

				yield return new WaitForSeconds (idleTime);

			}
			if (currentPosition >= positions.Length) {
				currentPosition = 0;
			}
			
			transform.position = Vector2.MoveTowards (transform.position, new Vector2 (positions [currentPosition].position.x, transform.position.y), speed);

			if (transform.position.x > positions[currentPosition].position.x) {
				transform.localScale = new Vector3 (-1,1,1);
			} else if (transform.position.x < positions[currentPosition].position.x){
				transform.localScale = new Vector3 (1,1,1);
			}
				
			yield return null;
		}
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawLine (transform.position, transform.position + transform.localScale.x * Vector3.right * sight);
	}

	IEnumerator OnTriggerEnter2D(Collider2D other){
		
		if (other.CompareTag ("Player")) {
			hitPlayer = true;
			yield return new WaitForSeconds (chaseCoolDown);
			hitPlayer = false;
		}

		if (other.CompareTag("Bullet") && !foundPlayer) {
			Vector3 dir = other.transform.position - transform.position;
			if(dir.x > 0){
				currentPosition = 0;
			}
			else if (dir.x < 0){
				currentPosition = 1;
			}

		}
		// 0 right
		// 1 left
		/*

		if (other.CompareTag ("Bullet") && foundPlayer == false) {

			idle = true;
			facingRight = !facingRight;
			Vector3 enemyScale = transform.localScale;
			enemyScale.x *= -1;
			transform.localScale = enemyScale;
			RaycastHit2D hit;
			if (facingRight) {
				hit = Physics2D.Raycast (sightPosition.position, transform.localScale.x * Vector2.right, sight);
			} else {
				hit = Physics2D.Raycast (sightPosition.position, transform.localScale.x * Vector2.left, sight);
			}
			if (hit.collider != null && hit.collider.CompareTag ("Player")) {
				foundPlayer = true;
				if (currentPosition == 0) {
					currentPosition = 1;
				} else {
					currentPosition = 0;
				}
			}
			yield return new WaitForSeconds (2);
			if (!foundPlayer){
				facingRight = !facingRight;
				enemyScale.x *= -1;
				transform.localScale = enemyScale;
			}
			idle = false;
		}
		yield return null;*/
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.gameObject.tag == "Enemy") {
			Collider2D[] colliders = GetComponents<Collider2D> ();
			Collider2D[] otherColliders = collision.gameObject.GetComponents<Collider2D> ();
			foreach (Collider2D c1 in colliders) {
				foreach (Collider2D c2 in otherColliders) {
					Physics2D.IgnoreCollision (c1, c2);
				}
			}
		}
	}


	void switchPosition(){
		if (currentPosition == 0) {
			currentPosition = 1;
		} else {
			currentPosition = 0;
		}
	}

		

}
