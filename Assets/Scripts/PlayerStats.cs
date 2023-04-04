using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public bool battleMode, enemy, speedCheck;
    public bool moveSet1, moveSet2, moveSet3, moveSetBoss;
    public int currentHealth, maxHealth, currentMana, maxMana, attackRating, armorRating, speed, battleAttack, battleDefence, reviveCharges;
    public int EcurrentHealth, EmaxHealth, EattackRating, EarmorRating, Espeed;
    public Text healthUI, manaUI, maxMPUI, attackUI, armorUI, speedUI, maxHPUI, currentBattleHP, maxBattleHP, currentBattleMP, maxBattleMP, battleAttackUI, battleDefenceUI, reviveChargesUI, battleReviveChargesUI;
    public GameObject character1, character2, character3, character4, character5;
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
                currentMana = PlayerPrefs.GetInt("ManaRating");
                maxMana = PlayerPrefs.GetInt("MaxManaRating");
                attackRating = PlayerPrefs.GetInt("AttackRating");
                armorRating = PlayerPrefs.GetInt("ArmorRating");
                speed = PlayerPrefs.GetInt("SpeedRating");
                reviveCharges = PlayerPrefs.GetInt("ReviveCharges");
                InstChar();
            }
            else
            {//Set players stats when starting the game
                SetStats();               
            }
        }
        else
        {
            if (PlayerPrefs.GetString("CurrentTile").Contains("Red"))
            {//Increase the enemies stats if landing on a red tile
                EcurrentHealth += 3;
                EmaxHealth += 3;
                EattackRating += 3;
                EarmorRating += 3;
                Espeed += 1;
            }
            if (PlayerPrefs.GetInt("PlayerRun") > 0 && (PlayerPrefs.GetString("CurrentTile").Contains("Green") || PlayerPrefs.GetString("CurrentTile").Contains("Red")))
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
            reviveCharges = PlayerPrefs.GetInt("ReviveCharges");
            currentHealth = PlayerPrefs.GetInt("HealthRating");
            maxHealth = PlayerPrefs.GetInt("MaxHealthRating");
            currentMana = PlayerPrefs.GetInt("ManaRating");
            maxMana = PlayerPrefs.GetInt("MaxManaRating");
            attackRating = PlayerPrefs.GetInt("AttackRating");
            armorRating = PlayerPrefs.GetInt("ArmorRating");
            speed = PlayerPrefs.GetInt("SpeedRating");
            //Display the players stats on a canvas
            healthUI.text = currentHealth.ToString();
            maxHPUI.text = maxHealth.ToString();
            manaUI.text = currentMana.ToString();
            maxMPUI.text = maxMana.ToString();
            attackUI.text = attackRating.ToString();
            armorUI.text = armorRating.ToString();
            speedUI.text = speed.ToString();
            reviveChargesUI.text = "x " + reviveCharges.ToString();
            if (battleMode)
            {
                if(currentHealth <= 0)
                {
                    PlayerPrefs.SetInt("HealthRating", 0);
                }
                else if (currentHealth >= maxHealth)
                {
                    PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("MaxHealthRating"));
                }
                if (currentMana <= 0)
                {
                    PlayerPrefs.SetInt("ManaRating", 0);
                }
                else if (currentMana >= maxMana)
                {
                    PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("MaxManaRating"));
                }
                currentBattleHP.text = currentHealth.ToString();
                maxBattleHP.text = maxHealth.ToString();
                currentBattleMP.text = currentMana.ToString();
                maxBattleMP.text = maxMana.ToString();
                battleAttackUI.text = "+ " + "(" + battleAttack.ToString() + ")";
                battleDefenceUI.text = "+ " + "(" + battleDefence.ToString() + ")";
                battleReviveChargesUI.text = "x " + reviveCharges.ToString();
            }
        }
        else
        {
            if (EcurrentHealth <= 0)
            {
                EcurrentHealth = 0;
            }
            else if (EcurrentHealth >= EmaxHealth)
            {
                EcurrentHealth = EmaxHealth;
            }
        }
    }
    public void EndBattle()
    {
        PlayerPrefs.SetInt("NextScene", 0);
        Destroy(gameObject);
    }
    public void SetStats()
    {
        Quaternion rotation = Quaternion.Euler(35, 0, 0);
        Vector3 pos = this.transform.position;
        if (PlayerPrefs.GetString("CharacterSelected") == "HeroKnight1")
        {
            PlayerPrefs.SetInt("HealthRating", 50);
            PlayerPrefs.SetInt("MaxHealthRating", 50);
            PlayerPrefs.SetInt("ManaRating", 50);
            PlayerPrefs.SetInt("MaxManaRating", 50);
            PlayerPrefs.SetInt("AttackRating", 12);
            PlayerPrefs.SetInt("ArmorRating", 14);
            PlayerPrefs.SetInt("SpeedRating", 11);
            PlayerPrefs.SetInt("ReviveCharges", 0);
            PlayerPrefs.Save();
            Instantiate(character1, pos, rotation, this.transform);
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "HeroKnight2")
        {
            PlayerPrefs.SetInt("HealthRating", 50);
            PlayerPrefs.SetInt("MaxHealthRating", 50);
            PlayerPrefs.SetInt("ManaRating", 50);
            PlayerPrefs.SetInt("MaxManaRating", 50);
            PlayerPrefs.SetInt("AttackRating", 9);
            PlayerPrefs.SetInt("ArmorRating", 21);
            PlayerPrefs.SetInt("SpeedRating", 9);
            PlayerPrefs.SetInt("ReviveCharges", 0);
            PlayerPrefs.Save();
            pos.y += -0.16f;
            Instantiate(character2, pos, rotation, this.transform);
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MartialHero")
        {
            PlayerPrefs.SetInt("HealthRating", 45);
            PlayerPrefs.SetInt("MaxHealthRating", 45);
            PlayerPrefs.SetInt("ManaRating", 55);
            PlayerPrefs.SetInt("MaxManaRating", 55);
            PlayerPrefs.SetInt("AttackRating", 15);
            PlayerPrefs.SetInt("ArmorRating", 6);
            PlayerPrefs.SetInt("SpeedRating", 12);
            PlayerPrefs.SetInt("ReviveCharges", 0);
            PlayerPrefs.Save();
            Instantiate(character3, pos, rotation, this.transform);
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MedievalWarrior1")
        {
            PlayerPrefs.SetInt("HealthRating", 52);
            PlayerPrefs.SetInt("MaxHealthRating", 52);
            PlayerPrefs.SetInt("ManaRating", 52);
            PlayerPrefs.SetInt("MaxManaRating", 52);
            PlayerPrefs.SetInt("AttackRating", 10);
            PlayerPrefs.SetInt("ArmorRating", 10);
            PlayerPrefs.SetInt("SpeedRating", 10);
            PlayerPrefs.SetInt("ReviveCharges", 0);
            PlayerPrefs.Save();
            pos.y += 0.22f;
            Instantiate(character4, pos, rotation, this.transform);
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MedievalWarrior2")
        {
            PlayerPrefs.SetInt("HealthRating", 50);
            PlayerPrefs.SetInt("MaxHealthRating", 50);
            PlayerPrefs.SetInt("ManaRating", 40);
            PlayerPrefs.SetInt("MaxManaRating", 40);
            PlayerPrefs.SetInt("AttackRating", 15);
            PlayerPrefs.SetInt("ArmorRating", 11);
            PlayerPrefs.SetInt("SpeedRating", 11);
            PlayerPrefs.SetInt("ReviveCharges", 0);
            PlayerPrefs.Save();
            Instantiate(character5, pos, rotation, this.transform);
        }
    }
    public void InstChar()
    {
        if (PlayerPrefs.GetString("CharacterSelected") == "HeroKnight1")
        {
            Instantiate(character1, this.transform.position, Quaternion.identity, this.transform);
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "HeroKnight2")
        {
            Vector3 newPos = this.transform.position;
            newPos.y += -0.16f;
            Instantiate(character2, newPos, Quaternion.identity, this.transform);
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MartialHero")
        {
            Instantiate(character3, this.transform.position, Quaternion.identity, this.transform);
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MedievalWarrior1")
        {
            Vector3 newPos = this.transform.position;
            newPos.y += 0.22f;
            Instantiate(character4, newPos, Quaternion.identity, this.transform);
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MedievalWarrior2")
        {
            Instantiate(character5, this.transform.position, Quaternion.identity, this.transform);
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
