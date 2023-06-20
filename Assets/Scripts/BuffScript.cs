
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BuffScript : MonoBehaviour
{
    public GameObject buffCanvas, player;
    public string optionName;
    public Button option1button, option2button, option3button;
    //Give the player permanent buffs to their stats
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    void Update()
    {
        if (PlayerPrefs.HasKey("ReviveCharges") && PlayerPrefs.GetInt("ReviveCharges") < 0)
        {
            PlayerPrefs.SetInt("ReviveCharges", 0);
        }
        else if (PlayerPrefs.HasKey("ReviveCharges") && PlayerPrefs.GetInt("ReviveCharges") > 3)
        {//Limit to 3 revive charges
            PlayerPrefs.SetInt("ReviveCharges", 3);
        }
        if (buffCanvas != null)
        {
            if (buffCanvas.activeSelf == true)
            {
                player.GetComponent<PlayerMovement>().movingPlayer = false;
                player.GetComponent<PlayerMovement>().stopped = true;
            }
        }
    }
    public void Option1()
    {//Buff stats by random amount 1-7
        int attack = PlayerPrefs.GetInt("AttackRating");
        attack += Random.Range(1, 7);
        PlayerPrefs.SetInt("AttackRating", attack);

        int defence = PlayerPrefs.GetInt("ArmorRating");
        defence += Random.Range(1, 7);
        PlayerPrefs.SetInt("ArmorRating", defence);

        int maxHP = PlayerPrefs.GetInt("MaxHealthRating");
        int HP = PlayerPrefs.GetInt("HealthRating");
        int increaseHP = Random.Range(1, 7);
        maxHP += increaseHP;
        HP += increaseHP;
        PlayerPrefs.SetInt("HealthRating", HP);
        PlayerPrefs.SetInt("MaxHealthRating", maxHP);

        CloseBuffCanvas();
    }
    public void Option2()
    {//Fully restore players hp and mp
        PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("MaxHealthRating"));
        PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("MaxManaRating"));
        
        CloseBuffCanvas();
    }
    public void Option3()
    {//Give player a revive charge
        if(PlayerPrefs.HasKey("ReviveCharges"))
        {
            PlayerPrefs.SetInt("ReviveCharges", PlayerPrefs.GetInt("ReviveCharges") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("ReviveCharges", 1);
        }

        CloseBuffCanvas();
    }
    public void CloseBuffCanvas()
    {
        buffCanvas.SetActive(false);
        player.GetComponent<PlayerMovement>().movingPlayer = true;
        player.GetComponent<PlayerMovement>().stopped = false;
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
