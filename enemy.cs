using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour
{
	private int hitpoints = 5;
    private int damage = 5;
	
	public GameObject fireball_prefab;
	
	private GameObject _player;
	private GameObject _attack_spawnpoint;
	
	private Vector3 _temp_rot;
	
	private bool _killed = false;

    public GameObject[] loot_prefabs;
   
	void Start()
	{		
		_attack_spawnpoint = transform.Find("attack_spawnpoint").gameObject;
		_player = GameObject.Find("/player");

        if (_player != null)
        {
            int difficulty = _player.GetComponent<player>().dungeon_level;
            hitpoints *= difficulty;
            damage *= difficulty;

            StartCoroutine(attack());
        }

        animation.Play("Idle");
	}
	
	void Update ()
	{		
		if (_player == null)
			return;
		
		if (_killed)
			return;
		
		if (Vector3.Distance(transform.position, _player.transform.position) <= 20)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_player.transform.position - transform.position), 200 * Time.deltaTime);
			
			_temp_rot = transform.rotation.eulerAngles;
			_temp_rot.x = 0;
			transform.rotation = Quaternion.Euler(_temp_rot);
		}
		
	}
	
	void FixedUpdate()
	{	
		if (_player == null)
			return;
		
		if (_killed)
			return;
		
		rigidbody.MovePosition(Vector3.MoveTowards(transform.position, _player.transform.position, 0.7f * Time.deltaTime));
	}
	
	void receive_damage(int amount)
	{
		if (_killed)
			return;
		
		hitpoints -= amount;
		
		if (hitpoints <= 0)
		{			
			_player.BroadcastMessage("killed_enemy", 10);
			
			_killed = true;
		
			animation.CrossFade("Death", 0.1f);
			
			audio.Play();
			
			StartCoroutine(death());
		}
	}
	
	IEnumerator death()
	{
        yield return new WaitForSeconds(0.5f);

        if (Random.Range(0f, 1f) > 0.4f)
            Instantiate(loot_prefabs[Random.Range(0, loot_prefabs.Length)], transform.position, Quaternion.identity);

		yield return new WaitForSeconds(5f);

        Destroy(gameObject);
	}
	
	IEnumerator attack()
	{
		while (true)
		{			
			yield return new WaitForSeconds(Random.Range(3, 9));

            if (_killed || _player == null)
				break;
			
			if (Vector3.Distance(transform.position, _player.transform.position) <= 20)
			{
				animation.CrossFade("Attack", 0.2f);
				
				yield return new WaitForSeconds(0.5f);

                if (_killed || _player == null)
					break;
	
				GameObject o = (GameObject)Instantiate(fireball_prefab, _attack_spawnpoint.transform.position, Quaternion.identity);
                o.BroadcastMessage("set_dir", transform.forward);
                o.BroadcastMessage("set_damage", damage);
							
				yield return new WaitForSeconds(0.5f);

                if (_killed || _player == null)
					break;
				
				animation.CrossFade("Idle", 0.2f);
			}
		}
	}
}
