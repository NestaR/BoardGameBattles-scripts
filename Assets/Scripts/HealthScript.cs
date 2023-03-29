using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public bool enemy;
    public float playerHP, playerMP;
    public float maxHP, maxMP;
    public Slider healthSlider, manaSlider;
    public PlayerStats playerStats;
    GameObject enemyPrefab;
    void Update()
    {       
        if (enemy)
        {
            enemyPrefab = GameObject.FindWithTag("Enemy");
            if(enemyPrefab != null)
            {//Calculate and display the enemies health
                playerStats = enemyPrefab.transform.GetChild(0).GetComponent<PlayerStats>();
                playerHP = playerStats.EcurrentHealth;
                maxHP = playerStats.EmaxHealth;
                checkDeath();
                checkMaxHP();
                healthSlider.value = CalculateHealth();
            }
        }
        else
        {//Calculate and display the players health
            playerStats = this.GetComponent<PlayerStats>();
            playerHP = playerStats.currentHealth;
            maxHP = playerStats.maxHealth;
            playerMP = playerStats.currentMana;
            maxMP = playerStats.maxMana;
            checkDeath();
            checkMaxHP();
            healthSlider.value = CalculateHealth();
            manaSlider.value = CalculateMana();
        }
    }

    float CalculateHealth()
    {//Calculates the percentage of health remaining to accurately update the slider
        return playerHP / maxHP;
    }    
    float CalculateMana()
    {//Calculates the percentage of mana remaining to accurately update the slider
        return playerMP / maxMP;
    }
    public void checkDeath()
    {
        if (playerHP <= 0)
        {
            playerHP = 0;
            //this.GetComponent<HealthScript>().enabled = false;
            //Destroy(gameObject);
        }
    }
    public void checkMaxHP()
    {
        if (playerHP > maxHP && enemy)
        {//Make sure the health doesnt go above max health
            playerHP = maxHP;
            playerStats.EcurrentHealth = playerStats.EmaxHealth;
        }
        else if (playerHP > maxHP && !enemy)
        {//Make sure the health doesnt go above max health
            playerHP = maxHP;
            playerStats.currentHealth = playerStats.maxHealth;
        }
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
