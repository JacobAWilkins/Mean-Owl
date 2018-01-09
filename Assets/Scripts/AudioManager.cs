using UnityEngine;

[System.Serializable]
public class Sound {
	
	public string name;
	public AudioClip clip;

	[Range (0f, 1f)]
	public float volume = 1f;
	[Range (0.5f, 1.5f)]
	public float pitch = 1f;

	[Range (0f, 0.5f)]
	public float volumeRandom = 0.1f;
	[Range (0f, 0.5f)]
	public float pitchRandom = 0.1f;

	AudioSource source;

	public void SetSource (AudioSource _source) {
		source = _source;
		source.clip = clip;
	}

	public void Play () {
		source.volume = volume * (1 + Random.Range(-volumeRandom / 2f, volumeRandom / 2f));
		source.pitch = pitch * (1 + Random.Range(-pitchRandom / 2f, pitchRandom / 2f));
		source.Play ();
	}

}

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	[SerializeField]
	Sound [] sounds;

	void Awake () {
		instance = this;
	}

	void Start () {
		for (int i = 0; i < sounds.Length; i++) {
			GameObject _go = new GameObject ("Sounds_" + i + "_" + sounds[i].name);
			_go.transform.SetParent (this.transform);
			sounds [i].SetSource (_go.AddComponent<AudioSource> ());
		}
	}

	public void PlaySound (string _name) {
		for (int i = 0; i < sounds.Length; i++) {
			if (sounds [i].name == _name) {
				sounds [i].Play ();
				return;
			}
		}
		Debug.LogWarning ("Sound named '" + _name + "' not found in list");
	}

}
