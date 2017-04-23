using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavNode : MonoBehaviour
{
    public List<NavNode> Connections;
    public LineRenderer Line;
    public Room InRoom;

    public List<NavNode> BorderConnections = new List<NavNode>();

    // Use this for initialization
    void Start()
    {
        GameController.obj.NavNodes.Add(this);
    }

    private void OnDestroy()
    {
        GameController.obj.NavNodes.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Connect(NavNode node)
    {
        if (!Connections.Contains(node))
        {
            Connections.Add(node);

            if (Line == null)
            {
                Line = this.gameObject.GetComponent<LineRenderer>();
            }

            Line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            Line.positionCount += 3;

            Vector3 thisNodePosition = this.transform.position;
            Vector3 thatNodePosition = node.transform.position;

            thisNodePosition.z = -8;
            thatNodePosition.z = -8;

            Line.SetPosition(Line.positionCount - 3, thisNodePosition);
            Line.SetPosition(Line.positionCount - 2, thatNodePosition);
            Line.SetPosition(Line.positionCount - 1, thisNodePosition);
        }
    }

    public void Disconnect(MovementNode node)
    {
        if (Connections.Contains(node))
        {
            Connections.Remove(node);
        }
    }

    public void ClearConnections()
    {
        Connections.Clear();

        if (Line == null)
        {
            Line = this.gameObject.GetComponent<LineRenderer>();
        }

        Line.positionCount = 0;
    }

    public void OnInternalConnected(MovementNode movementNode)
    {
        if (Line == null)
        {
            Line = this.gameObject.GetComponent<LineRenderer>();
        }

        Line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        Line.positionCount += 3;

        Vector3 thisNodePosition = this.transform.position;
        Vector3 thatNodePosition = movementNode.transform.position;

        thisNodePosition.z = -8;
        thatNodePosition.z = -8;

        Line.SetPosition(Line.positionCount - 3, thisNodePosition);
        Line.SetPosition(Line.positionCount - 2, thatNodePosition);
        Line.SetPosition(Line.positionCount - 1, thisNodePosition);

        BorderConnections.Add(movementNode);
    }

    public enum NavStates { UNCHECKED, EXPANDED }
    public NavStates NavState = NavStates.UNCHECKED;
    public List<NavNode> PathArchive;

    private List<NavNode> GetAllConnections()
    {
        List<NavNode> results = new List<NavNode>();
        if (this is MovementNode)
        {
            MovementNode thisMN = (MovementNode)this;
            results.AddRange(thisMN.InternalNodes);
        }
        results.AddRange(Connections);
        results.AddRange(BorderConnections);
        if (InRoom != null)
        {
            if (InRoom.NavNodeLeft != null && InRoom.NavNodeLeft != this)
            {
                results.Add(InRoom.NavNodeLeft);
            }
            if (InRoom.NavNodeRight != null && InRoom.NavNodeRight != this)
            {
                results.Add(InRoom.NavNodeRight);
            }
            if (InRoom.NavNodeDown != null && InRoom.NavNodeDown != this)
            {
                results.Add(InRoom.NavNodeDown);
            }
            if (InRoom.NavNodeUp != null && InRoom.NavNodeUp != this)
            {
                results.Add(InRoom.NavNodeUp);
            }
        }
        return results;
    }

    public List<NavNode> FindPathTo(NavNode navNode)
    {
        if (NavState == NavStates.EXPANDED)
        {
            return PathArchive;
        }

        this.NavState = NavStates.EXPANDED;
        if (navNode == this)
        {
            //Found solution
            PathArchive = new List<NavNode> { this };
            return PathArchive;
        }
        else
        {
            List<NavNode> Nodes = new List<NavNode>();
            foreach (NavNode potentialNavNode in GetAllConnections())
            {
                if (potentialNavNode.NavState == NavStates.UNCHECKED)
                {
                    Nodes.Add(potentialNavNode);
                }
            }

            if (Nodes.Count == 0)
            {
                //This was a dead end
                PathArchive = null;
                return PathArchive;
            }

            foreach (NavNode node in Nodes)
            {
                PathArchive = node.FindPathTo(navNode);
                if (PathArchive != null)
                {
                    PathArchive.Add(this);
                    return PathArchive;
                }
            }

            PathArchive = null;
            return PathArchive;
        }
    }

    public void NavReset()
    {
        NavState = NavStates.UNCHECKED;
        PathArchive = null;
    }

    public void OnPersonEnter(Person person)
    {

    }

    public void OnPersonExit(Person person)
    {

    }
}
