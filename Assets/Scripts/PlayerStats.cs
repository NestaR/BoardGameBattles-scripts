using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int health, attackRating, armorRating, speed;
    public Text healthUI, attackUI, armorUI, speedUI;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("HealthRating") && PlayerPrefs.HasKey("AttackRating") && PlayerPrefs.HasKey("ArmorRating") && PlayerPrefs.HasKey("SpeedRating"))
        {

        }
        else
        {
            PlayerPrefs.SetInt("HealthRating", 30);
            PlayerPrefs.SetInt("AttackRating", 15);
            PlayerPrefs.SetInt("ArmorRating", 10);
            PlayerPrefs.SetInt("SpeedRating", 13);
            PlayerPrefs.Save();
        }
        health = PlayerPrefs.GetInt("HealthRating");
        attackRating = PlayerPrefs.GetInt("AttackRating");
        armorRating = PlayerPrefs.GetInt("ArmorRating");
        speed = PlayerPrefs.GetInt("SpeedRating");
    }

    // Update is called once per frame
    void Update()
    {
        healthUI.text = health.ToString();
        attackUI.text = attackRating.ToString();
        armorUI.text = armorRating.ToString();
        speedUI.text = speed.ToString();
        PlayerPrefs.SetInt("HealthRating", health);
        PlayerPrefs.SetInt("AttackRating", attackRating);
        PlayerPrefs.SetInt("ArmorRating", armorRating);
        PlayerPrefs.SetInt("SpeedRating", speed);
        PlayerPrefs.Save();
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
    public void DeleteKey(string KeyName)
    {
        PlayerPrefs.DeleteKey(KeyName);
    }
}
