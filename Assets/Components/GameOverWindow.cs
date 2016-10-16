using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverWindow : MonoBehaviour {

	public Text resources;
	public Text enemies;

	// Use this for initialization
	void Start() {
		gameObject.SetActive(false);
	}
	
	public void Show(int r, int e) {
		resources.text = r.ToString();
		enemies.text = e.ToString();
		gameObject.SetActive(true);
	}

	public void Restart() {
		AkSoundEngine.SetState("States", "Lose");
		Application.LoadLevel("scene");
	}
}
