using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeisureNode : NavNode
{

    public float NeedSattisfactionShopping = 0;
    public float NeedSattisfactionSocial = 0;

    public float MoneyToUse = 150f;

    public Person Reservation = null;

    // Use this for initialization
    void Start()
    {
        GameController.obj.NavNodes.Add(this);
        if (NeedSattisfactionShopping > 0)
        {
            GameController.obj.Shops.Add(this);
        }
        if (NeedSattisfactionSocial > 0)
        {
            GameController.obj.Bars.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
