using UnityEngine;
using System.Collections;

public class player : MonoBehaviour
{
	public GUISkin _menu_skin;
	
	public AudioClip _sound_potion_pickup;
	public AudioClip _sound_potion_drink;
	
	public AudioClip _sound_key_pickup;
	public AudioClip _sound_key_use;
	public AudioClip _sound_door_opening;
	
	public AudioClip _sound_coins_pickup;
	
	public AudioClip _sound_levelup;

    public int last_exit_tile = -1;
			
	private int _hitpoints = 30;
	private int _hitpoints_max = 30;	
	private int _potions_num = 1;
	private int _coins_num = 0;
	private int _xp_num = 0;
	private int _keys_num = 0;
	private int _level = 0;
	private int _xp_next_level = 10;
	private int _learn_points = 0;
	
	private int _strength = 1;
	private int _speed = 1;
	private int _endurance = 1;

    public int dungeon_level = 1;
		
	private GameObject _hands_left = null;
	private GameObject _hands_right = null;
	
	private GameObject _health_potion = null;
	private GameObject _weapon = null;
	private GameObject _key = null;
		
	private TextMesh _potions_text;
	private TextMesh _hitpoints_text;
	
	private Animation _drinking_anim;
	private Animation _fighting_anim;
	
	private bool _can_drink = true;
	private bool _can_attack = true;
	
	private GameObject _door;
	
	private bool _show_gui = false;

    private Rect _gui_stats_window = new Rect(1, 1, 400, 400);
    private Rect _gui_exit_window = new Rect(1, 1, 200, 80);

    private Rect _gui_exit_label = new Rect(20, 20, 160, 40);
    	
	private Rect _gui_level_label = new Rect(20, 20, 70, 25);
	private Rect _gui_level_value = new Rect(100, 20, 25, 25);
	
	private Rect _gui_xp_label = new Rect(20, 50, 70, 25);
	private Rect _gui_xp_value = new Rect(100, 50, 80, 25);	
	
	private Rect _gui_lp_label = new Rect(20, 80, 70, 25);
	private Rect _gui_lp_value = new Rect(100, 80, 25, 25);
	
	private Rect _gui_attribs_strength_label = new Rect(20, 140, 70, 25);
	private Rect _gui_attribs_strength_value = new Rect(100, 140, 25, 25);
	private Rect _gui_attribs_strength_button = new Rect(130, 137, 25, 25);
	private Rect _gui_attribs_strength_desc = new Rect(20, 170, 360, 25);
	
	private Rect _gui_attribs_endurance_label = new Rect(20, 220, 70, 25);
	private Rect _gui_attribs_endurance_value = new Rect(100, 220, 25, 25);
	private Rect _gui_attribs_endurance_button = new Rect(130, 217, 25, 25);
	private Rect _gui_attribs_endurance_desc = new Rect(20, 250, 360, 25);
	
	private Rect _gui_attribs_speed_label = new Rect(20, 300, 70, 25);
	private Rect _gui_attribs_speed_value = new Rect(100, 300, 25, 25);
	private Rect _gui_attribs_speed_button = new Rect(130, 297, 25, 25);
	private Rect _gui_attribs_speed_desc = new Rect(20, 330, 360, 25);
	
	private Rect _gui_close_button = new Rect(290, 365, 100, 25);
	private Rect _gui_buy_button = new Rect(10, 365, 200, 25);
    private Rect _gui_quit_button = new Rect(250, 110, 100, 25);
	
	private Rect _gui_mouse_sensitivity_label = new Rect (250, 20, 130, 25);
	private Rect _gui_mouse_sensitivity_slider = new Rect (250, 50, 130, 25);
	private Rect _gui_mouse_sensitivity_slider2 = new Rect (250, 80, 130, 25);

    public Texture2D[] blood_overlay_tex;
    private Rect _blood_overlay_rect;
    private int _blood_overlay_index = 0;
    private bool _show_blood = false;

    public Texture2D levelup_overlay_tex;
    private Rect _levelup_overlay_rect;
    private Rect _levelup_overlay_rect2;
    private bool _show_levelup = false;
	
	private CharacterMotor _char_motor;
	private MouseLook _mouse_look_player;
	private MouseLook _mouse_look_camera;

