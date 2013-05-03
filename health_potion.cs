using UnityEngine;
using System.Collections;

public class health_potion : MonoBehaviour
{
	void Start()
    {
	}
	
	void Update()
	{	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals("Player"))
		{
			other.gameObject.SendMessage("pickup_health_potion", 1);
			Destroy(gameObject);
		}
	}
}
