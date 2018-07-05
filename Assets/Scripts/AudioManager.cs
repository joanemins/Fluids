using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

  // public vars
  public static AudioManager instance;
  public AudioSource fxSource;
  public bool volume_;
	

	// preload components
	void Awake() {
		
		if (instance == null) {
			volume_ = true;
			instance = this;
		}
		else if (instance != this) { Destroy(gameObject); }
		DontDestroyOnLoad(gameObject);
	}

	// play a fx clip
	public void PlayFx(AudioClip fx) {

		if (volume_) {
			if (!fxSource.isPlaying) {
				fxSource.clip = fx;
				fxSource.Play ();
			}

		}
	}

	public void stopLoop(){
		fxSource.loop = false;
	}

	public void startLoop(){
		fxSource.loop = true;
	}

	public void StopMusic() {
		fxSource.Stop ();
	}
}