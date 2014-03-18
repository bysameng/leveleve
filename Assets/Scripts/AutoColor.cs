using UnityEngine;
using System.Collections;

public class AutoColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<BoxCollider>().extents = (new Vector3(5, 5, 0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other){
		if (EventHandler.gameMode == 2 && EventHandler.IsPlaying && other.name == "Cursor"){
			Debug.Log("Trigger"+GetComponent<DeathWall>().wallColor);
			EventHandler.ChangeLense(gameObject.GetComponent<DeathWall>().wallColor, true);
		}
	}

	void OnTriggerExit (Collider other){
		if (EventHandler.gameMode == 2 && EventHandler.IsPlaying && other.name == "Cursor"){
			EventHandler.ChangeLense(GetComponent<DeathWall>().wallColor, false);
		}
	}
}
