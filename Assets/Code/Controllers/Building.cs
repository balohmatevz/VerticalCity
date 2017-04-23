using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public static Building obj;

    public List<Floor> Floors;
    public MovementNode EntryNode;

    public float FLOOR_HEIGHT = 2f;

    // Use this for initialization
    private void Awake()
    {
        obj = this;
    }

    void Start()
    {
        Floors = new List<Floor>();

        AddFloor();
        AddFloor();
        AddFloor();

        if (GameController.obj.BuildDefault)
        {

            Floors[0].Tiles[16].BuildRoom(GameController.obj.BuildableRooms[0]);

            Floors[1].Tiles[0].BuildRoom(GameController.obj.BuildableRooms[2]);
            Floors[1].Tiles[8].BuildRoom(GameController.obj.BuildableRooms[7]);
            Floors[1].Tiles[16].BuildRoom(GameController.obj.BuildableRooms[0]);
            Floors[1].Tiles[19].BuildRoom(GameController.obj.BuildableRooms[3]);
            Floors[1].Tiles[29].BuildRoom(GameController.obj.BuildableRooms[8]);

            Floors[2].Tiles[0].BuildRoom(GameController.obj.BuildableRooms[2]);
            Floors[2].Tiles[8].BuildRoom(GameController.obj.BuildableRooms[7]);
            Floors[2].Tiles[16].BuildRoom(GameController.obj.BuildableRooms[0]);
            Floors[2].Tiles[19].BuildRoom(GameController.obj.BuildableRooms[3]);
            Floors[2].Tiles[29].BuildRoom(GameController.obj.BuildableRooms[8]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddFloor()
    {
        GameObject go = Instantiate(GameController.obj.PF_Floor);
        Transform t = go.GetComponent<Transform>();
        Floor floor = go.GetComponent<Floor>();

        Floors.Add(floor);

        t.SetParent(this.transform);
        t.localScale = Vector3.one;
        t.rotation = Quaternion.identity;
        t.localPosition = new Vector3(0, (Floors.Count - 1) * FLOOR_HEIGHT, 0);

        floor.SetUp(this, Floors.Count - 1);
    }

    public void OnModeChange()
    {
        foreach (Floor floor in Floors)
        {
            floor.OnModeChange();
        }
    }
}
