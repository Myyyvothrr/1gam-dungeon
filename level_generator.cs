using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class level_generator : MonoBehaviour
{
    public int level_size = 50;

    public GameObject level_root;

    public GameObject player_prefab;

    public GameObject door_prefab;
    public GameObject key_prefab;
    public GameObject teleporter_prefab;

    public GameObject[] random_spawns;

    [System.Serializable]
    public class TileTypeCollection
    {
        public GameObject[] prefab_way;
        public GameObject[] prefab_turn;
        public GameObject[] prefab_tcross;
        public GameObject[] prefab_end;
        public GameObject[] prefab_open;
        public GameObject[] prefab_stairs;
    };

    public TileTypeCollection[] tile_types;

    private class Tile
    {
        public int x = -1;
        public int y = -1;
        public int type = -1;
        public Room room = null;
        public bool place_exit = false;
        public bool place_entrance = false;
        public bool has_door = false;
        public bool spawn_key = false;
        public bool visited = false;
        public GameObject go = null;
    };

    private class Room
    {
        public Tile[] room_tiles;
        public int type = -1;
        public bool connected = false;
        public int pos_x = -1;
        public int pos_y = -1;
        public int width = -1;
        public int height = -1;
    }

    private int index(int x, int y)
    {
        return y + x * level_size;
    }

    private int index(int x, int y, int h)
    {
        return y + x * h;
    }

    private int GRID_SIZE = 10;

    private Room[] rooms;
    private Tile[] tiles;
    private int tiles_visited;

    private Room[] open_rooms;
    private Room[] connected_rooms;

    private bool exit_reachable;

    bool _connected(int start_index, int end_index)
    {
        tiles_visited = 0;
        exit_reachable = false;

        _connected_step(start_index, end_index);

        return exit_reachable;
    }
    
    void _connected_step(int i, int end_index)
    {
        if (i == end_index)
            exit_reachable = true;

        if (i < 0 || i >= tiles.Length)
            return;

        if (tiles[i].visited)
            return;

        tiles[i].visited = true;
        ++tiles_visited;

        if (tiles[i].type == -1)
            return;

        if (tiles[i].has_door)
        {
            Tile[] tiles_key = new Tile[tiles_visited];

            int k = 0;            
            for (int j = 0; j < level_size * level_size; ++j)
            {
                if (tiles[j].visited & tiles[j].type != -1 && !tiles[j].spawn_key && !tiles[j].has_door && !tiles[j].place_entrance && !tiles[j].place_exit)
                {
                    tiles_key[k] = tiles[j];
                    ++k;
                }
            }

            tiles_key[Random.Range(0, k)].spawn_key = true;
        }

        _connected_step(index(tiles[i].x + 1, tiles[i].y), end_index);
        _connected_step(index(tiles[i].x - 1, tiles[i].y), end_index);
        _connected_step(index(tiles[i].x, tiles[i].y+1), end_index);
        _connected_step(index(tiles[i].x, tiles[i].y - 1), end_index);
    }

    void _create_way(Room r1, Room r2)
    {
        int x0 = r1.room_tiles[Random.Range(0, r1.room_tiles.Length)].x;
        int y0 = r1.room_tiles[Random.Range(0, r1.room_tiles.Length)].y;

        int x1 = r2.room_tiles[Random.Range(0, r2.room_tiles.Length)].x;
        int y1 = r2.room_tiles[Random.Range(0, r2.room_tiles.Length)].y;

        int room_type = r1.type;
        float half_distance = Vector2.Distance(new Vector2(x0, y0), new Vector2(x1, y1)) * 0.5f;

        // build way with Bresenham 
        // http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);

        int sx = -1;
        if (x0 < x1)
            sx = 1;

        int sy = -1;
        if (y0 < y1)
            sy = 1;

        int err = dx - dy;

        int e2;

        while (true)
        {
            if (Vector2.Distance(new Vector2(x0, y0), new Vector2(x1, y1)) <= half_distance)
                room_type = r2.type;
            else
                room_type = r1.type;

            if (x0 >= 0 && y0 >= 0 && x0 < level_size && y0 < level_size)
            {
                if (tiles[index(x0, y0)].type == -1)
                {
                    tiles[index(x0, y0)].type = room_type;
                    tiles[index(x0, y0)].x = x0;
                    tiles[index(x0, y0)].y = y0;
                }
            }

            switch (Random.Range(0, 2))
            {
                case 0:
                    {
                        if (x0 + sx >= 0 && x0 + sx < level_size)
                        {
                            if (tiles[index(x0 + sx, y0)].type == -1)
                            {
                                tiles[index(x0 + sx, y0)].type = room_type;
                                tiles[index(x0 + sx, y0)].x = x0 + sx;
                                tiles[index(x0 + sx, y0)].y = y0;
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        if (y0 + sy >= 0 && y0 + sy < level_size)
                        {
                            if (tiles[index(x0, y0 + sy)].type == -1)
                            {
                                tiles[index(x0, y0 + sy)].type = room_type;
                                tiles[index(x0, y0 + sy)].x = x0;
                                tiles[index(x0, y0 + sy)].y = y0 + sy;
                            }
                        }
                        break;
                    }
            }

            if (x0 == x1 && y0 == y1)
                break;

            e2 = 2 * err;

            if (e2 > -dy)
            {
                err = err - dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    void Start()
    {
        Debug.Log("The Mage Dungeon v1.5");

        GameObject player = GameObject.Find("/player");
        if (player == null)
        {
            player = (GameObject)Instantiate(player_prefab);
            player.name = "player";
        }

        int dungeon_level = player.GetComponent<player>().dungeon_level;

        int last_exit_tile = player.GetComponent<player>().last_exit_tile;
        if (last_exit_tile == -1)
            last_exit_tile = Random.Range(0, tile_types.Length);

        level_size = 10 + dungeon_level * 10;

        tiles = new Tile[level_size * level_size];
        for (int i = 0; i < level_size * level_size; ++i)
            tiles[i] = new Tile();

        int grid = level_size / GRID_SIZE;
        int grids = grid * grid;

        rooms = new Room[grids];

        int row = 0;
        int col = 0;
        for (int i = 0; i < grids; ++i)
        {
            int room_width = Random.Range(3, GRID_SIZE - 1);
            int room_height = Random.Range(3, GRID_SIZE - 1);

            int room_type = Random.Range(0, tile_types.Length);

            int room_pos_x = Random.Range(0, GRID_SIZE - room_width);
            int room_pos_y = Random.Range(0, GRID_SIZE - room_height);

            room_pos_x += col * GRID_SIZE;
            room_pos_y += row * GRID_SIZE;

            ++col;
            if (col >= grid)
            {
                col = 0;
                ++row;
            }
            
            rooms[i] = new Room();
            rooms[i].connected = false;
            rooms[i].type = room_type;
            rooms[i].room_tiles = new Tile[room_width * room_height];
            rooms[i].pos_x = room_pos_x;
            rooms[i].pos_y = room_pos_y;
            rooms[i].width = room_width;
            rooms[i].height = room_height;
            
            for (int x = 0; x < room_width; ++x)
            {
                for (int y = 0; y < room_height; ++y)
                {
                    tiles[index(room_pos_x + x, room_pos_y + y)].type = room_type;
                    tiles[index(room_pos_x + x, room_pos_y + y)].room = rooms[i];
                    tiles[index(room_pos_x + x, room_pos_y + y)].x = room_pos_x + x;
                    tiles[index(room_pos_x + x, room_pos_y + y)].y = room_pos_y + y;
                    rooms[i].room_tiles[index(x, y, room_height)] = tiles[index(room_pos_x + x, room_pos_y + y)];
                }
            }
        }

        bool exit_placed = false;
        bool entrance_placed = false;
        Vector2 entrance_pos = Vector2.zero;
        int exit_tile_index = -1;
        int entrance_tile_index = -1;
        bool creating_ways = true;
        Room room_connected = rooms[0];
        rooms[0].connected = true;
        int not_connected = rooms.Length-1;

        while (creating_ways)
        {
            open_rooms = new Room[not_connected];

            int k = 0;
            for (int m = 0; m < rooms.Length; ++m)
            {
                if (!rooms[m].connected)
                {
                    open_rooms[k] = rooms[m];
                    ++k;
                }
            }

            int i = Random.Range(0, open_rooms.Length);
            if (!entrance_placed && open_rooms[i].width >= 3 && open_rooms[i].height >= 3)
            {
                entrance_pos = new Vector2(open_rooms[i].pos_x + open_rooms[i].width / 2, open_rooms[i].pos_y + open_rooms[i].height / 2);
                entrance_placed = true;
                open_rooms[i].type = last_exit_tile;

                for (int x = 0; x < open_rooms[i].width; ++x)
                {
                    for (int y = 0; y < open_rooms[i].height; ++y)
                    {
                        tiles[index(open_rooms[i].pos_x + x, open_rooms[i].pos_y + y)].type = last_exit_tile;
                    }
                }

                entrance_tile_index = index(open_rooms[i].pos_x + open_rooms[i].width / 2, open_rooms[i].pos_y + open_rooms[i].height / 2);
                tiles[entrance_tile_index].place_entrance = true;
            }
            else if (!exit_placed && open_rooms[i].width >= 3 && open_rooms[i].height >= 3)
            {
                if (Vector2.Distance(new Vector2(open_rooms[i].pos_x + open_rooms[i].width / 2, open_rooms[i].pos_y + open_rooms[i].height / 2), entrance_pos) >= level_size * 0.5)
                {
                    exit_placed = true;
                    exit_tile_index = index(open_rooms[i].pos_x + open_rooms[i].width / 2, open_rooms[i].pos_y + open_rooms[i].height / 2);
                    tiles[exit_tile_index].place_exit = true;
                }
            }

            open_rooms[i].connected = true;
            --not_connected;

            _create_way(room_connected, open_rooms[i]);

            connected_rooms = new Room[rooms.Length - not_connected];

            int stop_creating_ways = 0;
            int o = 0;
            for (int j = 0; j < rooms.Length; ++j)
            {
                if (rooms[j].connected)
                {
                    connected_rooms[o] = rooms[j];
                    ++o;
                }
                else
                {
                    ++stop_creating_ways;
                }
            }
            
            creating_ways = stop_creating_ways > 0;

            room_connected = connected_rooms[Random.Range(0, connected_rooms.Length)];
        }

        bool placed_teleporter = false;
        
        for (int x = 0; x < level_size; ++x)
        {
            for (int y = 0; y < level_size; ++y)
            {
                Tile t = tiles[index(x, y)];
                if (t.type == -1)
                    continue;

                GameObject obj = null;
                GameObject prefab = null;

                Vector3 pos = new Vector3(10 * x, 0, 10 * y);
                Quaternion rot = Quaternion.Euler(0, 0, 0);

                bool[] neighbours = { false, false, false, false };

                if (x + 1 < level_size)
                    neighbours[0] = tiles[index(x + 1, y)].type != -1;
                if (y + 1 < level_size)
                    neighbours[1] = tiles[index(x, y + 1)].type != -1;
                if (x - 1 >= 0)
                    neighbours[2] = tiles[index(x - 1, y)].type != -1;
                if (y - 1 >= 0)
                    neighbours[3] = tiles[index(x, y - 1)].type != -1;

                // open
                if (neighbours[0] && neighbours[1] && neighbours[2] && neighbours[3])
                {
                    if (t.place_entrance)
                    {
                        prefab = _select_tile_prefab(tile_types[t.type].prefab_stairs, 0.5f);     
                        pos.y += 5;
                       
                        Vector3 pos2;
                        Quaternion rot2;

                        for (int bx = 0; bx < 5; ++bx)
                        {
                            for (int by = 0; by < 5; ++by)
                            {
                                if (bx == 2 && by == 2)
                                    continue;

                                pos2 = new Vector3(-20 + 10 * x + 10 * bx, 5, -20 + 10 * y + 10 * by);
                                rot2 = Quaternion.Euler(0, 90 * Random.Range(1, 4), 0);
                                obj = (GameObject)Instantiate(_select_tile_prefab(tile_types[t.type].prefab_open, 0.5f), pos2, rot2);
                                obj.transform.parent = level_root.transform;
                                Destroy(obj.GetComponent<tile>());
                            }
                        }
                    }
                    else if (t.place_exit)
                    {
                        prefab = _select_tile_prefab(tile_types[t.type].prefab_stairs, 0.5f);

                        Vector3 pos2;
                        Quaternion rot2;

                        for (int bx = 0; bx < 5; ++bx)
                        {
                            for (int by = 0; by < 5; ++by)
                            {
                                if (bx == 2 && by == 2)
                                    continue;

                                pos2 = new Vector3(-20 + 10 * x + 10 * bx, -5, -20 + 10 * y + 10 * by);
                                rot2 = Quaternion.Euler(0, 90 * Random.Range(1, 4), 0);
                                obj = (GameObject)Instantiate(_select_tile_prefab(tile_types[t.type].prefab_open, 0.5f), pos2, rot2);
                                obj.transform.parent = level_root.transform;
                                Destroy(obj.GetComponent<tile>());
                            }
                        }

                        obj = (GameObject)Instantiate(prefab, pos, rot);
                        obj.tag = "exit";
                        obj.GetComponentInChildren<tile_level_changer>().type = t.type;
                        obj.transform.parent = level_root.transform;
                        prefab = null;
                    }
                    else
                    {
                        if (dungeon_level > 1 && !placed_teleporter && Random.Range(0f, 1f) >= 0.5f)
                        {
                            placed_teleporter = true;
                            Instantiate(teleporter_prefab, pos, rot);
                        }

                        prefab = _select_tile_prefab(tile_types[t.type].prefab_open, 0.5f);
                    }

                    rot = Quaternion.Euler(0, 90 * Random.Range(1, 4), 0);
                } // ways
                else if (neighbours[0] && !neighbours[1] && neighbours[2] && !neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_way, 0.8f);
                    rot = Quaternion.Euler(0, 90, 0);

                    if (Random.Range(0f, 1f) > 0.9f)
                    {
                        t.has_door = true;
                        Instantiate(door_prefab, pos, rot);
                        prefab = tile_types[t.type].prefab_way[0];
                    }
                }
                else if (!neighbours[0] && neighbours[1] && !neighbours[2] && neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_way, 0.8f);
                    rot = Quaternion.Euler(0, 0, 0);

                    if (Random.Range(0f, 1f) > 0.9f)
                    {
                        t.has_door = true;
                        Instantiate(door_prefab, pos, rot);
                        prefab = tile_types[t.type].prefab_way[0];
                    }
                } // t cross
                else if (neighbours[0] && neighbours[1] && neighbours[2] && !neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_tcross, 0.7f);
                    rot = Quaternion.Euler(0, 270, 0);
                }
                else if (neighbours[0] && neighbours[1] && !neighbours[2] && neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_tcross, 0.7f);
                    rot = Quaternion.Euler(0, 0, 0);
                }
                else if (neighbours[0] && !neighbours[1] && neighbours[2] && neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_tcross, 0.7f);
                    rot = Quaternion.Euler(0, 90, 0);
                }
                else if (!neighbours[0] && neighbours[1] && neighbours[2] && neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_tcross, 0.7f);
                    rot = Quaternion.Euler(0, 180, 0);
                } // turn
                else if (neighbours[0] && neighbours[1] && !neighbours[2] && !neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_turn, 0.7f);
                    rot = Quaternion.Euler(0, 90, 0);
                }
                else if (!neighbours[0] && neighbours[1] && neighbours[2] && !neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_turn, 0.7f);
                    rot = Quaternion.Euler(0, 0, 0);
                }
                else if (!neighbours[0] && !neighbours[1] && neighbours[2] && neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_turn, 0.7f);
                    rot = Quaternion.Euler(0, 270, 0);
                }
                else if (neighbours[0] && !neighbours[1] && !neighbours[2] && neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_turn, 0.7f);
                    rot = Quaternion.Euler(0, 180, 0);
                } // end
                else if (neighbours[0] && !neighbours[1] && !neighbours[2] && !neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_end, 0.7f);
                    rot = Quaternion.Euler(0, 90, 0);
                }
                else if (!neighbours[0] && neighbours[1] && !neighbours[2] && !neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_end, 0.7f);
                    rot = Quaternion.Euler(0, 0, 0);
                }
                else if (!neighbours[0] && !neighbours[1] && neighbours[2] && !neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_end, 0.7f);
                    rot = Quaternion.Euler(0, 270, 0);
                }
                else if (!neighbours[0] && !neighbours[1] && !neighbours[2] && neighbours[3])
                {
                    prefab = _select_tile_prefab(tile_types[t.type].prefab_end, 0.7f);
                    rot = Quaternion.Euler(0, 180, 0);
                } // single tile, drop
                else if (!neighbours[0] && !neighbours[1] && !neighbours[2] && !neighbours[3])
                {
                    prefab = null;
                }

                if (prefab != null)
                {
                    obj = (GameObject)Instantiate(prefab, pos, rot);
                    obj.transform.parent = level_root.transform;
                    t.go = obj;
                }
            }
        }

        if (!entrance_placed)
        {
            Debug.Log("ERROR! Entrance not placed!");
            Application.LoadLevel(1);
        }
        if (!exit_placed)
        {
            Debug.Log("ERROR! Exit not placed!");
            Application.LoadLevel(1);
        }
        if (!_connected(entrance_tile_index, exit_tile_index))
        {
            Debug.Log("ERROR! Exit not reachable!");
            Application.LoadLevel(1);
        }

        for (int x = 0; x < level_size; ++x)
        {
            for (int y = 0; y < level_size; ++y)
            {
                Tile t = tiles[index(x, y)];
                if (t.type == -1)
                    continue;

                if (t.spawn_key)
                    Instantiate(key_prefab, new Vector3(10 * x, 3, 10 * y), Quaternion.identity);
                else if (t.go != null && !t.has_door && Random.Range(0f, 1f) > 0.7f)
                    t.go.SendMessage("set_random_spawn", random_spawns[Random.Range(0, random_spawns.Length)]);
            }
        }

        player.transform.position = new Vector3(entrance_pos.x * GRID_SIZE, 3f, entrance_pos.y * GRID_SIZE);    

        Destroy(gameObject);

        //     StaticBatchingUtility.Combine(level_root.gameObject);
    }

    void Update()
    {
    }

    GameObject _select_tile_prefab(GameObject[] prefab_type_array, float p)
    {
        if (prefab_type_array.Length > 1 && Random.Range(0f, 1f) > p)
            return prefab_type_array[Random.Range(1, prefab_type_array.Length)];
        else if (prefab_type_array.Length > 0)
            return prefab_type_array[0];
        else
            return null;
    }
}
