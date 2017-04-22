using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Floor : MonoBehaviour
{
    public Building Owner;

    public List<Tile> Tiles;

    public int TILES_PER_FLOOR = 40;
    public float TILE_WIDTH = 0.5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUp(Building building)
    {
        Owner = building;

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

        tile.SetUp(this);

        return tile;
    }
}
