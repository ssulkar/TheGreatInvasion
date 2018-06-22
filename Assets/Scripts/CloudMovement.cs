using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour {

	public float maxXPosition, minXPosition;
	public float maxYPosition, minYPosition;

	private float maxSpeed  = .1f;
	private float minSpeed = .05f;
	private float movementSpeed;

	// Use this for initialization
	void Start () {
		float randomXAxis = Random.Range (minXPosition, maxXPosition);
		float randomYAxis = Random.Range (minYPosition, maxYPosition);
		Vector3 spawnPosition = new Vector3 (randomXAxis, randomYAxis, transform.position.z);
		transform.position = spawnPosition;
		movementSpeed = Random.Range (minSpeed, maxSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x > maxXPosition ||
		    transform.position.x < minXPosition ||
		    transform.position.y > maxYPosition ||
		    transform.position.y < minYPosition) {

			resetPosition ();
		} else {
			transform.Translate (Vector2.left * movementSpeed);
		}
	}

	void resetPosition() {
		float randomYAxis = Random.Range (minYPosition, maxYPosition);
		Vector3 spawnPosition = new Vector3 (maxXPosition, randomYAxis, transform.position.z);
		transform.position = spawnPosition;
		movementSpeed = Random.Range (minSpeed, maxSpeed);
	}
}
