using UnityEngine;
using System.Collections;

public class DeathWall : MonoBehaviour{

	public string wallColor;

	void Update(){
		if (EventHandler.CheckLens(wallColor)){
			renderer.enabled = false;
		}
		else renderer.enabled = true;
	}

	void OnTriggerEnter(Collider other){
		if (other.name == "Cursor" && EventHandler.IsPlaying){
			if (!EventHandler.CheckLens(wallColor)){
				EventHandler.Dead();
				EventHandler.debugr.addErrorMessage("Killed by: " + wallColor);
			}
		}
	}
}

