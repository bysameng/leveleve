using UnityEngine;
using System.Collections;

public class Lenses{

	//colors
	public Color RedLensColor{
		get; set;
	}

	public Color BlueLensColor{
		get; set;
	}

	public Color PurpleLensColor{
		get; set;
	}


	//fading utilities
	private CameraFade cFader;

	public float FadeTime{
		get; private set;
	}

	//active lens booleans
	public bool RedLens{
		get; set;
	}

	public bool BlueLens{
		get; set;
	}

	public bool PurpleLens{
		get; private set;
	}

	
	public Lenses () {
		RedLensColor = new Color(1f, 10f/255f, 10f/255f, .5f);
		BlueLensColor = new Color(10f/255f, 10f/255f, 1f, .5f);
		PurpleLensColor = new Color(.8f, 10f/255f, .9f, .5f);
		FadeTime = .1f;
		cFader = new GameObject("CameraFader").AddComponent<CameraFade>();
	}
	
	// Update is called once per frame
	public void UpdateLens () {
		if (RedLens && BlueLens){
			SetPurpleLens();
			return;
		}

		if (RedLens){
			SetRedLens();
			return;
		}

		if (BlueLens){
			SetBlueLens();
			return;
		}

		ClearLens();

	}

	private void SetRedLens(){
		RedLens = true; BlueLens = false; PurpleLens = false;
		ChangeLens(RedLensColor);
	}

	private void SetBlueLens(){
		RedLens = false; BlueLens = true; PurpleLens = false;
		ChangeLens(BlueLensColor);
	}

	private void SetPurpleLens(){
		PurpleLens = true;
		ChangeLens(PurpleLensColor);
	}

	public void ChangeLens(Color c){
		//Debug.Log("Fading to" + c);
		if (FadeTime == 0) cFader.SetScreenOverlayColor(c);
		else cFader.StartFade(c, FadeTime);
	}


	public void ClearLens(){
		RedLens = BlueLens = PurpleLens = false;
		ChangeLens(Color.clear);
	}
}
