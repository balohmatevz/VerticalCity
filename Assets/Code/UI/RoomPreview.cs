using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPreview : MonoBehaviour
{
    public Transform t;
    public SpriteRenderer sr;
    public RoomData Data;

    public void SetRoomToPreview(RoomData data)
    {
        Data = data;

        if (sr == null)
        {
            sr = this.GetComponent<SpriteRenderer>();
        }

        if (data == null)
        {
            sr.sprite = null;
        }
        else
        {
            sr.sprite = data.Sprite;
        }
    }

    public void AttachToTile(Tile tile)
    {
        if (t == null)
        {
            t = this.transform;
        }

        if (tile == null)
        {
            t.SetParent(null);
            t.position = new Vector3(0, -1000);
            return;
        }

        if (tile.CanBuildRoom(Data))
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }

        t.SetParent(tile.transform);
        t.localScale = Vector3.one;
        t.localPosition = new Vector3(((Data.NeededTiles - 1) / 2f) * Floor.TILE_WIDTH, 0, -5);
        t.localRotation = Quaternion.identity;
    }
}
