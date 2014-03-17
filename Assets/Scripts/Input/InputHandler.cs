using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour{

	private bool enableKeys = true;
	private bool enableMouse = true;
	private bool enablePad = true;
	private bool isVisible = true;
	private bool canSlow = true;

	public int lastInputDevice {
		get; private set;
	}

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

	private float minx = 0f;
	private float maxx = 100f;
	private float miny = 0f;
	private float maxy = 100f;

	new public bool enabled = true;
	public bool fullEnabled = true;


	public string inputBuffer{
		get; private set;
	}

	public bool inputtingString = false;

	// Use this for initialization
	void Start () {

		QualitySettings.vSyncCount = 0;
		mousePosition = new Vector3(0, 0, 0);
	//	cursorController = cursor.GetComponent<CharacterController>();
		cursor = (GameObject)Instantiate(Resources.Load ("Prefabs/CursorMesh"), new Vector3(50, 30, 0), Quaternion.identity);
		cursorCollider = cursor.GetComponentInChildren<BoxCollider>();

		mousePosition = Input.mousePosition;
		lastMousePosition = mousePosition;
		mousePosition.z = cursor.transform.position.z;
	}
	
	// Update is called once per frame
	// do input stuff here
	void Update () {
		if(!fullEnabled) return;
		
		if (Input.GetKeyDown(KeyCode.Escape)){
			EventHandler.Dead();
			isVisible = false;
		}
		
		if (Input.GetButtonDown("Red") || Input.GetAxis ("GamepadLT") > 0){
			EventHandler.ChangeLense("Red", true);
		}
		
		if ((Input.GetButtonUp ("Red") || !(Input.GetAxis("GamepadLT") > 0)) && !Input.GetButton("Red") ){
			EventHandler.ChangeLense("Red", false);
		}
		
		if (Input.GetButtonDown("Blue") || Input.GetAxis ("GamepadRT") > 0){
			EventHandler.ChangeLense("Blue", true);
		}
		
		if ((Input.GetButtonUp ("Blue") || !(Input.GetAxis("GamepadRT") > 0)) && !Input.GetButton("Blue") ){
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
		enablePad = false;

		if (isVisible) Screen.showCursor = false;
		else Screen.showCursor = true;

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
			if (newPos.x < minx || newPos.x > maxx)
				trajectory.x = 0;
			if (newPos.y < miny || newPos.y > maxy)
				trajectory.y = 0;
			cursor.transform.Translate(trajectory);

			if (trajectory == Vector3.zero){
				enablePad = true;
			}
			else lastInputDevice = 0;
		}

		if (enablePad) {
			Vector3 trajectory = new Vector3(0,0,0);
			trajectory.x = Input.GetAxis("GamepadXLS");
			trajectory.y = -Input.GetAxis("GamepadYLS");
			if (trajectory == Vector3.zero){
				trajectory.x = Input.GetAxis("GamepadXRS");
				trajectory.y = -Input.GetAxis("GamepadYRS");
			}
			if (trajectory == Vector3.zero){
				trajectory.x = Input.GetAxis("GamepadXD");
				trajectory.y = Input.GetAxis("GamepadYD");
			}
			
			trajectory = trajectory * speed * 10 * Time.deltaTime;

			Vector3 newPos = cursor.transform.position + trajectory;
			if (newPos.x < minx || newPos.x > maxx)
				trajectory.x = 0;
			if (newPos.y < miny || newPos.y > maxy)
				trajectory.y = 0;
			cursor.transform.Translate(trajectory);

			if (trajectory == Vector3.zero){
				enableMouse = true;
			}
			else lastInputDevice = 1;
		}

		if (Input.mousePosition.x > Screen.width || Input.mousePosition.x < 0 ||
			Input.mousePosition.y > Screen.height || Input.mousePosition.y < 0) {
				enableMouse = false;
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
				lastInputDevice = 2;
			}

			if (deltaPos != Vector3.zero && lastInputDevice == 2){
				//Debug.Log (cursor.transform.position+ deltaPos);
				Debug.DrawLine(cursor.transform.position, origPos, Color.red, 5f);
				RaycastHit hit;
				if (Physics.Raycast(cursor.transform.position, deltaPos * -1f, out hit, deltaPos.magnitude)){
					Debug.Log ("raycast kill");
					hit.transform.gameObject.SendMessage("OnTriggerEnter", cursorCollider);
				}
			}

			lastMousePosition = mousePosition;

		}

	}

	

	public void SetEnable(bool isEnabled){
		enableMouse = isEnabled;
	}



	public void GetInput(){
		StartCoroutine(InputGetter());
	}



	IEnumerator InputGetter(){
		inputBuffer = "";
		if(inputtingString) yield break;
		inputtingString = true;
		while (inputtingString){
			foreach (char c in Input.inputString){
				if (c == "\b"[0]){
					if (inputBuffer.Length != 0){
						inputBuffer = inputBuffer.Substring(0, inputBuffer.Length-1);
					}
				}
				else if (c == "\n"[0] || c == "\r"[0]){
					inputtingString = false;
					Debug.LogError (inputtingString);
					yield break;
				}
				else if (inputBuffer.Length < 3)
					inputBuffer += c;
			}
			yield return null;
		}
		inputtingString = false;
	}

}