    private bool _exit_triggered = false;
    private bool _teleporter_triggered = false;
    private bool _teleporter_boss_triggered = false;

	void Start()
	{
        DontDestroyOnLoad(gameObject);

		_gui_stats_window.x = (Screen.width-400) * 0.5f;
        _gui_stats_window.y = (Screen.height - 400) * 0.5f;

        _gui_exit_window.x = (Screen.width - 200) * 0.5f;
        _gui_exit_window.y = (Screen.height - 80) * 0.5f;

        _blood_overlay_rect = new Rect(0, 0, Screen.width, Screen.height);

        _levelup_overlay_rect = new Rect((Screen.width - 128) / 2, Screen.height - 200, 128, 128);
        _levelup_overlay_rect2 = new Rect((Screen.width - 128) / 2, Screen.height - 70, 128, 70);
		
		Screen.lockCursor = true;		
				
		_hands_left = transform.Find("hands/left").gameObject;		
		_hands_right = transform.Find("hands/right").gameObject;
		
		_health_potion = transform.Find("hands/left/health_potion").gameObject;
		_weapon = transform.Find("hands/right/sword1").gameObject;	
		_key = transform.Find("hands/left/key").gameObject;
		
		_key.SetActive(false);
		
		_drinking_anim = _hands_left.GetComponent<Animation>();
		_fighting_anim = _hands_right.GetComponent<Animation>();
		
		_potions_text = _hands_left.GetComponentInChildren<TextMesh>();
		_hitpoints_text = _hands_right.GetComponentInChildren<TextMesh>();
		
		_char_motor = GetComponent<CharacterMotor>();
		_mouse_look_player = GetComponent<MouseLook>();
		_mouse_look_camera = Camera.main.GetComponent<MouseLook>();

        try
        {
            _mouse_look_player.sensitivityX = PlayerPrefs.GetFloat("sensitivityX", _mouse_look_player.sensitivityX);
            _mouse_look_camera.sensitivityY = PlayerPrefs.GetFloat("sensitivityY", _mouse_look_camera.sensitivityY);
        }
        catch (PlayerPrefsException ex)
        {
            Debug.Log(ex.Message);
        }
	}
	
	void OnGUI()
    {
        GUI.skin = _menu_skin;

        if (_show_blood)
            GUI.DrawTexture(_blood_overlay_rect, blood_overlay_tex[_blood_overlay_index], ScaleMode.ScaleAndCrop, true);

        if (_show_levelup)
        {
            GUI.DrawTexture(_levelup_overlay_rect, levelup_overlay_tex, ScaleMode.ScaleAndCrop, true);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.Label(_levelup_overlay_rect2, "You reached a new level! Press [ESC] ");
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
        }

        if (_exit_triggered || _teleporter_triggered || _teleporter_boss_triggered)
            _gui_exit_window = GUI.Window(0, _gui_exit_window, gui_exit_window, "");

		if (!_show_gui)
			return;
		
		_gui_stats_window = GUI.Window(0, _gui_stats_window, gui_stats_window, "");		
	}

    void gui_exit_window(int id)
    {
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
       
        if (_exit_triggered)
            GUI.Label(_gui_exit_label, "You are about to go down the stairs... ");
        else if (_teleporter_triggered)
            GUI.Label(_gui_exit_label, "Do you really want to enter? Be prepared!");
        else if (_teleporter_boss_triggered)
            GUI.Label(_gui_exit_label, "You can't go back... You better be ready!");

        GUI.skin.label.alignment = TextAnchor.UpperLeft;
    }

