using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerOptions : MonoBehaviour
{
    public string[] attackName;
    public GameObject attackCanvas, menuCanvas;
    public GameObject[] getButtons;
    public GameObject[] getButtonsText;
    public TextMeshProUGUI moveName, moveDes;
    // Start is called before the first frame update
    void Start()
    {
        attackName = new string[4];
        getButtons = new GameObject[4];
        getButtonsText = new GameObject[4];
    }

    // Update is called once per frame
    void Update()
    {
        if (attackName[0] == null && PlayerPrefs.GetString("CharacterSelected") == "MedievalWarrior2")
        {//Player always starts with a default attack
            attackName[0] = "Spear Thrust";
            attackName[1] = "";
            attackName[2] = "";
            attackName[3] = "";
        }
        else if(attackName[0] == null)
        {
            attackName[0] = "Slash";
            attackName[1] = "";
            attackName[2] = "";
            attackName[3] = "";
        }

        attackCanvas = GameObject.Find("AttackBG");
        menuCanvas = GameObject.Find("MenuCanvas");
        if(attackCanvas != null)
        {
            if (attackCanvas.activeSelf == true)
            {
                FindAttackButtons();
            }
        }
        else if(menuCanvas != null)
        {
            if (menuCanvas.activeSelf == true)
            {
                FindAttackButtons();
            }
        }
    }
    public void FindAttackButtons()
    {
        Array.Clear(getButtons, 0, getButtons.Length);
        Array.Clear(getButtonsText, 0, getButtonsText.Length);
        getButtons = GameObject.FindGameObjectsWithTag("AttackButtons");
        getButtonsText = GameObject.FindGameObjectsWithTag("AttackText");
        for (int i = 0; i < 4; i++)
        {//Only show buttons for the attacks that are available
            if (attackName[i] != "")
            {
                getButtons[i].GetComponent<Button>().interactable = true;
                getButtonsText[i].GetComponent<Text>().text = attackName[i];
            }
            else
            {
                getButtons[i].GetComponent<Button>().interactable = false;
                getButtonsText[i].GetComponent<Text>().text = "";
            }
        }
    }
    public void chooseAttack1()
    {//Check which move was pressed
        moveName.text = attackName[0];
        moveDes.text = changeTip(attackName[0]);
    }
    public void chooseAttack2()
    {
        moveName.text = attackName[1];
        moveDes.text = changeTip(attackName[1]);
    }
    public void chooseAttack3()
    {
        moveName.text = attackName[2];
        moveDes.text = changeTip(attackName[2]);
    }
    public void chooseAttack4()
    {
        moveName.text = attackName[3];
        moveDes.text = changeTip(attackName[3]);
    }
    public string changeTip(string name)
    {
        string tip;
        if (name.Contains("Fire Slash"))
        {
            moveName.text += " - 8 / 10MP";
            tip = "Imbue your weapon with flames to gain an attack buff after the move ends";
            return tip;
        }
        else if (name.Contains("Thunder Strike"))
        {
            moveName.text += " - 10|15 / 10MP";
            tip = "Strike your opponent with a thunderous force. Deals more damage if used at the beggining of the turn";
            return tip;
        }
        else if (name.Contains("Freezing Strike"))
        {
            moveName.text += " - 9 / 10MP";
            tip = "The chilling effects from this strike lowers the opponents attack";
            return tip;
        }
        else if (name.Contains("Toxic Slash"))
        {
            moveName.text += " - 7 / 10MP";
            tip = "A venomous attack that lowers the opponents defences";
            return tip;
        }
        else if (name.Contains("Double Strike"))
        {
            moveName.text += " - 12 / 8MP";
            tip = "Strike the user twice";
            return tip;
        }
        else if (name.Contains("Triple Strike"))
        {
            moveName.text += " - 15 / 15MP";
            tip = "Strike the user 3 times";
            return tip;
        }
        else if (name.Contains("Heavy Strike"))
        {
            moveName.text += " - 17 / 15MP";
            tip = "Strike the user with tremendous force";
            return tip;
        }
        else if (name.Contains("Healing Strike"))
        {
            moveName.text += " - 7 / 12MP";
            tip = "Divine energies restore the users health with this attack";
            return tip;
        }
        else if (name.Contains("Attack Buff"))
        {
            moveName.text += " - 5MP";
            tip = "Buff your attack by 15 points";
            return tip;
        }
        else if (name.Contains("Defence Buff"))
        {
            moveName.text += " - 5MP";
            tip = "Buff your defence by 15 points";
            return tip;
        }
        else if (name.Contains("Slash"))
        {
            moveName.text += " - 8";
            tip = "User slashes at their opponent";
            return tip;
        }
        else if (name.Contains("Spear Thrust"))
        {
            moveName.text += " - 8|12";
            tip = "Thrust at your opponent with precise accuracy. Has a 33% chance to crit";
            return tip;
        }
        else
        {
            return "";
        }
    }
}
