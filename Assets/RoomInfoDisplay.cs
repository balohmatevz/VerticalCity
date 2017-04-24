using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI RoomName;
    public TextMeshProUGUI RoomMoney;
    public TMP_InputField RentInputField;
    public TextMeshProUGUI RoomPeople;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.obj.SelectedRoom == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            RoomMoney.text = GameController.obj.SelectedRoom.GetMoneyText();
        }
    }

    public void INPUT_Close()
    {
        GameController.obj.SelectedRoom = null;
        gameObject.SetActive(false);
    }

    public void SetUp()
    {
        RoomName.text = GameController.obj.SelectedRoom.Data.Name;
        RoomMoney.text = GameController.obj.SelectedRoom.GetMoneyText();
        RentInputField.text = GameController.obj.SelectedRoom.Rent.ToString();

        string people = "";
        if (GameController.obj.SelectedRoom.WorkNodes.Count > 0)
        {
            people += "\n<b>Employees:</b>\n";
            foreach (WorkNode wn in GameController.obj.SelectedRoom.WorkNodes)
            {
                if (wn.Employee != null)
                {
                    people += wn.Employee.Name + "\n";
                }
                else
                {
                    people += "Vacancy\n";
                }
            }
        }
        if (GameController.obj.SelectedRoom.HomeNodes.Count > 0)
        {
            people += "\n<b>Residents:</b>\n";
            foreach (HomeNode hn in GameController.obj.SelectedRoom.HomeNodes)
            {
                if (hn.Resident != null)
                {
                    people += hn.Resident.Name + "\n";
                }
                else
                {
                    people += "Vacancy\n";
                }
            }
        }
        if (GameController.obj.SelectedRoom.LeisureNodes.Count > 0)
        {
            people += "\n<b>Customers:</b>\n";
            foreach (LeisureNode ln in GameController.obj.SelectedRoom.LeisureNodes)
            {
                if (ln.Reservation != null)
                {
                    people += ln.Reservation.Name + "\n";
                }
                else
                {
                    people += "None (£" + ln.MoneyToUse.ToString("0") + ")\n";
                }
            }
        }

        RoomPeople.text = people;
    }

    public void INPUT_SetRentEditEnd()
    {
        string valueStr = RentInputField.text;
        int value;
        if (int.TryParse(valueStr, out value))
        {
            GameController.obj.SelectedRoom.Rent = value;
        }
    }
}
