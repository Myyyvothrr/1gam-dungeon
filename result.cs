using UnityEngine;
using System.Collections;

public class result : MonoBehaviour
{	
	public GUISkin _menu_skin;
	
	private Rect _rect_credits = new Rect(160, 400, 100, 30);
	private Rect _rect_credits_text = new Rect(160, 400, 200, 100);

	// Use this for initialization
	void Start ()
	{
		_rect_credits.x = (Screen.width-100)*0.5f;
		_rect_credits.y = (Screen.height-30)*0.5f + 50;
		
		_rect_credits_text.x = (Screen.width-200)*0.5f;
		_rect_credits_text.y = (Screen.height-50)*0.5f - 100;
		
		Screen.lockCursor = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnGUI()
	{
		GUI.skin = _menu_skin;
		
		GUI.skin.label.alignment = TextAnchor.UpperCenter;		
		GUI.Label(_rect_credits_text, "You killed all mages in the dungeon. It's over...\n\n\nThank you for playing!");		
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		
		if (GUI.Button(_rect_credits, "Done"))
			Application.LoadLevel(0);
	}
}
