using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMoves : MonoBehaviour
{
    public GameObject player, enemy, playerAttackP, enemyAttackP;
    public Vector3 startPos, enemystartPos, PAttackPos, EAttackPos;
    public bool attackChosen, attackReady, isEnemy, attacking, turnFinished;
    bool attack1, attack2, attack3, attack4, moveFinished, stationaryAttack;
    public Button battack1, battack2, battack3, battack4, backButton;
    public string attackName;
    public int battleTurn, playerHP, battleAttack, battleDefence, battleSpeed;
    public Text attackUI, defenceUI;
    // Start is called before the first frame update
    void Awake()
    {
        if(isEnemy)
        {       
            enemy = GameObject.FindWithTag("Enemy");
            player = GameObject.FindWithTag("Player");
            //battleAttack = enemy.GetComponent<PlayerStats>().EattackRating;

            enemystartPos = enemy.transform.position;
            EAttackPos.x = enemyAttackP.transform.position.x;
            EAttackPos.y = enemy.transform.position.y;
            EAttackPos.z = enemy.transform.position.z;
        }
        else
        {
            //battleAttack = PlayerPrefs.GetInt("AttackRating");
            enemy = GameObject.FindWithTag("Enemy");
            player = GameObject.FindWithTag("Player");
            PAttackPos.x = playerAttackP.transform.position.x;
            PAttackPos.y = player.transform.position.y;
            PAttackPos.z = player.transform.position.z;

            startPos = player.transform.position;
            
            battack1.onClick.AddListener(chooseAttack1);
            battack2.onClick.AddListener(chooseAttack2);
            battack3.onClick.AddListener(chooseAttack3);
            battack4.onClick.AddListener(chooseAttack4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //playerHP = PlayerPrefs.GetInt("HealthRating");
        battleTurn = PlayerPrefs.GetInt("BattleTurn");
        if(battleTurn == 0 && isEnemy && !attacking)
        {
            disableButtons();
            attackName = "SimpleStrike";
            checkStationary();
            playerAttackPosition();
        }
        if(!isEnemy)
        {//Show buffs the player has
            attackUI.text = "+ " + "(" + battleAttack.ToString() + ")";
            defenceUI.text = "+ " + "(" + battleDefence.ToString() + ")";
        }
        if(battleTurn == 1 && !isEnemy && turnFinished)
        {
            enableButtons();
        }
        if(attackReady)
        {
            if(attack1 || attack2 || attack3 || attack4)
            {
                Invoke(attackName, 0.1f);
                attackReady = false;
                attackChosen = false;
            }
            if (isEnemy)
            {
                Invoke(attackName, 0f);
                attackReady = false;
            }
        }
        else if (attackChosen)
        {
            turnFinished = false;
            disableButtons();
            playerAttackPosition();
        }
        else if(moveFinished)
        {
            Invoke("playerStartPosition", 0.5f);
            //playerStartPosition();
        }
    }
    public void chooseAttack1()
    {
        attackName = battack1.GetComponentInChildren<Text>().text;
        attackName = attackName.Replace(" ", "");
        attack1 = true;
        attackChosen = true;
        checkStationary();
    }
    public void chooseAttack2()
    {
        attackName = battack2.GetComponentInChildren<Text>().text;
        attackName = attackName.Replace(" ", "");
        attack2 = true;
        attackChosen = true;
        checkStationary();
    }
    public void chooseAttack3()
    {
        attackName = battack3.GetComponentInChildren<Text>().text;
        attackName = attackName.Replace(" ", "");
        attack3 = true;
        attackChosen = true;
        checkStationary();
    }
    public void chooseAttack4()
    {
        attackName = battack4.GetComponentInChildren<Text>().text;
        attackName = attackName.Replace(" ", "");
        attack4 = true;
        attackChosen = true;
        checkStationary();
    }
    public void SimpleStrike()
    {
        if(isEnemy && !moveFinished)
        {
            //player.GetComponent<PlayerStats>().currentHealth -= 10;
            PlayerPrefs.SetInt("HealthRating", (player.GetComponent<PlayerStats>().currentHealth - (10 + calcAttackValue() - calcDefenceValue())));
        }
        else if(!isEnemy && !moveFinished)
        {
            enemy.GetComponent<PlayerStats>().EcurrentHealth -= (10 + calcAttackValue() - calcDefenceValue());
        }
        moveFinished = true;
    }
    public void SimpleSpell()
    {
        if (isEnemy && !moveFinished)
        {
            PlayerPrefs.SetInt("HealthRating", (player.GetComponent<PlayerStats>().currentHealth - (5 + calcAttackValue() - calcDefenceValue())));
        }
        else if(!isEnemy && !moveFinished)
        {
            enemy.GetComponent<PlayerStats>().EcurrentHealth -= (5 + calcAttackValue() - calcDefenceValue());
        }
        moveFinished = true;
    }
    public void AttackBuff()
    {
        if (isEnemy && !moveFinished)
        {
            battleAttack += 15;
            //battleAttack = enemy.GetComponent<PlayerStats>().EattackRating + battleAttack + 3;
            //enemy.GetComponent<PlayerStats>().EattackRating += 3;
        }
        else if (!isEnemy && !moveFinished)
        {
            battleAttack += 15;
            //PlayerPrefs.SetInt("BattleAttack", battleAttack)
            //PlayerPrefs.SetInt("AttackRating", (player.GetComponent<PlayerStats>().attackRating + 3));
        }
        moveFinished = true;
    }
    public void DefenceBuff()
    {
        if (isEnemy && !moveFinished)
        {
            battleDefence += 15;
            //battleAttack = enemy.GetComponent<PlayerStats>().EattackRating + battleAttack + 3;
            //enemy.GetComponent<PlayerStats>().EattackRating += 3;
        }
        else if (!isEnemy && !moveFinished)
        {
            battleDefence += 15;
            //PlayerPrefs.SetInt("BattleAttack", battleAttack)
            //PlayerPrefs.SetInt("AttackRating", (player.GetComponent<PlayerStats>().attackRating + 3));
        }
        moveFinished = true;
    }
    public void playerAttackPosition()
    {
        if(isEnemy)
        {
            if (!stationaryAttack)
            {
                enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, EAttackPos, Time.deltaTime * 4f);
                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(enemy.transform.position, EAttackPos) < 0.001f)
                {
                    attackReady = true;
                    attacking = true;
                }
            }
            else
            {
                attackReady = true;
                attacking = true;
            }
            
        }
        else
        {
            if (!stationaryAttack)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, PAttackPos, Time.deltaTime * 4f);
                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(player.transform.position, PAttackPos) < 0.001f)
                {
                    attackReady = true;
                }
            }
            else
            {
                attackReady = true;
            }
        }
    }
    public void playerStartPosition()
    {
        if(isEnemy)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemystartPos, Time.deltaTime * 3f);
            if (Vector3.Distance(enemy.transform.position, enemystartPos) < 0.001f)
            {
                attackReady = false;
                attackName = "";
                moveFinished = false;
                stationaryAttack = false;
                attacking = false;
                PlayerPrefs.SetInt("BattleTurn", 1);
            }
        }
        else
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, startPos, Time.deltaTime * 3f);
            if (Vector3.Distance(player.transform.position, startPos) < 0.001f)
            {
                attackReady = false;
                attackChosen = false;
                attackName = "";
                attack1 = false;
                attack2 = false;
                attack3 = false;
                attack4 = false;
                moveFinished = false;
                stationaryAttack = false;
                turnFinished = true;
                enableButtons();
                PlayerPrefs.SetInt("BattleTurn", 0);
            }
        }
        //PlayerPrefs.Save();
        //PlayerPrefs.Save();
    }

    void disableButtons()
    {
        battack1.enabled = false;
        battack2.enabled = false;
        battack3.enabled = false;
        battack4.enabled = false;    
        backButton.enabled = false;
    }
    void enableButtons()
    {
        battack1.enabled = true;
        battack2.enabled = true;
        battack3.enabled = true;
        battack4.enabled = true;
        backButton.enabled = true;
    }
    void checkStationary()
    {
        if (attackName.Contains("Spell") || attackName.Contains("Buff"))
        {
            stationaryAttack = true;
        }
    }
    public int calcAttackValue()
    {
        if(isEnemy)
        {
            return Mathf.RoundToInt((enemy.GetComponent<PlayerStats>().EattackRating + battleAttack) / 5);
        }
        else
        {
            return Mathf.RoundToInt((player.GetComponent<PlayerStats>().attackRating + battleAttack) / 5);
        }
        return 0;
    }
    public int calcDefenceValue()
    {
        if (isEnemy)
        {
            return Mathf.RoundToInt((player.GetComponent<PlayerStats>().armorRating + player.GetComponent<BattleMoves>().battleDefence) / 7);
        }
        else
        {
            return Mathf.RoundToInt((enemy.GetComponent<PlayerStats>().EarmorRating + enemy.GetComponent<BattleMoves>().battleDefence) / 7);
        }
        return 0;
    }
    public void SetInt(string KeyName, int Value)
    {
        PlayerPrefs.SetInt(KeyName, Value);
    }
    public int GetInt(string KeyName)
    {
        return PlayerPrefs.GetInt(KeyName);
    }
}
