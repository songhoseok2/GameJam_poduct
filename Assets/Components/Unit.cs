using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class Unit : MonoBehaviour {
  public int maxHp;
  public int attackPower;
  public float knockback;
  public bool canAttack;
  public Vector3 healthbarScale = new Vector3(0, 0, 0);

  private GameObject healthbar;
  public GameObject healthbarPrefab;

  private int hp;

	// Use this for initialization
	void Start()
  {
    hp = maxHp;
    if (!(healthbarScale == Vector3.zero))
    {
      Vector3 healthbarPos = transform.position;
      healthbarPos.y += 1;
      healthbarPos.z = -1;
      healthbar = Instantiate(healthbarPrefab, healthbarPos, Quaternion.identity) as GameObject;
      healthbar.transform.parent = transform;
      healthbar.transform.localScale = healthbarScale;
    }
  }

	// Update is called once per frame
	void Update()
  {
    if (hp <= 0) {
      Destroy(gameObject);
    }
	}

  // Causes this unit to attack the given unit
  public void Attack(Unit target)
  {
    if (!canAttack) {
      return;
    }
    // play attack animation
    Debug.Log(name + " attacks " + target.name + " for " + attackPower + " damage");
    target.Damage(attackPower);
    Rigidbody2D targetRB = target.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
    targetRB.isKinematic = false;
    targetRB.AddForce(
      Vector3.Normalize(target.transform.position - transform.position) * knockback,
      ForceMode2D.Impulse
    );
    targetRB.isKinematic = true;
  }

  // Causes the Unit to sustain x damage
  public void Damage(int x)
  {
    hp -= x;
    Debug.Log(name + " sustained " + x + " damage");
  }

}
