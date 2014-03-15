using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

	private Font font;
	private float wait = 2f;

	private bool displayingtitle = true;

	public bool started = false;

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
	
	private string text = "leveleveleve";
	private string othermessage = "[ start ]";

	// Use this for initialization
	void Start () {
		font = (Font)Resources.Load("Fonts/Silkscreen/slkscr");
		StartCoroutine(DoTitle());
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if (started) return;
		if(displayingtitle)
			GUI.Label(new Rect(Screen.width/2-51, Screen.height/2-80, 100, 100), text, myGUIStyle);
		else{
			GUI.Label(new Rect(Screen.width/2-51, Screen.height/2-80, 100, 100), othermessage, myGUIStyle);
		}

	}

	IEnumerator DoTitle(){
		yield return new WaitForSeconds(wait);
		displayingtitle = false;
	}
}
