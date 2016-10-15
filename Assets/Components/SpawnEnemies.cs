using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject EnemyToSpawn;


    public float spawnInterval;
    private float next_spawn;

    IEnumerator wait(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
    }
    
    // Use this for initialization
    void Start ()
    {
        next_spawn = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Time.time >= next_spawn)
        {
            Instantiate(EnemyToSpawn,
                new Vector3(Random.Range(transform.position.x - 5, transform.position.x + 5), Random.Range(transform.position.y - 5, transform.position.y + 5), transform.position.z),
                Quaternion.identity);

            next_spawn = Time.time + spawnInterval;
        }

        wait(5);
    }
}
