using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public GameObject openSwitch, closeSwitch, player;

	private BoxCollider2D doorCollider;
	private bool doorOpen;

	// Use this for initialization
	void Start () {
		doorOpen = false;
		doorCollider = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (openSwitch == null && doorOpen == false) {
			openDoor();
		}
		if (closeSwitch == null && doorOpen == true) {
			closeDoor ();
		}
	}

	void openDoor() {
		GetComponent<SpriteRenderer> ().enabled = false;
		doorOpen = true;
		Physics2D.IgnoreCollision (player.GetComponent<BoxCollider2D> (), doorCollider); 
		
	}

	void closeDoor() {
		GetComponent<SpriteRenderer> ().enabled = true;
		doorOpen = false;
		Physics2D.IgnoreCollision (player.GetComponent<BoxCollider2D> (), doorCollider, false);
	}
}
