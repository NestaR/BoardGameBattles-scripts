using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleMenu : MonoBehaviour
{
    public GameObject attackButton, playerAttackButton1, playerAttacksPanel, menuPanel, abilityPanel, bagPanel, deadTreeVariant, TreeVariant;   

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(attackButton);
        if (PlayerPrefs.GetString("MapSelected").Contains("1"))
        {
            deadTreeVariant.SetActive(true);
            TreeVariant.SetActive(false);
        }
        else if (PlayerPrefs.GetString("MapSelected").Contains("2"))
        {
            deadTreeVariant.SetActive(false);
            TreeVariant.SetActive(true);
        }
    }
    public void showPlayerAttacks()
    {//Show the players available attacks in battle
        playerAttacksPanel.SetActive(true);
        menuPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(playerAttackButton1);
    }
    public void showBattleMenu()
    {//Open players battle menu for attacking and using items
        playerAttacksPanel.SetActive(false);
        bagPanel.SetActive(false);
        abilityPanel.SetActive(false);
        menuPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(attackButton);
    }
    public void showAbilityMenu()
    {//Open players battle menu for attacking and using items
        playerAttacksPanel.SetActive(false);
        abilityPanel.SetActive(true);
    }
    public void showBagMenu()
    {//Open players battle menu for attacking and using items
        playerAttacksPanel.SetActive(false);
        bagPanel.SetActive(true);
    }
    public string GetString(string KeyName)
    {
        return PlayerPrefs.GetString(KeyName);
    }
}
