﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartPanelController : MonoBehaviour {

	List<GameObject> hearts = new List<GameObject>();

	void Start() {
		foreach (Transform child in transform) {
			hearts.Add (child.gameObject);
		}
		
	}
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetHeartCount(int hp) {
		for(int i = 0; i < hearts.Count; i++) {
			Color newColor = hearts[i].GetComponent<Image> ().color;
			newColor.a = i < hp ? 1 : 0;
			hearts[i].GetComponent<Image> ().color = newColor;
		}
	}
}
