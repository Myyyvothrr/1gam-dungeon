using UnityEngine;
using System.Collections;

public class credits : MonoBehaviour
{	
	public GUISkin _menu_skin;
	
	private Rect _rect_credits = new Rect(160, 400, 100, 30);
    private Rect _rect_credits_text = new Rect(160, 400, 200, 200);

    public GameObject[] way_prefabs;
    public GameObject[] turn_prefabs;

    public Texture2D ogam_logo;
    private Rect ogam_logo_rect = new Rect(0, 0, 128, 128);

	void Start ()
	{
		_rect_credits.x = (Screen.width-100)*0.5f;
		_rect_credits.y = (Screen.height-30)*0.5f + 50;
		
		_rect_credits_text.x = (Screen.width-200)*0.5f;
		_rect_credits_text.y = (Screen.height-50)*0.5f - 200;

        ogam_logo_rect.x = Screen.width - 200;
        ogam_logo_rect.y = Screen.height - 200;
		
		Screen.lockCursor = false;

        GameObject obj;

        int i = Random.Range(0, way_prefabs.Length);
        
        obj = (GameObject)Instantiate(way_prefabs[i], new Vector3(0, 0, 10), Quaternion.Euler(0, 0, 0));
        obj.GetComponent<tile>().dont_wait_spawning = true;
        obj = (GameObject)Instantiate(way_prefabs[i], new Vector3(0, 0, -10), Quaternion.Euler(0, 0, 0));
        obj.GetComponent<tile>().dont_wait_spawning = true;
        obj = (GameObject)Instantiate(way_prefabs[i], new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        obj.GetComponent<tile>().dont_wait_spawning = true;
        obj = (GameObject)Instantiate(turn_prefabs[i], new Vector3(0, 0, 20), Quaternion.Euler(0, 270, 0));
        obj.GetComponent<tile>().dont_wait_spawning = true;
        obj = (GameObject)Instantiate(turn_prefabs[i], new Vector3(0, 0, -20), Quaternion.Euler(0, 0, 0));
        obj.GetComponent<tile>().dont_wait_spawning = true;
	}
	
	void Update()
	{	
	}
	
	void OnGUI()
	{
		GUI.skin = _menu_skin;
		
		GUI.skin.label.alignment = TextAnchor.UpperCenter;		
		GUI.Label(_rect_credits_text, "#1GAM March\n\nDaniel Baumartz\n\nonegameamonth.com/Myyyvothrr\nMyyyvothrr.de\n\nThanks PhilBam\n\nMade with Unity");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;

        GUI.DrawTexture(ogam_logo_rect, ogam_logo, ScaleMode.ScaleToFit, true);
		
		if (GUI.Button(_rect_credits, "Back"))
			Application.LoadLevel(0);
	}
}
