using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour {

	public float thrust;
	public float xSpeed;
	public float ySpeed;

	private Rigidbody2D tankRb;
	private Animator anim;
	private bool hasDriver = false;

	void Start () {
		tankRb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		anim.SetFloat ("speed", tankRb.velocity.magnitude);
	}

	void FixedUpdate(){
		handleSpeed ();
		if (hasDriver) {
			handleMovement ();
		}
	}

	void handleMovement(){
		if (hasDriver) {
			//print("GOOOO");
			tankRb.AddForce (new Vector2 (thrust, 0), ForceMode2D.Force);
		}
	}

	void handleSpeed(){

		float currentXSpeed = Mathf.Abs (tankRb.velocity.x);
		float currentYSpeed = Mathf.Abs (tankRb.velocity.y);

		if (currentXSpeed > xSpeed) {
			tankRb.velocity = new Vector2((tankRb.velocity.normalized * xSpeed).x, tankRb.velocity.y);
		}
		if (currentYSpeed > ySpeed) {
			tankRb.velocity = new Vector2(tankRb.velocity.x, (tankRb.velocity.normalized * xSpeed).y);
		}
	}

	void OnTriggerStay2D(Collider2D other){

		if(other.gameObject.tag == "Player"){
			other.transform.parent = transform;
			hasDriver = true;
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			other.transform.parent = null;
			hasDriver = false;
		}
	}
}
