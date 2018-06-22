using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploderMovement : MonoBehaviour {
	public Transform sightPosition;
	public Transform rearSightPosition;
	public float speed;
	public float idleTime;
	public float sight;
	public float chaseSpeed;
	public float chaseCoolDown;

	private Rigidbody2D rb;
	private int currentPosition;


	private bool hitPlayer = false;
	private bool foundPlayer = false;


	//coroutine checks
	private bool patrolling = true;
	private bool fighting = false;

	private float shiftRate = 3f;
	private float shiftCooldown = 0;




	//patrol

	//Animations
	private Animator anim;

	//private bool chasing;

	public GameObject enemyExplosion;


	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		currentPosition = 0;

		anim = GetComponent<Animator> ();

		StartCoroutine ("Patrol");
	}

	void Update(){
		if (fighting == false) {

			RaycastHit2D front = Physics2D.Raycast (transform.position, transform.localScale.x * Vector2.right, sight, ~(1 << LayerMask.NameToLayer ("Enemy")));
			RaycastHit2D back = Physics2D.Raycast (transform.position, transform.localScale.x * Vector2.left, sight, ~(1 << LayerMask.NameToLayer ("Enemy")));


			if (patrolling) {
				if (front.collider != null && front.collider.CompareTag ("Player")) {
					patrolling = false;
					StopCoroutine ("Patrol");
				}
				shiftCooldown = Time.time + shiftRate;
			} else if (!patrolling) {
				if (front.collider != null && front.collider.CompareTag ("Player")) {
					if (front.distance > sightPosition.localPosition.x + 0.5f) {
						if (fighting == true) {
							//fighting = false;
							//StopCoroutine ("Fight");
						}

						RaycastHit2D frontDown = Physics2D.Raycast (sightPosition.position, new Vector2 (0, -1), 1.5f, ~((1 << LayerMask.NameToLayer ("Enemy")) | (1 << LayerMask.NameToLayer ("Player"))));
						if (frontDown.collider != null && frontDown.collider.IsTouchingLayers (LayerMask.GetMask ("Touchable"))) {

							handleAnimations ("chasing");
							transform.position = Vector2.MoveTowards (transform.position, new Vector2 (front.collider.transform.position.x, transform.position.y), chaseSpeed);

						}
					} else if (front.distance <= sightPosition.localPosition.x + 0.5f) {
						if (fighting != true) {
							fighting = true;

							StartCoroutine ("Fight");
						}
					}
					shiftCooldown = Time.time + shiftRate;
				} else if (back.collider != null && back.collider.CompareTag ("Player")) {
					if (fighting == true) {
						//fighting = false;
						//StopCoroutine ("Fight");
					}
					transform.localScale = new Vector3 (transform.localScale.x * -1, 1, 1);
					shiftCooldown = Time.time + shiftRate;
				} else {
					if (shiftCooldown < Time.time) {
						shiftCooldown = Time.time + shiftRate;
						patrolling = true;
						StartCoroutine ("Patrol");
					} else {
						handleAnimations ("idle");
					}
				}

			}
		}
	}

	void handleAnimations(string name){
		if ("fighting".Equals (name)) {
			anim.SetBool ("chasing", false);
			anim.SetBool("patrolling", false);
			anim.SetBool ("fighting", true);
			anim.SetBool ("idle", false);
		}
		else if ("idle".Equals (name)) {
			anim.SetBool ("chasing", false);
			anim.SetBool("patrolling", false);
			anim.SetBool ("fighting", false);
			anim.SetBool ("idle", true);
		}
		else if ("patrolling".Equals (name)) {
			anim.SetBool ("chasing", false);
			anim.SetBool("patrolling", true);
			anim.SetBool ("fighting", false);
			anim.SetBool ("idle", false);
		} else if ("chasing".Equals (name)) {
			anim.SetBool ("chasing", true);
			anim.SetBool("patrolling", false);
			anim.SetBool ("fighting", false);
			anim.SetBool ("idle", false);
		}
	}

	IEnumerator Fight() {
		StopCoroutine ("Patrol");
		fighting = true;

		//show animation
		anim.Play ("ExploderFight");
		handleAnimations("Fighting");
		yield return new WaitForSeconds(0.5f);
		anim.Play ("ExploderFight");
		handleAnimations("Fighting");
		//explode
		Instantiate (enemyExplosion, transform.position, Quaternion.identity);
		if (transform.parent != null) {
			Destroy (transform.parent.gameObject);
		} else {
			Destroy (gameObject);
		}
		yield return null;
	}

	IEnumerator Patrol() {
		patrolling = true;

		if (fighting == true) {
			//fighting = false;
			//StopCoroutine ("Fight");
		}

		handleAnimations ("patrolling");

		while (true) {
			RaycastHit2D front = Physics2D.Raycast (sightPosition.position, transform.localScale.x * Vector2.right, .25f, ~(1 << LayerMask.NameToLayer("Enemy")));
			RaycastHit2D frontDown = Physics2D.Raycast (sightPosition.position, new Vector2 (0, -1), 1.5f, ~(1 << LayerMask.NameToLayer("Enemy")));

			if (front.collider == null && frontDown.collider != null && frontDown.collider.IsTouchingLayers (LayerMask.GetMask ("Touchable"))) {
				if (transform.localScale.x > 0) {
					transform.position = Vector2.MoveTowards (transform.position, new Vector2 (transform.position.x + 20, transform.position.y), speed);
				} else {
					transform.position = Vector2.MoveTowards (transform.position, new Vector2 (transform.position.x - 20, transform.position.y), speed);
				}
			} else {
				handleAnimations ("idle");
				yield return new WaitForSeconds (idleTime);
				handleAnimations ("patrolling");
				transform.localScale = new Vector3 (transform.localScale.x * -1, 1, 1);
			}
			yield return null;
		}
		yield return null;
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.black;
		Gizmos.DrawLine (transform.position, transform.position + transform.localScale.x * Vector3.right * sight);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (patrolling && (other.gameObject.layer == LayerMask.NameToLayer("Bullet") 
			|| other.gameObject.layer == LayerMask.NameToLayer("Secondary") || other.CompareTag("Player"))) {
			shiftCooldown = Time.time + shiftRate;
			patrolling = false;
			StopCoroutine ("Patrol");

			Vector3 dir = other.transform.position - transform.position;
			if ((transform.localScale.x > 0 && dir.x < 0) || (transform.localScale.x < 0 && dir.x > 0)) {
				transform.localScale = new Vector3 (transform.localScale.x * -1,1,1);
			}
			rb.velocity = Vector2.zero;
		}
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
}
