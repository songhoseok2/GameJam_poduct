using UnityEngine;
using System;
using System.Collections;

[RequireComponent (typeof(Unit))]
public class TroopAI : MonoBehaviour
{

	public float chaseRange;
	public string targetTag;
	public float attackRange;

	public float idleRotateSpeed = 45;

	// Time between attacks
	public float attackCooldown;

	public float sleepLength;

	// Maximum distance where the Troop close enough to the clicked location for
	// moving to be complete
	public float movingRange;

	public float velocity;

	private float nextAttack;

	private float wakeUp;

	private GameObject chasingTarget;

	private Vector3 movingTarget;

	private enum State
	{
		Sleeping,
		Moving,
		Chasing,
		Searching
	}
	private State state;

	// Use this for initialization
	void Start()
	{
		state = State.Searching;
		nextAttack = Time.time;
		wakeUp = Time.time;
	}

	// Update is called once per frame
	void Update()
	{

		if (state == State.Sleeping)
		{
			if (Time.time >= wakeUp) {
				state = State.Searching;
			}
			transform.eulerAngles = new Vector3(
				transform.rotation.x,
				transform.rotation.y,
				transform.rotation.z + idleRotateSpeed * Time.deltaTime
			);
		}

		else if (state == State.Moving)
		{

			float dist = Vector3.Distance(
				movingTarget,
				transform.position
			);

			if (dist <= movingRange)
			{
				state = State.Searching;
			}

			else
			{
				transform.position = Vector3.MoveTowards(
					transform.position,
					movingTarget,
					Time.deltaTime * velocity
				);
				transform.eulerAngles = new Vector3(
					transform.rotation.x,
					transform.rotation.y,
					calcAngle(movingTarget)
				);
			}

		}

		else if (state == State.Searching)
		{

			// Find closest enemy in range
			GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
			GameObject target = null;
			float minDist = Mathf.Infinity;
			foreach (GameObject enemy in enemies)
			{
				float dist = Vector3.Distance(
					enemy.transform.position, transform.position
				);
				if (dist <= chaseRange && dist < minDist)
				{
					target = enemy;
					minDist = dist;
				}
			}

			// If a target was found, switch state to chasing
			if (target != null) {
				chasingTarget = target;
				state = State.Chasing;
			}

			// Otherwise, go to sleep
			else {
				wakeUp = Time.time + sleepLength;
				state = State.Sleeping;
			}

		}

		else if (state == State.Chasing)
		{

			if (chasingTarget != null)
			{

				// Distance between troop and target
				float dist = Vector3.Distance(
					chasingTarget.transform.position,
					transform.position
				);

				// If target is out of chase range, give up
				if (dist >= chaseRange)
				{
					chasingTarget = null;
					state = State.Searching;
				}

				// If target is in attack range and cooldown is up, attack
				else if (dist <= attackRange)
				{
					if (Time.time >= nextAttack)
					{
						Unit myUnit = GetComponent(typeof(Unit)) as Unit;
						Unit targetUnit = chasingTarget.GetComponent(typeof(Unit)) as Unit;
						myUnit.Attack(targetUnit);
						nextAttack = Time.time + attackCooldown;
					}
				}

				// Move toward the target
				else
				{
					transform.position = Vector3.MoveTowards(
						transform.position,
						chasingTarget.transform.position,
						Time.deltaTime * velocity
					);
					transform.eulerAngles = new Vector3(
						transform.eulerAngles.x,
						transform.eulerAngles.y,
						calcAngle(chasingTarget.transform.position)
					);
				}

			}

			else
			{
				state = State.Searching;
			}

		}

	}

	public void MoveTo(Vector2 loc)
	{
		state = State.Moving;
		movingTarget = new Vector3(loc.x, loc.y, transform.position.z);
		chasingTarget = null;
	}

	float calcAngle(Vector3 dest)
	{
		Vector2 difference = dest - transform.position;
		double angle = Math.Atan2(difference.x, -difference.y);
		return (float) (angle * (180.0 / Math.PI));
	}

}
