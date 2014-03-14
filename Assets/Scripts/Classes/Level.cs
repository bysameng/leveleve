using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour{

	private int _sizex;
	public int sizex{
		get {return this._sizex;}
		private set {this._sizex = value;}
	}
	private int _sizey;
	public int sizey{
		get {return this._sizey;}
		private set {this._sizey = value;}
	}

	protected int[,] grid;

	public List<GameObject> instantiatedObjects;

	void Start(){
		this.sizex = (int)transform.localScale.x;
		this.sizey = (int)transform.localScale.y;
		grid = new int[sizex, sizey];
		for (int i = 0; i < sizex; i++)
			for (int j = 0; j < sizey; j++)
				grid[i, j] = 0;

	}

	public void DestroyLevelFade(float seconds){
		StartCoroutine(Fader(seconds));
	}

	public int GetPos(int x, int y){
		return grid[x, y];
	}

	public void SetPos(int x, int y, int val){
		if (x < 0 || x >= sizex || y < 0 || y >= sizey){
			Debug.LogError("Error, out of range for Level.SetPos()");
			return;
		}
		grid[x, y] = val;
	}

	public void PopulateGrid(){
		Debug.Log("Populating grid");

	}



	IEnumerator Fader(float seconds){
		instantiatedObjects.Add(this.gameObject);
		Material[] materials = new Material[instantiatedObjects.Count];
		for (int i = 0; i < instantiatedObjects.Count; i++){
			materials[i] = new Material(instantiatedObjects[i].renderer.material);
			instantiatedObjects[i].renderer.material = materials[i];
		}
		for (float t = 0; t < seconds; t += Time.deltaTime){
			float alpha = Mathf.SmoothStep(1, 0, t/seconds);
			for (int i = 0; i < instantiatedObjects.Count; i++){
				instantiatedObjects[i].renderer.material.color = new Color(materials[i].color.r, materials[i].color.g, materials[i].color.b, alpha);
			}
			yield return null;
		}
		yield return new WaitForSeconds(seconds);
		for (int i = 0; i < instantiatedObjects.Count; i++){
			GameObject.Destroy(instantiatedObjects[i]);
		}
	}

}
