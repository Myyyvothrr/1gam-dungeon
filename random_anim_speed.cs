using UnityEngine;
using System.Collections;

public class random_anim_speed : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		foreach (AnimationState state in animation)
        	state.speed = Random.Range(0.2f, 1f);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
