using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class BuffScript : MonoBehaviour
{
    public GameObject buffCanvas;
    public string optionName;
    public Button option1button, option2button, option3button;
    //Give the player permanent buffs to their stats
    public void Option1()
    {
        int attack = PlayerPrefs.GetInt("AttackRating");
        attack += Random.Range(5, 10);
        PlayerPrefs.SetInt("AttackRating", attack);
        buffCanvas.SetActive(false);
    }
    public void Option2()
    {
        int defence = PlayerPrefs.GetInt("ArmorRating");
        defence += Random.Range(4, 9);
        PlayerPrefs.SetInt("ArmorRating", defence);
        buffCanvas.SetActive(false);
    }
    public void Option3()
    {
        int maxHP = PlayerPrefs.GetInt("MaxHealthRating");
        int HP = PlayerPrefs.GetInt("HealthRating");
        int increaseHP = Random.Range(1, 4);
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
    public void LoadMainScene()
    {
        PlayerPrefs.DeleteAll();
        StartCoroutine(LoadYourAsyncScene());
    }
    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
