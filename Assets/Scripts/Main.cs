using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	private LevelWriter lWriter;
	private LevelStack lStack;
	
	void Awake(){
		lWriter = GetComponent<LevelWriter>();
		lStack = GetComponent<LevelStack>();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void NextLevel(){
		lStack.NextLevel();
	}

}
