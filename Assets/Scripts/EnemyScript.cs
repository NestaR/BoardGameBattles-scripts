using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public int battleTurn;
    public GameObject battleManager, enemy, enemy1, enemy2, enemy3, enemyBoss;
    BattleMoves battleMoves;
    PlayerStats enemyStats;
    public Text EcurrentBattleHP, EmaxBattleHP;
    // Start is called before the first frame update
    void Awake()
    {
        spawnEnemy();
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
        }
    }
    public void spawnEnemy()
    {
        if(PlayerPrefs.GetString("CurrentTile") == "Green1" || PlayerPrefs.GetString("CurrentTile") == "Red1")
        {
            Instantiate(enemy1, this.transform.position, Quaternion.identity, this.transform);
            battleManager.GetComponent<BattleMoves>().moveSet1 = true;
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Green2" || PlayerPrefs.GetString("CurrentTile") == "Red2")
        {
            Instantiate(enemy2, this.transform.position, Quaternion.identity, this.transform);
            battleManager.GetComponent<BattleMoves>().moveSet2 = true;
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Green3" || PlayerPrefs.GetString("CurrentTile") == "Red3")
        {
            Instantiate(enemy3, this.transform.position, Quaternion.identity, this.transform);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.2f, this.transform.position.z);
            battleManager.GetComponent<BattleMoves>().moveSet3 = true;
        }
        else if (PlayerPrefs.GetString("CurrentTile") == "Black")
        {
            Instantiate(enemyBoss, this.transform.position, Quaternion.identity, this.transform);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.1f, this.transform.position.z);
            battleManager.GetComponent<BattleMoves>().moveSetBoss = true;
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
