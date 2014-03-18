using UnityEngine;
using System.Collections;

public class EventHandler : MonoBehaviour {

	public static Main main;
	public static LevelStack lStack;
	public static DebuggerGUI debugr;
	public static Lenses Lens;
	public static InputHandler iHandler;
	public static TitleScreen tScreen;
	public static HighScoreHandler hsHandler;
	public static AudioHandler aHandler;
	public static bool IsPlaying{
		get{return main.isPlaying;}
	}

	public static int LevelCount{
		get; private set;
	}

	public static int gameMode;


	void Awake(){
		//gameMode = 2;
		tScreen = gameObject.AddComponent<TitleScreen>();
		main = GetComponent<Main>();
		lStack = GetComponent<LevelStack>();
		Lens = main.Lens;
		iHandler = GetComponent<InputHandler>();
		debugr = GetComponent<DebuggerGUI>();
		hsHandler = GetComponent<HighScoreHandler>();
		aHandler = GetComponent<AudioHandler>();
	}



	public static void NewGame(){
		tScreen.started = false;
		iHandler.cursorCollider.enabled = true;
		iHandler.enabled = true;
		iHandler.Mode = gameMode;
	}

	public static void NextLevel(){
		aHandler.PlayExitSound(new Vector3((float)lStack.GetCurrentLevel().ExitX, (float)lStack.GetCurrentLevel().ExitY, 1f));
		iHandler.Destination = new Vector3((float)lStack.GetNextLevel().ExitX, (float)lStack.GetNextLevel().ExitY, 0f);
		tScreen.started = true;
		main.NextLevel();
		LevelCount++;
	}

	public static Vector3 GetCurrentLevelExit(){
		return lStack.GetCurrentLevel().Exit.transform.position;
	}

	public static void GenerateNewLevel(Level l, int generatedLevelCount){
		main.GenerateNewLevel(l, generatedLevelCount);
	}

	public static void Dead(){
		aHandler.PlayDeathSound(iHandler.cursor.transform.position);
		tScreen.started = true;
		LevelCount = 0;
		main.GameOver();
		iHandler.cursorCollider.enabled = false;
	
	}

	public static bool CheckLens(string lensName){
		switch (lensName){
		case "Red":		return (Lens.RedLens && !Lens.BlueLens && !Lens.PurpleLens);
		case "Blue":	return (!Lens.RedLens && Lens.BlueLens && !Lens.PurpleLens);
		case "Purple":	return (Lens.PurpleLens);
		case "Clear":	return (!Lens.RedLens && !Lens.BlueLens && !Lens.PurpleLens);
		default: break;
		}
		return false;
	}


	public static void ChangeLense(string lensName, bool putOn){
		if (lensName == "Red"){
			//Debug.Log ("Changing to red");
			Lens.RedLens = putOn;
		}
		else if (lensName == "Blue"){
			Lens.BlueLens = putOn;
		}
		else if (lensName == "Purple"){
			Lens.RedLens = putOn;
			Lens.BlueLens = putOn;
		}
		else if (lensName == "Clear"){
			if (putOn)
				Lens.ClearLens();
		}

		Lens.UpdateLens();
	}

	public static void Restart(){
		LevelCount = 0;
		main.Restart();
		Lens.UpdateLens();
	}


	public static void DisplayHighScores(){

	}

	public static void AddScore(int score){
		StaticCoroutine.DoCoroutine(ScoreAdder(score));
	}

	public static void StartInput(){
		iHandler.GetInput();
	}


	public static void SetInputEnabled(bool enabled){
		iHandler.fullEnabled = enabled;
	}


	public static void ErrorMessage(string error){
		debugr.addErrorMessage(error);
	}


	static IEnumerator ScoreAdder(int score){
		if(!main.singleUserScores){
		StartInput();
		hsHandler.GettingInput = true;
		while (iHandler.inputtingString){
			hsHandler.nameBuffer = iHandler.inputBuffer;
			yield return null;
		}
		if(iHandler.inputBuffer != null){
			Debug.Log("saved");
			hsHandler.AddScore(score, hsHandler.nameBuffer);
		}
		else hsHandler.Cancel();
		}
		else
			hsHandler.AddScore(score, "you");
	
	}


	public static void ChangeMode(int mode){
		gameMode = mode;
	}

}
