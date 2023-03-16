using System;
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

    public void Option1()
    {
        //Debug.Log("Option1 selected!");
        int attack = PlayerPrefs.GetInt("AttackRating");
        attack += Random.Range(5, 9);
        PlayerPrefs.SetInt("AttackRating", attack);
        buffCanvas.SetActive(false);
    }
    public void Option2()
    {
        //Debug.Log("Option2 selected!");
        int defence = PlayerPrefs.GetInt("ArmorRating");
        defence += Random.Range(4, 8);
        PlayerPrefs.SetInt("ArmorRating", defence);
        buffCanvas.SetActive(false);
    }
    public void Option3()
    {
        //Debug.Log("Option3 selected!");
        int maxHP = PlayerPrefs.GetInt("MaxHealthRating");
        int HP = PlayerPrefs.GetInt("HealthRating");
        int increaseHP = Random.Range(1, 3);
        maxHP += increaseHP;
        HP += increaseHP;
        PlayerPrefs.SetInt("HealthRating", HP);
        PlayerPrefs.SetInt("MaxHealthRating", maxHP);
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
}
