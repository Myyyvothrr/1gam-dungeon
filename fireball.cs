using UnityEngine;
using System.Collections;

public class fireball : MonoBehaviour
{	
	private int damage = 5;
   
	void Start()
	{
        StartCoroutine(destroy());
	}
	
	void Update()
	{
	}
	
	void set_dir(Vector3 dir)
	{		
		rigidbody.velocity = dir * 10;
	}

    void set_damage(int dmg)
    {
        damage = dmg;
    }
	
	IEnumerator destroy()
	{
		yield return new WaitForSeconds(5);
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
