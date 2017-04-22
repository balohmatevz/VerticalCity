using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Floor Owner;
    public SpriteRenderer sr;

    public Tile Up;
    public Tile Down;
    public Tile Left;
    public Tile Right;

    public Room BuiltRoom = null;

    public const float TILE_BUILD_TIME = 5f;

    public int TilePosition;
    public float BuildingTimer = TILE_BUILD_TIME;

    private TileState _type = TileState.NONE;
    public TileState State
    {
        get { return _type; }
        set
        {
            if (_type != value)
            {
                _type = value;
                switch (_type)
                {
                    case TileState.NONE:
                    case TileState.EMPTY:
                        sr.sprite = null;
                        break;
                    case TileState.BUILDING:
                        sr.sprite = GameController.obj.TileBuilding;
                        break;
                    case TileState.BUILT:
                        sr.sprite = GameController.obj.TileBuilt;
                        break;
                }
                UpdateNeighborGraphics();
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (State == TileState.BUILDING)
        {
            BuildingTimer -= Time.deltaTime;
            if (BuildingTimer <= 0)
            {
                State = TileState.BUILT;
            }
        }
    }

    public void SetUp(Floor floor, int tilePosition, TileState tileState)
    {
        sr = this.GetComponent<SpriteRenderer>();
        Owner = floor;
        State = tileState;
        TilePosition = tilePosition;

        name = "Tile " + Owner.FloorNumber + ":" + TilePosition;

        UpdateNeighbors();
    }

    public void OnMouseUp()
    {
        if (CameraMove.obj.IsDrag)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        switch (GameController.obj.State)
        {
            case GameState.PLAY_MODE:
                GameController.obj.SelectedTile = this;
                break;
            case GameState.BUILD_MODE:
                switch (State)
                {
                    case TileState.EMPTY:
                    case TileState.NONE:
                        if (CanBuild())
                        {
                            State = TileState.BUILDING;
                        }
                        break;
                    case TileState.BUILT:
                        if (GameController.obj.BuildableRoomPreview.Data != null)
                        {
                            if (CanBuildRoom(GameController.obj.BuildableRoomPreview.Data))
                            {
                                BuildRoom(GameController.obj.BuildableRoomPreview.Data);
                            }
                        }
                        break;
                }
                break;
        }
    }

    public void OnMouseEnter()
    {
        if (GameController.obj.BuildableRoomPreview.Data != null)
        {
            GameController.obj.BuildableRoomPreview.AttachToTile(this);
        }
    }

    public void OnSelect()
    {
        sr.color = Color.red;
    }

    public void OnDeselect()
    {
        sr.color = Color.white;
    }

    public void UpdateGraphics()
    {
        switch (GameController.obj.State)
        {
            case GameState.PLAY_MODE:
                switch (State)
                {
                    case TileState.NONE:
                    case TileState.EMPTY:
                        sr.sprite = null;
                        break;
                    case TileState.BUILDING:
                        sr.sprite = GameController.obj.TileBuilding;
                        break;
                    case TileState.BUILT:
                        sr.sprite = GameController.obj.TileBuilt;
                        break;
                }
                break;
            case GameState.BUILD_MODE:
                switch (State)
                {
                    case TileState.NONE:
                    case TileState.EMPTY:
                        if (CanBuild())
                        {
                            sr.sprite = GameController.obj.TileAdd;
                        }
                        else
                        {
                            sr.sprite = null;
                        }
                        break;
                    case TileState.HAS_ROOM:
                        sr.sprite = null;
                        break;
                    case TileState.BUILDING:
                        sr.sprite = GameController.obj.TileBuilding;
                        break;
                    case TileState.BUILT:
                        sr.sprite = GameController.obj.TileBuilt;
                        break;
                }
                break;
        }
    }

    public void OnModeChange()
    {
        UpdateGraphics();
    }

    public void UpdateNeighbors()
    {
        if (Owner.Tiles.Count > TilePosition + 1)
        {
            Right = Owner.Tiles[TilePosition + 1];
            Right.OnNeighborUpdated(Direction.LEFT, this);
        }
        else
        {
            Right = null;
        }

        if (TilePosition - 1 >= 0)
        {
            Left = Owner.Tiles[TilePosition - 1];
            Left.OnNeighborUpdated(Direction.RIGHT, this);
        }
        else
        {
            Left = null;
        }

        if (Owner.Down != null && Owner.Down.Tiles.Count > TilePosition)
        {
            Down = Owner.Down.Tiles[TilePosition];
            Down.OnNeighborUpdated(Direction.UP, this);
        }
        else
        {
            Down = null;
        }

        if (Owner.Up != null && Owner.Up.Tiles.Count > TilePosition)
        {
            Up = Owner.Up.Tiles[TilePosition];
            Up.OnNeighborUpdated(Direction.DOWN, this);
        }
        else
        {
            Up = null;
        }
    }

    public void OnNeighborUpdated(Direction updatedNeighbor, Tile neighbor)
    {
        switch (updatedNeighbor)
        {
            case Direction.UP:
                Up = neighbor;
                break;
            case Direction.DOWN:
                Down = neighbor;
                break;
            case Direction.LEFT:
                Left = neighbor;
                break;
            case Direction.RIGHT:
                Right = neighbor;
                break;
        }
    }

    public void UpdateNeighborGraphics()
    {
        if (Up != null)
        {
            Up.UpdateGraphics();
        }
        if (Down != null)
        {
            Down.UpdateGraphics();
        }
        if (Left != null)
        {
            Left.UpdateGraphics();
        }
        if (Right != null)
        {
            Right.UpdateGraphics();
        }
    }

    public bool CanBuild()
    {
        if (Owner.FloorNumber == 0)
        {
            return true;
        }
        if (Down != null && (Down.State == TileState.BUILT || Down.State == TileState.HAS_ROOM))
        {
            return true;
        }
        if (Left != null && (Left.State == TileState.BUILT || Left.State == TileState.HAS_ROOM))
        {
            return true;
        }
        if (Right != null && (Right.State == TileState.BUILT || Right.State == TileState.HAS_ROOM))
        {
            return true;
        }
        return false;
    }

    public bool CanBuildRoom(RoomData room)
    {
        int neededTiles = room.NeededTiles;

        Tile tile = this;
        while (neededTiles > 0)
        {
            if (tile == null || !tile.IsSuitableForRoomBuilding())
            {
                return false;
            }
            tile = tile.Right;
            neededTiles--;
        }

        return true;
    }

    public bool IsSuitableForRoomBuilding()
    {
        if (State != TileState.BUILT)
        {
            //Tile not in a suitable state for new room to be built in
            return false;
        }
        return true;
    }

    public void BuildRoom(RoomData data)
    {
        if (!CanBuildRoom(data))
        {
            return;
        }

        GameObject go = Instantiate(data.Prefab);
        Transform t = go.transform;
        Room room = go.GetComponent<Room>();
        t.SetParent(this.transform);
        t.localScale = Vector3.one;
        t.localPosition = new Vector3(((data.NeededTiles - 1) / 2f) * Floor.TILE_WIDTH, 0, -1);
        t.localRotation = Quaternion.identity;

        Tile tile = this;
        tile.BuiltRoom = room;
        tile.State = TileState.HAS_ROOM;
        int neededTiles = data.NeededTiles - 1;
        while (neededTiles > 0)
        {
            tile = tile.Right;
            tile.BuiltRoom = room;
            tile.State = TileState.HAS_ROOM;
            tile.UpdateGraphics();
            neededTiles--;
        }

        Owner.RegenerateNavigationGraph();
        room.OnRoomCreated();
    }
}
