using UnityEngine;
using System.Collections;

public class tile_level_changer : MonoBehaviour
{
    public int type = -1;

	void Start()
    {	
	}
	
	void Update()
    {	
	}

    void OnTriggerEnter(Collider other)
    {
        if (transform.parent.gameObject.tag.Equals("exit") && other.tag.Equals("Player"))
        {
            other.SendMessage("level_changer_trigger", type);

            if (other.GetComponent<player>().dungeon_level > 5)
            {
                Application.LoadLevel(5);
                return;
            }

            Application.LoadLevel(1);
        }
        else if (transform.parent.gameObject.tag.Equals("teleporter") && other.tag.Equals("Player"))
        {
            other.SendMessage("level_changer_trigger", type);
            Application.LoadLevel(5);
        }
    }
}
