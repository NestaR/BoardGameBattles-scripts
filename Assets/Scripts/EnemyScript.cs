using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public int battleTurn;
    bool enemySpawned;
    public GameObject enemy1, enemy2, enemy3, enemyBoss;
    BattleMoves battleMoves;
    PlayerStats playerStats;
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
        {
            spawnEnemy();
            
        }
        playerStats = GameObject.FindWithTag("Enemy").GetComponent<PlayerStats>();
        EcurrentBattleHP.text = playerStats.EcurrentHealth.ToString();
        EmaxBattleHP.text = playerStats.EmaxHealth.ToString();

        if (battleTurn == 0)
        {

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
