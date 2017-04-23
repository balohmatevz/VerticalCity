using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour
{

    public const float MOVEMENT_SPEED = 25f;

    public List<NavNode> CurrentPath;
    public NavNode CurrentNavNode;
    public NavNode TargetNavNode;
    public int PositionOnPath = 0;

    public WorkNode Employment;
    public HomeNode Home;

    public float Money = 0;

    // Use this for initialization
    void Start()
    {
        GameController.obj.People.Add(this);

        this.transform.position = Building.obj.EntryNode.transform.position;
        CurrentNavNode = Building.obj.EntryNode;
    }

    private void OnDestroy()
    {
        GameController.obj.People.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Finding a job
        if (Employment == null)
        {
            if (GameController.obj.JobListings.Count > 0)
            {
                int rnd = Random.Range(0, GameController.obj.JobListings.Count);
                Employment = GameController.obj.JobListings[rnd];
                GameController.obj.JobListings.Remove(Employment);
            }
        }

        //Finding a home
        if (Home == null)
        {
            if (GameController.obj.Vacancies.Count > 0)
            {
                int rnd = Random.Range(0, GameController.obj.Vacancies.Count);
                Home = GameController.obj.Vacancies[rnd];
                GameController.obj.Vacancies.RemoveAt(rnd);
            }
        }

        //Movement
        if (TargetNavNode != null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, TargetNavNode.transform.position, MOVEMENT_SPEED * GameController.FrameTimeDiff());
            if (Vector2.Distance(this.transform.position, TargetNavNode.transform.position) < 0.001f)
            {
                CurrentNavNode = TargetNavNode;
                PositionOnPath++;
                if (CurrentPath.Count > PositionOnPath)
                {
                    TargetNavNode = CurrentPath[PositionOnPath];
                }
                else
                {
                    TargetNavNode = null;
                }
            }
        }
        else
        {
            Act();
        }


    }

    public string Action = "";

    public void Act()
    {
        Action = "";

        //Sleep
        if (Home != null)
        {
            if (GameController.obj.CurrentTime >= GameController.SLEEP_START &&
                GameController.obj.CurrentTime < GameController.SLEEP_END)
            {
                if (CurrentNavNode == Home)
                {
                    //Already sleeping
                    Action = "Sleeping";
                }
                else
                {
                    MoveToNode(Home);
                    Action = "Going to Sleep";
                }
            }
        }

        //Work
        if (Employment != null)
        {
            if (GameController.obj.CurrentTime >= Employment.StartTime &&
                GameController.obj.CurrentTime < Employment.EndTime)
            {
                if (CurrentNavNode == Employment)
                {
                    //Already working
                    Employment.Money += GameController.FrameTimeDiff() * Employment.EarningsPerHour;
                    Employment.PayEmployee(this);
                    Action = "Working at job";
                }
                else
                {
                    MoveToNode(Employment);
                    Action = "Going to work at job";
                }
            }
        }
        else
        {
            if (GameController.obj.CurrentTime >= GameController.OUTSIDE_WORK_START &&
                GameController.obj.CurrentTime < GameController.OUTSIDE_WORK_END)
            {
                if (CurrentNavNode == Building.obj.EntryNode)
                {
                    //Already working
                    Money += GameController.FrameTimeDiff() * GameController.OUTSIDE_WORK_MONEY_PER_HOUR;
                    Action = "Working outside";
                }
                else
                {
                    MoveToNode(Building.obj.EntryNode);
                    Action = "Going to work outside";
                }
            }
        }

        if (Action == "")
        {
            Action = "Random";
            int rnd = Random.Range(0, GameController.obj.NavNodes.Count);
            MoveToNode(GameController.obj.NavNodes[rnd]);
        }
    }

    public void MoveToNode(NavNode targetNode)
    {
        Debug.Log("Pathfinding to " + targetNode);
        GameController.obj.ResetNavGraph();
        List<NavNode> pathFound = CurrentNavNode.FindPathTo(targetNode);
        if (pathFound != null && pathFound.Count > 0)
        {
            pathFound.Reverse();
            CurrentPath = new List<NavNode>();
            Room room = null;
            NavNode nodeArchive = null;
            foreach (NavNode node in pathFound)
            {
                if (node.InRoom != room)
                {
                    if (nodeArchive != null)
                    {
                        CurrentPath.Add(nodeArchive);
                    }
                    CurrentPath.Add(node);
                }
                room = node.InRoom;
                nodeArchive = node;
            }
            CurrentPath.Add(nodeArchive);
            PositionOnPath = 0;
            TargetNavNode = CurrentPath[PositionOnPath];
        }
        else
        {
            Debug.Log("NO PATH FOUND");
        }
    }
}
