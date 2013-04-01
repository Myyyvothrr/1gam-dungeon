using UnityEngine;
using System.Collections;

public class health_potion : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
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
