using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public int resources;
	public List<Selectable> selected;

	void Update()
	{

		// Move selected TroopAI units
		if (Input.GetMouseButtonDown(1) && selected.Count > 0)
		{
			foreach (Selectable unit in selected)
			{
				if (unit.GetComponenInput.mousePositiont<TroopAI>() != null)
				{
					TroopAI troop = unit.GetComponent<TroopAI>();
					Vector3 dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					Debug.Log(troop.transform.position.z);
					dest.z = troop.transform.position.z;
					Debug.Log(dest);
					troop.MoveTo(dest);
				}
			}
		}

	}

}
