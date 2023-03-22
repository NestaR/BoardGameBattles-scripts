using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public bool battleMode, enemy, speedCheck;
    public bool moveSet1, moveSet2, moveSet3, moveSetBoss;
    public int currentHealth, maxHealth, attackRating, armorRating, speed, battleAttack, battleDefence;
    public int EcurrentHealth, EmaxHealth, EattackRating, EarmorRating, Espeed;
    public Text healthUI, attackUI, armorUI, speedUI, maxHPUI, currentBattleHP, maxBattleHP, battleAttackUI, battleDefenceUI;
    GameObject enemyObject;
    //public Text EcurrentBattleHP, EmaxBattleHP;
    void Awake()
    {
        //PlayerPrefs.DeleteKey("HealthRating");
        if(!enemy)
        {
            if (PlayerPrefs.HasKey("HealthRating"))
            {//Get all the players stats            
                currentHealth = PlayerPrefs.GetInt("HealthRating");
                maxHealth = PlayerPrefs.GetInt("MaxHealthRating");
                attackRating = PlayerPrefs.GetInt("AttackRating");
                armorRating = PlayerPrefs.GetInt("ArmorRating");
                speed = PlayerPrefs.GetInt("SpeedRating");
            }
            else
            {//Set players stats when starting the game
                PlayerPrefs.SetInt("HealthRating", 30);
                PlayerPrefs.SetInt("MaxHealthRating", 30);
                PlayerPrefs.SetInt("AttackRating", 10);
                PlayerPrefs.SetInt("ArmorRating", 10);
                PlayerPrefs.SetInt("SpeedRating", 11);
                PlayerPrefs.Save();
            }
        }
        else
        {
            if (PlayerPrefs.GetString("CurrentTile").Contains("Red"))
            {//Increase the enemies stats if landing on a red tile
                EcurrentHealth += 5;
                EmaxHealth += 5;
                EattackRating += 5;
                EarmorRating += 4;
                Espeed += 3;
            }
            if (PlayerPrefs.GetInt("PlayerRun") > 0)
            {//Increase the enemies stats after completing a run
                int statIncrease = PlayerPrefs.GetInt("PlayerRun");
                EcurrentHealth += statIncrease * 2;
                EmaxHealth += statIncrease * 2;
                EattackRating += statIncrease * 2;
                EarmorRating += statIncrease * 2;
                //Espeed += statIncrease;
            }
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
            //Display the players stats on a canvas
            healthUI.text = currentHealth.ToString();
            maxHPUI.text = maxHealth.ToString();
            attackUI.text = attackRating.ToString();
            armorUI.text = armorRating.ToString();
            speedUI.text = speed.ToString();
            if (battleMode)
            {
                if(currentHealth <= 0)
                {
                    currentHealth = 0;
                }
                currentBattleHP.text = currentHealth.ToString();
                maxBattleHP.text = maxHealth.ToString();
                battleAttackUI.text = "+ " + "(" + battleAttack.ToString() + ")";
                battleDefenceUI.text = "+ " + "(" + battleDefence.ToString() + ")";
            }
            //Before the battle starts check their speeds to determine who attacks first
            enemyObject = GameObject.FindWithTag("Enemy");
            if (!speedCheck && enemyObject != null && enemyObject.GetComponent<PlayerStats>().Espeed > speed)
            {
                PlayerPrefs.SetInt("BattleTurn", 0);
                speedCheck = true;
            }
            else if (!speedCheck && enemyObject != null && enemyObject.GetComponent<PlayerStats>().Espeed <= speed)
            {
                PlayerPrefs.SetInt("BattleTurn", 1);
                speedCheck = true;
            }
        }
        else
        {
            if (EcurrentHealth <= 0)
            {
                EcurrentHealth = 0;
            }
        }
    }
    public void EndBattle()
    {
        PlayerPrefs.SetInt("NextScene", 0);
        Destroy(gameObject);
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
