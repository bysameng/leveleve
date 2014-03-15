using UnityEngine;
using System.Collections;

public class EventHandler : MonoBehaviour {

	public static Main main;
	public static DebuggerGUI debugr;
	public static Lenses Lens;
	public static InputHandler iHandler;
	public static TitleScreen tScreen;
	public static bool IsPlaying{
		get{return main.isPlaying;}
	}

	public static int LevelCount{
		get; private set;
	}


	void Awake(){
		tScreen = gameObject.AddComponent<TitleScreen>();
		main = GetComponent<Main>();
		Lens = main.Lens;
		iHandler = GetComponent<InputHandler>();
		debugr = GetComponent<DebuggerGUI>();
	}

	public static void NewGame(){
		iHandler.cursorCollider.enabled = true;
		iHandler.enabled = true;
	}

	public static void NextLevel(){
		tScreen.started = true;
		main.NextLevel();
		LevelCount++;
	}

	public static void GenerateNewLevel(Level l, int generatedLevelCount){
		main.GenerateNewLevel(l, generatedLevelCount);
	}

	public static void Dead(){
		LevelCount = 0;
		main.GameOver();
		iHandler.cursorCollider.enabled = false;
	}

	public static bool CheckLens(string lensName){
		switch (lensName){
		case "Red":		return (Lens.RedLens && !Lens.BlueLens && !Lens.PurpleLens);
		case "Blue":	return (!Lens.RedLens && Lens.BlueLens && !Lens.PurpleLens);
		case "Purple":	return (Lens.PurpleLens);
		default: break;
		}
		return false;
	}


	public static void ChangeLense(string lensName, bool putOn){
		if (lensName == "Red"){
			Lens.RedLens = putOn;
		}
		else if (lensName == "Blue"){
			Lens.BlueLens = putOn;
		}

		Lens.UpdateLens();
	}

	public static void Restart(){
		LevelCount = 0;
		main.Restart();
		Lens.UpdateLens();
	}

	public static void ErrorMessage(string error){
		debugr.addErrorMessage(error);
	}
}
