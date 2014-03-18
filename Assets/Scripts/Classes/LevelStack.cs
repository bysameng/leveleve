using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelStack : MonoBehaviour {

	public GameObject levelObject;
	public int levelDepthToGenerate;
	public int levelDepth;
	public float transitionTime;
	public float newGameTransitionTime = .1f;
	public bool rotating = false;

	private int levelCount;
	private GameObject[] levelStack; //used for translating planes

	private bool transitioning;


	public void NextLevel(){
		StartCoroutine(SmoothForward(transitionTime));

	}

	// Use this for initialization
	public void Start () {
		if(levelStack != null){
			for (int i = 0; i < levelStack.Length; i++){
				Destroy(levelStack[i]);
			}
		}

		levelStack = new GameObject[levelDepthToGenerate+1];
		levelCount = 0;
		while (levelCount < levelDepthToGenerate){
			GameObject l = CreateNewLevel();
			Level lev = l.GetComponent<Level>();
			lev.Init();
			EventHandler.GenerateNewLevel(lev, levelCount);
			levelStack[levelCount++] = l;
		}
	}


	// Update is called once per frame
	void Update () {
		if (rotating && EventHandler.IsPlaying){
			for(int i = 0; i < levelStack.Length-1; i++){
				Transform t = levelStack[i].transform;
				t.Rotate(0, 0, Time.deltaTime * EventHandler.LevelCount * 2);
				EventHandler.debugr.rotation = ""+Time.deltaTime * EventHandler.LevelCount * 2;
			}
		}
	}

	public void NewGame(){
		StartCoroutine(DoNewGame());
	}

	private GameObject CreateNewLevel(){
		GameObject level = (GameObject)Instantiate(levelObject, this.transform.position, Quaternion.identity);
		Vector3 levelPos = new Vector3(this.transform.position.x, this.transform.position.y, levelCount * levelDepth + 19f);
		level.transform.position = levelPos;
		return level;
	}

	public void Rotate(float speed){

	}

	public int GetNextWallCount(){
		return levelStack[1].GetComponent<Level>().WallCount();
	}


	IEnumerator DoNewGame(){
		for(int i = 0; i < levelDepthToGenerate; i++)
			StartCoroutine(SmoothForward(newGameTransitionTime));
		yield return new WaitForSeconds ((levelDepthToGenerate)* newGameTransitionTime);
		Start();
		EventHandler.NewGame();
	}


	IEnumerator SmoothForward(float seconds){
		while (transitioning) yield return null;

		transitioning = true;


	//	Debug.Log ("Going next level");
		GameObject removedLevel = levelStack[0];
		Vector3 removedLevelOriginalPos = removedLevel.transform.position;
		Level l = removedLevel.gameObject.GetComponent<Level>();
		if(seconds == newGameTransitionTime)
			l.DestroyLevelFade(seconds/4f);
		else l.DestroyLevelFade(seconds);

		levelCount--;

		for (int i = 0; i < levelCount; i++){
			levelStack[i] = levelStack[i+1];
		}

		
		GameObject level = CreateNewLevel();
		Level lev = level.GetComponent<Level>();
		Level lastLev = levelStack[levelCount-1].GetComponent<Level>();
		lev.Init();
		lev.LastExitX = lastLev.LastExitX;
		lev.LastExitY = lastLev.LastExitY;
		EventHandler.GenerateNewLevel(lev, EventHandler.LevelCount+levelDepthToGenerate);
		//Debug.Log (levelCount);
		levelStack[levelCount++] = level;


		//debug
		/*
		Level curLevel = levelStack[0].GetComponent<Level>();
		string curB = "\n";
		for(int i = 0; i < curLevel.instantiatedObjects.Count; i++){
			curB = curB+curLevel.instantiatedObjects[i].name+"\n";
		}
		EventHandler.debugr.currentBoxes = curB;
		*/


		Vector3[] originalPositions = new Vector3[levelCount];
		for (int i = 0; i < levelCount; i++){
			originalPositions[i] = levelStack[i].transform.position;
		}
		for (float t = 0; t < seconds; t += Time.deltaTime){
			for (int i = 0; i < levelCount-1; i++){
				Vector3 curPos = levelStack[i].transform.position;
				curPos.z = Mathf.SmoothStep(originalPositions[i].z, originalPositions[i].z-levelDepth, t/seconds);
				levelStack[i].transform.position = curPos;
			}
			Vector3 removedCurPos = removedLevel.transform.position;
			removedCurPos.z = Mathf.SmoothStep(removedLevelOriginalPos.z, removedLevelOriginalPos.z-levelDepth, t/seconds);
			removedLevel.transform.position = removedCurPos;
			yield return null;
		}
		for (int i = 0; i < levelCount-1; i++){
			Vector3 curPos = levelStack[i].transform.position;
			curPos.z = originalPositions[i].z-levelDepth;
			levelStack[i].transform.position = curPos;
		}

		transitioning = false;
	}
}
