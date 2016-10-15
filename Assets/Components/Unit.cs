using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class Unit : MonoBehaviour {
  public int maxHp;
  public int attackPower;
  public float knockback;
  public bool canAttack;

  private int hp;

	// Use this for initialization
	void Start()
  {
    hp = maxHp;
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
