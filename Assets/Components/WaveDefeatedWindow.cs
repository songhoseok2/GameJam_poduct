using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveDefeatedWindow : MonoBehaviour {

	public Text enemiesDefeated;
	public Text resourcesEarned;
	public Text roundBonus;

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Show(int numEnemies, int numResources, int bonus) {
		enemiesDefeated.text = numEnemies.ToString();
		resourcesEarned.text = numEnemies.ToString();
		roundBonus.text = bonus.ToString();
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

}
