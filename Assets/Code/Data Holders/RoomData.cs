using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData
{
    public string Name;
    public int NeededTiles;
    public Sprite Sprite;
    public GameObject Prefab;
    public bool NeedsRoomAboveAndBelow;

    public RoomData(string name, int neededTiles, Sprite sprite, GameObject prefab, bool needsRoomAboveAndBelow)
    {
        Name = name;
        NeededTiles = neededTiles;
        Sprite = sprite;
        Prefab = prefab;
        NeedsRoomAboveAndBelow = needsRoomAboveAndBelow;
    }
}
