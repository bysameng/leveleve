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


	

	public void Start(){
		ReadHighScores();
		ReadHighScoreNames();
	}

	void OnGUI(){
		if (GettingInput){
			EventHandler.SetInputEnabled(false);
			GUI.Label(new Rect(Screen.width/2-51, Screen.height/2+50, 100, 100), "NEW HIGH SCORE!", EventHandler.main.myGUIStyle);
			GUI.Label(new Rect(Screen.width/2-51, Screen.height/2+70, 100, 100), "NAME: "+ EventHandler.iHandler.inputBuffer, EventHandler.main.myGUIStyle);
		}
	}

	
	public void NewScore(int score){
		int rank = CheckIfHigh(score);
		if (rank > -1){
			EventHandler.AddScore(score);
		}
	}


	private void ReadHighScoreNames(){
		string[] scoreNames = new string[scoreboardSize];
		int scoreNum = 0;
		for (int i = 0; i < scoreboardSize; i++){
			scoreNames[i] = PlayerPrefs.GetString("HighScoreName"+scoreNum);
		}
		this.names = scoreNames;
	}

	private void ReadHighScores(){
		int[] scores = new int[scoreboardSize];
		int scoreNum = 0;
		for (int i = 0; i < scoreboardSize; i++){
			scores[i] = PlayerPrefs.GetInt("HighScore"+scoreNum);
		}
		this.scores = scores;
	}


	private int CheckIfHigh(int score){
		Debug.Log("High "+score);
		int ans = -1;
		for (int i = 0; i < scoreboardSize; i++){
			if (scores[i] < score){
				ans = i;
			}
		}
		return ans;
	}

	public void AddScore(int score, string name){
		int index = CheckIfHigh(score);
		scores[0] = score;
		names[0] = name;
		for (int i = index; i < scoreboardSize; i++){
			if (i-1 >= 0){
				scores[i] = scores[i-1];
				names[i] = names[i-1];
			}
		}
		scores[index] = score;
		WriteScores();
		ReadHighScores();
		ReadHighScoreNames();
	}

	private void WriteScores(){
		for(int i = 0; i < scoreboardSize; i++){
			PlayerPrefs.SetInt("HighScore"+i, scores[i]);
			PlayerPrefs.SetString("HighScoreName"+i, names[i]);
		}
	}
	
}
