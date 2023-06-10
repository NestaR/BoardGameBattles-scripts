
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BuffScript : MonoBehaviour
{
    public GameObject buffCanvas;
    public string optionName;
    public Button option1button, option2button, option3button;
    //Give the player permanent buffs to their stats
    void Update()
    {
        if (PlayerPrefs.HasKey("ReviveCharges") && PlayerPrefs.GetInt("ReviveCharges") < 0)
        {
            PlayerPrefs.SetInt("ReviveCharges", 0);
        }
        else if (PlayerPrefs.HasKey("ReviveCharges") && PlayerPrefs.GetInt("ReviveCharges") > 3)
        {
            PlayerPrefs.SetInt("ReviveCharges", 3);
        }
        if (buffCanvas != null)
        {
            if (buffCanvas.activeSelf == true)
            {
                Time.timeScale = 0;
            }
        }
    }
    public void Option1()
    {
        int attack = PlayerPrefs.GetInt("AttackRating");
        attack += Random.Range(5, 10);
        PlayerPrefs.SetInt("AttackRating", attack);

        int defence = PlayerPrefs.GetInt("ArmorRating");
        defence += Random.Range(4, 9);
        PlayerPrefs.SetInt("ArmorRating", defence);

        int maxHP = PlayerPrefs.GetInt("MaxHealthRating");
        int HP = PlayerPrefs.GetInt("HealthRating");
        int increaseHP = Random.Range(1, 4);
        maxHP += increaseHP;
        HP += increaseHP;
        PlayerPrefs.SetInt("HealthRating", HP);
        PlayerPrefs.SetInt("MaxHealthRating", maxHP);

        Time.timeScale = 1;
        buffCanvas.SetActive(false);
    }
    public void Option2()
    {
        PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("MaxHealthRating"));
        PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("MaxManaRating"));
        Time.timeScale = 1;
        buffCanvas.SetActive(false);
    }
    public void Option3()
    {
        if(PlayerPrefs.HasKey("ReviveCharges"))
        {
            PlayerPrefs.SetInt("ReviveCharges", PlayerPrefs.GetInt("ReviveCharges") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("ReviveCharges", 1);
        }
        Time.timeScale = 1;
        buffCanvas.SetActive(false);
    }
    public void SetInt(string KeyName, int Value)
    {
        PlayerPrefs.SetInt(KeyName, Value);
    }
    public int GetInt(string KeyName)
    {
        return PlayerPrefs.GetInt(KeyName);
    }
    public bool HasKey(string KeyName)
    {
        if (PlayerPrefs.HasKey(KeyName))
        {
            Debug.Log("The key " + KeyName + " exists");
            return true;
        }
        else
        {
            Debug.Log("The key " + KeyName + " does not exist");
            return false;
        }
    }
}
