using UnityEngine;
using System.Collections;

public class boss : MonoBehaviour
{
    public int hitpoints = 500;
    
    private GameObject _player;

    public Transform head_bone;

    public GameObject fireball_prefab;
    public GameObject fireball_left_spawnpoint;
    public GameObject fireball_right_spawnpoint;

    public int damage = 10;

    public GameObject mage_prefab;
    public GameObject mage_spawn_pos;

    public GameObject attack_explosion;

    private bool _killed = false;

    public GameObject player_prefab;

	void Start()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

        animation["Begin"].speed = 0.25f;
        animation["Idle"].speed = 0.5f;
        animation["Attack1"].speed = 0.25f;
        animation["Attack2"].speed = 0.5f;
        animation["Attack3"].speed = 0.25f;
        animation["Death"].speed = 0.5f;
        animation["Looking"].speed = 0.125f;

        _player = GameObject.Find("/player");
        if (_player == null)
        {
            _player = (GameObject)Instantiate(player_prefab);
            _player.name = "player";
        }

        _player.transform.position = new Vector3(0, 3, 105);
        _player.transform.rotation = Quaternion.Euler(0, 180, 0);
        
        StartCoroutine(wait_for_player());
	}
	
	void Update()
    {	
	}

    IEnumerator wait_for_player()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, _player.transform.position) > 195)
                yield return new WaitForSeconds(1f);
            else
                break;
        }

        animation.CrossFade("Begin", 0f);

        yield return new WaitForSeconds(0.01f);

        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;

        yield return new WaitForSeconds(10f);

        animation.CrossFade("Idle", 0.5f);

        StartCoroutine(attack());
    }

    IEnumerator attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2, 5));

            if (_killed)
                break;

            switch (Random.Range(0, 5))
            {
                case 0:
                    {
                        animation.CrossFade("Idle", 0.2f);
                        yield return new WaitForSeconds(Random.Range(1f, 3f));
                        break;
                    }
                case 1:
                    {
                        animation.CrossFade("Attack1", 0.2f);

                        yield return new WaitForSeconds(2.5f);

                        GameObject o = (GameObject)Instantiate(fireball_prefab, fireball_left_spawnpoint.transform.position, Quaternion.identity);
                        o.BroadcastMessage("set_player_pos", _player.transform.position);
                        o.BroadcastMessage("set_damage", damage);

                        o = (GameObject)Instantiate(fireball_prefab, fireball_right_spawnpoint.transform.position, Quaternion.identity);
                        o.BroadcastMessage("set_player_pos", _player.transform.position);
                        o.BroadcastMessage("set_damage", damage);

                        yield return new WaitForSeconds(2.75f);

                        break;
                    }
                case 2:
                    {
                        animation.CrossFade("Attack2", 0.2f);

                        yield return new WaitForSeconds(3f);

                        mage_spawn_pos.audio.Play();

                        yield return new WaitForSeconds(1f);

                        for (int i = 0; i < 10; ++i)
                        {
                            Vector3 pos = mage_spawn_pos.transform.position;
                            pos.x += Random.Range(-10f, 10f);
                            pos.y = 0f;
                            pos.z += Random.Range(-10f, 10f);
                            Instantiate(attack_explosion, pos, Quaternion.identity);
                        }

                        yield return new WaitForSeconds(0.05f);

                        break;
                    }
                case 3:
                    {
                        animation.CrossFade("Attack3", 0.2f);

                        yield return new WaitForSeconds(4f);

                        Instantiate(mage_prefab, mage_spawn_pos.transform.position, Quaternion.identity);

                        yield return new WaitForSeconds(1.5f);

                        break;
                    }
                case 4:
                    {
                        animation.CrossFade("Looking", 0.2f);
                        yield return new WaitForSeconds(15f);
                        break;
                    }
            }
        }
    }

   /* void LateUpdate()
    {
        head_bone.rotation = Quaternion.RotateTowards(head_bone.rotation, Quaternion.LookRotation(_player.transform.position - head_bone.position, Vector3.up), 360f);
    }*/

    void receive_damage(int amount)
    {
        if (_killed)
            return;

        hitpoints -= amount;

        if (hitpoints <= 0)
        {
            _killed = true;

            animation.CrossFade("Death", 0.1f);

            StartCoroutine(death());
        }
    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(8f);

        Destroy(gameObject);
        
        Application.LoadLevel(2);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_killed)
            return;

        if (other.gameObject.tag.Equals("Player"))
            other.gameObject.SendMessage("receive_damage", damage);
    }
}
