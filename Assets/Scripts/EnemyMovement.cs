using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public float min=2f;
	public float max=2f;
	private Rigidbody2D rigidBody; 
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		min=transform.position.x - min;
		max=transform.position.x + max;
	}

	// Update is called once per frame
	void Update () {
		transform.position =new Vector2(Mathf.PingPong(Time.time*2,max-min)+min, transform.position.y);
	}

	/*
	void OnCollisionEnter2D(Collision2D c)
	{
		Vector2 dir = gameObject.GetComponent<Rigidbody2D>().velocity;
		dir.x = dir.x * -1;
		gameObject.GetComponent<Rigidbody2D>().velocity = dir;
	}*/
}
