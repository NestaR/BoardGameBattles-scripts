using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public int battleTurn;
    bool enemySpawned;
    public GameObject enemy, enemy1, enemy2, enemy3, enemyBoss;
    BattleMoves battleMoves;
    PlayerStats enemyStats;
    public Text EcurrentBattleHP, EmaxBattleHP;
    // Start is called before the first frame update
    void Start()
    {
        enemySpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!enemySpawned)
        {//Spawn an enemy depending on which part of the map was reached
            spawnEnemy();
            
        }
        enemy = GameObject.FindWithTag("Enemy");
        if(enemy != null)
        {//Set the enemy's ui text
            enemyStats = enemy.GetComponent<PlayerStats>();
            EcurrentBattleHP.text = enemyStats.EcurrentHealth.ToString();
            EmaxBattleHP.text = enemyStats.EmaxHealth.ToString();
        }
    }
    public void spawnEnemy()
    {
        if(PlayerPrefs.GetString("CurrentTile") == "Green1" || PlayerPrefs.GetString("CurrentTile") == "Red1")
        {
            Instantiate(enemy1, this.transform.position, Quaternion.identity);
            enemySpawned = true;
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Green2" || PlayerPrefs.GetString("CurrentTile") == "Red2")
        {
            Instantiate(enemy2, this.transform.position, Quaternion.identity);
            enemySpawned = true;
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Green3" || PlayerPrefs.GetString("CurrentTile") == "Red3")
        {
            Instantiate(enemy3, this.transform.position, Quaternion.identity);
            enemySpawned = true;
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Black")
        {
            Instantiate(enemyBoss, this.transform.position, Quaternion.identity);
            enemySpawned = true;
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
