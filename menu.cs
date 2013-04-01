using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour
{	
	public GUISkin _menu_skin;
	
	private Rect _rect_play = new Rect(160, 350, 100, 30);
	private Rect _rect_credits = new Rect(160, 400, 100, 30);
	private Rect _rect_quit = new Rect(160, 450, 100, 30);

	// Use this for initialization
	void Start ()
	{
		_rect_play.x = (Screen.width-100)*0.5f;
		_rect_credits.x = (Screen.width-100)*0.5f;
		_rect_quit.x = (Screen.width-100)*0.5f;
		
		_rect_play.y = (Screen.height-30)*0.5f - 50;
		_rect_credits.y = (Screen.height-30)*0.5f;
		_rect_quit.y = (Screen.height-30)*0.5f + 50;
		
		Screen.lockCursor = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnGUI()
	{
		GUI.skin = _menu_skin;
		
		if (GUI.Button(_rect_play, "Play"))
			Application.LoadLevel(1);
		
		if (GUI.Button(_rect_credits, "Credits"))
			Application.LoadLevel(4);
		
		if (GUI.Button(_rect_quit, "Quit"))
			Application.Quit();
	}
}
