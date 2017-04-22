using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildModeButton : MonoBehaviour
{

    public Button Btn;
    public GameObject RoomButtons;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void INPUT_OnButtonPress()
    {
        if (GameController.obj.State == GameState.BUILD_MODE)
        {
            GameController.obj.State = GameState.PLAY_MODE;
            RoomButtons.gameObject.SetActive(false);
        }
        else
        {
            GameController.obj.State = GameState.BUILD_MODE;
            RoomButtons.gameObject.SetActive(true);
        }
    }
}
