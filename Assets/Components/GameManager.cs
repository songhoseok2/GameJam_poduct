using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour
{
	public int resources;
	public List<Selectable> selected;
	public GameObject troopPrefab;
	public Text ResourceText;
	public Text NotEnoughResourcesText;
	public float cameraVelocity;
	public GameObject healthbarPrefab;

	public Unit[] unitMap;

	private int selectedUnit = 0;
	private float disappearTime = 3;
	private float messageTime = 3;

		void Start()
		{
				NotEnoughResourcesText.enabled = false;

				disappearTime = Time.time;
		}

		void displayMessageForSeconds(float seconds)
		{
			NotEnoughResourcesText.enabled = true;
			disappearTime = Time.time + 3;
			if(NotEnoughResourcesText.enabled && Time.time >= disappearTime)
			{
					NotEnoughResourcesText.enabled = false;
			}

		}

	void Update()
	{
		ResourceText.text = "Resource: " + resources.ToString();

		if (Input.GetKey(KeyCode.W))
		{
			Camera.main.transform.position += Vector3.up * cameraVelocity * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.A))
		{
			Camera.main.transform.position += Vector3.left * cameraVelocity * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.S))
		{
			Camera.main.transform.position += Vector3.down * cameraVelocity * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.D))
		{
			Camera.main.transform.position += Vector3.right * cameraVelocity * Time.deltaTime;
		}

		// Move selected TroopAI units
		if (Input.GetMouseButtonDown(1) && selected.Count > 0)
		{
			foreach (Selectable unit in selected)
			{
				if (unit.GetComponent<TroopAI>() != null)
				{
					TroopAI troop = unit.GetComponent<TroopAI>();
					Vector3 dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					dest.z = troop.transform.position.z;
					troop.MoveTo(dest);
				}
			}
		}

		else if (Input.GetMouseButtonDown(1))
		{
			Build(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}

	}

	public void UnitButtonClicked(int id)
	{
		Debug.Log(id);
	}

	public void Build(Vector2 position)
	{
		if (resources >= unitMap[selectedUnit].cost)
		{
			Vector3 pos = new Vector3(position.x, position.y, 0);
			Instantiate(unitMap[selectedUnit], pos, Quaternion.identity);
			resources -= unitMap[selectedUnit].cost;
		}
		else
		{
			displayMessageForSeconds(messageTime);
		}
	}

	public void SetSelection(int i)
	{
		if (0 <= i && i < unitMap.Length)
		{
			selectedUnit = i;
		}
		else
		{
			Debug.Log("Invalid index passed to GameManager.SetSelection");
		}
	}

}
