using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool canRoll, mouseEnabled;
    public string tileColour;
    public GameObject player, sceneObjects, battleObjects, diceObject, menuCanvas, chestCanvas, shopCanvas, buffCanvas, statueBouqet1, statueBouqet2;
    public Text map1Wins, map2Wins, map3Wins;
    public int m1wins, m2wins, m3wins;
    PlayerMovement playerM;
    void Start()
    {        
        if (player != null)
        {
            playerM = player.GetComponent<PlayerMovement>();
        }
        //PlayerPrefs.SetString("MapWins", "Map1Map1Map3Map2Map2Map2");
        foreach (var ch in PlayerPrefs.GetString("MapWins"))
        {
            if (ch.ToString().Contains("1"))
            {
                m1wins += 1;
            }
            else if (ch.ToString().Contains("2"))
            {
                m2wins += 1;
            }
            else if (ch.ToString().Contains("3"))
            {
                m3wins += 1;
            }
        }
        if (playerM != null)
        {
            playerM.showRoll = true;
        }     
    }
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (player == null && scene.name == "StartScene")
        {
            map1Wins.text = m1wins.ToString();
            map2Wins.text = m2wins.ToString();
            map3Wins.text = m3wins.ToString();
        }
        if (Input.GetKeyDown("m") && !menuCanvas.activeSelf && this.GetComponent<TransitionScene>().animationFinished)
        {//Open menu panel
            menuCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown("m") && menuCanvas.activeSelf)
        {//Close menu panel
            menuCanvas.SetActive(false);
            Time.timeScale = 1;
        }
        if (Input.GetMouseButtonUp(1) && canRoll)
        {
            canRoll = false;
            //GameObject.FindWithTag("Upgrades").GetComponent<ChestScript>().movePicked = false;
        }
        if (PlayerPrefs.HasKey("NextScene") && PlayerPrefs.GetInt("NextScene") == 0)
        {
            if (this.GetComponent<TransitionScene>().animationFinished)
            {//Show the main scene
                SceneManager.UnloadSceneAsync("BattleScene");
                activateAll();
                canRoll = true;
                playerM.showRoll = true;
                PlayerPrefs.DeleteKey("NextScene");
            }
            else
            {//Start the transition into the scene
                this.GetComponent<TransitionScene>().animateInScene();
            }
        }
        else if (PlayerPrefs.HasKey("NextScene") && PlayerPrefs.GetInt("NextScene") == 3)
        {
            if (this.GetComponent<TransitionScene>().animationFinished)
            {//Show the main scene after losing
                SceneManager.UnloadSceneAsync("BattleScene");                
                PlayerPrefs.DeleteKey("NextScene");
                SceneManager.LoadScene("VictoryScene");
            }
            else
            {//Start the transition into the scene
                this.GetComponent<TransitionScene>().animateInScene();
            }
        }
        else if (PlayerPrefs.HasKey("NextScene") && PlayerPrefs.GetInt("NextScene") == 2)
        {
            if (this.GetComponent<TransitionScene>().animationFinished)
            {//Show the main scene after losing
                SceneManager.UnloadSceneAsync("BattleScene");
                activateAll();
                canRoll = true;
                playerM.endReached = true;
                playerM.defeated = true;
                PlayerPrefs.DeleteKey("NextScene");
                statueBouqet1.SetActive(false);
                statueBouqet2.SetActive(false);
            }
            else
            {//Start the transition into the scene
                this.GetComponent<TransitionScene>().animateInScene();
            }
        }
        if (tileColour.Contains("Green") || tileColour.Contains("Red"))
        {//Start the scene transition animation and open the battle scene
            if (this.GetComponent<TransitionScene>().animationFinished)
            {
                BattleScene();
            }
            else
            {
                this.GetComponent<TransitionScene>().animateInScene();
            }

        }
        else if (tileColour.Contains("Yellow") && chestCanvas.activeSelf == false)
        {//Player can add a new attack to their moveset
         //ChestScene();           
            Invoke("ChestScene", 2f);
        }
        else if (tileColour.Contains("Blue"))
        {//Player can choose between a permament buff, hp/mp restore or a revival charge
            BuffScene();
        }
        else if (tileColour.Contains("Black"))
        {//Start the scene transition animation and open the boss battle scene  
            if (this.GetComponent<TransitionScene>().animationFinished)
            {
                BattleScene();
            }
            else
            {

                this.GetComponent<TransitionScene>().animateInScene();
            }
        }
        else if (tileColour == "Start")
        {//Start the scene transition animation and open the boss battle scene  
            canRoll = true;
        }
    }
    public void UnloadScene()
    {//Unload the additive scene
        PlayerPrefs.SetInt("NextScene", 0);
        SceneManager.UnloadSceneAsync(1);
    }
    public void BattleScene()
    {
        tileColour = "";
        deactivateAll();
        this.GetComponent<TransitionScene>().resetPositions();    
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
    }
    public void ChestScene()
    {
        chestCanvas.SetActive(true);
        tileColour = "";
        PlayerPrefs.DeleteKey("CurrentTile");      
    }
    public void BuffScene()
    {
        if (tileColour.Contains("2"))
        {
            statueBouqet2.SetActive(true);
        }
        else
        {
            statueBouqet1.SetActive(true);
        }
        if (buffCanvas.activeSelf == false)
        {
            buffCanvas.SetActive(true);
            tileColour = "";
            PlayerPrefs.DeleteKey("CurrentTile");
        }
    }
    public void deactivateAll()
    {
        PlayerPrefs.SetInt("NextScene", 1);
        sceneObjects.SetActive(false);
        battleObjects.SetActive(true);
        diceObject = GameObject.Find("d6(Clone)");
        GameObject.Destroy(diceObject);
    }
    public void activateAll()
    {
        sceneObjects.SetActive(true);
        battleObjects.SetActive(false);
        this.GetComponent<TransitionScene>().resetPositions();
        this.GetComponent<TransitionScene>().animateOutScene();
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void deleteAllKeys()
    {
        PlayerPrefs.DeleteAll();
    }
    public void CloseMenu()
    {
        menuCanvas.SetActive(false);
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
