using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PersonInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI PersonName;
    public TextMeshProUGUI PersonWork;
    public TextMeshProUGUI PersonHome;
    public TextMeshProUGUI PersonMoney;
    public TextMeshProUGUI PersonCurrentAction;
    public TextMeshProUGUI PersonNeeds;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.obj.SelectedPerson == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            PersonCurrentAction.text = "<b>Current action:</b> " + GameController.obj.SelectedPerson.Action;
            PersonNeeds.text = "<b>Needs:</b>\nSocial: " + Mathf.RoundToInt(GameController.obj.SelectedPerson.NeedSocial * 100) + "%\nShopping: " + Mathf.RoundToInt(GameController.obj.SelectedPerson.NeedShopping * 100) + "%\nSleep: " + Mathf.RoundToInt(GameController.obj.SelectedPerson.NeedSleep * 100) + "%\n";
        }
    }

    public void SetUp()
    {
        PersonName.text = GameController.obj.SelectedPerson.Name;
        PersonWork.text = "<b>Employment:</b> " + ((GameController.obj.SelectedPerson.Employment != null) ? GameController.obj.SelectedPerson.Employment.InRoom.Data.Name : "In town");
        PersonHome.text = "<b>Home:</b> " + ((GameController.obj.SelectedPerson.Home != null) ? GameController.obj.SelectedPerson.Home.InRoom.Data.Name : "Homeless");
        PersonCurrentAction.text = "<b>Current action:</b> " + GameController.obj.SelectedPerson.Action;
        PersonMoney.text = "Money: £" + GameController.obj.SelectedPerson.Money.ToString("0");
        PersonNeeds.text = "<b>Needs:</b>\nSocial: " + Mathf.RoundToInt(GameController.obj.SelectedPerson.NeedSocial * 100) + "%\nShopping: " + Mathf.RoundToInt(GameController.obj.SelectedPerson.NeedShopping * 100) + "%\nSleep: " + Mathf.RoundToInt(GameController.obj.SelectedPerson.NeedSleep * 100) + "%\n";
    }

    public void INPUT_Close()
    {
        GameController.obj.SelectedRoom = null;
        gameObject.SetActive(false);
    }
}
