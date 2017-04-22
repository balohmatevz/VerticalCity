using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{

    public List<Floor> Floors;


    public float FLOOR_HEIGHT = 2f;

    // Use this for initialization
    void Start()
    {
        Floors = new List<Floor>();
        
        AddFloor();
        AddFloor();
        AddFloor();
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

        floor.SetUp(this);
    }
}
