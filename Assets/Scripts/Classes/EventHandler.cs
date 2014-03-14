using UnityEngine;
using System.Collections;

public class EventHandler : MonoBehaviour {

	public static Main main;

	void Start(){
		main = GetComponent<Main>();
	}

	public static void NextLevel(){
		main.NextLevel();
	}
}
