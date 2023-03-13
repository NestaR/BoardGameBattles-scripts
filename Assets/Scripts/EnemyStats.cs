using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    //public int currentHealth, maxHealth, attackRating, armorRating, speed;
    //public Text currentBattleHP, maxBattleHP;
    // Start is called before the first frame update
    void Start()
    {
        //currentEHealth = 20;
        //maxEHealth = 20;
        //attackRating = 10;
        //armorRating = 10;
        //speed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //if(maxHealth == 0)
        //{
        //    currentHealth = 20;
        //    maxHealth = 20;
        //}
        //currentBattleHP.text = currentHealth.ToString();
        //maxBattleHP.text = maxHealth.ToString();
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
            //Debug.Log("The key " + KeyName + " exists");
            return true;
        }
        else
        {
            //Debug.Log("The key " + KeyName + " does not exist");
            return false;
        }
    }
    public void DeleteKey(string KeyName)
    {
        PlayerPrefs.DeleteKey(KeyName);
    }
}
