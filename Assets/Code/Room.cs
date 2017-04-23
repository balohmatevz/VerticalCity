using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public MovementNode NavNodeLeft;
    public MovementNode NavNodeRight;
    public MovementNode NavNodeUp;
    public MovementNode NavNodeDown;

    public List<WorkNode> WorkNodes;
    public List<LeisureNode> LeisureNodes;
    public List<HomeNode> HomeNodes;

    public int XPosition;
    public RoomData Data;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRoomCreated(int xPosition, RoomData data)
    {
        RecreateInternalNavGraph();
        XPosition = xPosition;
        Data = data;
    }

    public void ClearNavGraph()
    {
        if (NavNodeLeft != null)
        {
            NavNodeLeft.ClearConnections();
        }
        if (NavNodeRight != null)
        {
            NavNodeRight.ClearConnections();
        }
        if (NavNodeUp != null)
        {
            NavNodeUp.ClearConnections();
        }
        if (NavNodeDown != null)
        {
            NavNodeDown.ClearConnections();
        }
    }

    public void NavConnect(Direction dir, MovementNode node)
    {
        switch (dir)
        {
            case Direction.UP:
                if (NavNodeUp != null)
                {
                    NavNodeUp.Connect(node);
                }
                break;
            case Direction.DOWN:
                if (NavNodeDown != null)
                {
                    NavNodeDown.Connect(node);
                }
                break;
            case Direction.LEFT:
                if (NavNodeLeft != null)
                {
                    NavNodeLeft.Connect(node);
                }
                break;
            case Direction.RIGHT:
                if (NavNodeRight != null)
                {
                    NavNodeRight.Connect(node);
                }
                break;
        }
    }

    public void RecreateInternalNavGraph()
    {
        List<MovementNode> EdgeNodes = new List<MovementNode>();
        if (NavNodeUp != null)
        {
            EdgeNodes.Add(NavNodeUp);
        }
        if (NavNodeDown != null)
        {
            EdgeNodes.Add(NavNodeDown);
        }
        if (NavNodeLeft != null)
        {
            EdgeNodes.Add(NavNodeLeft);
        }
        if (NavNodeRight != null)
        {
            EdgeNodes.Add(NavNodeRight);
        }

        foreach (MovementNode movementNode in EdgeNodes)
        {
            movementNode.InternalNodes = new List<NavNode>();
            foreach (NavNode navNode in WorkNodes)
            {
                movementNode.ConnectInternal(navNode);
            }
            foreach (NavNode navNode in LeisureNodes)
            {
                movementNode.ConnectInternal(navNode);
            }
            foreach (NavNode navNode in HomeNodes)
            {
                movementNode.ConnectInternal(navNode);
            }
        }

    }
}
