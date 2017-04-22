using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectButton : MonoBehaviour
{
    public RoomData Data;
    public TextMeshProUGUI Text;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUp(RoomData data)
    {
        Data = data;
        Text.text = data.Name;
    }

    public void INPUT_OnClick()
    {
        GameController.obj.SelectedBuildableRoom = Data;
        GameController.obj.BuildableRoomPreview.SetRoomToPreview(Data);
    }
}
