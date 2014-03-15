using UnityEngine;
using System.Collections;

public class DebuggerGUI : MonoBehaviour {

	public Font font;

	private GUIStyle _myGUIStyle;
	public GUIStyle myGUIStyle{
		get{
			if (_myGUIStyle == null){
				_myGUIStyle = new GUIStyle();
				_myGUIStyle.font = font;
			}
			return _myGUIStyle;
		}
	}

	public string errmessage;
	public string rotation;
	public string currentBoxes;

	new public bool enabled = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if (enabled)
			GUI.Label(new Rect(Screen.width/100, Screen.height/100, Screen.width, 100), "[Debugger]\n" + "rotation: "+rotation +"\nwallcount: "+ currentBoxes+ errmessage, myGUIStyle);
	}

	public void addErrorMessage(string err){
		errmessage = errmessage + "\n" + err;
	}
}
