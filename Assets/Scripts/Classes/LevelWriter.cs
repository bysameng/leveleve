using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelWriter : MonoBehaviour{

	public GameObject[] ObjectsToPopulateWith;
	public int difficulty = 1;
	public int levelCount = 0;
	public int mode = 1;

	void Start(){
	}

	private void WriteLevel(Level level){

		for (int i = 0; i < level.SizeX; i++){
			for (int j = 0; j < level.SizeY; j++){
				int currentPos = level.GetPos(i, j);
				if  (currentPos > 0){
					GameObject obj = (GameObject)Instantiate(ObjectsToPopulateWith[currentPos], new Vector3(i, j, level.transform.position.z - 19f), level.transform.rotation);
					level.AddObject(obj);
					obj.transform.parent = level.transform;
				}
			}
		}

	}

	public void DeleteLevel(Level level){
		level = null;
	}

	public void GenerateLevel(Level level, int generatedLevelCount){
		if (mode == 1){

			int marginx = level.SizeX / 10;
			int marginy = level.SizeY / 10;
			int randomx = Random.Range(marginx, level.SizeX - marginx);
			int randomy = Random.Range(marginy, level.SizeY - marginy);
		
			if (generatedLevelCount == 0) {
				randomx = 50; randomy = 50;
			}
			level.SetPos(randomx, randomy, 1);


			int colorCount = 1;

			for (int i = 1; i < Random.Range (0, 2) * Mathf.Sqrt(generatedLevelCount); i++){
				if (generatedLevelCount > 5) colorCount = 2;
				if (generatedLevelCount > 20) colorCount = 3;
				WriteRectangleAround(level, level.ExitX, level.ExitY, 10*i*i, 10*i*i, Random.Range(2, 2+colorCount));
				generatedLevelCount++;
			}
		}
		WriteLevel(level);
	}


	private void GenerateLine(Level level, int x0, int y0, int x1, int y1, int val){
		int sx = -1, sy = -1;
		int deltaX = Mathf.Abs (x1 - x0);
		int deltaY = Mathf.Abs (y1 - y0);

		if (x0 < x1) sx = 1; 
		if (y0 < y1) sy = 1;

		int err = deltaX - deltaY;

		while (true){
			level.SetPos(x0, y0, val);
			if (x0 == x1 && y0 == y1) break;
			int e2 = err * 2;
			if (e2 > -deltaY){
				err = err - deltaY;
				x0 = x0 + sx;
			}
			if (e2 < deltaX){
				err = err + deltaX;
				y0 = y0 + sy;
			}
		}
	}

	private void WriteBlock(Level level, int x0, int y0, int x1, int y1, int val){
		int deltaX = Mathf.Abs (x1 - x0);
		int deltaY = Mathf.Abs (y1 - y0);

		int posX = (x0 + x1)/2;
		int posY = (y0 + y1)/2;

		while (x0 != x1){
			//level.SetPos(x0, y0, val);
			if (x0 < x1) x0++;
				else x0--;
		}

		while (y0 != y1){
			//level.SetPos(x0, y0, val);
			if (y0 < y1) y0++;
			else y0--;
		}
		GameObject obj = (GameObject)Instantiate(ObjectsToPopulateWith[val], new Vector3(posX, posY, level.transform.position.z - 19f), level.transform.rotation);
		obj.transform.localScale = new Vector3(deltaX + 1, deltaY + 1, .5f);
		obj.transform.parent = level.transform;
		level.AddObject(obj);

	}

	private void WriteRectangle(Level level, int x, int y, int w, int h, int val){
		if (w == 0 || h == 0) return;
		WriteBlock(level, x, y, x+w, y, val);
		WriteBlock(level, x, y, x, y+h, val);
		WriteBlock(level, x+w, y, x+w, y+h, val);
		WriteBlock(level, x, y+h, x+w, y+h, val);
	}


	private void WriteRectangleAround(Level level, int x, int y, int w, int h, int val){
		if (w == 0 || h == 0) return;
		WriteRectangle(level, x-w/2, y-h/2, w, h, val); 
	}
	

}