    void gui_stats_window(int id)
    {
        GUI.Label(_gui_mouse_sensitivity_label, "Mouse Sensitivity");
        _mouse_look_player.sensitivityX = GUI.HorizontalSlider(_gui_mouse_sensitivity_slider, _mouse_look_player.sensitivityX, 1f, 30f);
        _mouse_look_camera.sensitivityY = GUI.HorizontalSlider(_gui_mouse_sensitivity_slider2, _mouse_look_camera.sensitivityY, 1f, 30f);

        GUI.Label(_gui_level_label, "Level ");
        GUI.Label(_gui_level_value, _level.ToString());

        GUI.Label(_gui_xp_label, "XP ");
        GUI.Label(_gui_xp_value, _xp_num.ToString() + " / " + _xp_next_level);

        GUI.Label(_gui_lp_label, "Learnpoints ");
        GUI.Label(_gui_lp_value, _learn_points.ToString());

        GUI.Label(_gui_attribs_strength_label, "Strength ");
        GUI.Label(_gui_attribs_strength_value, _strength.ToString());
        if (GUI.Button(_gui_attribs_strength_button, "+"))
        {
            if (_learn_points > 0)
            {
                ++_strength;
                --_learn_points;

                audio.PlayOneShot(_sound_levelup);
            }
        }
        GUI.Label(_gui_attribs_strength_desc, "\"More strength means more damage!\"");

        GUI.Label(_gui_attribs_endurance_label, "Endurance ");
        GUI.Label(_gui_attribs_endurance_value, _endurance.ToString());
        if (GUI.Button(_gui_attribs_endurance_button, "+"))
        {
            if (_learn_points > 0)
            {
                ++_endurance;
                --_learn_points;

                _hitpoints_max += 10;
                _hitpoints = _hitpoints_max;

                audio.PlayOneShot(_sound_levelup);
            }
        }
        GUI.Label(_gui_attribs_endurance_desc, "\"You can't hurt me! And better potions.\"");

        GUI.Label(_gui_attribs_speed_label, "Speed ");
        GUI.Label(_gui_attribs_speed_value, _speed.ToString());
        if (GUI.Button(_gui_attribs_speed_button, "+"))
        {
            if (_learn_points > 0)
            {
                ++_speed;
                --_learn_points;

                _char_motor.movement.maxForwardSpeed += 0.5f;
                _char_motor.movement.maxSidewaysSpeed += 0.5f;

                audio.PlayOneShot(_sound_levelup);
            }
        }
        GUI.Label(_gui_attribs_speed_desc, "\"Run faster! Kill faster!\"");

        if (GUI.Button(_gui_buy_button, "Buy Potion: 50 Gold"))
        {
            if (_coins_num > 50)
            {
                ++_potions_num;
                _coins_num -= 50;
                audio.PlayOneShot(_sound_coins_pickup);
            }
        }

        if (GUI.Button(_gui_close_button, "Done"))
            show_attribs_gui(false);

        if (GUI.Button(_gui_quit_button, "Quit"))
            Application.LoadLevel(0);
    }
	
	void Update()
	{
		_potions_text.text = "Potions: " + _potions_num + "\nKeys: " + _keys_num + "\nCoins: " + _coins_num;
		_hitpoints_text.text = "HP: " + _hitpoints + "/" + _hitpoints_max + "\nLvl: " + _level + "\nXP: " + _xp_num + "/" + _xp_next_level;
			
		if (Input.GetButtonUp("Fire3"))
			show_attribs_gui(!_show_gui);

        if (Input.GetKeyDown(KeyCode.Escape))
            show_attribs_gui(!_show_gui);
		
		if (_show_gui)
			return;
		
		if (_door != null && _keys_num > 0 && _can_drink && Input.GetButton("Fire2"))
			StartCoroutine(use_key());
		else if (_door == null && _can_drink && Input.GetButton("Fire2"))
			StartCoroutine(drink_health_potion());
		
		if (_can_attack && Input.GetButton("Fire1"))
			StartCoroutine(attack());
	}
	
	void set_near_door(GameObject door)
	{
		_door = door;
	}
	
	void update_near_door(bool state)
	{		
		if (state)
		{
			_health_potion.SetActive(false);
				
			if (_keys_num > 0)
				_key.SetActive(true);
		}
		else
		{		
			_door = null;
			
			_key.SetActive(false);
			
			if (_potions_num > 0)
				_health_potion.SetActive(true);
		}
	}
	
	void pickup_health_potion(float num)
	{
		_potions_num += (int)num;
		
		_health_potion.SetActive(true);
		
		audio.PlayOneShot(_sound_potion_pickup);
	}
	
	void pickup_key(int num)
	{
		_keys_num += num;
		
		audio.PlayOneShot(_sound_key_pickup);
	}
	
