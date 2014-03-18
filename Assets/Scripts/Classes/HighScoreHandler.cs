using UnityEngine;
using System.Collections;

public class HighScoreHandler : MonoBehaviour{

	public int scoreboardSize = 5;
	public bool GettingInput{
		get; set;
	}


	public int[] scores{
		get; private set;
	}

	public string[] names{
		get; private set;
	}

	public string nameBuffer;
	public int newScoreIndex;


	public void Start(){
		ReadHighScores();
		ReadHighScoreNames();
	}

	void OnGUI(){
		if (GettingInput){
			EventHandler.SetInputEnabled(false);
			GUI.Label(new Rect(Screen.width/2-51, Screen.height/100-20, 100, 100), "NEW HIGH SCORE!", EventHandler.main.myGUIStyle);
			//GUI.Label(new Rect(Screen.width/2-51, Screen.height/2+30, 100, 100), "NAME: "+ nameBuffer, EventHandler.main.myGUIStyle);
		}
		else EventHandler.SetInputEnabled(true);
	}

	
	public void NewScore(int score){
		int rank = CheckIfHigh(score);
		if (rank > -1){
			newScoreIndex = rank;
			EventHandler.AddScore(score);
		}
	}


	private void ReadHighScoreNames(){
		string[] scoreNames = new string[scoreboardSize];
		int scoreNum = 0;
		for (int i = 0; i < scoreboardSize; i++){
			scoreNames[i] = PlayerPrefs.GetString("HighScoreName"+scoreNum++);
		}
		this.names = scoreNames;
	}

	private void ReadHighScores(){
		int[] scores = new int[scoreboardSize];
		int scoreNum = 0;
		for (int i = 0; i < scoreboardSize; i++){
			scores[i] = PlayerPrefs.GetInt("HighScore"+scoreNum++);
		}
		this.scores = scores;
	}



	private int CheckIfHigh(int score){
		Debug.Log("High "+score);
		int ans = -1;
		for (int i = 0; i < scoreboardSize; i++){
			if (scores[i] < score){
				Debug.Log ("score "+i);
				ans = i;
				break;
			}
		}
		return ans;
	}



	public void AddScore(int score, string name){
		int index = CheckIfHigh(score);
		if (index == -1) Debug.LogError("This shouldn't happen.");	
		for (int i = scoreboardSize; i >= index; i--){
			if (i-1 >= 0 && i < scoreboardSize){
				scores[i] = scores[i-1];
				names[i] = names[i-1];
			}
		}
		scores[index] = score;
		names[index] = name;
		WriteScores();
		ReadHighScores();
		ReadHighScoreNames();
		GettingInput = false;
		newScoreIndex = -1;
		nameBuffer = "";
	}

	public void Cancel(){
		ReadHighScores();
		ReadHighScoreNames();
		GettingInput = false;
		newScoreIndex = -1;
		nameBuffer = "";
	}



	private void WriteScores(){
		for(int i = 0; i < scoreboardSize; i++){
			PlayerPrefs.SetInt("HighScore"+i, scores[i]);
			PlayerPrefs.SetString("HighScoreName"+i, names[i]);
		}
	}
	
}
