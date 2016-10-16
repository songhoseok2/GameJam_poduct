using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class Unit : MonoBehaviour {
	public int maxHp;
	public int attackPower;
	public float knockback;
	public bool canAttack;

	public Vector3 healthbarScale = new Vector3(1, 1, 1);
	public Vector3 healthbarPos = new Vector3(0, 1, -1);
	public Vector3 healthbarAngles = new Vector3(0, 0, 0);

	public int cost = 0;

	private GameObject healthbar;
	private static float invisibleZ = -11;
	private float resetZ;

	private int hp;

	// Use this for initialization
	void Start()
	{
		hp = maxHp;
		Vector3 pos = transform.position + healthbarPos;
		resetZ = pos.z;
		pos.z = invisibleZ;
		healthbar = Instantiate(Utils.GameManager().healthbarPrefab, pos, Quaternion.identity) as GameObject;
		healthbar.transform.localScale = Vector3.Scale(
			healthbar.transform.localScale,
			healthbarScale
		);
		healthbar.transform.eulerAngles = healthbarAngles;
	}

	// Update is called once per frame
	void Update()
	{
		if (hp <= 0)
		{
			if (healthbar != null)
			{
				Destroy(healthbar);
			}
			Destroy(gameObject);
			if (gameObject.tag == "Enemy")
			{
				Utils.GameManager().EnemyKilled();
			}
			AkSoundEngine.PostEvent("Death", gameObject);
		}
		if (healthbar == null)
		{
			return;
		}
		healthbar.transform.position = transform.position + healthbarPos;
		if (hp == maxHp)
		{
			healthbar.transform.position = new Vector3(
				healthbar.transform.position.x,
				healthbar.transform.position.y,
				invisibleZ
			);
		}
		else
		{
			healthbar.transform.position = new Vector3(
				healthbar.transform.position.x,
				healthbar.transform.position.y,
				resetZ
			);
			float fHp = (float) hp;
			float fMax = (float) maxHp;
			Transform bar = healthbar.transform.GetChild(2);
			bar.localScale = new Vector3(
				fHp / fMax,
				bar.localScale.y,
				bar.localScale.z
			);
		}
	}

	// Causes this unit to attack the given unit
	public void Attack(Unit target)
	{
		if (!canAttack) {
			return;
		}
		// play attack animation
		target.Damage(attackPower);
		//Rigidbody2D targetRB = target.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
		//targetRB.isKinematic = false;
		//targetRB.AddForce(
		//	Vector3.Normalize(target.transform.position - transform.position) * knockback,
		//	ForceMode2D.Impulse
		//);
		//targetRB.isKinematic = true;
		AkSoundEngine.PostEvent("PlaySwords", gameObject);
	}

	// Causes the Unit to sustain x damage
	public void Damage(int x)
	{
		hp -= x;
	}

}
