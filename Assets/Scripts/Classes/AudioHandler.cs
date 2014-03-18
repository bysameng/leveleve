using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {


	private AudioSource aSource;
	public AudioClip exitSound;
	public AudioClip redLensSound;
	public AudioClip blueLensSound;
	public AudioClip deathSound;


	// Use this for initialization
	void Start () {
		aSource = gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Play(){
		aSource.Play();
	}

	public void PlayExitSound(Vector3 position){
		if(exitSound != null){
			AudioSource.PlayClipAtPoint(exitSound, position);
		}
	}

	public void PlayDeathSound(Vector3 position){
		if(deathSound != null){
			AudioSource.PlayClipAtPoint(deathSound, position);
		}
	}

}
