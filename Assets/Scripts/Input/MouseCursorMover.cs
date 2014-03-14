using UnityEngine;
using System.Collections;

public class MouseCursorMover : MonoBehaviour{

	private bool enableMouse = true;
	private bool isVisible = true;

	private Vector3 mousePosition;

	public GameObject cursor;

	// Use this for initialization
	void Start () {
		mousePosition = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (enableMouse){
			if (isVisible) Screen.showCursor = false;
			mousePosition = Input.mousePosition;
			mousePosition.z = cursor.transform.position.z- Camera.main.transform.position.z;
			cursor.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
		}
	}


	public void SetEnable(bool isEnabled){
		enableMouse = isEnabled;
	}
}
