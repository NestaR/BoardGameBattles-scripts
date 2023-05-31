
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class BuffScript : MonoBehaviour
{
    public GameObject buffCanvas, characterSelect, mapSelect;
    public string optionName, characterSelected, mapSelected;
    bool charChosen;
    public Button option1button, option2button, option3button, nextButton, beginButton;
    //Give the player permanent buffs to their stats
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "StartScene")
        {
            PlayerPrefs.DeleteAll();
        }
    }
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
        if (mapSelect != null)
        {
            if (mapSelected == "")
            {
                nextButton.enabled = false;
            }
            else
            {
                nextButton.enabled = true;
            }
        }
        if (characterSelect != null)
        {
            if(characterSelected == "")
            {
                beginButton.enabled = false;
            }
            else
            {
                beginButton.enabled = true;
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
    public void LoadMainScene()
    {
        PlayerPrefs.SetString("MapSelected", mapSelected);
        PlayerPrefs.SetString("CharacterSelected", characterSelected);
        StartCoroutine(LoadYourAsyncScene());
    }
    public void MapSelectCanvas()
    {
        mapSelect.SetActive(true);       
    }
    public void CharacterSelectCanvas()
    {
        characterSelect.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }    
    public void CloseCharacterSelect()
    {
        characterSelected = "";
        characterSelect.SetActive(false);
    }
    public void CloseMapSelect()
    {
        mapSelected = "";
        mapSelect.SetActive(false);
    }
    public void Character1()
    {
        characterSelected = "HeroKnight1";
    }
    public void Character2()
    {
        characterSelected = "HeroKnight2";
    }
    public void Character3()
    {
        characterSelected = "MartialHero";
    }
    public void Character4()
    {
        characterSelected = "MedievalWarrior1";
    }
    public void Character5()
    {
        characterSelected = "MedievalWarrior2";
    }
    public void Map1()
    {
        mapSelected = "Map1";
    }
    public void Map2()
    {
        mapSelected = "Map2";
    }
    public void Map3()
    {
        mapSelected = "Map3";
    }
    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mapSelected);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
