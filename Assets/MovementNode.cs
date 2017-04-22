using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementNode : NavNode
{
    public List<NavNode> InternalNodes;

    public void ConnectInternal(NavNode navNode)
    {
        InternalNodes.Add(navNode);
        navNode.OnInternalConnected(this);
    }
}
