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

  private GameObject healthbar;
  private static float invisibleZ = -11;
  private float resetZ;
  public GameObject healthbarPrefab;

  private int hp;

	// Use this for initialization
	void Start()
  {
    hp = maxHp;
    Vector3 pos = transform.position + healthbarPos;
    resetZ = pos.z;
    healthbarPos.z = invisibleZ;
    healthbar = Instantiate(healthbarPrefab, pos, Quaternion.identity) as GameObject;
    healthbar.transform.parent = transform;
    healthbar.transform.localScale = healthbarScale;
  }

	// Update is called once per frame
	void Update()
  {
    if (hp <= 0)
    {
      Destroy(gameObject);
    }
    else if (hp == maxHp)
    {
      if (healthbar == null) {
        return;
      }
      healthbar.transform.position = new Vector3(
        healthbar.transform.position.x,
        healthbar.transform.position.y,
        invisibleZ
      );
    }
    else
    {
      if (healthbar == null) {
        return;
      }
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
