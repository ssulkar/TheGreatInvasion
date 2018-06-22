using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlerMovement : MonoBehaviour {

	public Transform[] positions;
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


	//melee
	public GameObject meleeAttack;
	private float meleeRate = 1f;
	private float meleeCooldown;

	//patrol
	private bool scriptedPatrol;

	//Animations
	Animator anim;

	//private bool chasing;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		currentPosition = 0;

		anim = GetComponent<Animator> ();

		if (transform.position.x <= positions [0].position.x && transform.position.x >= positions [1].position.x) {
			scriptedPatrol = true;
		} else {
			scriptedPatrol = false;
		}
		StartCoroutine ("Patrol");
	}

	void Update(){

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
				if (front.distance > sightPosition.localPosition.x + 0.1f) {
					if (fighting == true) {
						fighting = false;
						StopCoroutine ("Fight");
					}

					RaycastHit2D frontDown = Physics2D.Raycast (sightPosition.position, new Vector2 (0, -1), 1.5f, ~((1 << LayerMask.NameToLayer ("Enemy")) | (1 << LayerMask.NameToLayer ("Player"))));
					if (frontDown.collider != null && frontDown.collider.IsTouchingLayers (LayerMask.GetMask ("Touchable"))) {
						
						handleAnimations ("chasing");
						transform.position = Vector2.MoveTowards (transform.position, new Vector2 (front.collider.transform.position.x, transform.position.y), chaseSpeed);

					}
				} else if (front.distance <= sightPosition.localPosition.x + 0.1f) {
					if (fighting != true) {
						fighting = true;

						StartCoroutine ("Fight");
					}
				}
				shiftCooldown = Time.time + shiftRate;
			} else if (back.collider != null && back.collider.CompareTag ("Player")) {
				if (fighting == true) {
					fighting = false;
					StopCoroutine ("Fight");
				}
				transform.localScale = new Vector3 (transform.localScale.x * -1, 1, 1);
				shiftCooldown = Time.time + shiftRate;
			} else {
				if (shiftCooldown < Time.time) {
					shiftCooldown = Time.time + shiftRate;
					patrolling = true;

					if (transform.position.x <= positions [0].position.x && transform.position.x >= positions [1].position.x) {
						scriptedPatrol = true;
					} else {
						scriptedPatrol = false;
					}
					StartCoroutine ("Patrol");
				} else {
					handleAnimations ("idle");
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
	/*void handleAnimations(){
		

		anim.SetBool ("chasing", chasingAnim);
		anim.SetBool("patrolling", patrollingAnim);
		anim.SetBool ("fighting", fightingAnim);
		anim.SetBool ("idle", idleAnim);
	}*/

	IEnumerator Fight() {
		fighting = true;

		while(true) {

			anim.Play ("BrawlerIdle");
			yield return new WaitForSeconds(.25f);
			anim.Play ("BrawlerAttack");
			//handleAnimations ("idle");
			GameObject swing = null;


			if (transform.localScale.x > 0) {
				swing = Instantiate (meleeAttack, sightPosition.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
			} else {
				swing = Instantiate (meleeAttack, sightPosition.position, Quaternion.Euler (new Vector3 (0, 0, 180f)));
			}
			swing.transform.SetParent (sightPosition);
			//yield return new WaitForSeconds(Random.Range(meleeRate, meleeRate+0.5f));


			yield return new WaitForSeconds(Random.Range(meleeRate, meleeRate+0.5f));
			//handleAnimations ("fighting");
		}
	}

	IEnumerator Patrol() {
		patrolling = true;

		if (fighting == true) {
			fighting = false;
			StopCoroutine ("Fight");
		}

		handleAnimations ("patrolling");

		while (true) {
			if (transform.position.x <= positions [0].position.x && transform.position.x >= positions [1].position.x) {
				scriptedPatrol = true;
			} else {
				scriptedPatrol = false;
			}

			RaycastHit2D front = Physics2D.Raycast (sightPosition.position, transform.localScale.x * Vector2.right, .25f, ~(1 << LayerMask.NameToLayer("Enemy")));
			RaycastHit2D frontDown = Physics2D.Raycast (sightPosition.position, new Vector2 (0, -1), 1.5f, ~(1 << LayerMask.NameToLayer("Enemy")));

			if (scriptedPatrol) {
				if (currentPosition >= positions.Length) {
					currentPosition = 0;
				}
				if ((transform.position.x == positions [currentPosition].position.x)||!(front.collider == null && frontDown.collider != null && frontDown.collider.IsTouchingLayers (LayerMask.GetMask ("Touchable")))) {
					currentPosition++;

					handleAnimations ("idle");
					yield return new WaitForSeconds (idleTime);
					handleAnimations ("patrolling");

				}
				if (currentPosition >= positions.Length) {
					currentPosition = 0;
				}

				transform.position = Vector2.MoveTowards (transform.position, new Vector2 (positions [currentPosition].position.x, transform.position.y), speed);

				if (transform.position.x > positions [currentPosition].position.x) {
					transform.localScale = new Vector3 (-1, 1, 1);
				} else if (transform.position.x < positions [currentPosition].position.x) {
					transform.localScale = new Vector3 (1, 1, 1);
				}
				yield return null;
			} else {
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
