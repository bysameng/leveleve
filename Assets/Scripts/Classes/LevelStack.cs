using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelStack : MonoBehaviour {

	public GameObject levelObject;
	public int levelDepthToGenerate;
	public int levelDepth;
	public float transitionTime;
	public bool rotating = false;

	private int levelCount;
	private GameObject[] levelStackObjects; //used for translating planes
	private Queue<Level> levelStack;

	private bool transitioning;


	public void NextLevel(){
		if (transitioning) return;
		StartCoroutine(SmoothForward(transitionTime));
	}

	// Use this for initialization
	void Start () {
		rotating = false;
		levelStack = new Queue<Level>();
		levelStackObjects = new GameObject[levelDepthToGenerate];
		levelCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (levelCount < levelDepthToGenerate){
			GameObject l = CreateNewLevel();
			Level lev = l.GetComponent<Level>();
			lev.Init();
			EventHandler.GenerateNewLevel(lev);
			levelStackObjects[levelCount++] = l;
			//levelStack.Enqueue(l.GetComponent<Level>());
		}

		if (rotating){
			for(int i = 0; i < levelStackObjects.Length; i++){
				Transform t = levelStackObjects[i].transform;
				t.RotateAround(t.position, new Vector3(0, 0, 1), 1f);
			}
		}
	}

	private GameObject CreateNewLevel(){
		GameObject level = (GameObject)Instantiate(levelObject, this.transform.position, Quaternion.identity);
		Vector3 levelPos = new Vector3(this.transform.position.x, this.transform.position.y, levelCount * levelDepth);
		level.transform.position = levelPos;
		return level;
	}

	public void Rotate(float speed){

	}

	IEnumerator SmoothForward(float seconds){
		transitioning = true;

		Debug.Log ("Going next level");
		GameObject removedLevel = levelStackObjects[0];
		Vector3 removedLevelOriginalPos = removedLevel.transform.position;
		Level l = removedLevel.gameObject.GetComponent<Level>();
		l.DestroyLevelFade(transitionTime);

		levelCount--;

		for (int i = 0; i < levelCount; i++){
			levelStackObjects[i] = levelStackObjects[i+1];
		}


		Vector3[] originalPositions = new Vector3[levelCount];
		for (int i = 0; i < levelCount; i++){
			originalPositions[i] = levelStackObjects[i].transform.position;
		}
		for (float t = 0; t < seconds; t += Time.deltaTime){
			for (int i = 0; i < levelCount-1; i++){
				Vector3 curPos = levelStackObjects[i].transform.position;
				curPos.z = Mathf.SmoothStep(originalPositions[i].z, originalPositions[i].z-levelDepth, t/seconds);
				levelStackObjects[i].transform.position = curPos;
			}
			Vector3 removedCurPos = removedLevel.transform.position;
			removedCurPos.z = Mathf.SmoothStep(removedLevelOriginalPos.z, removedLevelOriginalPos.z-levelDepth, t/seconds);
			removedLevel.transform.position = removedCurPos;
			yield return null;
		}
		transitioning = false;
	}
}
