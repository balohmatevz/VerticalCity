using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Floor : MonoBehaviour
{
    public Building Owner;

    public List<Tile> Tiles;
    public int FloorNumber;

    public Floor Up;
    public Floor Down;

    public const int TILES_PER_FLOOR = 40;
    public const float TILE_WIDTH = 0.5f;

    public Transform RoomsAnchor;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUp(Building building, int floorNumber)
    {
        Owner = building;
        FloorNumber = floorNumber;

        name = "Floor " + FloorNumber;

        UpdateNeighbors();

        Tiles = new List<Tile>();
        for (int i = 0; i < TILES_PER_FLOOR; i++)
        {
            Tiles.Add(CreateTile(i));
        }
    }

    public Tile CreateTile(int x)
    {
        GameObject go = Instantiate(GameController.obj.PF_Tile);
        Transform t = go.GetComponent<Transform>();
        Tile tile = go.GetComponent<Tile>();

        t.SetParent(this.transform);
        t.localScale = Vector3.one;
        t.rotation = Quaternion.identity;
        t.transform.localPosition = new Vector3(((-TILES_PER_FLOOR / 2) + x) * TILE_WIDTH, 0, 0);

        tile.SetUp(this, x, TileState.BUILT);

        return tile;
    }

    public void OnModeChange()
    {
        foreach (Tile tile in Tiles)
        {
            tile.OnModeChange();
        }
    }

    public void UpdateNeighbors()
    {
        if (Owner.Floors.Count > FloorNumber + 1)
        {
            if (Up != null)
            {
                Up.OnNeighborUpdated(Direction.DOWN, null);
            }
            Up = Owner.Floors[FloorNumber + 1];
            Up.OnNeighborUpdated(Direction.DOWN, this);
        }
        else
        {
            Up = null;
        }

        if (FloorNumber - 1 >= 0 && Owner.Floors.Count > FloorNumber - 1)
        {
            if (Down != null)
            {
                Down.OnNeighborUpdated(Direction.DOWN, null);
            }
            Down = Owner.Floors[FloorNumber - 1];
            Down.OnNeighborUpdated(Direction.UP, this);
        }
        else
        {
            Down = null;
        }
    }

    public void OnNeighborUpdated(Direction updatedNeighbor, Floor neighbor)
    {
        switch (updatedNeighbor)
        {
            case Direction.UP:
                Up = neighbor;
                break;
            case Direction.DOWN:
                Down = neighbor;
                break;
        }
    }

    public void BuildRoomAt(int x, RoomData room)
    {
        if (CanBuildRoomAt(x, room))
        {
            GameObject go = Instantiate(room.Prefab);
            Transform t = go.GetComponent<Transform>();
            t.SetParent(RoomsAnchor);
            t.localScale = Vector3.one;
            t.rotation = Quaternion.identity;
            t.localPosition = new Vector3(Tiles[x].transform.position.x, 0, 0);
        }
    }

    public bool CanBuildRoomAt(int x, RoomData room)
    {
        int neededTiles = room.NeededTiles;

        if (Tiles.Count <= x)
        {
            return false;
        }

        return Tiles[x].CanBuildRoom(room);
    }

    public void RegenerateNavigationGraph()
    {
        Room room = null;
        for (int i = 0; i < Tiles.Count; i++)
        {
            Tile tile = Tiles[i];

            if (tile.State != TileState.BUILT && tile.State != TileState.HAS_ROOM)
            {
                //Break in building
                room = null;
                continue;
            }

            if (tile.BuiltRoom != null && tile.BuiltRoom != room)
            {
                tile.BuiltRoom.ClearNavGraph();

                if (room != null)
                {
                    room.NavConnect(Direction.RIGHT, tile.BuiltRoom.NavNodeLeft);
                    tile.BuiltRoom.NavConnect(Direction.LEFT, room.NavNodeRight);
                }

                if (tile.BuiltRoom.NavNodeUp != null && this.Up)
                {
                    if (this.Up.Tiles.Count > i && this.Up.Tiles[i].BuiltRoom != null)
                    {
                        if (this.Up.Tiles[i].BuiltRoom.NavNodeDown != null)
                        {
                            //This room has a nav node up, room above has a nav node down, connect.
                            this.Up.Tiles[i].BuiltRoom.NavConnect(Direction.DOWN, tile.BuiltRoom.NavNodeUp);
                            tile.BuiltRoom.NavConnect(Direction.UP, this.Up.Tiles[i].BuiltRoom.NavNodeDown);
                        }
                    }
                }

                if (tile.BuiltRoom.NavNodeDown != null && this.Down)
                {
                    if (this.Down.Tiles.Count > i && this.Down.Tiles[i].BuiltRoom != null)
                    {
                        if (this.Down.Tiles[i].BuiltRoom.NavNodeUp != null)
                        {
                            //This room has a nav node down, room below has a nav node up, connect.
                            this.Down.Tiles[i].BuiltRoom.NavConnect(Direction.UP, tile.BuiltRoom.NavNodeDown);
                            tile.BuiltRoom.NavConnect(Direction.DOWN, this.Down.Tiles[i].BuiltRoom.NavNodeUp);
                        }
                    }
                }

                room = tile.BuiltRoom;
            }
        }
    }
}
