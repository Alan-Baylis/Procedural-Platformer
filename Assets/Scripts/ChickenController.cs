﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class ChickenController : MonoBehaviour {

	// Use this for initialization
	Animator animator;
	Vector2 destination;
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
		SelectFacing ();
		StartCoroutine (SelectNextAction ());
	}

	void SelectFacing() {
		Vector3 newScale = gameObject.transform.localScale;
		if (Random.value < 0.5f) {
			newScale.x = -newScale.x;
		}
		gameObject.transform.localScale = newScale;
	}
	// Update is called once per frame
	float lastFlapTime = 0f;
	public float flapSpeed = 0.25f;
	public Vector2 flapForce = Vector2.up;
	void Update () {
		if (!IsGrounded ()) {
			animator.SetBool ("Flying", true);
			animator.SetBool ("Walking", false);

			if (Time.time > lastFlapTime + flapSpeed) {
				Flap (flapForce);
			}
				
		} else {
			animator.SetBool ("Flying", false);
			if (gameObject.GetComponent<Rigidbody2D> ().velocity.x != 0f) {
				animator.SetBool ("Walking", true);
			} else {
				animator.SetBool ("Walking", false);

			}
		}

		if(gameObject.GetComponent<Rigidbody2D>().velocity.x != 0f && Mathf.Sign(gameObject.GetComponent<Rigidbody2D>().velocity.x) != Mathf.Sign(transform.localScale.x)) {
			Vector3 newScale = gameObject.transform.localScale;
			newScale.x = -newScale.x;
			gameObject.transform.localScale = newScale;

		}

		
	}

	void Flap(Vector2 force) {
		lastFlapTime = Time.time;
		gameObject.GetComponent<Rigidbody2D> ().AddForce (flapForce, ForceMode2D.Impulse);
	}

	bool IsGrounded() {
		RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.up, 0.51f);
		if (hit.collider != null) {
			return true;
		}
		return false;
	}
	IEnumerator SelectNextAction() {
		yield return null;
	}
		
	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
			Vector2 direction = gameObject.transform.position - col.gameObject.transform.position;
			gameObject.GetComponent<Rigidbody2D> ().AddForce (direction * 100 * Time.deltaTime);
			RaycastHit2D hit = Physics2D.Raycast (transform.position, new Vector2 (transform.localScale.x, 0), 0.6f);
			if (hit.collider != null) {
				if (Time.time > lastFlapTime + flapSpeed) {
					Flap (flapForce);
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "Player") {
			if (col.contacts [0].normal.y < 0) {
				if (col.relativeVelocity.magnitude > 1f) {
					col.gameObject.GetComponent<PlayerController> ().BounceOffEnemy (1f);
					gameObject.SendMessage ("TakeDamage", 1);
				}
			}
		}
	}

	public GameObject featherPrefab;
	void Die() {
		GameObject newFeather = GameObject.Instantiate(featherPrefab, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);
		newFeather.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-8f, 8f), Random.Range(2f, 4f)), ForceMode2D.Impulse);
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<CircleCollider2D> ().enabled = false;
		gameObject.GetComponent<ParticleSystem> ().Play ();
		Destroy (gameObject, 0.5f);
	}
}
