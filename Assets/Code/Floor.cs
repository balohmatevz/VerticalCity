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

        tile.SetUp(this, x, TileState.EMPTY);

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
}
