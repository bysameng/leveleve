using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	private bool triggered = false;

	void OnTriggerEnter(Collider other){
		if (other.transform.name == "Cursor" && !triggered){
			EventHandler.NextLevel();
			triggered = true;
		}
	}

	void OnTriggerStay(Collider other){
		if (other.transform.name == "Cursor" && !triggered){
			EventHandler.NextLevel();
			triggered = true;
		}
	}


}
