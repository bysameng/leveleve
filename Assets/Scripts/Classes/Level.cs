using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour{


	public int SizeX{
		get;
		private set;
	}

	public int SizeY{
		get;
		private set;
	}

	public int ExitX{
		get; private set;
	}

	public int ExitY{
		get; private set;
	}

	public int LastExitX{
		get; set;
	}

	public int LastExitY{
		get; set;
	}

	public GameObject Exit{
		get; set;
	}

	protected int[,] grid;

	public List<GameObject> instantiatedObjects;


	private Fader fader;

	public void Init(){
		SizeX = (int)transform.localScale.x;
		SizeY = (int)transform.localScale.y;
		LastExitX = LastExitY = 0;
		fader = gameObject.AddComponent<Fader>();
		grid = new int[SizeX, SizeY];
		for (int i = 0; i < SizeX; i++)
			for (int j = 0; j < SizeY; j++)
				grid[i, j] = 0;
	//	fader.MakeVisible();
	}

	public void DestroyLevelFade(float seconds){
		StartCoroutine(Fader(seconds));
	}

	public int GetPos(int x, int y){
		return grid[x, y];
	}

	public void SetPos(int x, int y, int val){
		if (x < 0 || x >= SizeX || y < 0 || y >= SizeY){
			Debug.LogError("Error, out of range for Level.SetPos()");
			Debug.Log (SizeX);
			return;
		}
		grid[x, y] = val;
		if (val == 1){
			ExitX = x;
			ExitY = y;
		}
	}

	public void AddObject(GameObject obj){
		//Debug.Log ("Adding"+obj.name);
		instantiatedObjects.Add(obj);
	}

	public int WallCount(){

		return (instantiatedObjects.Count-1)/4;
	}

	public void PopulateGrid(){
		Debug.Log("Populating grid");

	}
	
	IEnumerator Fader(float seconds){
		fader.fadeOutTime = seconds*2f;
		fader.MakeInvisible ();
		yield return new WaitForSeconds(seconds * 4f);
		for (int i = 0; i < instantiatedObjects.Count; i++){
			Destroy (instantiatedObjects[i]);
		}
		Destroy(this.gameObject);
	}

}
