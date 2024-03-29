using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public int battleTurn;
    public GameObject battleManager, enemy, enemyBoss;
    public GameObject[] enemies;
    BattleMoves battleMoves;
    PlayerStats enemyStats;
    public Text EcurrentBattleHP, EmaxBattleHP;
    SpriteRenderer enemySprite;
    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.GetString("MapSelected") == "Map1")
        {
            spawnEnemyMap1();
        }
        else if (PlayerPrefs.GetString("MapSelected") == "Map2")
        {
            spawnEnemyMap2();
        }
        else if (PlayerPrefs.GetString("MapSelected") == "Map3")
        {
            spawnEnemyMap3();
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemy = GameObject.FindWithTag("Enemy");
        if(enemy != null)
        {//Set the enemy's ui text
            enemyStats = enemy.transform.GetChild(0).GetComponent<PlayerStats>();
            EcurrentBattleHP.text = enemyStats.EcurrentHealth.ToString();
            EmaxBattleHP.text = enemyStats.EmaxHealth.ToString();
            enemySprite = GetComponentInChildren<SpriteRenderer>();
            if (PlayerPrefs.GetString("CurrentTile").Contains("Red") && enemySprite != null)
            {//Make enraged enemies red
                enemySprite.color = new Color(1,0,0,1);
            }
        }

    }
    public void spawnEnemyMap1()
    {
        if(PlayerPrefs.GetString("CurrentTile") == "Green1" || PlayerPrefs.GetString("CurrentTile") == "Red1")
        {//Goblin
            Instantiate(enemies[0], this.transform.position, Quaternion.identity, this.transform);
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Green2" || PlayerPrefs.GetString("CurrentTile") == "Red2")
        {//FlyingEye
            Instantiate(enemies[1], this.transform.position, Quaternion.identity, this.transform);
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Green3" || PlayerPrefs.GetString("CurrentTile") == "Red3")
        {//Skeleton
            Instantiate(enemies[2], this.transform.position, Quaternion.identity, this.transform);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.2f, this.transform.position.z);
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Black")
        {//BringerOfDeath
            Instantiate(enemies[3], this.transform.position, Quaternion.identity, this.transform);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.1f, this.transform.position.z);
        }
    }
    public void spawnEnemyMap2()
    {
        if (PlayerPrefs.GetString("CurrentTile") == "Green1" || PlayerPrefs.GetString("CurrentTile") == "Red1")
        {//Warrior
            Instantiate(enemies[4], this.transform.position, Quaternion.identity, this.transform);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.1f, this.transform.position.z);
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Green2" || PlayerPrefs.GetString("CurrentTile") == "Red2")
        {//Knight
            Instantiate(enemies[5], this.transform.position, Quaternion.identity, this.transform);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.1f, this.transform.position.z);
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Black")
        {//TheKing
            Instantiate(enemies[6], this.transform.position, Quaternion.identity, this.transform);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
        }
    }
    public void spawnEnemyMap3()
    {
        if (PlayerPrefs.GetString("CurrentTile") == "Green1" || PlayerPrefs.GetString("CurrentTile") == "Red1")
        {//Mushroom
            Instantiate(enemies[7], this.transform.position, Quaternion.identity, this.transform);
            //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.1f, this.transform.position.z);
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Green2" || PlayerPrefs.GetString("CurrentTile") == "Red2")
        {//Lizard
            Instantiate(enemies[8], this.transform.position, Quaternion.identity, this.transform);
            //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.1f, this.transform.position.z);
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Green3" || PlayerPrefs.GetString("CurrentTile") == "Red3")
        {//WizardEye
            Instantiate(enemies[9], this.transform.position, Quaternion.identity, this.transform);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.1f, this.transform.position.z);
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Black")
        {//EvilWizard
            Instantiate(enemies[10], this.transform.position, Quaternion.identity, this.transform);
            //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.1f, this.transform.position.z);
        }
    }
    public int GetInt(string KeyName)
    {
        return PlayerPrefs.GetInt(KeyName);
    }
    public string GetString(string KeyName)
    {
        return PlayerPrefs.GetString(KeyName);
    }
}
