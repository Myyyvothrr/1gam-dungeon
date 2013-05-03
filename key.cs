using UnityEngine;
using System.Collections;

public class key : MonoBehaviour
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
			other.gameObject.SendMessage("pickup_key", 1);
			Destroy(gameObject);
		}
	}
}
