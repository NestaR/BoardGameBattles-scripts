using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public bool battleMode, enemy, enemyHit;
    public int currentHealth, maxHealth, attackRating, armorRating, speed;
    public int EcurrentHealth, EmaxHealth, EattackRating, EarmorRating, Espeed;
    public Text healthUI, attackUI, armorUI, speedUI, maxHPUI, currentBattleHP, maxBattleHP;
    public Text EcurrentBattleHP, EmaxBattleHP;
    void Start()
    {
        //PlayerPrefs.DeleteKey("HealthRating");
        if(!enemy)
        {
            if (PlayerPrefs.HasKey("HealthRating"))
            {            
                currentHealth = PlayerPrefs.GetInt("HealthRating");
                maxHealth = PlayerPrefs.GetInt("MaxHealthRating");
                attackRating = PlayerPrefs.GetInt("AttackRating");
                armorRating = PlayerPrefs.GetInt("ArmorRating");
                speed = PlayerPrefs.GetInt("SpeedRating");
            }
            else
            {
                PlayerPrefs.SetInt("HealthRating", 30);
                PlayerPrefs.SetInt("MaxHealthRating", 30);
                PlayerPrefs.SetInt("AttackRating", 10);
                PlayerPrefs.SetInt("ArmorRating", 10);
                PlayerPrefs.SetInt("SpeedRating", 11);
                PlayerPrefs.Save();
            }
            PlayerPrefs.SetInt("BattleTurn", 1);
        }
        else
        {
            EcurrentHealth = 20;
            EmaxHealth = 20;
            EattackRating = 5;
            EarmorRating = 10;
            Espeed = 10;
        }
    }


    void Update()
    {
        if (!enemy)
        {            
            currentHealth = PlayerPrefs.GetInt("HealthRating");
            maxHealth = PlayerPrefs.GetInt("MaxHealthRating");
            attackRating = PlayerPrefs.GetInt("AttackRating");
            armorRating = PlayerPrefs.GetInt("ArmorRating");
            speed = PlayerPrefs.GetInt("SpeedRating");

            healthUI.text = currentHealth.ToString();
            maxHPUI.text = maxHealth.ToString();
            attackUI.text = attackRating.ToString();
            armorUI.text = armorRating.ToString();
            speedUI.text = speed.ToString();
            if (battleMode)
            {
                currentBattleHP.text = currentHealth.ToString();
                maxBattleHP.text = maxHealth.ToString();
            }
        }
        else
        {
            EcurrentBattleHP.text = EcurrentHealth.ToString();
            EmaxBattleHP.text = EmaxHealth.ToString();
        }
        if(battleMode && EcurrentHealth <= 0 && enemy)
        {
            Destroy(gameObject);
        }
        else if(battleMode && currentHealth <= 0 && !enemy)
        {
            Destroy(gameObject);
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
    public void DeleteKey(string KeyName)
    {
        PlayerPrefs.DeleteKey(KeyName);
    }
}
