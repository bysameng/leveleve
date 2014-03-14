using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelWriter : MonoBehaviour{

	public GameObject[] ObjectsToPopulateWith;
	public int difficulty = 1;
	public int mode = 1;

	void Start(){
	}

	private void WriteLevel(Level level){
		List<GameObject> instantiatedObjects = new List<GameObject>();
		for (int i = 0; i < level.SizeX; i++){
			for (int j = 0; j < level.SizeY; j++){
				int currentPos = level.GetPos(i, j);
				if  (currentPos > 0){
					GameObject obj = (GameObject)Instantiate(ObjectsToPopulateWith[currentPos], new Vector3(i, j, level.transform.position.z), level.transform.rotation);
					instantiatedObjects.Add(obj);
					obj.transform.parent = level.transform;

				}
			}
		}
		level.instantiatedObjects = instantiatedObjects;
	}

	public void DeleteLevel(Level level){
		level = null;
	}

	public void GenerateLevel(Level level){
		if (mode == 1){
			int marginx = level.SizeX / 100;
			int marginy = level.SizeY / 100;
			int randomx = Random.Range(marginx, level.SizeX - marginx);
			int randomy = Random.Range(marginy, level.SizeY - marginy);
			level.SetPos(randomx, randomy, 1);
			for (int i = 0; i < Random.Range(0, 5); i++){
				randomx = Random.Range(marginx, level.SizeX - marginx);
				randomy = Random.Range(marginy
			}
		}

		WriteLevel(level);
	}

}
