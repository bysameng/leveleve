using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour{

	private bool enableKeys = true;
	private bool enableMouse = false;
	private bool isVisible = true;
	private bool canSprint = true;

	private Vector3 mousePosition;

	public GameObject cursor;

	public float speed = 2f;
	public float sprintSpeed = 4f;

	private float minx = 1f;
	private float maxx = 99f;
	private float miny = 1f;
	private float maxy = 99f;

	// Use this for initialization
	void Start () {
		mousePosition = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isVisible) Screen.showCursor = false;
		if (enableMouse){
			mousePosition = Input.mousePosition;
			mousePosition.z = cursor.transform.position.z- Camera.main.transform.position.z;
			cursor.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);



		}
		if (enableKeys){
			Vector3 trajectory = new Vector3(0, 0, 0);
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.H)){
				trajectory.y = 1;
			}
			else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey (KeyCode.J)){
				trajectory.y = -1;
			}
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.K)){
				trajectory.x = -1;
			}
			else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.L)){
				trajectory.x = 1;			
			}
			trajectory.Normalize();
			if (canSprint && Input.GetKey(KeyCode.LeftShift)){
				trajectory = trajectory * sprintSpeed;
			}
			else trajectory = trajectory * speed;
			//Debug.Log (cursor.transform.position);
			Vector3 newPos = cursor.transform.position + trajectory;
			Debug.Log (newPos);
			if (!(newPos.x < minx || newPos.x > maxx || newPos.y < miny || newPos.y > maxy)){
				cursor.transform.Translate(trajectory);
			}
		
		}
	}


	public void SetEnable(bool isEnabled){
		enableMouse = isEnabled;
	}
}
