using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainMovement : MonoBehaviour {

	public Transform[] spawnPositions;
	public Transform[] barrels;
	public GameObject henchmen;
	public GameObject bullet;

	private bool drop1 = false;
	private bool drop2 = false;
	private bool drop3 = false; 

	private EnemyHealth health;
	private float high = 0;
	private float mid = 0;
	private float low = 0;

	// Use this for initialization
	void Start () {
		health = GetComponent<EnemyHealth> ();
		float max = health.enemyMaxHealth;
		high = max;
		mid = (2f/3f)*max;
		low = (1f/3f)*max;
		StartCoroutine ("fire");
	}
	
	// Update is called once per frame
	void Update () {
		handleHenchmenDrop();
	}

	IEnumerator fire(){
		while (true) {
			int i = 0;
			while (i < 4) {
				GameObject liveBullet = (GameObject)Instantiate (bullet, barrels [Random.Range (0, barrels.Length)].position, Quaternion.identity);
				liveBullet.GetComponent<Rigidbody2D> ().velocity = Vector2.left * 7;
				i++;
				yield return new WaitForSeconds (1.5f);
			}
			yield return new WaitForSeconds (3.0f);
		}
	}


	void handleHenchmenDrop(){
		float cur = health.currentHealth;

		if (drop1 == false && cur > mid && cur <= high) {
			Instantiate (henchmen, spawnPositions[0].position, Quaternion.identity);
			Instantiate (henchmen, spawnPositions[1].position, Quaternion.identity);
			Instantiate (henchmen, spawnPositions[2].position, Quaternion.identity);
			drop1 = true;
		}
		else if (drop2 == false && cur > low && cur <= mid){
			Instantiate (henchmen, spawnPositions[0].position, Quaternion.identity);
			Instantiate (henchmen, spawnPositions[1].position, Quaternion.identity);
			Instantiate (henchmen, spawnPositions[2].position, Quaternion.identity);
			drop2 = true;
		}
		else if (drop3 == false && cur <= low){
			Instantiate (henchmen, spawnPositions[0].position, Quaternion.identity);
			Instantiate (henchmen, spawnPositions[1].position, Quaternion.identity);
			Instantiate (henchmen, spawnPositions[2].position, Quaternion.identity);
			drop3 = true;
		}
	}
}