	void pickup_coins(float num)
	{
		_coins_num += (int)num;
		
		audio.PlayOneShot(_sound_coins_pickup);
	}
	
	void receive_damage(int amount)
	{
		_hitpoints -= amount;

        StartCoroutine(show_blood());
		
		if (_hitpoints <= 0)
		{						
			Application.LoadLevel(3);
			Screen.lockCursor = false;
		}
	}

    void OnLevelWasLoaded(int level)
    {
        _exit_triggered = false;

        if (level != 1 && level != 5)
            Destroy(gameObject);

        Time.timeScale = 1f;
    }
	
	void killed_enemy(int xp)
	{
		_xp_num += xp;
		
		if (_xp_num >= _xp_next_level)
		{
			++_level;
			_learn_points += 2;
			_xp_next_level = (_level + _level) * 10;
			
			//show_attribs_gui(true);
            _show_levelup = true;
			
			audio.PlayOneShot(_sound_levelup);
		}
	}
	
	void show_attribs_gui(bool show)
	{
        Time.timeScale = !show ? 1f : 0f;

        _show_levelup = false;

		_show_gui = show;
		Screen.lockCursor = !show;
		
		_can_attack = !show;
		_can_drink = !show;
		
		_mouse_look_player.enabled = !show;
		_mouse_look_camera.enabled = !show;

        if (!show)
        {
            try
            {
                PlayerPrefs.SetFloat("sensitivityX", _mouse_look_player.sensitivityX);
                PlayerPrefs.SetFloat("sensitivityY", _mouse_look_camera.sensitivityY);
            }
            catch (PlayerPrefsException ex)
            {
                Debug.Log(ex.Message);
            }
        }
	}

    IEnumerator show_blood()
    {
        for (int i = 0; i < blood_overlay_tex.Length; ++i)
        {
            _show_blood = true;
            _blood_overlay_index = i;
            yield return new WaitForSeconds(Random.Range(0.15f, 0.25f));
        }

        _show_blood = _hitpoints <= dungeon_level * 5 && _hitpoints_max > dungeon_level * 5;
    }
	
	IEnumerator attack()
	{
		_can_attack = false;
		
		_weapon.BroadcastMessage("set_damage", _strength);
		_weapon.BroadcastMessage("can_attack", true);
		
		_fighting_anim.Play();
					
		yield return new WaitForSeconds(0.8f);
			
		_weapon.BroadcastMessage("can_attack", false);
		
		_can_attack = true;
	}
	
	IEnumerator use_key()
	{	
		_can_drink = false;
	
		_drinking_anim.Play("hands_left_key");
		
		audio.PlayOneShot(_sound_key_use);
		
		yield return new WaitForSeconds(0.5f);
				
		--_keys_num;
		_door.SendMessage("open");
		
		audio.PlayOneShot(_sound_door_opening);	
			
		yield return new WaitForSeconds(0.5f);
		
		_drinking_anim.Stop();		
			
		update_near_door(false);
		
		_door = null;
		_can_drink = true;
	}
	
	IEnumerator drink_health_potion()
	{
		if (_potions_num > 0 && _hitpoints < _hitpoints_max)
		{
			_can_drink = false;
			
			--_potions_num;
			_drinking_anim.Play("hends_left_drinking");
			
			audio.PlayOneShot(_sound_potion_drink);
			
			_hitpoints += 2 * _endurance;
            _hitpoints = Mathf.Min(_hitpoints, _hitpoints_max);

            _show_blood = _hitpoints <= dungeon_level * 5 && _hitpoints_max > dungeon_level * 5;
			
			yield return new WaitForSeconds(1);
			
			if (_potions_num <= 0)
				_health_potion.SetActive(false);
			
			_can_drink = true;
		}
	}

    void exit_triggered(bool state)
    {
        _exit_triggered = state;
    }

    void teleporter_triggered(bool state)
    {
        _teleporter_triggered = state;
    }

    void teleporter_boss_triggered(bool state)
    {
        _teleporter_boss_triggered = state;
    }

    void level_changer_trigger(int tile)
    {
        last_exit_tile = tile;
        ++dungeon_level;

        _exit_triggered = false;
        _teleporter_triggered = false;
        _teleporter_boss_triggered = false;
    }
}
