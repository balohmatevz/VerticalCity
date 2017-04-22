using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavNode : MonoBehaviour
{
    public List<NavNode> Connections;
    public LineRenderer Line;

    // Use this for initialization
    void Start()
    {
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
    }
}
