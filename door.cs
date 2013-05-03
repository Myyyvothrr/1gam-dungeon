using UnityEngine;
using System.Collections;

public class door : MonoBehaviour
{
	private GameObject _left;
	private GameObject _right;
	
	private bool _opened = false;
	
	void Start()
	{
		_left = transform.Find("door_left").gameObject;
        _right = transform.Find("door_right").gameObject;
	}
	
	void Update ()
	{	
	}
	
	void open()
	{
		_left.animation.Play();
		_right.animation.Play();
		_opened = true;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (!_opened && other.gameObject.tag.Equals("Player"))
		{
			other.gameObject.SendMessage("update_near_door", true);
			other.gameObject.SendMessage("set_near_door", gameObject);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag.Equals("Player"))
		{
			other.gameObject.SendMessage("update_near_door", false);
		}
	}
}
