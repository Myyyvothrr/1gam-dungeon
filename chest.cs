using UnityEngine;
using System.Collections;

public class chest : MonoBehaviour
{
	private GameObject _coins;
	
	private int _amount = 0;

	void Start()
	{
		_coins = transform.Find("coins").gameObject;
    }
	
	void Update()
	{
	}
	
	void set_coins_amount(float amount)
	{
		_amount = (int)amount;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (_amount > 0 && other.gameObject.tag.Equals("Player"))
		{
			other.gameObject.SendMessage("pickup_coins", _amount);
			_amount = 0;
			Destroy(_coins);
		}
	}
}
