using UnityEngine;
using System.Collections;

public class random_anim_speed : MonoBehaviour
{
	void Start()
	{
		foreach (AnimationState state in animation)
        	state.speed = Random.Range(0.2f, 1f);
	}
	
	void Update()
	{	
	}
}
