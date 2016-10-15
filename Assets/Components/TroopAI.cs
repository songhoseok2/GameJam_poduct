using UnityEngine;
using System.Collections;

public class TroopAI : MonoBehaviour {

  private enum State {
    Sleeping,
    Moving,
    Chasing,
    Searching
  }
  private State state;



	// Use this for initialization
	void Start()
  {

	}

	// Update is called once per frame
	void Update()
  {

    if (State == Sleeping)
    {
      // decrement some timer
      // if timer is up, switch to searching
    }

    else if (State == Moving)
    {
      // if reached destination, set status to searching
    }

    else if (State == Searching)
    {
      // find a target
      // if no target, go to sleep
      // otherwise, start chasing
    }

    else // chasing
    {
      // if target in attack range
        // if attackCooldown up
          // attack
        // else
          // increment attack cooldown
      // else
        // set navmesh destionation to target pos


      // if target is dead or out of chasing range
        // switch to searching
    }

	}

  private Enemy FindTarget()
  {

  }

  private void Sleep()
  {

  }



}
