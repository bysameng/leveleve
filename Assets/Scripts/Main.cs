using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {


	public Font font;

	private float timer = -1f;
	private float beginTimer;
	private float totalTime = 0f;
	public float Score{
		get; private set;
	}

	private LevelWriter lWriter;
	private LevelStack lStack;

	private static InputHandler iHandler;

	public bool isPlaying{
		get; private set;
	}

	public bool gameOverScreen{
		get; private set;
	}


	private GUIStyle _myGUIStyle;
	public GUIStyle myGUIStyle{
		get{
			if (_myGUIStyle == null){
				_myGUIStyle = new GUIStyle();
				_myGUIStyle.font = font;
				_myGUIStyle.alignment = TextAnchor.MiddleCenter;
			}
			return _myGUIStyle;
		}
	}

	public Lenses Lens{
		get;
		private set;
	}

	public int LevelCount{
		get;
		private set;
	}

	void Awake(){
		lWriter = GetComponent<LevelWriter>();
		lStack = GetComponent<LevelStack>();
		Lens = new Lenses();
		iHandler = GetComponent<InputHandler>();
		isPlaying = true;
		gameOverScreen = false;

		if (!Screen.fullScreen){
			Screen.SetResolution(Screen.height, Screen.height, false);
		}
	}

	void OnGUI(){
		if (!isPlaying && gameOverScreen){
			GUI.Label(new Rect(Screen.width/2-51, Screen.height/2-70, 100, 100), "GAME OVER", myGUIStyle);
			GUI.Label(new Rect(Screen.width/2-51, Screen.height/2-30, 100, 100), "TIME: " + string.Format("{0:0.0000}", totalTime + timer), myGUIStyle);
			switch (iHandler.lastInputDevice){
			case 0:
				GUI.Label(new Rect(Screen.width/2-51, Screen.height/2-10, 100, 100), "r to restart", myGUIStyle);
				break;
			case 1:
				GUI.Label(new Rect(Screen.width/2-51, Screen.height/2-10, 100, 100), "start to restart", myGUIStyle);
				break;
			case 2:
				GUI.Label(new Rect(Screen.width/2-51, Screen.height/2-10, 100, 100), "r / middle mouse to restart", myGUIStyle);
				break;
			}

			GUI.Label(new Rect(Screen.width/2-51, Screen.height/2-50, 100, 100), "LEVELS: " + LevelCount, myGUIStyle);
			GUI.Label(new Rect(Screen.width/2-51, Screen.height/100, 100, 100), "SCORE: " + Mathf.FloorToInt(Score), myGUIStyle);
		}
		else if (isPlaying && EventHandler.tScreen.started){
			GUI.Label(new Rect(Screen.width/2-51, Screen.height/2-50, 100, 100), "" + LevelCount, myGUIStyle);
			if (Score > 0){
				GUI.Label(new Rect(Screen.width/2-51, Screen.height/100, 100, 100), "SCORE: " + Mathf.FloorToInt(Score), myGUIStyle);
			}
			else {
				//GUI.Label(new Rect(Screen.width/2-51, Screen.height/100, 100, 100), "BEST: " + Mathf.FloorToInt(Score), myGUIStyle);
			}
			if(LevelCount > 0){
				if (timer > 0)
					GUI.Label(new Rect(Screen.width/2-51, Screen.height/2-30, 100, 100), string.Format("{0:0.0000}", timer), myGUIStyle);
				else if (timer == 0){
					GUI.Label(new Rect(Screen.width/2-51, Screen.height/2-30, 100, 100), "no bonus", myGUIStyle);
				}
			}
		}
	}

	void Update(){
		if (isPlaying){
			totalTime += Time.deltaTime;
			if(timer > 0){
				timer -= Time.deltaTime;
			}
			else timer = 0;
		}

	}

	public void NewGame(){
		gameOverScreen = false;
		LevelCount = 0;
		timer = -1f;
		totalTime = 0f;
		Score = 0;
		lStack.NewGame();
	}


	public void NextLevel(){
		isPlaying = true;
		Score++;
		Score+=timer;
		EventHandler.debugr.currentBoxes = ""+lStack.GetNextWallCount();
		timer = (lStack.GetNextWallCount() * .3f) + 1f + .01f * LevelCount;
		lStack.NextLevel();
		LevelCount++;
	}


	public void GenerateNewLevel(Level l, int generatedLevelCount){
		lWriter.GenerateLevel(l, generatedLevelCount);
	}


	public void GameOver(){
		gameOverScreen = true;
		isPlaying = false;
		Debug.Log ("Dead");
		iHandler.enabled = false;

	}

	public void Restart(){
		Debug.Log ("Again");
		iHandler.enabled = true;

		float tempTransSpeed = lStack.transitionTime;
		lStack.transitionTime = .1f;

		NewGame();

		lStack.transitionTime = tempTransSpeed;


	}

}
