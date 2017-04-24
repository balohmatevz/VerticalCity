using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeNode : NavNode
{
    public Person Resident;

    // Use this for initialization
    void Start()
    {
        GameController.obj.NavNodes.Add(this);
        GameController.obj.Vacancies.Add(this);
    }

    private void OnDestroy()
    {
        if (GameController.obj.Vacancies.Contains(this))
        {
            GameController.obj.Vacancies.Remove(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
