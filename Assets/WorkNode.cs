using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkNode : NavNode
{
    public float StartTime = 8;
    public float EndTime = 17;

    public float WagePerHour = 10f;
    public float EarningsPerHour = 5f;

    public float Money = 0;

    // Use this for initialization
    void Start()
    {
        GameController.obj.NavNodes.Add(this);
        GameController.obj.JobListings.Add(this);
    }

    private void OnDestroy()
    {
        if (GameController.obj.JobListings.Contains(this))
        {
            GameController.obj.JobListings.Remove(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PayEmployee(Person person)
    {
        if (Money > 0)
        {
            float amountToPay = GameController.FrameTimeDiff() * WagePerHour;
            person.Money += amountToPay;
            Money -= amountToPay;
            Money = Mathf.Max(0, Money);
        }
    }
}
