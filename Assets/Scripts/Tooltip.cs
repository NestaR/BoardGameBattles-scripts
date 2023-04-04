using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI tipText, nameText;
    public RectTransform tipWindow;
    public static Action<string, string> OnMouseHover;
    public static Action OnMouseLoseFocus;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }
    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    void Start()
    {
        HideTip();
    }
    public string changeTip(string name, string replaceTip)
    {
        string tip;
        if (name.Contains("Fire Slash"))
        {
            nameText.text += " - 8 / 10MP";
            tip = "Imbue your weapon with flames to gain an attack buff after the move ends";
            return tip;
        }
        else if (name.Contains("Thunder Strike"))
        {
            nameText.text += " - 10|15 / 10MP";
            tip = "Strike your opponent with a thunderous force. Deals more damage if used at the beggining of the turn";
            return tip;
        }
        else if (name.Contains("Freezing Strike"))
        {
            nameText.text += " - 9 / 10MP";
            tip = "The chilling effects from this strike lowers the opponents attack";
            return tip;
        }
        else if (name.Contains("Toxic Slash"))
        {
            nameText.text += " - 7 / 10MP";
            tip = "A venomous attack that lowers the opponents defences";
            return tip;
        }
        else if (name.Contains("Double Strike"))
        {
            nameText.text += " - 12 / 8MP";
            tip = "Strike the user twice";
            return tip;
        }
        else if (name.Contains("Triple Strike"))
        {
            nameText.text += " - 15 / 15MP";
            tip = "Strike the user 3 times";
            return tip;
        }
        else if (name.Contains("Heavy Strike"))
        {
            nameText.text += " - 17 / 15MP";
            tip = "Strike the user with tremendous force";
            return tip;
        }
        else if (name.Contains("Healing Strike"))
        {
            nameText.text += " - 7 / 12MP";
            tip = "Divine energies restore the users health with this attack";
            return tip;
        }
        else if (name.Contains("Attack Buff"))
        {
            nameText.text += " - 5MP";
            tip = "Buff your attack by 15 points";
            return tip;
        }
        else if (name.Contains("Defence Buff"))
        {
            nameText.text += " - 5MP";
            tip = "Buff your defence by 15 points";
            return tip;
        }
        else if (name.Contains("Slash"))
        {
            nameText.text += " - 8";
            tip = "User slashes at their opponent";
            return tip;
        }
        else if (name.Contains("Spear Thrust"))
        {
            nameText.text += " - 8|12";
            tip = "Thrust at your opponent with precise accuracy. Has a 33% chance to crit";
            return tip;
        }
        else
        {
            return replaceTip;
        }
    }
    private void ShowTip(string name, string tip)
    {     
        if(name.Contains("ChestOption1"))
        {
            name = GameObject.Find("ChestOption1").GetComponentInChildren<Text>().text;
        }
        else if (name.Contains("ChestOption2"))
        {
            name = GameObject.Find("ChestOption2").GetComponentInChildren<Text>().text;
        }
        else if (name.Contains("ChestOption3"))
        {
            name = GameObject.Find("ChestOption3").GetComponentInChildren<Text>().text;
        }
        else if (name.Contains("AttackButton1"))
        {
            name = GameObject.Find("AttackButton1").GetComponentInChildren<Text>().text;
        }
        else if (name.Contains("AttackButton2"))
        {
            name = GameObject.Find("AttackButton2").GetComponentInChildren<Text>().text;
        }
        else if (name.Contains("AttackButton3"))
        {
            name = GameObject.Find("AttackButton3").GetComponentInChildren<Text>().text;
        }
        else if (name.Contains("AttackButton4"))
        {
            name = GameObject.Find("AttackButton4").GetComponentInChildren<Text>().text;
        }
        nameText.text = name;
        
        tipText.text = changeTip(name, tip);
        
        //tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 1200 ? 1200 : tipText.preferredWidth, nameText.preferredHeight + tipText.preferredHeight);

        tipWindow.gameObject.SetActive(true);
        //tipWindow.transform.position = new Vector2(mousePos.x, mousePos.y);
    }

    private void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
}
