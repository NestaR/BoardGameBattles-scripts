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
    // Start is called before the first frame update
    void Start()
    {
        if(!enemy)
        {
            if (PlayerPrefs.HasKey("HealthRating") && PlayerPrefs.HasKey("MaxHealthRating") && PlayerPrefs.HasKey("AttackRating") && PlayerPrefs.HasKey("ArmorRating") && PlayerPrefs.HasKey("SpeedRating"))
            {

            }
            else
            {
                PlayerPrefs.SetInt("HealthRating", 30);
                PlayerPrefs.SetInt("MaxHealthRating", 30);
                PlayerPrefs.SetInt("AttackRating", 15);
                PlayerPrefs.SetInt("ArmorRating", 10);
                PlayerPrefs.SetInt("SpeedRating", 13);
                PlayerPrefs.Save();
            }
            currentHealth = PlayerPrefs.GetInt("HealthRating");
            maxHealth = PlayerPrefs.GetInt("MaxHealthRating");
            attackRating = PlayerPrefs.GetInt("AttackRating");
            armorRating = PlayerPrefs.GetInt("ArmorRating");
            speed = PlayerPrefs.GetInt("SpeedRating");
            PlayerPrefs.SetInt("BattleTurn", 1);

        }
        else
        {
            EcurrentHealth = 20;
            EmaxHealth = 20;
            EattackRating = 10;
            EarmorRating = 10;
            Espeed = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!enemy)
        {
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
            PlayerPrefs.SetInt("HealthRating", currentHealth);
            PlayerPrefs.SetInt("AttackRating", attackRating);
            PlayerPrefs.SetInt("ArmorRating", armorRating);
            PlayerPrefs.SetInt("SpeedRating", speed);
            PlayerPrefs.Save();
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
