using UnityEngine;
using System.Collections;

public class fireball_boss : MonoBehaviour
{
    private int damage = 5;

    void Start()
    {
        StartCoroutine(destroy());
    }

    void Update()
    {
    }

    void set_player_pos(Vector3 pos)
    {
        Vector3 vel = pos - transform.position;
        vel.Normalize();

        rigidbody.velocity = vel * 25;
    }

    void set_damage(int dmg)
    {
        damage = dmg;
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.SendMessage("receive_damage", damage);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("weapon"))
            return;

        Destroy(gameObject);
    }
}
