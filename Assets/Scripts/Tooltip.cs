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
    {//Show a description for each move
        string tip;
        if (name.Contains("Fire Slash"))
        {
            nameText.text += " - 8 / 8MP";
            tip = "Imbue your weapon with flames to gain an attack buff after the move ends";
            return tip;
        }
        else if (name.Contains("Thunder Strike"))
        {
            nameText.text += " - 10|15 / 8MP";
            tip = "Strike your opponent with a thunderous force. Deals more damage if used at the beggining of the turn";
            return tip;
        }
        else if (name.Contains("Freezing Strike"))
        {
            nameText.text += " - 9 / 8MP";
            tip = "The chilling effects from this strike lowers the opponents attack";
            return tip;
        }
        else if (name.Contains("Toxic Slash"))
        {
            nameText.text += " - 7 / 8MP";
            tip = "A venomous attack that lowers the opponents defences";
            return tip;
        }
        else if (name.Contains("Double Strike"))
        {
            nameText.text += " - 11 / 3MP";
            tip = "Strike the user twice";
            return tip;
        }
        else if (name.Contains("Triple Strike"))
        {
            nameText.text += " - 15 / 9MP";
            tip = "Strike the user 3 times";
            return tip;
        }
        else if (name.Contains("Heavy Strike"))
        {
            nameText.text += " - 13 / 6MP";
            tip = "Strike the user with tremendous force";
            return tip;
        }
        else if (name.Contains("Healing Strike"))
        {
            nameText.text += " - 7 / 10MP";
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
        else if (name.Contains("Radiant Flicker"))
        {
            nameText.text += " - 8 / 3MP";
            tip = "Cast a beam of light";
            return tip;
        }
        else if (name.Contains("Luminous Spark"))
        {
            nameText.text += " - 12 / 9MP";
            tip = "Cast a strong ball of energy";
            return tip;
        }
        else if (name.Contains("Eruption"))
        {
            nameText.text += " - 0.5*BA / 10MP";
            tip = "Consume your battle attack to increase damage";
            return tip;
        }
        else if (name.Contains("Thunderball"))
        {
            nameText.text += " - 2+S / 8MP";
            tip = "Deals bonus damage based on speed";
            return tip;
        }
        else if (name.Contains("Plunge"))
        {
            nameText.text += " - 8 / 8MP";
            tip = "Cleanse targets battle attack";
            return tip;
        }
        else if (name.Contains("Rock Blast"))
        {
            nameText.text += " - 8 / 8MP";
            tip = "Destroy targets battle defence";
            return tip;
        }
        else if (name.Contains("Signature Ability"))
        {
            if (PlayerPrefs.GetString("SignatureAbility").Contains("In Bloom"))
            {//Using an attack restores health and mana
                nameText.text = "In Bloom";
                tip = "Using an attack restores 4 + (" + PlayerPrefs.GetInt("EnemiesDefeated") + ") health and mana";
                return tip;
            }
            else if (PlayerPrefs.GetString("SignatureAbility").Contains("Mayday"))
            {//Using an attack restores health and mana
                nameText.text = "Mayday";
                tip = "Getting hit boosts your battle defence by 5 + (" + PlayerPrefs.GetInt("EnemiesDefeated") + ")";
                return tip;
            }
            else if (PlayerPrefs.GetString("SignatureAbility").Contains("Sunrise/Sunset"))
            {//Using an attack restores health and mana
                nameText.text = "Sunrise/Sunset";
                tip = "All attacks hit twice";
                return tip;
            }
            else if (PlayerPrefs.GetString("SignatureAbility").Contains("Roundabout"))
            {//Using an attack restores health and mana
                nameText.text = "Roundabout";
                tip = "Using the same attack previously used boosts battle attack by 11 + (" + PlayerPrefs.GetInt("EnemiesDefeated") + ")";
                return tip;
            }
            else if (PlayerPrefs.GetString("SignatureAbility").Contains("Duelist"))
            {//Chance of avoiding an attack
                nameText.text = "Duelist";
                tip = "Slash and Strike attacks have a 12% + (" + PlayerPrefs.GetInt("EnemiesDefeated") + "%) chance to be avoided";
                return tip;
            }
            else
            {
                return replaceTip;
            }
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
