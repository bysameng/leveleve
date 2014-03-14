using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.transform.name == "Cursor"){
			EventHandler.NextLevel();
		}
	}
}
