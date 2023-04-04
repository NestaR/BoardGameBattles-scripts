
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST }

public class BattleMoves : MonoBehaviour
{
    public BattleState battleState;
    public GameObject player, enemy, spellObject, playerAttackP, enemyAttackP, battleCanvas;
    public Animator playerAnimator, enemyAnimator;
    public Vector3 startPos, enemystartPos, PAttackPos, EAttackPos;
    public bool attackChosen, playerRunning, enemyRunning, attackReady, isEnemy, moveSet1, moveSet2, moveSet3, moveSetBoss, firstTurn, turnFinished = false;
    bool attacking, attack1, attack2, attack3, attack4;
    bool playerFlipped, enemyFlipped;
    public Button battack1, battack2, battack3, battack4, backButton;
    public string attackName;
    string[] moveSet;
    public int battleTurn, playerHP, battleSpeed;
    public Text attackUI, defenceUI, turnDescription;
    int counter, numAttAnimations;
    public bool speedCheck, hasClicked, moveFinished, hit, stationaryAttack;
    PlayerStats playerStats, enemyStats;
    public GameManager battleManager;
    void Start()
    {
        moveSet = new string[4];
        if (!PlayerPrefs.HasKey("PlayerRun"))
        {
            PlayerPrefs.SetInt("PlayerRun", 0);
        }
        if (PlayerPrefs.GetString("CharacterSelected") == "HeroKnight1")
        {
            numAttAnimations = 3;
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "HeroKnight2")
        {
            numAttAnimations = 1;
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MartialHero")
        {
            numAttAnimations = 3;
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MedievalWarrior1")
        {
            numAttAnimations = 3;
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MedievalWarrior2")
        {
            numAttAnimations = 4;
        }
        else
        {
            Debug.Log("No character");
        }
        startPos = player.transform.position;
        PAttackPos.x = playerAttackP.transform.position.x;
        PAttackPos.y = playerAttackP.transform.position.y;
        PAttackPos.z = playerAttackP.transform.position.z;

        EAttackPos.x = enemyAttackP.transform.position.x;
        EAttackPos.y = enemyAttackP.transform.position.y;
        EAttackPos.z = enemyAttackP.transform.position.z;

        enemyStats = enemy.transform.GetChild(0).GetComponent<PlayerStats>();
        playerStats = player.GetComponent<PlayerStats>();
        enemystartPos = enemy.transform.position;

        enemyAnimator = enemy.transform.GetChild(0).GetComponent<Animator>();
        playerAnimator = player.transform.GetChild(0).GetComponent<Animator>();
        //Buttons for storing the players chosen attack
        battack1.onClick.AddListener(chooseAttack1);
        battack2.onClick.AddListener(chooseAttack2);
        battack3.onClick.AddListener(chooseAttack3);
        battack4.onClick.AddListener(chooseAttack4);

        if (enemyStats.moveSet1)
        {
            moveSet[0] = "Slash";
            moveSet[1] = "Double Strike";
            moveSet[2] = "Attack Buff";
            moveSet[3] = "Slash";
        }
        else if (enemyStats.moveSet2)
        {
            moveSet[0] = "Slash";
            moveSet[1] = "Thunder Strike";
            moveSet[2] = "Thunder Strike";
            moveSet[3] = "Slash";
        }
        else if (enemyStats.moveSet3)
        {
            moveSet[0] = "Slash";
            moveSet[1] = "Heavy Strike";
            moveSet[2] = "Defence Buff";
            moveSet[3] = "Slash";
        }
        else if (enemyStats.moveSetBoss)
        {
            moveSet[0] = "Slash";
            moveSet[1] = "Soul Siphon";
            moveSet[2] = "Death Slash";
            moveSet[3] = "Toxic Slash";
        }

        battleState = BattleState.START;
        
    }
    IEnumerator BeginBattle()
    {//Find the player and enemy at the start of the battle
        yield return new WaitForSeconds(1);

        if (!speedCheck && enemy != null && enemyStats.Espeed > playerStats.speed)
        {//If the enemy is faster they attack first           
            disableButtons();
            //yield return new WaitForSeconds(1);
            speedCheck = true;
            battleState = BattleState.ENEMYTURN;
            yield return StartCoroutine(EnemyTurn());
        }
        else if (!speedCheck && enemy != null && enemyStats.Espeed <= playerStats.speed)
        {//If the player is faster they attack first
            enableButtons();
            //yield return new WaitForSeconds(1);
            speedCheck = true;
            battleState = BattleState.PLAYERTURN;
            yield return StartCoroutine(PlayerTurn());
        }
    }
    IEnumerator PlayerTurn()
    {
        
        //speedCheck = true;
        turnDescription.text = "Player's turn";
        // probably display some message 
        // stating it's player's turn here
        //yield return new WaitForSeconds(0);
        turnFinished = false;
        attackReady = false;
        // release the blockade on clicking 
        // so that player can click on 'attack' button    
        hasClicked = false;
        checkHP();
        //enableButtons();
        yield return null;
    }

    void Update()
    { 
        if (!speedCheck)
        {
            StartCoroutine(BeginBattle());
        }


    }
    public void checkHP()
    {
        if (enemyStats.EcurrentHealth <= 0)
        {
            // if the enemy health drops to 0 
            // we won!
            battleState = BattleState.WIN;
            enemyAnimator.SetBool("Death", true);
            turnDescription.text = "Enemy defeated!";
            StartCoroutine(EndBattle());
        }
        else if (playerStats.currentHealth <= 0 && playerStats.reviveCharges > 0)
        {
            // if the player health drops to 0 
            // we revive!           
            PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("MaxHealthRating") / 2);
            PlayerPrefs.SetInt("ReviveCharges", PlayerPrefs.GetInt("ReviveCharges") - 1);
            StartCoroutine(PlayerTurn());
        }
        else if (playerStats.currentHealth <= 0 && playerStats.reviveCharges <= 0)
        {
            // if the player health drops to 0 
            // we lost!
            battleState = BattleState.LOST;
            playerAnimator.SetBool("Death", true);
            turnDescription.text = "Player defeated!";
            StartCoroutine(EndBattle());
        }
        else if(battleState == BattleState.PLAYERTURN)
        {
            enableButtons();
        }
    }
    public void chooseAttackAnimation(string AttackName)
    {
        if(numAttAnimations == 3 && AttackName.Contains("Triple"))
        {
            playerAnimator.SetTrigger("Attack1");
            playerAnimator.SetTrigger("Attack3");
        }
        else if (numAttAnimations == 3 && AttackName.Contains("Double"))
        {
            playerAnimator.SetTrigger("Attack3");
        }
        else if ((numAttAnimations == 3 || numAttAnimations == 4) && AttackName.Contains("Strike"))
        {
            playerAnimator.SetTrigger("Attack2");
        }
        else if (numAttAnimations == 4 && AttackName.Contains("Heavy") || AttackName.Contains("Strong"))
        {
            playerAnimator.SetTrigger("Attack4");
        }
        else if (numAttAnimations == 4 && AttackName.Contains("Spear"))
        {
            playerAnimator.SetTrigger("Attack3");
        }
        else
        {
            playerAnimator.SetTrigger("Attack1");
        }
    }
    public void chooseEnemyAttackAnimation(string AttackName)
    {
        if (AttackName.Contains("Double"))
        {
            enemyAnimator.SetTrigger("Attack3");
        }
        else if (AttackName.Contains("Strike") || AttackName.Contains("Heavy") || AttackName.Contains("Strong"))
        {
            enemyAnimator.SetTrigger("Attack2");
        }
        else
        {
            enemyAnimator.SetTrigger("Attack1");
        }
    }
    IEnumerator PlayerAttack()
    {//Perform the players chosen attack
        disableButtons();
        checkStationary();
        moveFinished = false;
        while (!attackReady)
        {
            playerAttackPosition(); 
            yield return null;
        }
        if (!stationaryAttack)
        {
            chooseAttackAnimation(attackName);
        }
        turnDescription.text = "Player used " + attackName + "!";
        attackName = attackName.Replace(" ", "");
        
        yield return new WaitForSeconds(1.5f);
        if (!stationaryAttack)
        {
            enemyAnimator.SetTrigger("Hit");
        }
        Invoke(attackName, 0f);


        while (!moveFinished)
        {
            yield return null;
        }
        while (!turnFinished)
        {
            playerStartPosition();
            yield return null;
        }
        battleState = BattleState.ENEMYTURN;
        yield return StartCoroutine(EnemyTurn());
    }
    IEnumerator EnemyTurn()
    {//Pick a random attack from the enemies moveset
        turnDescription.text = "Enemy's turn";
        checkHP();
        yield return new WaitForSeconds(1);
        turnFinished = false;
        moveFinished = false;
        attackReady = false;  
        
        while (attackName == null || attackName == "")
        {

            attackName = moveSet[Random.Range(0, 4)];
            yield return null;
        }

        checkStationary();
        while (!attackReady)
        {
            playerAttackPosition();
            yield return null;
        }
        if(!stationaryAttack)
        {
            chooseEnemyAttackAnimation(attackName);
        }
        else if(stationaryAttack && moveSetBoss)
        {
            enemyAnimator.SetTrigger("Cast");
            yield return new WaitForSeconds(0.3f);
            enemy.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        turnDescription.text = "Enemy used " + attackName + "!";
        attackName = attackName.Replace(" ", "");
        

        yield return new WaitForSeconds(2f);
        if (!stationaryAttack)
        {
            playerAnimator.SetTrigger("Hit");          
        }
        else if (stationaryAttack && moveSetBoss)
        {
            playerAnimator.SetTrigger("Hit");
            enemy.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
        Invoke(attackName, 0f);

        while (!moveFinished)
        {
            yield return null;
        }

        while (!turnFinished)
        {
            playerStartPosition();
            yield return null;
        }
        battleState = BattleState.PLAYERTURN;
        yield return StartCoroutine(PlayerTurn());
    }
    IEnumerator EndBattle()
    {
        battleManager.GetComponent<TransitionScene>().resetPositions();
        // check if we won
        if (battleState == BattleState.WIN)
        {
            // you may wish to display some kind
            // of message or play a victory fanfare
            // here          
            BuffAllStats();
            yield return new WaitForSeconds(1);
            Destroy(enemy.gameObject);
            PlayerPrefs.SetInt("NextScene", 0);
        }
        // otherwise check if we lost
        // You probably want to display some kind of
        // 'Game Over' screen to communicate to the 
        // player that the game is lost
        else if (battleState == BattleState.LOST)
        {
            // you may wish to display some kind
            // of message or play a sad tune here!
            
            yield return new WaitForSeconds(1);
            if(enemyStats.moveSetBoss)
            {
                PlayerPrefs.SetInt("PlayerRun", PlayerPrefs.GetInt("PlayerRun") + 1);
            }
            Destroy(enemy.gameObject);
            PlayerPrefs.SetInt("NextScene", 2);
            //SceneManager.UnloadSceneAsync(1);
        }
    }

    public bool checkMana(string moveName)
    {
        if (moveName == "Fire Slash" && playerStats.currentMana < 10)
        {
            return false;
        }
        else if (moveName == "Thunder Strike" && playerStats.currentMana < 10)
        {
            return false;
        }
        else if (moveName == "Freezing Strike" && playerStats.currentMana < 10)
        {
            return false;
        }
        else if (moveName == "Toxic Slash" && playerStats.currentMana < 10)
        {
            return false;
        }
        else if (moveName == "Double Strike" && playerStats.currentMana < 8)
        {
            return false;
        }
        else if (moveName == "Triple Strike" && playerStats.currentMana < 15)
        {
            return false;
        }
        else if (moveName == "Heavy Strike" && playerStats.currentMana < 15)
        {
            return false;
        }
        else if (moveName == "Healing Strike" && playerStats.currentMana < 12)
        {
            return false;
        }
        else if (moveName == "Attack Buff" && playerStats.currentMana < 5)
        {
            return false;
        }
        else if (moveName == "Defence Buff" && playerStats.currentMana < 5)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void chooseAttack1()
    {//Check which move was pressed
        if (!hasClicked)
        {
            attackName = battack1.GetComponentInChildren<Text>().text;
            if (checkMana(attackName))
            {
                StartCoroutine(PlayerAttack());
                hasClicked = true;
            }
            else
            {
                Debug.Log("Insufficient Mana!");
                hasClicked = false;
            }
        }
    }
    public void chooseAttack2()
    {
        if (!hasClicked)
        {
            attackName = battack2.GetComponentInChildren<Text>().text;
            if(checkMana(attackName))
            {
                StartCoroutine(PlayerAttack());
                hasClicked = true;
            }
            else
            {
                Debug.Log("Insufficient Mana!");
                hasClicked = false;
            }

            // block user from repeatedly 
            // pressing attack button  
        }
    }
    public void chooseAttack3()
    {
        if (!hasClicked)
        {
            attackName = battack3.GetComponentInChildren<Text>().text;
            if (checkMana(attackName))
            {
                StartCoroutine(PlayerAttack());
                hasClicked = true;
            }
            else
            {
                Debug.Log("Insufficient Mana!");
                hasClicked = false;
            }
        }
    }
    public void chooseAttack4()
    {
        if (!hasClicked)
        {
            attackName = battack4.GetComponentInChildren<Text>().text;
            if (checkMana(attackName))
            {
                StartCoroutine(PlayerAttack());
                hasClicked = true;
            }
            else
            {
                Debug.Log("Insufficient Mana!");
                hasClicked = false;
            }
        }
    }

    public void playerAttackPosition()
    {//Move the attacker to their attack position
        if (battleState == BattleState.PLAYERTURN)
        {
            if (!stationaryAttack)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, PAttackPos, Time.deltaTime * 3f);
                playerRunning = true;
                playerAnimator.SetBool("Running", true);
                // Check if the position of the enemy and their attack position are approximately equal.
                if (Vector3.Distance(player.transform.position, PAttackPos) < 0.001f)
                {
                    attackReady = true;
                    playerRunning = false;
                    playerAnimator.SetBool("Running", false);
                }
            }
            else
            {
                attackReady = true;
            }
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            if (!stationaryAttack)
            {
                enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, EAttackPos, Time.deltaTime * 3f);
                enemyRunning = true;
                enemyAnimator.SetBool("Running", true);
                // Check if the position of the enemy and their attack position are approximately equal.
                if (Vector3.Distance(enemy.transform.position, EAttackPos) < 0.001f)
                {
                    enemyAnimator.SetBool("Running", false);
                    attackReady = true;
                    enemyRunning = false;
                }
            }
            else
            {
                attackReady = true;
            }
        }
    }
    public void playerStartPosition()
    {//Return the attacker to their starting position
        stationaryAttack = false;
        attackReady = false;
        if (battleState == BattleState.PLAYERTURN)
        {
            if(!playerFlipped && !turnFinished)
            {
                Vector3 newScale = player.transform.localScale;
                newScale.x *= -1;
                player.transform.localScale = newScale;
                playerFlipped = true;
            }
            player.transform.position = Vector3.MoveTowards(player.transform.position, startPos, Time.deltaTime * 3f);
            playerRunning = true;
            playerAnimator.SetBool("Running", true);
            if (Vector3.Distance(player.transform.position, startPos) < 0.001f)
            {
                playerRunning = false;
                playerAnimator.SetBool("Running", false);
                Vector3 newScale = player.transform.localScale;
                newScale.x *= -1;
                player.transform.localScale = newScale;
                playerFlipped = false;
                turnFinished = true;
                attackName = "";               
            }
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            if (!enemyFlipped)
            {
                Vector3 newScale = enemy.transform.localScale;
                newScale.x *= -1;
                enemy.transform.localScale = newScale;
                enemyFlipped = true;
            }
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemystartPos, Time.deltaTime * 3f);
            enemyRunning = true;
            enemyAnimator.SetBool("Running", true);
            if (Vector3.Distance(enemy.transform.position, enemystartPos) < 0.001f)
            {
                enemyRunning = false;
                enemyAnimator.SetBool("Running", false);
                Vector3 newScale = enemy.transform.localScale;
                newScale.x *= -1;
                enemy.transform.localScale = newScale;
                enemyFlipped = false;
                turnFinished = true;
                attackName = "";
            }
        }
    }

    void disableButtons()
    {//Stop the player from pressing during an attack
        battleCanvas.SetActive(false);
    }
    void enableButtons()
    {
        battleCanvas.SetActive(true);
    }
    void checkStationary()
    {//If a move that doesn't require movement is used the user will stay in their current position
        if (attackName.Contains("Strike") || attackName.Contains("Slash") || attackName.Contains("Spear"))
        {
            stationaryAttack = false;
        }
        else
        {
            stationaryAttack = true;
        }
    }
    public int calcAttackValue()
    {//Before an attack hits check the user or opponents attack rating
        if (battleState == BattleState.PLAYERTURN)
        {
            return Mathf.RoundToInt((playerStats.attackRating + playerStats.battleAttack) / 5);
        }
        else
        {
            return Mathf.RoundToInt((enemyStats.EattackRating + enemyStats.battleAttack) / 5);
        }
    }
    public int calcDefenceValue()
    {//Before an attack hits check the user or opponents defence rating
        if (battleState == BattleState.PLAYERTURN)
        {           
            return Mathf.RoundToInt((enemyStats.EarmorRating + enemyStats.battleDefence) / 7);
        }
        else
        {
            return Mathf.RoundToInt((playerStats.armorRating + playerStats.battleDefence) / 7);
        }
    }

    public void Slash()
    {
        if(battleState == BattleState.PLAYERTURN)
        {
            enemyStats.EcurrentHealth -= (8 + calcAttackValue() - calcDefenceValue());
            moveFinished = true;
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (8 + calcAttackValue() - calcDefenceValue())));
            moveFinished = true;
        }
    }
    public void SpearThrust()
    {
        if (battleState == BattleState.PLAYERTURN)
        {
            int critChance = Random.Range(1, 4);
            if(critChance == 3)
            {//This move has a random chance of dealing extra damage
                enemyStats.EcurrentHealth -= (12 + calcAttackValue() - calcDefenceValue());
            }
            else
            {
                enemyStats.EcurrentHealth -= (8 + calcAttackValue() - calcDefenceValue());
            }
            moveFinished = true;
        }
    }
    public void SimpleSpell()
    {
        if(battleState == BattleState.PLAYERTURN)
        {
            enemyStats.EcurrentHealth -= (8 + calcAttackValue() - calcDefenceValue());
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (8 + calcAttackValue() - calcDefenceValue())));
        }
        moveFinished = true;
    }
    public void AttackBuff()
    {//Increase users attack during battle
        if (battleState == BattleState.PLAYERTURN)
        {
            playerStats.battleAttack += 15;
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 5);
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            enemyStats.battleAttack += 15;
        }
        moveFinished = true;
    }
    public void DefenceBuff()
    {//Increase users defence during battle
        if (battleState == BattleState.PLAYERTURN)
        {
            playerStats.battleDefence += 15;
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 5);
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            enemyStats.battleDefence += 15;
        }
        moveFinished = true;
    }
    public void FireSlash()
    {//Increase users attack after attacking
        if (battleState == BattleState.PLAYERTURN)
        {
            enemyStats.EcurrentHealth -= (8 + calcAttackValue() - calcDefenceValue());
            playerStats.battleAttack += 10;
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 10);
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (8 + calcAttackValue() - calcDefenceValue())));
            enemyStats.battleAttack += 10;
        }
        moveFinished = true;
    }
    public void ThunderStrike()
    {//If this is the first move used it does additional damage, otherwise does damage based on users speed
        if (battleState == BattleState.PLAYERTURN)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 10);
            if (firstTurn)
            {
                enemyStats.EcurrentHealth -= (15 + calcAttackValue() - calcDefenceValue());
            }
            else
            {
                enemyStats.EcurrentHealth -= (10 + calcAttackValue() - calcDefenceValue());
            }

        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            if (firstTurn)
            {
                PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (15 + calcAttackValue() - calcDefenceValue())));
            }
            else
            {
                PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (10 + calcAttackValue() - calcDefenceValue())));
            }
            
        }
        moveFinished = true;
    }
    public void FreezingStrike()
    {//Lower the opponents attack
        if (battleState == BattleState.PLAYERTURN)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 10);
            enemyStats.EcurrentHealth -= (9 + calcAttackValue() - calcDefenceValue());
            enemyStats.battleAttack -= 10;
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (9 + calcAttackValue() - calcDefenceValue())));
            playerStats.battleAttack -= 10;
        }
        moveFinished = true;
    }
    public void HealingStrike()
    {//Heal the user for a percentage of the damage dealt
        if (battleState == BattleState.PLAYERTURN)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 12);
            enemyStats.EcurrentHealth -= (7 + calcAttackValue() - calcDefenceValue());
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth + (7 + (calcAttackValue() - calcDefenceValue()) / 2)));
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (7 + calcAttackValue() - calcDefenceValue())));
            enemyStats.EcurrentHealth += (7 + (calcAttackValue() - calcDefenceValue()) / 2);
        }
        moveFinished = true;
    }
    public void HeavyStrike()
    {//Perform a heavy attack
        if (battleState == BattleState.PLAYERTURN)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 15);
            enemyStats.EcurrentHealth -= (17 + calcAttackValue() - calcDefenceValue());
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (17 + calcAttackValue() - calcDefenceValue())));
        }
        moveFinished = true;
    }
    public void DoubleStrike()
    {//Hit the opponent 2 times
        if (battleState == BattleState.PLAYERTURN && !moveFinished)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 8);
            enemyStats.EcurrentHealth -= (12 + calcAttackValue() - calcDefenceValue());
        }
        else if (battleState == BattleState.ENEMYTURN && !moveFinished)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (12 + calcAttackValue() - calcDefenceValue())));
        }
        moveFinished = true;
    }
    public void TripleStrike()
    {//Hit the opponent 3 times
        if (battleState == BattleState.PLAYERTURN && !moveFinished)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 15);
            enemyStats.EcurrentHealth -= (15 + calcAttackValue() - calcDefenceValue());
        }
        else if (battleState == BattleState.ENEMYTURN && !moveFinished)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (15 + calcAttackValue() - calcDefenceValue())));
        }
    }
    public void ToxicSlash()
    {//Lower opponents defence
        if (battleState == BattleState.PLAYERTURN)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 10);
            enemyStats.EcurrentHealth -= (7 + calcAttackValue() - calcDefenceValue());
            enemyStats.battleDefence -= 15;
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (7 + calcAttackValue() - calcDefenceValue())));
            playerStats.battleDefence -= 15;
        }
        moveFinished = true;
    }
    public void SoulSiphon()
    {//Heal the user for a percentage of the damage dealt
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - (6 + calcAttackValue() - calcDefenceValue())));
            PlayerPrefs.SetInt("ManaRating", (playerStats.currentMana - (1 + calcAttackValue() - calcDefenceValue())));
            enemyStats.EcurrentHealth += (6 + (calcAttackValue() - calcDefenceValue()) / 2);
        }
        moveFinished = true;
    }
    public void DeathSlash()
    {//Heal the user for a percentage of the damage dealt
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", Mathf.RoundToInt(playerStats.currentHealth / 2));
        }
        moveFinished = true;
    }
    public void BuffAllStats()
    {
        PlayerPrefs.SetInt("HealthRating", playerStats.currentHealth + 3);
        PlayerPrefs.SetInt("MaxHealthRating", playerStats.maxHealth + 2);
        PlayerPrefs.SetInt("AttackRating", playerStats.attackRating + 2);
        PlayerPrefs.SetInt("ArmorRating", playerStats.armorRating + 2);
        PlayerPrefs.Save();
    }
    public void UseHealthPotion()
    {//Restore HP with a health potion
        PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("HealthRating") + 30);
    }
    public void UseManaPotion()
    {//Restore MP with a mana potion
        PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") + 30);
    }
    public void Block()
    {//Block the next attack
        battleState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    public void PlayerHit()
    {//Block the next attack
        hit = true;
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
