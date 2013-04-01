using UnityEngine;
using System.Collections;

public class coins : MonoBehaviour
{
	public GameObject coin_prefab;

	// Use this for initialization
	void Start ()
	{
		int r = Random.Range (10, 50);
		for (int i = 0; i < r; ++i)
		{
			Vector3 rv =new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
			GameObject o = (GameObject)Instantiate(coin_prefab, transform.position + rv, transform.rotation);
			o.transform.parent = transform;
		}
		
		gameObject.transform.parent.gameObject.BroadcastMessage("set_coins_amount", r);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
