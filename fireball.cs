using UnityEngine;
using System.Collections;

public class fireball : MonoBehaviour
{
//	private Vector3 _dir = Vector3.zero;
	
	private int damage = 5;
			
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(destroy());
	}
	
	// Update is called once per frame
	void Update ()
	{
		//transform.Translate(_dir * 7 * Time.deltaTime, transform);
	}
	
	void set_dir(Vector3 dir)
	{
	/*	transform.rotation = dir;
		
		_dir = dir.eulerAngles;
		_dir.Normalize();*/
		
		rigidbody.velocity = dir * 10;
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
