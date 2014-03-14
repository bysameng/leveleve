using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	private LevelWriter lWriter;
	private LevelStack lStack;

	public int LevelCount{
		get;
		private set;
	}

	void Awake(){
		lWriter = GetComponent<LevelWriter>();
		lStack = GetComponent<LevelStack>();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

	void OnGUI(){
		GUI.Label(new Rect(0, 0, 100, 100), "" + LevelCount);
	}


	public void NextLevel(){
		lStack.NextLevel();
		LevelCount++;
	}

	public void GenerateNewLevel(Level l){
		lWriter.GenerateLevel(l);
	}

}
