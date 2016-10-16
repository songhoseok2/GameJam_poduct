using UnityEngine;
using System.Collections;

public class MusicStarter : MonoBehaviour {
	private static bool started = false;
	void Start() {
		if (!started) {
			AkSoundEngine.PostEvent("InitializeMusic", gameObject);
			started = true;
		}
	}
}
