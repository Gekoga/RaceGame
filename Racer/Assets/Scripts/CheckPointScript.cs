using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour {

	public int index;
	public GameControler gamecontroller;

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		gamecontroller.UpdateCheckpoints (other.gameObject, index);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
