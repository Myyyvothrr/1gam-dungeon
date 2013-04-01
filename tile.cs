using UnityEngine;
using System.Collections;

public class tile : MonoBehaviour
{
	public GameObject[] random_spawns;

	// Use this for initialization
	void Start ()
	{
		int r = Random.Range(0, 100);
		
		if (r > 40)
		{	
			GameObject o = (GameObject)Instantiate(random_spawns[Random.Range(0, random_spawns.Length)], transform.position, transform.rotation);
			o.transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
