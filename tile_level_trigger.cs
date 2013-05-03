using UnityEngine;
using System.Collections;

public class tile_level_trigger : MonoBehaviour
{
	void Start()
    {
	}
	
	void Update()
    {	
	}

    void OnTriggerEnter(Collider other)
    {
        if (tag.Equals("exit") && other.tag.Equals("Player"))
            other.SendMessage("exit_triggered", true);
        else if (tag.Equals("teleporter") && other.tag.Equals("Player"))
            other.SendMessage("teleporter_triggered", true);
        else if (tag.Equals("teleporter_boss") && other.tag.Equals("Player"))
            other.SendMessage("teleporter_boss_triggered", true);
    }

    void OnTriggerExit(Collider other)
    {
        if (tag.Equals("exit") && other.tag.Equals("Player"))
            other.SendMessage("exit_triggered", false);
        else if (tag.Equals("teleporter") && other.tag.Equals("Player"))
            other.SendMessage("teleporter_triggered", false);
        else if (tag.Equals("teleporter_boss") && other.tag.Equals("Player"))
            other.SendMessage("teleporter_boss_triggered", false);
    }
}
