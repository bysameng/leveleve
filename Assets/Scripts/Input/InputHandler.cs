using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour{

	private bool enableKeys = true;
	private bool enableMouse = true;
	private bool isVisible = true;
	private bool canSlow = true;

	private Vector3 mousePosition;
	private Vector3 lastMousePosition;

	public GameObject cursor{
		get; private set;
	}

	public BoxCollider cursorCollider{
		get; private set;
	}
//	private CharacterController cursorController;

	public float speed = 20f;
	public float slowSpeed = 10f;

	private float minx = 1f;
	private float maxx = 99f;
	private float miny = 1f;
	private float maxy = 99f;

	new public bool enabled = true;

	// Use this for initialization
	void Start () {

		QualitySettings.vSyncCount = 0;
		mousePosition = new Vector3(0, 0, 0);
	//	cursorController = cursor.GetComponent<CharacterController>();
		cursor = (GameObject)Instantiate(Resources.Load ("Prefabs/CursorMesh"), new Vector3(50, 30, 0), Quaternion.identity);
		cursorCollider = cursor.GetComponentInChildren<BoxCollider>();
	}
	
	// Update is called once per frame
	// do input stuff here
	void Update () {

		if (Input.GetKeyDown(KeyCode.Escape)){
			EventHandler.Dead();
		}
		
		if (Input.GetButtonDown("Red")){
			EventHandler.ChangeLense("Red", true);
		}
		
		if (Input.GetButtonUp ("Red")){
			EventHandler.ChangeLense("Red", false);
		}
		
		if (Input.GetButtonDown("Blue")){
			EventHandler.ChangeLense("Blue", true);
		}
		
		if (Input.GetButtonUp ("Blue")){
			EventHandler.ChangeLense("Blue", false);
		}

		if (!enabled){
			cursorCollider.enabled = false;
			if (Input.GetButtonDown ("Restart")){
				EventHandler.Restart();
			}
			return;
		}

		enableKeys = true; 
		enableMouse = false;
		if (isVisible) Screen.showCursor = false;

		if (enableKeys){
			Vector3 trajectory = new Vector3(0, 0, 0);
			if (Input.GetButton("Up") || Input.GetKey(KeyCode.K)){
				trajectory.y = 1;
			}
			else if (Input.GetButton("Down") || Input.GetKey (KeyCode.J)){
				trajectory.y = -1;
			}
			if (Input.GetButton("Left") || Input.GetKey(KeyCode.H)){
				trajectory.x = -1;
			}
			else if (Input.GetButton("Right") || Input.GetKey(KeyCode.L)){
				trajectory.x = 1;			
			}
			trajectory.Normalize();
			if (canSlow && Input.GetKey(KeyCode.LeftShift)){
				trajectory = trajectory * slowSpeed * Time.deltaTime;;
			}
			else trajectory = trajectory * speed * Time.deltaTime;
			//Debug.Log (cursor.transform.position);
			Vector3 newPos = cursor.transform.position + trajectory;
			//Debug.Log (newPos);
			if (!(newPos.x < minx || newPos.x > maxx || newPos.y < miny || newPos.y > maxy)){
				cursor.transform.Translate(trajectory);
			}
			if (trajectory == Vector3.zero)
				enableMouse = true;
		}

		if (enableMouse){
			Vector3 origPos = cursor.transform.position;

			cursorCollider.center = Vector3.zero;
			cursorCollider.transform.rotation = Quaternion.identity;
			cursorCollider.transform.localScale = new Vector3(1,1,1); 
			mousePosition = Input.mousePosition;
			mousePosition.z = cursor.transform.position.z- Camera.main.transform.position.z;
			Vector3 deltaPos = Camera.main.ScreenToWorldPoint(mousePosition) - cursor.transform.position;
			//Debug.Log (deltaPos);


			if (mousePosition != lastMousePosition && deltaPos != Vector3.zero){
				cursor.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
				//cursorCollider.center = (deltaPos * -1) / 2; //midpoint of movement
				float angle = Vector3.Angle(new Vector3(1,0,0), deltaPos);
				if(deltaPos.y < 0){
					angle = -angle;
				}
				//cursorCollider.transform.Rotate(new Vector3(0, 0, angle));
				//cursorCollider.transform.localScale = new Vector3(deltaPos.magnitude, 1, 1);


			}

			if (deltaPos != Vector3.zero){
				//Debug.Log (cursor.transform.position+ deltaPos);
				Debug.DrawLine(cursor.transform.position, origPos, Color.red, 5f);
				RaycastHit hit;
				if (Physics.Raycast(cursor.transform.position, deltaPos * -1f, out hit, deltaPos.magnitude)){
					hit.transform.gameObject.SendMessage("OnTriggerEnter", cursorCollider);
				}
			}

			lastMousePosition = mousePosition;
		}


	}


	public void SetEnable(bool isEnabled){
		enableMouse = isEnabled;
	}
}
