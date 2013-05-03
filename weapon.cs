using UnityEngine;
using System.Collections;

public class weapon : MonoBehaviour
{
	public int damage = 1;
	
	public GameObject hit_prefab;
	
	private bool _swinging = false;
	
	public AudioClip _sound_hit;
	
	void Start()
	{	
	}
	
	void Update()
	{	
	}
	
	void can_attack(bool attack)
	{
		_swinging = attack;
	}
	
	void set_damage(int amount)
	{
		damage = amount;
	}
	
	void OnCollisionEnter(Collision other)
	{
		if (_swinging && other.gameObject.tag.Equals("enemy"))
		{
			for (int i = 0; i < other.contacts.Length; ++i)
			{			
				GameObject o = (GameObject)Instantiate(hit_prefab, other.contacts[i].point, Quaternion.LookRotation(other.contacts[i].normal));
				o.transform.parent = other.transform;
				Destroy(o, 2);
			}
			
			audio.PlayOneShot(_sound_hit);	
			
			other.gameObject.SendMessageUpwards("receive_damage", damage);
			_swinging = false;
		}
	}
}
