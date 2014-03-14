using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour{

//	private int _sizex;
	public int SizeX{
		get;
		private set;
	}
//	private int _sizey;
	public int SizeY{
		get;
		private set;
	}

	protected int[,] grid;

	public List<GameObject> instantiatedObjects;


	private Fader fader;

	public void Init(){
		SizeX = (int)transform.localScale.x;
		SizeY = (int)transform.localScale.y;
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
	}

	public void PopulateGrid(){
		Debug.Log("Populating grid");

	}



	IEnumerator Fader(float seconds){
		fader.fadeOutTime = seconds * 2f;
		fader.MakeInvisible ();
		yield return new WaitForSeconds(seconds * 2f);
		for (int i = 0; i < instantiatedObjects.Count; i++){
			Destroy (instantiatedObjects[i]);
		}
		Destroy(this.gameObject);

	}

}
