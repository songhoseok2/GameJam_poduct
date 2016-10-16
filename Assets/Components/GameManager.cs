using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour {

	public int resources;
	public Text resourceText;
	public Text errorText;
	public float cameraVelocity = 10;
	public GameObject healthbarPrefab;
	public Unit[] unitMap;
	public Unit[] enemyMap;
	public int[] enemyDifficulties;
	public float enemySpawnInterval = 5;
	public float intermissionLength = 30;
	public string enemySpawnLocationTag = "Spawner";
	public Text statusText;
	public List<Button> unitButtons;
	public WaveDefeatedWindow waveDefeatedWindow;
	public GameOverWindow gameOverWindow;
	public int waveBonusScale = 10;
	public List<Unit> intensityUnits;
	public List<Narrate.NarrationList> dialog;

	private enum State {
		Combat,
		Intermission
	}

	private float minX = -6;
	private float maxX = 6;
	private float minY = -7;
	private float maxY = 7;

	private List<Selectable> selected = new List<Selectable>();
	private State state;
	private int selectedUnit = 0;
	private float errorMessageFadeTime;
	private int waveNumber = 0;
	private float nextWaveTime;
	private int[] waveEnemies;
	private int enemiesRemaining;
	private int enemiesRemainingToSpawn;
	private float nextEnemyTime;
	private int initialEnemiesRemaining;
	private System.Random random = new System.Random();
	private int resourcesEarnedInRound = 0;
	private int totalResourcesEarned = 0;
	private int totalEnemiesDefeated = 0;

	void Start() {
		state = State.Intermission;
		nextWaveTime = Time.time + (intermissionLength * 3);
		prepareWave();
		errorText.enabled = false;
		errorMessageFadeTime = Time.time;
		nextEnemyTime = Time.time;
		SetSelection(0);
		AkSoundEngine.SetState("States", "Prepare");
		if (waveNumber < dialog.Count) {
			dialog[waveNumber].Play();
		}
	}

	void Update() {
		resourceText.text = "Resources: " + resources.ToString();

		transform.position = new Vector3(
				Mathf.Clamp(transform.position.x, minX, maxX),
				Mathf.Clamp(transform.position.y, minY, maxY),
				-10
		);

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			Camera.main.transform.position += Vector3.up * cameraVelocity * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			Camera.main.transform.position += Vector3.left * cameraVelocity * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			Camera.main.transform.position += Vector3.down * cameraVelocity * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			Camera.main.transform.position += Vector3.right * cameraVelocity * Time.deltaTime;
		}

		// Move selected TroopAI units
		if (Input.GetMouseButtonDown(1) && selected.Count > 0) {
			foreach (Selectable unit in selected) {
				if (unit.GetComponent<TroopAI>() != null) {
					TroopAI troop = unit.GetComponent<TroopAI>();
					Vector3 dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					dest.z = troop.transform.position.z;
					troop.MoveTo(dest);
				}
			}
			AkSoundEngine.PostEvent("PlayerMove", gameObject);
		}

		else if (Input.GetMouseButtonDown(1)) {
			Build(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}

		if (errorText.enabled && Time.time >= errorMessageFadeTime) {
			errorText.enabled = false;
		}

		// Spawn enemies
		if (state == State.Combat && Time.time >= nextEnemyTime) {
			if (enemiesRemainingToSpawn >= 1) {
				GameObject[] spawns = GameObject.FindGameObjectsWithTag(enemySpawnLocationTag);
				Vector3 loc = spawns[random.Next(0, spawns.Length)].transform.position;
				int enemyIndex = random.Next(0, enemyMap.Length); // BAD
				while (waveEnemies[enemyIndex] <= 0) // BAD
				{ // BAD
					enemyIndex = random.Next(0, enemyMap.Length); // BAD
				}
				waveEnemies[enemyIndex]--;
				enemiesRemainingToSpawn--;
				Instantiate(enemyMap[enemyIndex], loc, Quaternion.identity);
				nextEnemyTime = Time.time + enemySpawnInterval;
			}
		}

		// Update status text
		if (state == State.Combat) {
			statusText.text = "Enemies remaining: " + enemiesRemaining;
			updateIntensity();
		}
		else {
			TimeSpan t = TimeSpan.FromSeconds(nextWaveTime - Time.time);
			statusText.text = t.Minutes + ":" + t.Seconds + " until next wave";
		}

		// State flow
		if (state == State.Intermission && Time.time >= nextWaveTime) {
			state = State.Combat;
			AkSoundEngine.SetState("States", "Battle");
		}

		else if (state == State.Combat && enemiesRemaining <= 0) {
			nextWaveTime = Time.time + intermissionLength;
			state = State.Intermission;
			if (waveNumber % 2 == 1) {
				AkSoundEngine.SetState("States", "Prepare");
			}
			else {
				AkSoundEngine.SetState("States", "Menu");
			}
			waveDefeatedWindow.Show(
				initialEnemiesRemaining, resourcesEarnedInRound, waveNumber * waveBonusScale
			);
			resources += waveNumber * waveBonusScale;
			totalResourcesEarned += waveNumber * waveBonusScale;
      waveNumber++;
			if (waveNumber < dialog.Count) {
				dialog[waveNumber].Play();
			}
			prepareWave();
		}

	}

	public void Build(Vector2 position) {
		if (state == State.Combat) {
			DisplayErrorMessage("You cannot build during combat.", 1);
			return;
		}
		if (resources >= unitMap[selectedUnit].cost) {
			Vector3 pos = new Vector3(position.x, position.y, 0);
			Instantiate(unitMap[selectedUnit], pos, Quaternion.identity);
			resources -= unitMap[selectedUnit].cost;
			AkSoundEngine.PostEvent("Purchase", gameObject);
		}
		else {
			DisplayErrorMessage("You have insufficient resources.", 5);
		}
	}

	public void SetSelection(int i) {
		if (0 <= i && i < unitMap.Length) {
			unitButtons[selectedUnit].interactable = true;
			selectedUnit = i;
			unitButtons[i].interactable = false;
		}
		else {
			Debug.Log("Invalid index passed to GameManager.SetSelection");
		}
	}

	public void DisplayErrorMessage(string error, float length) {
		AkSoundEngine.PostEvent("Error", gameObject);
		errorText.text = error;
		errorText.enabled = true;
		errorMessageFadeTime = Time.time + length;
	}

	private void prepareWave() {
		waveEnemies = new int[] { (waveNumber + 1) * (waveNumber + 1) };
		enemiesRemaining = 0;
		foreach (int i in waveEnemies) {
			enemiesRemaining += i;
		}
		enemiesRemainingToSpawn = enemiesRemaining;
		initialEnemiesRemaining = enemiesRemaining;
		resourcesEarnedInRound = 0;
	}

	public void EnemyKilled(int reward) {
		resources += reward;
		resourcesEarnedInRound += reward;
		enemiesRemaining--;
		totalEnemiesDefeated++;
		totalResourcesEarned += reward;
	}

	public void SetSelected(List<Selectable> s) {
		selected = s;
	}

	public void GameOver() {
		gameOverWindow.Show(totalResourcesEarned, totalEnemiesDefeated);
	}

	void updateIntensity() {
		float troopProp = ((float) 1) - ((float)enemiesRemaining) / ((float)initialEnemiesRemaining);
		float num = 0;
		float den = 0;
		foreach (Unit u in intensityUnits) {
			num += u.GetHp();
			den += u.maxHp;
		}
		float castleProp = num / den;
		float intensity = (troopProp * 0.5f) + (castleProp * 0.5f);
		Debug.Log(intensity);
		AkSoundEngine.SetRTPCValue("Intensity", intensity, gameObject);
	}


}
