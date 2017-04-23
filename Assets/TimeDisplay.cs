using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI Text;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Text.text = Mathf.FloorToInt(GameController.obj.CurrentTime) + ":" + Mathf.FloorToInt(Mathf.FloorToInt((GameController.obj.CurrentTime % 1) * 4) * 15).ToString().PadLeft(2, '0');
    }
}
