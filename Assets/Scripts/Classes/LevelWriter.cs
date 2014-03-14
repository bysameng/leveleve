using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelWriter : MonoBehaviour{

	public GameObject[] ObjectsToPopulateWith;
	
	void Start(){

	}

	public void WriteLevel(Level level){
		List<GameObject> instantiatedObjects = new List<GameObject>();
		for (int i = 0; i < level.sizex; i++){
			for (int j = 0; j < level.sizey; j++){
				int currentPos = level.GetPos(i, j);
				if  (currentPos > 0){
					GameObject obj = (GameObject)Instantiate(ObjectsToPopulateWith[currentPos], new Vector3(i, j, level.transform.position.z), Quaternion.identity);
					instantiatedObjects.Add(obj);
				}
			}
		}
		level.instantiatedObjects = instantiatedObjects;
	}

	public void DeleteLevel(Level level){
		level = null;
	}

}
