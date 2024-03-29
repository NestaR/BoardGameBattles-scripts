using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST }

public class BattleMoves : MonoBehaviour
{
    public BattleState battleState;
    public GameObject player, enemy, spellObject, playerAttackP, enemyAttackP, battleCanvas, battleEffects;
    public Animator playerAnimator, enemyAnimator;
    public Vector3 startPos, enemystartPos, PAttackPos, EAttackPos;
    Vector3 pEffectPos, eEffectPos;
    public bool attackChosen, playerRunning, enemyRunning, attackReady, isEnemy, firstTurn, turnFinished = false;
    bool attacking, dodged, attack1, attack2, attack3, attack4;
    bool playerFlipped, enemyFlipped, shielding, fireAbility1, fireAbility2, fireAbility3, fireAbility4;
    public Button battack1, battack2, battack3, battack4, backButton, abilityButton;
    public string signatureAbility;
    public string attackName, previousAttack;
    string[] moveSet, abilityPool;
    public int battleTurn, playerHP, battleSpeed;
    public Text attackUI, defenceUI, turnDescription, moveDescription, healthPotionAmount, manaPotionAmount, mixedPotionAmount, abilityUI, playerName, enemyName;
    int counter, kingCounter, numAttAnimations, attSequence, attackManaCost;
    public bool speedCheck, hasClicked, moveFinished, hit, stationaryAttack;
    PlayerStats playerStats, enemyStats;
    public GameManager battleManager;
    AttackEffects attEffect;
    SoundScript sound;
    void Start()
    {
        //"In Bloom" - Attacks restore hp and mana
        //"Mayday" - Increase defence after being hit
        //"Sunrise/Sunset" - Attacks hit twice
        //"Roundabout" - Using a repeat move boosts attack
        //"Duelist" - Chance to dodge physical attacks
        attEffect = battleEffects.GetComponent<AttackEffects>();
        sound = battleManager.GetComponent<SoundScript>();
        moveSet = new string[4];
        if (!PlayerPrefs.HasKey("PlayerRun"))
        {//Record amount of runs in a single map
            PlayerPrefs.SetInt("PlayerRun", 0);
        }
        if (PlayerPrefs.GetString("CharacterSelected") == "HeroKnight1")
        {
            numAttAnimations = 3;
            playerName.text = "April";
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "HeroKnight2")
        {
            numAttAnimations = 1;
            playerName.text = "Maye";
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MartialHero")
        {
            numAttAnimations = 3;
            playerName.text = "Geshi";
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MedievalWarrior1")
        {
            numAttAnimations = 3;
            playerName.text = "Janus";
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MedievalWarrior2")
        {
            numAttAnimations = 4;
            playerName.text = "Dezima";
        }
        else
        {
            Debug.Log("No character");
        }
        if (PlayerPrefs.GetInt("EnemiesDefeated") > 4)
        {//Unlock characters signature ability
            AddAbility();
            abilityButton.interactable = true;
        }         
        startPos = player.transform.position;
        PAttackPos.x = playerAttackP.transform.position.x;
        PAttackPos.y = playerAttackP.transform.position.y;
        PAttackPos.z = playerAttackP.transform.position.z;
        //Location of fighters attack positions
        EAttackPos.x = enemyAttackP.transform.position.x;
        EAttackPos.y = enemyAttackP.transform.position.y;
        EAttackPos.z = enemyAttackP.transform.position.z;
        //Location of attack effects
        pEffectPos = PAttackPos - new Vector3(1, 0, 0);
        eEffectPos = EAttackPos + new Vector3(1, 0, 0);
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

        moveSet[0] = enemyStats.enemyattack1;
        moveSet[1] = enemyStats.enemyattack2;
        moveSet[2] = enemyStats.enemyattack3;
        moveSet[3] = enemyStats.enemyattack4;
        enemyName.text = enemyStats.enemyMoveSet;
        //if (enemyStats.enemyMoveSet.Contains("Goblin"))
        //{//Enemy movesets
        //    moveSet[0] = "Slash";
        //    moveSet[1] = "Double Strike";
        //    moveSet[2] = "Attack Buff";
        //    moveSet[3] = "Slash";
        //    enemyName.text = "Goblin";
        //}
        //else if (enemyStats.enemyMoveSet.Contains("FlyingEye"))
        //{//Flying Eye
        //    moveSet[0] = "Slash";
        //    moveSet[1] = "Thunder Strike";
        //    moveSet[2] = "Thunder Strike";
        //    moveSet[3] = "Thunder Strike";
        //    enemyName.text = "Flying Eye";
        //}
        //else if (enemyStats.enemyMoveSet.Contains("Skeleton"))
        //{//Skeleton
        //    moveSet[0] = "Slash";
        //    moveSet[1] = "Heavy Strike";
        //    moveSet[2] = "Defence Buff";
        //    moveSet[3] = "Defence Buff";
        //    enemyName.text = "Skeleton";
        //}
        //else if (enemyStats.enemyMoveSet.Contains("BringerOfDeath"))
        //{//Bringer of Death
        //    moveSet[0] = "Slash";
        //    moveSet[1] = "Soul Siphon";
        //    moveSet[2] = "Death Slash";
        //    moveSet[3] = "Slash";
        //    enemyName.text = "Bringer of Death";
        //}
        //else if (enemyStats.enemyMoveSet.Contains("Warrior"))
        //{//Mushroom
        //    moveSet[0] = "Double Slash";
        //    moveSet[1] = "Heavy Strike";
        //    moveSet[2] = "Fire Slash";
        //    moveSet[3] = "Double Slash";
        //    enemyName.text = "Warrior";
        //}
        //else if (enemyStats.enemyMoveSet.Contains("Knight"))
        //{
        //    moveSet[0] = "Shield Brace";
        //    moveSet[1] = "Triple Slash";
        //    moveSet[2] = "Defence Buff";
        //    moveSet[3] = "Defence Buff";
        //    enemyName.text = "Knight";
        //}
        if (enemyStats.enemyMoveSet.Contains("TheKing"))
        {
            //moveSet[0] = "Royal Ruse";
            //moveSet[1] = "First Decree";
            //moveSet[2] = "Second Decree";
            //moveSet[3] = "Final Decree";
            //enemyName.text = "The King";
            EAttackPos.y += 0.5f;
        }
        //else if (enemyStats.enemyMoveSet.Contains("Mushroom"))
        //{
        //    moveSet[0] = "Double Strike";
        //    moveSet[1] = "Toxic Slash";
        //    moveSet[2] = "Toxic Slash";
        //    moveSet[3] = "Healing Strike";
        //    enemyName.text = "Mushroom";
        //}
        //else if (enemyStats.enemyMoveSet.Contains("Lizard"))
        //{
        //    moveSet[0] = "Double Strike";
        //    moveSet[1] = "Toxic Slash";
        //    moveSet[2] = "Toxic Slash";
        //    moveSet[3] = "Healing Strike";
        //    enemyName.text = "Lizard";
        //}
        //else if (enemyStats.enemyMoveSet.Contains("WizardEye"))
        //{
        //    moveSet[0] = "Incendio";
        //    moveSet[1] = "Eruption";
        //    moveSet[2] = "Minds Eye";
        //    moveSet[3] = "Incendio";
        //    enemyName.text = "Wizard Eye";
        //}
        //else if (enemyStats.enemyMoveSet.Contains("EvilWizard"))
        //{
        //    moveSet[0] = "Fire Force";
        //    moveSet[1] = "Blue Blur";
        //    moveSet[2] = "Purple Haze";
        //    moveSet[3] = "Eternal Flame";
        //    enemyName.text = "Evil Wizard";
        //}
        battleState = BattleState.START;
        
    }

    void AddAbility()
    {
        if (PlayerPrefs.GetString("CharacterSelected") == "HeroKnight1")
        {//Using an attack restores health and mana
            signatureAbility = "In Bloom";
            PlayerPrefs.SetString("SignatureAbility", signatureAbility);
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "HeroKnight2")
        {//Getting hit boosts battle defence            
            signatureAbility = "Mayday";
            PlayerPrefs.SetString("SignatureAbility", signatureAbility);               
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MartialHero")
        {//Attacks hit twice
            signatureAbility = "Sunrise/Sunset";
            PlayerPrefs.SetString("SignatureAbility", signatureAbility);               
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MedievalWarrior1")
        {//Using the same attack increases its strength
            signatureAbility = "Roundabout";
            PlayerPrefs.SetString("SignatureAbility", signatureAbility);
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetString("CharacterSelected") == "MedievalWarrior2")
        {//Gain the chance to avoid an attack
            signatureAbility = "Duelist";
            PlayerPrefs.SetString("SignatureAbility", signatureAbility);
            PlayerPrefs.Save();
        }     
    }
    IEnumerator BeginBattle()
    {//Find the player and enemy at the start of the battle
        yield return new WaitForSeconds(0.5f);
        firstTurn = true;
        checkAbility("Fearless");
        if (!speedCheck && enemy != null && enemyStats.Espeed > playerStats.speed)
        {//If the enemy is faster they attack first           
            disableButtons();
            speedCheck = true;
            battleState = BattleState.ENEMYTURN;
            yield return StartCoroutine(EnemyTurn());
        }
        else if (!speedCheck && enemy != null && enemyStats.Espeed <= playerStats.speed)
        {//If the player is faster they attack first
            enableButtons();
            speedCheck = true;
            battleState = BattleState.PLAYERTURN;
            yield return StartCoroutine(PlayerTurn());
        }
    }
    IEnumerator PlayerTurn()
    {
        yield return new WaitForSeconds(1f);
        turnDescription.text = playerName.text + "'s turn";
        turnFinished = false;
        attackReady = false; 
        hasClicked = false;
        checkHP();
        yield return null;
    }

    void Update()
    { 
        if (!speedCheck)
        {
            StartCoroutine(BeginBattle());
        }
        //Display player info
        healthPotionAmount.text = PlayerPrefs.GetInt("HealthPotions").ToString();
        manaPotionAmount.text = PlayerPrefs.GetInt("ManaPotions").ToString();
        mixedPotionAmount.text = PlayerPrefs.GetInt("SpecialPotions").ToString();
        abilityUI.text = signatureAbility;      
    }
    public void checkHP()
    {
        if (enemyStats.EcurrentHealth <= 0)
        {
            // if the enemy health drops to 0 
            // we won!
            battleState = BattleState.WIN;
            enemyAnimator.SetBool("Death", true);
            turnDescription.text = enemyName.text + " defeated!";
            StartCoroutine(EndBattle());
        }
        else if (playerStats.currentHealth <= 0 && playerStats.reviveCharges > 0)
        {
            // if the player health drops to 0 
            // we revive!           
            PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("MaxHealthRating") / 2);
            PlayerPrefs.SetInt("ReviveCharges", PlayerPrefs.GetInt("ReviveCharges") - 1);
            attEffect.playEffect(13, eEffectPos);
            sound.playClip("revive");
            battleState = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }
        else if (playerStats.currentHealth <= 0 && playerStats.reviveCharges <= 0)
        {
            // if the player health drops to 0 
            // we lost!
            battleState = BattleState.LOST;
            playerAnimator.SetBool("Death", true);
            turnDescription.text = playerName.text + " defeated!";
            StartCoroutine(EndBattle());
        }
        else if(battleState == BattleState.PLAYERTURN)
        {
            enableButtons();
        }
    }
    public void chooseAttackAnimation(string AttackName)
    {//Determine which animation to use
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
        else if (AttackName.Contains("First Decree"))
        {
            enemyAnimator.SetTrigger("Attack2");
        }
        else if (AttackName.Contains("Second Decree"))
        {
            enemyAnimator.SetTrigger("Attack3");
        }
        else if (AttackName.Contains("Final Decree"))
        {
            enemyAnimator.SetTrigger("Attack4");
        }
        else if (AttackName.Contains("Fire Force"))
        {
            enemyAnimator.SetTrigger("Attack1");
        }
        else if (AttackName.Contains("Purple Haze"))
        {
            enemyAnimator.SetTrigger("Attack2");
        }
        else if (AttackName.Contains("Blue Blur"))
        {
            enemyAnimator.SetTrigger("Attack3");
        }
        else if (AttackName.Contains("Eternal Flame"))
        {
            enemyAnimator.SetTrigger("Attack4");
        }
        else if (AttackName.Contains("Strike") || AttackName.Contains("Heavy") || AttackName.Contains("Strong"))
        {
            enemyAnimator.SetTrigger("Attack2");
        }
        else if (AttackName.Contains("Shield"))
        {
            enemyAnimator.SetBool("Shield", true);
            //shielding = true;
        }
        else if (AttackName.Contains("Eruption"))
        {
            enemyAnimator.SetTrigger("Attack2");
        }
        else
        {
            enemyAnimator.SetTrigger("Attack1");
        }
    }
    void checkAbility(string abilityName)
    {
        //Check if the players ability should be activated
        if (signatureAbility == "Elementalist" && abilityName == "Elementalist")
        {//Elementalist - Heal for half of mana cost
            PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("HealthRating") + Mathf.RoundToInt(attackManaCost / 2));
        }
        else if (signatureAbility == "Roundabout" && abilityName == "Roundabout")
        {//Roundabout - Increase damage for using same attack
            if (attackName == previousAttack)
            {
                playerStats.battleAttack += 11;
                sound.playClip("atkbuff");
            }
            else
            {
                playerStats.battleAttack = 0;
                sound.playClip("debuff");
            }
        }
        else if (signatureAbility == "Duelist" && abilityName == "Duelist")
        {//Duelist - Chance to dodge physical attacks
            if (Random.Range(0, 100) <= (12 + PlayerPrefs.GetInt("EnemiesDefeated")))
            {
                dodged = true;
                attEffect.playEffect(32, eEffectPos);
            }
            else
            {
                dodged = false;
            }
        }
        else if (moveFinished && signatureAbility == "In Bloom" && abilityName == "In Bloom")
        {//In Bloom - Heal after using a move
            PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("HealthRating") + PlayerPrefs.GetInt("EnemiesDefeated") + 4);
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") + PlayerPrefs.GetInt("EnemiesDefeated") + 4);
            sound.playClip("potion");
        }
        else if (signatureAbility == "Mayday" && abilityName == "Mayday")
        {//Mayday - Increase Defence after being hit
            playerStats.battleDefence += 5 + PlayerPrefs.GetInt("EnemiesDefeated");
            sound.playClip("atkbuff");
        }
        else if (signatureAbility == "Fearless" && abilityName == "Fearless" && enemy != null)
        {//Fearless - Lower enemy attack and defence
            enemyStats.battleAttack -= (3 * PlayerPrefs.GetInt("EnemiesDefeated"));
            enemyStats.battleDefence -= (3 * PlayerPrefs.GetInt("EnemiesDefeated"));
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

        chooseAttackAnimation(attackName);

        turnDescription.text = playerName.text + " used " + attackName + "!";
        attackName = attackName.Replace(" ", "");
        
        yield return new WaitForSeconds(1f);
        if (!stationaryAttack)
        {
            enemyAnimator.SetTrigger("Hit");
            checkAbility("Roundabout");
        }
        
        if(shielding)
        {
            sound.playClip("block");
            moveFinished = true;
            shielding = false;
        }
        else
        {
            Invoke(attackName, 0f);
        }
        checkAbility("Elementalist");

        while (!moveFinished)
        {
            yield return null;
        }

        previousAttack = attackName;
        if (!stationaryAttack && moveFinished && signatureAbility == "Sunrise/Sunset")
        {//Attack again
            chooseAttackAnimation("Strike");
            yield return new WaitForSeconds(0.9f);
            enemyAnimator.SetTrigger("Hit");
            Invoke(attackName, 0f);
        }
        yield return new WaitForSeconds(0.4f);
        checkAbility("In Bloom");

        if (fireAbility1 && !stationaryAttack)
        {//Different modes for wizard boss
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - 3));
            moveDescription.text = playerName.text + " took damage from the extreme heat";
        }
        else if (fireAbility2)
        {
            playerStats.battleAttack -= 5;
            moveDescription.text = playerName.text + " lost some Attack";
        }
        else if (fireAbility3)
        {
            PlayerPrefs.SetInt("ManaRating", (playerStats.currentMana - 5));
            moveDescription.text = playerName.text + " lost some Mana";
        }
        else if (fireAbility4)
        {
            int hp = PlayerPrefs.GetInt("HealthRating");
            int mp = PlayerPrefs.GetInt("ManaRating");
            PlayerPrefs.SetInt("HealthRating", mp);
            PlayerPrefs.SetInt("ManaRating", hp);
            moveDescription.text = playerName.text + "'s Health and Mana were swapped!";
        }
        while (!turnFinished)
        {
            playerStartPosition();
            yield return null;
        }
        firstTurn = false;
        battleState = BattleState.ENEMYTURN;
        yield return StartCoroutine(EnemyTurn());
    }
    IEnumerator EnemyTurn()
    {//Pick a random attack from the enemies moveset
        enemyAnimator.SetBool("Shield", false);
        turnDescription.text = enemyName.text + "'s turn";
        checkHP();
        yield return new WaitForSeconds(1);
        turnFinished = false;
        moveFinished = false;
        attackReady = false;  
        
        while (attackName == null || attackName == "")
        {
            if (enemyStats.enemyMoveSet.Contains("TheKing") && enemyStats.EcurrentHealth < (enemyStats.EmaxHealth / 2) && kingCounter < 3)
            {//This boss uses moves in a certain order
                kingCounter += 1;
                attackName = moveSet[kingCounter];
            }
            else if (enemyStats.enemyMoveSet.Contains("TheKing"))
            {
                attackName = moveSet[0];
                kingCounter = 0;
            }
            else
            {
                attackName = moveSet[Random.Range(0, 4)];
            }
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
        else if(stationaryAttack && enemyStats.enemyMoveSet.Contains("BringerOfDeath"))
        {//Spawn attack animation for this boss
            enemyAnimator.SetTrigger("Cast");
            yield return new WaitForSeconds(0.3f);
            enemy.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        else if (stationaryAttack && enemyStats.enemyMoveSet.Contains("EvilWizard"))
        {
            chooseEnemyAttackAnimation(attackName);
        }
        turnDescription.text = enemyName.text + " used " + attackName + "!";
        attackName = attackName.Replace(" ", "");
        

        yield return new WaitForSeconds(1.2f);
        if (!stationaryAttack)
        {
            playerAnimator.SetTrigger("Hit");
            checkAbility("Mayday");
        }
        else if (stationaryAttack && enemyStats.enemyMoveSet.Contains("BringerOfDeath"))
        {
            playerAnimator.SetTrigger("Hit");
            enemy.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            checkAbility("Mayday");
        }
        else if (stationaryAttack && enemyStats.enemyMoveSet.Contains("EvilWizard"))
        {
            playerAnimator.SetTrigger("Hit");
            checkAbility("Mayday");
        }
        checkAbility("Duelist");
        if ((attackName.Contains("Slash") || attackName.Contains("Strike")) && dodged)
        {
            Debug.Log("Attack Dodged!");
            moveDescription.text = "Attack Dodged!";
            playerAnimator.SetTrigger("Dodge");
            sound.playClip("dodge");
            dodged = false;
            moveFinished = true;
        }
        else
        {
            Invoke(attackName, 0f);
        }
        

        while (!moveFinished)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);
        while (!turnFinished)
        {
            playerStartPosition();
            yield return null;
        }
        firstTurn = false;
        battleState = BattleState.PLAYERTURN;
        yield return StartCoroutine(PlayerTurn());
    }
    void mapSelectedWin()
    {
        if(PlayerPrefs.HasKey("MapWins"))
        {
            PlayerPrefs.SetString("MapWins", PlayerPrefs.GetString("MapWins") + "" + PlayerPrefs.GetString("MapSelected"));
        }
        else
        {
            PlayerPrefs.SetString("MapWins", PlayerPrefs.GetString("MapSelected"));
        }
    }
    IEnumerator EndBattle()
    {
        battleManager.GetComponent<TransitionScene>().resetPositions();
        // check if we won
        if (battleState == BattleState.WIN && (enemyStats.enemyMoveSet.Contains("BringerOfDeath") || enemyStats.enemyMoveSet.Contains("TheKing") || enemyStats.enemyMoveSet.Contains("EvilWizard")))
        {     
            PlayerPrefs.SetInt("EnemiesDefeated", PlayerPrefs.GetInt("EnemiesDefeated") + 1);           
            mapSelectedWin();
            yield return new WaitForSeconds(2);
            Destroy(enemy.gameObject);
            PlayerPrefs.SetInt("NextScene", 3);
        }
        else if (battleState == BattleState.WIN)
        {
            sound.playClip("battleVictory");
            BuffAllStats();
            PlayerPrefs.SetInt("EnemiesDefeated", PlayerPrefs.GetInt("EnemiesDefeated") + 1);
            int potionChance;
            potionChance = Random.Range(0, 10);
            if (potionChance == 0 || potionChance == 1)
            {
                PlayerPrefs.SetInt("HealthPotions", PlayerPrefs.GetInt("HealthPotions") + 1);
                moveDescription.text = "A Health Potion was found!";
            }
            else if (potionChance == 2 || potionChance == 3)
            {
                PlayerPrefs.SetInt("ManaPotions", PlayerPrefs.GetInt("ManaPotions") + 1);
                moveDescription.text = "A Mana Potion was found!";
            }
            else if (potionChance == 4)
            {
                PlayerPrefs.SetInt("SpecialPotions", PlayerPrefs.GetInt("SpecialPotions") + 1);
                moveDescription.text = "A Special Potion was found!";
            }
            yield return new WaitForSeconds(2);
            Destroy(enemy.gameObject);
            PlayerPrefs.SetInt("NextScene", 0);
        }
        else if (battleState == BattleState.LOST)
        {//check if we lost
            sound.playClip("death");
            yield return new WaitForSeconds(2);
            if(enemyStats.enemyMoveSet.Contains("BringerOfDeath") || enemyStats.enemyMoveSet.Contains("TheKing") || enemyStats.enemyMoveSet.Contains("EvilWizard"))
            {
                PlayerPrefs.SetInt("PlayerRun", PlayerPrefs.GetInt("PlayerRun") + 1);
            }
            Destroy(enemy.gameObject);
            PlayerPrefs.SetInt("NextScene", 2);
        }
    }

    public bool checkMana(string moveName)
    {
        if (moveName == "Fire Slash" && playerStats.currentMana < 8)
        {
            return false;
        }
        else if (moveName == "Thunder Strike" && playerStats.currentMana < 8)
        {
            return false;
        }
        else if (moveName == "Freezing Strike" && playerStats.currentMana < 8)
        {
            return false;
        }
        else if (moveName == "Toxic Slash" && playerStats.currentMana < 8)
        {
            return false;
        }
        else if (moveName == "Double Strike" && playerStats.currentMana < 3)
        {
            return false;
        }
        else if (moveName == "Triple Strike" && playerStats.currentMana < 9)
        {
            return false;
        }
        else if (moveName == "Heavy Strike" && playerStats.currentMana < 6)
        {
            return false;
        }
        else if (moveName == "Healing Strike" && playerStats.currentMana < 10)
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
        else if (moveName == "Radiant Flicker" && playerStats.currentMana < 3)
        {
            return false;
        }
        else if (moveName == "Luminous Spark" && playerStats.currentMana < 9)
        {
            return false;
        }
        else if (moveName == "Eruption" && playerStats.currentMana < 10)
        {
            return false;
        }
        else if (moveName == "Thunderball" && playerStats.currentMana < 8)
        {
            return false;
        }
        else if (moveName == "Plunge" && playerStats.currentMana < 8)
        {
            return false;
        }
        else if (moveName == "Rock Blast" && playerStats.currentMana < 8)
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
                sound.playClip("declineClick");
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
                sound.playClip("declineClick");
                Debug.Log("Insufficient Mana!");
                hasClicked = false;
            }
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
                sound.playClip("declineClick");
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
                sound.playClip("declineClick");
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
            else if (stationaryAttack && enemyStats.enemyMoveSet.Contains("EvilWizard"))
            {
                enemy.transform.position = EAttackPos;
                attackReady = true;
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
            if (enemyStats.enemyMoveSet.Contains("EvilWizard"))
            {
                enemy.transform.position = enemystartPos;
                turnFinished = true;
            }
            else
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
        if (attackName.Contains("Strike") || attackName.Contains("Slash") || attackName.Contains("Spear") || attackName.Contains("Royal") || attackName.Contains("Decree"))
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
    {//Each attack has a damage calculations to obtain a value and play a sound/visual effect
        if(battleState == BattleState.PLAYERTURN && !shielding)
        {
            enemyStats.EcurrentHealth -= Math.Abs((8 + calcAttackValue() - calcDefenceValue()));
            attEffect.playEffect(45, pEffectPos);
            moveDescription.text = "";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((8 + calcAttackValue() - calcDefenceValue()))));
            attEffect.playEffect(45, eEffectPos);
            moveDescription.text = "";
        }
        moveFinished = true;
        sound.playClip("attack");
    }
    public void Bite()
    {
        if (battleState == BattleState.PLAYERTURN && !shielding)
        {
            enemyStats.EcurrentHealth -= Math.Abs((8 + calcAttackValue()));
            attEffect.playEffect(11, pEffectPos);
            moveDescription.text = "The Bite ignores Defence!";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((8 + calcAttackValue()))));
            attEffect.playEffect(11, eEffectPos);
            moveDescription.text = "The Bite ignores Defence!";
        }
        moveFinished = true;
        sound.playClip("attack");
    }
    public void SpearThrust()
    {
        if (battleState == BattleState.PLAYERTURN && !shielding)
        {
            int critChance = Random.Range(1, 4);
            if(critChance == 3)
            {//This move has a random chance of dealing extra damage
                enemyStats.EcurrentHealth -= Math.Abs((12 + calcAttackValue() - calcDefenceValue()));
                moveDescription.text = "Critical Hit!";
            }
            else
            {
                enemyStats.EcurrentHealth -= Math.Abs((8 + calcAttackValue() - calcDefenceValue()));
            }
            attEffect.playEffect(5, pEffectPos);
        }
        moveFinished = true;
        sound.playClip("heavyatk");
    }
    public void RadiantFlicker()
    {
        if(battleState == BattleState.PLAYERTURN)
        {
            enemyStats.EcurrentHealth -= Math.Abs((8 + calcAttackValue()));
            attEffect.playEffect(29, pEffectPos);
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 3);
            attackManaCost = 3;
            moveDescription.text = "";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((8 + calcAttackValue()))));
            attEffect.playEffect(29, eEffectPos);
        }
        sound.playClip("attack");
        moveFinished = true;
    }
    public void Eruption()
    {//Consumes the users battle attack
        if (battleState == BattleState.PLAYERTURN)
        {
            enemyStats.EcurrentHealth -= Math.Abs((Mathf.RoundToInt(0.5f * playerStats.battleAttack) + calcAttackValue()));
            playerStats.battleAttack = 0;
            attEffect.playEffect(26, pEffectPos);
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 10);
            attackManaCost = 10;
            moveDescription.text = playerName.text + "'s battle attack was consumed!";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((Mathf.RoundToInt(0.5f * enemyStats.battleAttack)) + calcAttackValue())));
            enemyStats.battleAttack = 0;
            attEffect.playEffect(26, eEffectPos);
            moveDescription.text = enemyName.text + "'s battle attack was consumed!";
        }
        sound.playClip("fireatk");
        moveFinished = true;
    }
    public void Thunderball()
    {//Deal bonus damage based on speed
        if (battleState == BattleState.PLAYERTURN)
        {
            enemyStats.EcurrentHealth -= Math.Abs((2 + calcAttackValue() + playerStats.speed));
            attEffect.playEffect(0, pEffectPos);
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 8);
            attackManaCost = 8;
            moveDescription.text = "";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((8 + calcAttackValue() + enemyStats.Espeed))));
            attEffect.playEffect(0, eEffectPos);
            moveDescription.text = "";
        }
        sound.playClip("thunderatk");
        moveFinished = true;
    }
    public void Plunge()
    {//Remove opponents battle attack
        if (battleState == BattleState.PLAYERTURN)
        {
            enemyStats.EcurrentHealth -= Math.Abs((8 + calcAttackValue()));
            attEffect.playEffect(22, pEffectPos);
            enemyStats.battleAttack = 0;
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 8);
            attackManaCost = 8;
            moveDescription.text = enemyName.text + "'s battle attack was cleansed!";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((8 + calcAttackValue()))));
            playerStats.battleAttack = 0;
            attEffect.playEffect(22, eEffectPos);
            moveDescription.text = playerName.text + "'s battle attack was cleansed!";
        }
        sound.playClip("iceatk");
        Invoke("debuffSound", 0.5f);
        moveFinished = true;
    }
    public void RockBlast()
    {//Remove opponents battle defence
        if (battleState == BattleState.PLAYERTURN)
        {
            enemyStats.EcurrentHealth -= Math.Abs((8 + calcAttackValue()));
            attEffect.playEffect(44, pEffectPos);
            enemyStats.battleDefence = 0;
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 8);
            attackManaCost = 8;
            moveDescription.text = enemyName.text + "'s battle defence was destroyed!";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((8 + calcAttackValue()))));
            attEffect.playEffect(44, eEffectPos);
            playerStats.battleDefence = 0;
            moveDescription.text = playerName.text + "'s battle defence was destroyed!";
        }
        sound.playClip("thunderatk");
        Invoke("debuffSound", 0.5f);
        moveFinished = true;
    }
    public void LuminousSpark()
    {
        if (battleState == BattleState.PLAYERTURN)
        {
            enemyStats.EcurrentHealth -= Math.Abs((12 + calcAttackValue()));
            attEffect.playEffect(43, pEffectPos);
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 9);
            attackManaCost = 9;
            moveDescription.text = "";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((12 + calcAttackValue()))));
            attEffect.playEffect(43, eEffectPos);
            moveDescription.text = "";
        }
        sound.playClip("attack");
        moveFinished = true;
    }
    public void AttackBuff()
    {//Increase users attack during battle
        if (battleState == BattleState.PLAYERTURN)
        {
            if (signatureAbility == "Snowball")
            {
                playerStats.battleAttack += 30;
            }
            else
            {
                playerStats.battleAttack += 15;
            }
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 5);
            attackManaCost = 5;
            this.GetComponent<StatusEffects>().chooseBuff(0);
            this.GetComponent<StatusEffects>().ApplyBuff(player.transform.position);
            moveDescription.text = playerName.text + "'s Attack increased!";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            enemyStats.battleAttack += 15;
            this.GetComponent<StatusEffects>().chooseBuff(0);
            this.GetComponent<StatusEffects>().ApplyBuff(enemy.transform.position);
            moveDescription.text = enemyName.text + "'s Attack increased!";
        }
        sound.playClip("atkbuff");
        moveFinished = true;
    }
    public void DefenceBuff()
    {//Increase users defence during battle
        if (battleState == BattleState.PLAYERTURN)
        {
            if (signatureAbility == "Snowball")
            {
                playerStats.battleDefence += 30;
            }
            else
            {
                playerStats.battleDefence += 15;
            }
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 5);
            attackManaCost = 5;
            this.GetComponent<StatusEffects>().chooseBuff(1);
            this.GetComponent<StatusEffects>().ApplyBuff(player.transform.position);
            moveDescription.text = playerName.text + "'s Defence increased!";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            enemyStats.battleDefence += 15;
            this.GetComponent<StatusEffects>().chooseBuff(1);
            this.GetComponent<StatusEffects>().ApplyBuff(enemy.transform.position);
            moveDescription.text = enemyName.text + "'s Defence increased!";
        }
        sound.playClip("atkbuff");
        moveFinished = true;
    }
    public void FireSlash()
    {//Increase users attack after attacking
        if (battleState == BattleState.PLAYERTURN && !shielding)
        {
            enemyStats.EcurrentHealth -= Math.Abs((8 + calcAttackValue() - calcDefenceValue()));
            if (signatureAbility == "Snowball")
            {
                playerStats.battleAttack += 20;
            }
            else
            {
                playerStats.battleAttack += 10;
            }
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 8);
            attackManaCost = 8;
            attEffect.playEffect(13, pEffectPos);
            this.GetComponent<StatusEffects>().chooseBuff(0);
            this.GetComponent<StatusEffects>().ApplyBuff(player.transform.position);
            moveDescription.text = playerName.text + "'s attack increased!";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((8 + calcAttackValue() - calcDefenceValue()))));
            enemyStats.battleAttack += 10;
            attEffect.playEffect(13, eEffectPos);
            this.GetComponent<StatusEffects>().chooseBuff(0);
            this.GetComponent<StatusEffects>().ApplyBuff(enemy.transform.position);
            moveDescription.text = enemyName.text + "'s attack increased!";
        }
        sound.playClip("fireatk");
        Invoke("buffSound", 0.5f);
        moveFinished = true;
    }
    public void ThunderStrike()
    {//If this is the first move used it does additional damage, otherwise does damage based on users speed
        if (battleState == BattleState.PLAYERTURN && !shielding)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 8);
            attackManaCost = 8;
            if (firstTurn)
            {
                enemyStats.EcurrentHealth -= Math.Abs((15 + calcAttackValue() - calcDefenceValue()));
                moveDescription.text = "The attack did bonus damage!";
            }
            else
            {
                enemyStats.EcurrentHealth -= Math.Abs((10 + calcAttackValue() - calcDefenceValue()));
                moveDescription.text = "";
            }
            attEffect.playEffect(24, pEffectPos);
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            if (firstTurn)
            {
                PlayerPrefs.SetInt("HealthRating", Math.Abs((playerStats.currentHealth - (15 + calcAttackValue() - calcDefenceValue()))));
                moveDescription.text = "The attack did bonus damage!";
            }
            else
            {
                PlayerPrefs.SetInt("HealthRating", Math.Abs((playerStats.currentHealth - (10 + calcAttackValue() - calcDefenceValue()))));
                moveDescription.text = "";
            }
            attEffect.playEffect(24, eEffectPos);
        }
        sound.playClip("thunderatk");
        moveFinished = true;
    }
    public void FreezingStrike()
    {//Lower the opponents attack
        if (battleState == BattleState.PLAYERTURN && !shielding)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 8);
            attackManaCost = 8;
            enemyStats.EcurrentHealth -= Math.Abs((9 + calcAttackValue() - calcDefenceValue()));
            enemyStats.battleAttack -= 10;
            attEffect.playEffect(46, pEffectPos);
            moveDescription.text = enemyName.text + "'s attack decreased!";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", Math.Abs((playerStats.currentHealth - (9 + calcAttackValue() - calcDefenceValue()))));
            playerStats.battleAttack -= 10;
            attEffect.playEffect(46, eEffectPos);
            moveDescription.text = playerName.text + "'s attack decreased!";
        }
        sound.playClip("iceatk");
        Invoke("debuffSound", 0.5f);
        moveFinished = true;
    }
    public void HealingStrike()
    {//Heal the user for a percentage of the damage dealt
        if (battleState == BattleState.PLAYERTURN && !shielding)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 10);
            attackManaCost = 10;
            enemyStats.EcurrentHealth -= Math.Abs((7 + calcAttackValue() - calcDefenceValue()));
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth + Math.Abs((7 + (calcAttackValue() - calcDefenceValue()) / 2))));
            attEffect.playEffect(29, pEffectPos);
            moveDescription.text = playerName.text + " regained some HP";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((7 + calcAttackValue() - calcDefenceValue()))));
            enemyStats.EcurrentHealth += Math.Abs((7 + (calcAttackValue() - calcDefenceValue()) / 2));
            attEffect.playEffect(29, eEffectPos);
            moveDescription.text = enemyName.text + " regained some HP";
        }
        sound.playClip("absorb");
        moveFinished = true;
    }
    public void HeavyStrike()
    {//Perform a strong attack
        if (battleState == BattleState.PLAYERTURN && !shielding)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 6);
            attackManaCost = 6;
            enemyStats.EcurrentHealth -= Math.Abs((13 + calcAttackValue() - calcDefenceValue()));
            attEffect.playEffect(47, pEffectPos);
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((13 + calcAttackValue() - calcDefenceValue()))));
            attEffect.playEffect(47, eEffectPos);
        }
        sound.playClip("heavyatk");
        moveFinished = true;
    }
    public void DoubleStrike()
    {//Hit the opponent twice
        if (battleState == BattleState.PLAYERTURN && !shielding)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 3);
            attackManaCost = 3;
            enemyStats.EcurrentHealth -= Math.Abs((11 + calcAttackValue() - calcDefenceValue()));
            attEffect.playEffect(3, pEffectPos);
            moveDescription.text = "";
            Invoke("addAtt", 0.2f);
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((11 + calcAttackValue() - calcDefenceValue()))));
            attEffect.playEffect(3, eEffectPos);
            moveDescription.text = "";
            Invoke("addAtt", 0.2f);
        }
        sound.playClip("heavyatk");
        moveFinished = true;
    }
    public void TripleStrike()
    {//Hit the opponent 3 times
        if (battleState == BattleState.PLAYERTURN && !shielding)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 9);
            attackManaCost = 9;
            enemyStats.EcurrentHealth -= Math.Abs((15 + calcAttackValue() - calcDefenceValue()));
            attEffect.playEffect(3, pEffectPos);
            moveDescription.text = "";
            Invoke("addAtt", 0.2f);
            Invoke("addAtt", 0.2f);
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((15 + calcAttackValue() - calcDefenceValue()))));
            attEffect.playEffect(3, eEffectPos);
            moveDescription.text = "";
            Invoke("addAtt", 0.2f);
            Invoke("addAtt", 0.2f);
        }
        sound.playClip("heavyatk");
        moveFinished = true;
    }
    public void addAtt()
    {
        if (battleState == BattleState.PLAYERTURN)
        {
            attEffect.playEffect(3, pEffectPos);
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            attEffect.playEffect(3, eEffectPos);
        }
    }
    public void debuffSound()
    {
        sound.playClip("debuff");
    }
    public void buffSound()
    {
        sound.playClip("atkbuff");
    }
    public void ToxicSlash()
    {//Lower opponents defence
        if (battleState == BattleState.PLAYERTURN && !shielding)
        {
            PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") - 8);
            attackManaCost = 8;
            enemyStats.EcurrentHealth -= Math.Abs((7 + calcAttackValue() - calcDefenceValue()));
            enemyStats.battleDefence -= 15;
            attEffect.playEffect(10, pEffectPos);
            moveDescription.text = enemyName.text + "'s Defence decreased!";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((7 + calcAttackValue() - calcDefenceValue()))));
            playerStats.battleDefence -= 15;
            attEffect.playEffect(10, eEffectPos);
            moveDescription.text = playerName.text + "'s Defence decreased!";
        }
        sound.playClip("toxicatk");
        Invoke("debuffSound", 0.5f);
        moveFinished = true;
    }
    public void SoulSiphon()
    {//Heal the user for a percentage of the damage dealt
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((6 + calcAttackValue() - calcDefenceValue()))));
            PlayerPrefs.SetInt("ManaRating", (playerStats.currentMana - Math.Abs((1 + calcAttackValue() - calcDefenceValue()))));
            enemyStats.EcurrentHealth += Math.Abs((6 + (calcAttackValue() - calcDefenceValue()) / 2));
            attEffect.playEffect(4, eEffectPos);
            moveDescription.text = "A portion of " + playerName.text + "'s Health and Mana was taken!";
        }
        sound.playClip("absorb");
        moveFinished = true;
    }
    public void DeathSlash()
    {//Lower the targets hp by half of their current hp
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", Mathf.RoundToInt(playerStats.currentHealth / 2));
            attEffect.playEffect(38, eEffectPos);
            moveDescription.text = playerName.text + "'s HP was cut in half!";
        }
        sound.playClip("heavyatk");
        moveFinished = true;
    }
    public void DoubleSlash()
    {//Lower the targets attack and defence
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((8 + calcAttackValue() - calcDefenceValue()))));
            playerStats.battleAttack -= 5;
            playerStats.battleDefence -= 5;
            attEffect.playEffect(11, eEffectPos);
            moveDescription.text = "Attack and Defence lowered!";
        }
        sound.playClip("heavyatk");
        Invoke("debuffSound", 0.5f);
        moveFinished = true;
    }
    public void TripleSlash()
    {//
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((10 + calcAttackValue() - calcDefenceValue()))));
            attEffect.playEffect(11, eEffectPos);
            moveDescription.text = "";
        }
        sound.playClip("heavyatk");
        moveFinished = true;
    }
    public void ShieldBrace()
    {//Take 0 damage from next physical attack
        if (battleState == BattleState.PLAYERTURN)
        {
            moveDescription.text = "User is protected against next physical attack";
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            moveDescription.text = "User is protected against next physical attack";
            shielding = true;
        }
        sound.playClip("atkbuff");
        moveFinished = true;
    }
    public void RoyalRuse()
    {//Increase the players atk and hp and lower their mana
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", playerStats.currentHealth + 9);
            PlayerPrefs.SetInt("ManaRating", playerStats.currentMana - 9);
            playerStats.battleAttack += 13;
            attEffect.playEffect(47, eEffectPos);
            moveDescription.text = playerName.text + "'s Health & Attack Increased!";
        }
        sound.playClip("heavyatk");
        moveFinished = true;
    }
    public void FirstDecree()
    {//Takes all of the players potions
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthPotions", 0);
            PlayerPrefs.SetInt("ManaPotions", 0);
            PlayerPrefs.SetInt("SpecialPotions", 0);
            attEffect.playEffect(47, eEffectPos);
            moveDescription.text = "No more potions!";
        }
        sound.playClip("heavyatk");
        Invoke("debuffSound", 0.5f);
        moveFinished = true;
    }
    public void SecondDecree()
    {//Take all of the players health
        if (battleState == BattleState.ENEMYTURN)
        {
            enemyStats.EcurrentHealth += (PlayerPrefs.GetInt("HealthRating") - 1);
            PlayerPrefs.SetInt("HealthRating", 1);
            attEffect.playEffect(47, eEffectPos);
            moveDescription.text = "Your life belongs to me!";
        }
        sound.playClip("heavyatk");
        moveFinished = true;
    }
    public void FinalDecree()
    {//Deal 1 damage to the player
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", playerStats.currentHealth - 1);
            attEffect.playEffect(47, eEffectPos);
            moveDescription.text = "Bow down before The King!";
        }
        sound.playClip("heavyatk");
        moveFinished = true;
    }
    public void Incendio()
    {//Increases the users attack for 1 turn
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((9 + calcAttackValue()))));
            enemyStats.battleAttack = 33;
            attEffect.playEffect(40, eEffectPos);
            moveDescription.text = "Battle Attack increased for the next attack";
        }
        sound.playClip("fireatk");
        Invoke("buffSound", 0.5f);
        moveFinished = true;
    }
    public void MindsEye()
    {//Increase users attack/defence/health
        if (battleState == BattleState.ENEMYTURN)
        {
            enemyStats.EcurrentHealth += 15;
            enemyStats.battleAttack += 15;
            enemyStats.battleDefence += 15;
            attEffect.playEffect(42, pEffectPos);
            moveDescription.text = "Attack/Defence/Health Increased!";
        }
        sound.playClip("atkbuff");
        moveFinished = true;
    }
    public void fireAbility(int abilityNumber)
    {
        if(abilityNumber == 1)
        {
            fireAbility1 = true;
            fireAbility2 = false;
            fireAbility3 = false;
            fireAbility4 = false;
        }
        else if (abilityNumber == 2)
        {
            fireAbility1 = false;
            fireAbility2 = true;
            fireAbility3 = false;
            fireAbility4 = false;
        }
        else if (abilityNumber == 3)
        {
            fireAbility1 = false;
            fireAbility2 = false;
            fireAbility3 = true;
            fireAbility4 = false;
        }
        else if (abilityNumber == 4)
        {
            fireAbility1 = false;
            fireAbility2 = false;
            fireAbility3 = false;
            fireAbility4 = true;
        }
    }
    public void FireForce()
    {//Attacking the wizard after using this move deals damage to the player
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((9 + calcAttackValue()))));
            fireAbility(1);
            moveDescription.text = "The wizard is covered with an infernal blaze";
        }
        sound.playClip("fireatk");
        moveFinished = true;
    }
    public void PurpleHaze()
    {//Attacking the wizard after using this move lowers the players attack
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((9 + calcAttackValue()))));
            fireAbility(2);
            moveDescription.text = "The wizard is enveloped in a misty haze";
        }
        sound.playClip("fireatk");
        moveFinished = true;
    }
    public void BlueBlur()
    {//Attacking the wizard after using this move lowers the players mana
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((9 + calcAttackValue()))));
            fireAbility(3);
            moveDescription.text = "The wizard is shrouded in a mysterious aura";
        }
        sound.playClip("fireatk");
        moveFinished = true;
    }
    public void EternalFlame()
    {//Attacking the wizard after using this move swaps the players health and mana
        if (battleState == BattleState.ENEMYTURN)
        {
            PlayerPrefs.SetInt("HealthRating", (playerStats.currentHealth - Math.Abs((9 + calcAttackValue()))));
            fireAbility(4);
            moveDescription.text = "The wizard is engulfed by the eternal flame";
        }
        sound.playClip("fireatk");
        moveFinished = true;
    }
    public void BuffAllStats()
    {
        PlayerPrefs.SetInt("HealthRating", playerStats.currentHealth + 3);
        PlayerPrefs.SetInt("MaxHealthRating", playerStats.maxHealth + 3);
        PlayerPrefs.SetInt("AttackRating", playerStats.attackRating + 3);
        PlayerPrefs.SetInt("ArmorRating", playerStats.armorRating + 3);
        PlayerPrefs.Save();
    }
    public void UseHealthPotion()
    {//Restore HP with a health potion
        if(PlayerPrefs.GetInt("HealthPotions") > 0)
        {
            this.GetComponent<StatusEffects>().chooseBuff(2);
            this.GetComponent<StatusEffects>().ApplyBuff(player.transform.position);
            if (signatureAbility == "Alchemy")
            {
                PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("HealthRating") + 25 + (2 + PlayerPrefs.GetInt("EnemiesDefeated")));
                if(Random.Range(0, PlayerPrefs.GetInt("EnemiesDefeated")) > (PlayerPrefs.GetInt("EnemiesDefeated") / 2))
                {
                    Debug.Log("Potion not consumed!");
                }
                else
                {
                    PlayerPrefs.SetInt("HealthPotions", PlayerPrefs.GetInt("HealthPotions") - 1);
                }
            }
            else
            {
                PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("HealthRating") + 25);
                PlayerPrefs.SetInt("HealthPotions", PlayerPrefs.GetInt("HealthPotions") - 1);
            }
            sound.playClip("potion");
        }
        else
        {
            Debug.Log("No potions left!");
            sound.playClip("declineClick");
        }
    }
    public void UseManaPotion()
    {//Restore MP with a mana potion
        if (PlayerPrefs.GetInt("ManaPotions") > 0)
        {
            this.GetComponent<StatusEffects>().chooseBuff(2);
            this.GetComponent<StatusEffects>().ApplyBuff(player.transform.position);
            if (signatureAbility == "Alchemy")
            {
                PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") + 25 + (2 + PlayerPrefs.GetInt("EnemiesDefeated")));
                if (Random.Range(0, PlayerPrefs.GetInt("EnemiesDefeated")) > (PlayerPrefs.GetInt("EnemiesDefeated") / 2))
                {
                    Debug.Log("Potion not consumed!");
                }
                else
                {
                    PlayerPrefs.SetInt("ManaPotions", PlayerPrefs.GetInt("ManaPotions") - 1);
                }
            }
            else
            {
                PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") + 25);
                PlayerPrefs.SetInt("ManaPotions", PlayerPrefs.GetInt("ManaPotions") - 1);
            }
            sound.playClip("potion");
        }
        else
        {
            Debug.Log("No potions left!");
            sound.playClip("declineClick");
        }
    }
    public void UseSpecialPotion()
    {//Restore HP and MP with a special potion
        if (PlayerPrefs.GetInt("SpecialPotions") > 0)
        {
            this.GetComponent<StatusEffects>().chooseBuff(2);
            this.GetComponent<StatusEffects>().ApplyBuff(player.transform.position);
            if (signatureAbility == "Alchemy")
            {
                PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("HealthRating") + 20 + (2 + PlayerPrefs.GetInt("EnemiesDefeated")));
                PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") + 20 + (2 + PlayerPrefs.GetInt("EnemiesDefeated")));
                if (Random.Range(0, PlayerPrefs.GetInt("EnemiesDefeated")) > (PlayerPrefs.GetInt("EnemiesDefeated") / 2))
                {
                    Debug.Log("Potion not consumed!");
                }
                else
                {
                    PlayerPrefs.SetInt("SpecialPotions", PlayerPrefs.GetInt("SpecialPotions") - 1);
                }
            }
            else
            {
                PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("HealthRating") + 20);
                PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("ManaRating") + 20);
                PlayerPrefs.SetInt("SpecialPotions", PlayerPrefs.GetInt("SpecialPotions") - 1);
            }
            sound.playClip("potion");
        }
        else
        {
            Debug.Log("No potions left!");
            sound.playClip("declineClick");
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
}
