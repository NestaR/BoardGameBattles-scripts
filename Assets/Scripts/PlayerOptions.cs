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
    public Button[] b;
    public TextMeshProUGUI moveName, moveDes;
    // Start is called before the first frame update
    void Start()
    {
        attackName = new string[4];
        getButtons = new GameObject[4];
        getButtonsText = new GameObject[4];
        b = new Button[4];
        //b[0].onClick.AddListener(chooseAttack1);
        //b[1].onClick.AddListener(chooseAttack2);
        //b[2].onClick.AddListener(chooseAttack3);
        //b[3].onClick.AddListener(chooseAttack4);
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
        if (attackCanvas != null)
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
                moveName = GameObject.Find("TipName").GetComponent<TextMeshProUGUI>();
                moveDes = GameObject.Find("TipText").GetComponent<TextMeshProUGUI>();
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
            if (menuCanvas != null)
            {
                if (attackName[i] != "" && menuCanvas.activeSelf == true)
                {
                    b[i] = getButtons[i].GetComponent<Button>();
                    AddLis(i);
                }
            }
        }
    }
    public void AddLis(int num)
    {//Add listeners to available attacks 
        if(num == 0)
        {
            b[num].onClick.AddListener(chooseAttack1);
        }
        else if (num == 1)
        {
            b[num].onClick.AddListener(chooseAttack2);
        }
        else if (num == 2)
        {
            b[num].onClick.AddListener(chooseAttack3);
        }
        else if (num == 3)
        {
            b[num].onClick.AddListener(chooseAttack4);
        }
    }
    public void chooseAttack1()
    {//Show a description of the move
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
            moveName.text += " - 8 / 8MP";
            tip = "Imbue your weapon with flames to gain an attack buff after the move ends";
            return tip;
        }
        else if (name.Contains("Thunder Strike"))
        {
            moveName.text += " - 10|15 / 8MP";
            tip = "Strike your opponent with a thunderous force. Deals more damage if used at the beggining of the turn";
            return tip;
        }
        else if (name.Contains("Freezing Strike"))
        {
            moveName.text += " - 9 / 8MP";
            tip = "The chilling effects from this strike lowers the opponents attack";
            return tip;
        }
        else if (name.Contains("Toxic Slash"))
        {
            moveName.text += " - 7 / 8MP";
            tip = "A venomous attack that lowers the opponents defences";
            return tip;
        }
        else if (name.Contains("Double Strike"))
        {
            moveName.text += " - 11 / 3MP";
            tip = "Strike the user twice";
            return tip;
        }
        else if (name.Contains("Triple Strike"))
        {
            moveName.text += " - 15 / 9MP";
            tip = "Strike the user 3 times";
            return tip;
        }
        else if (name.Contains("Heavy Strike"))
        {
            moveName.text += " - 13 / 6MP";
            tip = "Strike the user with tremendous force";
            return tip;
        }
        else if (name.Contains("Healing Strike"))
        {
            moveName.text += " - 7 / 10MP";
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
        else if (name.Contains("Radiant Flicker"))
        {
            moveName.text += " - 8 / 3MP";
            tip = "Cast a beam of light";
            return tip;
        }
        else if (name.Contains("Luminous Spark"))
        {
            moveName.text += " - 12 / 9MP";
            tip = "Cast a strong ball of energy";
            return tip;
        }
        else if (name.Contains("Eruption"))
        {
            moveName.text += " - 0.5*BA / 10MP";
            tip = "Consume your battle attack to increase damage";
            return tip;
        }
        else if (name.Contains("Thunderball"))
        {
            moveName.text += " - 2+S / 8MP";
            tip = "Deals bonus damage based on speed";
            return tip;
        }
        else if (name.Contains("Plunge"))
        {
            moveName.text += " - 8 / 8MP";
            tip = "Cleanse targets battle attack";
            return tip;
        }
        else if (name.Contains("Rock Blast"))
        {
            moveName.text += " - 8 / 8MP";
            tip = "Destroy targets battle defence";
            return tip;
        }
        else
        {
            return "";
        }
    }
}
