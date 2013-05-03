using UnityEngine;
using System.Collections;

public class tile : MonoBehaviour
{
	public GameObject random_spawn;

    public int random_props_max = 5;

    public GameObject[] light_prefabs = null;
    public GameObject[] random_props = null;
    public GameObject[] special_props = null;

    private GameObject _player;

    public bool dont_wait_spawning = false;

	void Start()
	{
        _player = GameObject.Find("/player");

        if (dont_wait_spawning)
            _spawn();
        else
            StartCoroutine(spawn());
	}
	
	void Update()
	{	
	}

    void set_random_spawn(GameObject obj)
    {
        random_spawn = obj;
    }

    void _spawn()
    {
        GameObject obj;
        Vector3 pos;
        float s;
        Quaternion rot;

        if (light_prefabs.Length > 0 && Random.Range(0f, 1f) > 0.4f)
        {
            //rot = transform.rotation *Quaternion.AngleAxis(90, Vector3.up);
            obj = (GameObject)Instantiate(light_prefabs[Random.Range(0, light_prefabs.Length)], transform.position, transform.rotation);
            obj.transform.parent = transform;
        }

        if (random_props.Length > 0)
        {
            for (int i = 0; i < random_props.Length; ++i)
            {
                for (int j = 0; j < Random.Range(1, random_props_max); ++j)
                {
                    pos = new Vector3(Random.Range(-5, 5), Random.Range(0f, 0.15f), Random.Range(-5, 5));
                    rot = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);

                    obj = (GameObject)Instantiate(random_props[i], transform.position + pos, rot);
                    s = Random.Range(0.3f, 1.3f);
                    obj.transform.localScale = new Vector3(s, s, s);
                    obj.transform.parent = transform;
                }
            }
        }

        if (special_props.Length > 0 && Random.Range(0f, 1f) > 0.5f)
        {
            for (int i = 0; i < special_props.Length; ++i)
            {
                pos = new Vector3(Random.Range(-5, 5), Random.Range(0f, 0.15f), Random.Range(-5, 5));
                rot = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);

                obj = (GameObject)Instantiate(special_props[i], transform.position + pos, rot);
                s = Random.Range(0.3f, 1.3f);
                obj.transform.localScale = new Vector3(s, s, s);
                obj.transform.parent = transform;
            }
        }

        if (random_spawn != null)
        {
            GameObject o = (GameObject)Instantiate(random_spawn, transform.position, transform.rotation);
            //o.transform.parent = transform;
        }

        //   StaticBatchingUtility.Combine(gameObject);
    }

    IEnumerator spawn()
    {
        while (_player != null)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) > 100)
            {
                yield return new WaitForSeconds(2);
            }
            else
            {
                _spawn();
                break;
            }
        }
    }
}
