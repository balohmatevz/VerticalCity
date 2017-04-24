using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public int Rent;

    public float Money = 0;

    // Use this for initialization
    void Start()
    {
        GameController.obj.Rooms.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetMoneyText()
    {
        float money = Money;
        foreach (HomeNode home in HomeNodes)
        {
            if (home.Resident != null)
            {
                money += home.Resident.Money;
            }
        }

        return "£" + money.ToString("0");
    }

    public void OnRoomCreated(int xPosition, RoomData data)
    {
        RecreateInternalNavGraph();
        XPosition = xPosition;
        Data = data;
    }

    public void OnMouseUp()
    {
        if (CameraMove.obj.IsDrag)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        GameController.obj.SelectedRoom = this;
        GameController.obj.SelectedPerson = null;
        GameController.obj.RoomDisplay.gameObject.SetActive(true);
        GameController.obj.PersonDisplay.gameObject.SetActive(false);
        GameController.obj.RoomDisplay.SetUp();
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

    public void PayRent()
    {
        if (Money > Rent)
        {
            Money -= Rent;
            GameController.obj.Money += Rent;
        }
        else
        {
            //Not enough to pay rent, take everything
            float missingRent = Rent - Money;
            GameController.obj.Money += (int)Money;
            Money = 0;

            int residents = 0;
            if (HomeNodes.Count > 0)
            {
                foreach (HomeNode hn in HomeNodes)
                {
                    if (hn.Resident != null)
                    {
                        residents++;
                    }
                }
            }

            if (residents > 0)
            {
                //Take from residents
                float sharePerResident = missingRent / residents;
                foreach (HomeNode hn in HomeNodes)
                {
                    if (hn.Resident != null)
                    {
                        if (hn.Resident.Money > sharePerResident)
                        {
                            //Take share
                            hn.Resident.Money -= sharePerResident;
                            GameController.obj.Money += (int)sharePerResident;
                        }
                        else
                        {
                            //Take all
                            GameController.obj.Money += (int)hn.Resident.Money;
                            hn.Resident.Money = 0;
                        }
                    }
                }
            }

        }
    }
}
