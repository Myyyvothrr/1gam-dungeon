using UnityEngine;
using System.Collections;

public class clouds_movement : MonoBehaviour
{
    public Vector3 up = Vector3.up;
    public float speed = 0.1f;

	void Start()
    {	
	}

	void Update()
    {
	    transform.RotateAround(up, speed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.SendMessage("receive_damage", 100000);
        }
    }
}
