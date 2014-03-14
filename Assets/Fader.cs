using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fader : MonoBehaviour {
	
	public bool visible;
	
	protected bool faded, fading;
	public float fadeInTime = .5f, fadeOutTime = 1f, betweenFadeTime = .01f;
	
	private Renderer[] renderers;
	private Color[] originalColors;
	protected float expire = 10f;
	
	
	// Use this for initialization
	public void Start () {
		
		//get all the renderers and then duplicate their materials and store original colors
		Renderer[] temp = transform.GetComponentsInChildren<Renderer>();
		renderers = new Renderer[temp.Length+1];
		originalColors = new Color[renderers.Length];
		for (int i = 0; i < renderers.Length; i++){
			if (i < temp.Length){
				renderers[i] = temp[i];
			}
			else renderers[i] = this.gameObject.renderer;
			renderers[i].material = new Material(renderers[i].material);
			originalColors[i] = renderers[i].material.color;
			//Debug.Log (originalColors[i]);
		}
		
		
		faded = false;
		fading = false;
		visible = true;
		/*
		for (int i = 0; i < renderers.Length; i++){
			Color c = renderers[i].material.color;
			c.a = 0;
			renderers[i].material.color = c;
		}
		*/
	}
	
	// Update is called once per frame
	public void Update () {
	}
	
	
	//called upon interaction (with Use() )
	public void MakeVisible() {
		if (fading){
			Debug.Log ("It's fading!");
			return;
		}
		visible = true;
		if (faded){
			StartCoroutine (FadeBack (fadeOutTime, fadeInTime, betweenFadeTime));
		}
		else StartCoroutine(FadeIn(fadeInTime));
	}
	
	public void MakeInvisible(){
		StartCoroutine(FadeAway(fadeOutTime));
	}
	
	//used when wanna refresh
	IEnumerator FadeBack(float outTime, float inTime, float waitTime){
		StartCoroutine (FadeOut(outTime));
		yield return new WaitForSeconds(outTime+waitTime);
		StartCoroutine (FadeIn(inTime));
	}
	
	IEnumerator FadeAway(float outTime){
		while (fading) yield return null;
		StartCoroutine (FadeOut(outTime));
	}
	
	
	//fades in the bubble
	IEnumerator FadeIn(float seconds){
		expire = 7f;
		fading = true;
		for (float i = 0; i < seconds; i += Time.deltaTime){
			for (int j = 0; j < renderers.Length; j++){
				Color current = renderers[j].material.color;
				current.a = Mathf.SmoothStep (current.a, originalColors[j].a, i/seconds);
				renderers[j].material.color = current;
			}
			
			yield return null;
		}
		faded = true;
		fading = false;
		visible = true;
		expire = 10f;
	}
	
	
	//fades out the bubble
	IEnumerator FadeOut(float seconds){
		fading = true;
		for (float i = 0; i < seconds; i += Time.deltaTime){
			for (int j = 0; j < renderers.Length; j++){
				Color current = renderers[j].material.color;
				current.a = Mathf.SmoothStep (current.a, 0, i/seconds);
				renderer.material.color = current;
				//if (current.a > 0)
				//Debug.Log (current);
			}
			yield return null;
		}
		faded = false;
		fading = false;
		visible = false;
	}
}
