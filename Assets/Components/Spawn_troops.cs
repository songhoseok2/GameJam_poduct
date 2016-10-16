using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Spawn_troops : MonoBehaviour
{

	public Unit troop_to_spawn;
	public int spawnDist;

	char spawn_where = 'n';

	void OnMouseDown()
	{
		if (Utils.GameManager().resources >= troop_to_spawn.cost)//enough resource
		{
			switch (spawn_where)
			{
				case 'n':
					Instantiate(troop_to_spawn, transform.position + new Vector3(0, spawnDist, 0), transform.rotation);
					spawn_where = 'e';
					break;
				case 'e':
					Instantiate(troop_to_spawn, transform.position + new Vector3(spawnDist, 0, 0), transform.rotation);
					spawn_where = 'w';
					break;
				case 'w':
					Instantiate(troop_to_spawn, transform.position + new Vector3(-spawnDist, 0, 0), transform.rotation);
					spawn_where = 's';
					break;
				case 's':
					Instantiate(troop_to_spawn, transform.position + new Vector3(0, -spawnDist, 0), transform.rotation);
					spawn_where = 'n';
					break;
			}

			Utils.GameManager().resources =
				Utils.GameManager().resources - troop_to_spawn.cost;
		}

		else
		{
			Utils.GameManager().DisplayErrorMessage("You have insufficient resources.", 5);
		}

	}

	// Update is called once per frame
	void Update()
	{




	}
}
