using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public bool enemy;
    public float playerHP;
    public float maxHP;
    public Slider slider;
    PlayerStats playerStats;
    //EnemyStats enemyStats;
    void Start()
    {
        //playerStats = this.GetComponent<PlayerStats>();
        //playerHP = playerStats.currentHealth;
        //maxHP = playerStats.maxHealth;

        //slider.value = CalculateHealth();
    }

    void Update()
    {
        playerStats = this.GetComponent<PlayerStats>();
        if (enemy)
        {
            playerHP = playerStats.EcurrentHealth;
            maxHP = playerStats.EmaxHealth;
        }
        else
        {
            playerHP = playerStats.currentHealth;
            maxHP = playerStats.maxHealth;
        }
        slider.value = CalculateHealth();

        if (playerHP < maxHP)
        {
            //healthBarUI.SetActive(true);
        }
        if (playerHP <= 0)
        {
            //Destroy(gameObject);
        }
        if (playerHP > maxHP)
        {
            playerHP = maxHP;
        }
    }

    float CalculateHealth()
    {//Calculates the percentage of health remaining to accurately update the slider
        return playerHP / maxHP;
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
