﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickCatcher : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        GameController.obj.SelectedTile = null;
        GameController.obj.BuildableRoomPreview.SetRoomToPreview(null);
    }
}
