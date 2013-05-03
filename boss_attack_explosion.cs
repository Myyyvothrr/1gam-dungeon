using UnityEngine;
using System.Collections;

public class boss_attack_explosion : MonoBehaviour
{
    public int damage = 5;

	void Start()
    {
        Destroy(gameObject, Random.Range(5f, 15f));
	}
	
	void Update()
    {	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.SendMessage("receive_damage", damage);
            Destroy(gameObject);
        }
    }
}
