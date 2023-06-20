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
    public GameObject player, sceneObjects, battleObjects, diceObject, menuCanvas, chestCanvas, helpCanvas, buffCanvas, statueBouqet1, statueBouqet2;
    public Text map1Wins, map2Wins, map3Wins, enemiesDefeated;
    public int m1wins, m2wins, m3wins;

    public GameObject characterSelect, mapSelect;
    public string characterSelected, mapSelected;
    bool charChosen;
    public Button nextButton, beginButton;
    string sceneName;
    public Button map3, char3;
    public Image mapImage3, playerImage3;
    PlayerMovement playerM;
    SoundScript sound;
    AudioSource myAudio;
    void Start()
    {
        sound = this.GetComponent<SoundScript>();
        myAudio = GetComponent<AudioSource>();
        if (player != null)
        {
            playerM = player.GetComponent<PlayerMovement>();
        }
        Scene scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
        if (sceneName == "StartScene")
        {
            deleteAllKeys();
        }
        //PlayerPrefs.SetString("MapWins", "");
        foreach (var ch in PlayerPrefs.GetString("MapWins"))
        {//Show each maps wins
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
        if(m1wins >= 1 && m2wins >= 1 && sceneName == "StartScene")
        {//Unlock a new map and character after finishing the first 2 maps
            map3.interactable = true;
            char3.interactable = true;
            mapImage3.color = Color.white;
            playerImage3.color = Color.white;
        }
        if (playerM != null)
        {
            playerM.showRoll = true;
        }     
    }
    void Update()
    {
        if (mapSelect != null)
        {
            if (mapSelected == "")
            {
                nextButton.enabled = false;
            }
            else
            {
                nextButton.enabled = true;
            }
        }
        if (characterSelect != null)
        {
            if (characterSelected == "")
            {
                beginButton.enabled = false;
            }
            else
            {
                beginButton.enabled = true;
            }
        }

        if (player == null && sceneName == "StartScene")
        {           
            map1Wins.text = m1wins.ToString();
            map2Wins.text = m2wins.ToString();
            map3Wins.text = m3wins.ToString();
        }
        if (Input.GetKeyDown("m") && !menuCanvas.activeSelf && this.GetComponent<TransitionScene>().animationFinished)
        {//Open menu panel
            ShowMenu();
        }
        else if (Input.GetKeyDown("m") && menuCanvas.activeSelf)
        {//Close menu panel
            CloseMenu();
        }
        if (Input.GetMouseButtonUp(1) && canRoll)
        {
            sound.playClip("diceRoll");
            canRoll = false;
            //GameObject.FindWithTag("Upgrades").GetComponent<ChestScript>().movePicked = false;
        }
        if (PlayerPrefs.HasKey("NextScene") && PlayerPrefs.GetInt("NextScene") == 0)
        {
            if (this.GetComponent<TransitionScene>().animationFinished)
            {//Show the map after winning a battle
                SceneManager.UnloadSceneAsync("BattleScene");
                activateAll();
                myAudio.Play();
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
            {//Show the victory scene after defeating the boss
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
            Invoke("ChestScene", 1f);
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
    public void LoadMainScene()
    {
        PlayerPrefs.SetString("MapSelected", mapSelected);
        PlayerPrefs.SetString("CharacterSelected", characterSelected);
        StartCoroutine(LoadYourAsyncScene());
    }
    public void MapSelectCanvas()
    {
        mapSelect.SetActive(true);
    }
    public void CharacterSelectCanvas()
    {
        characterSelect.SetActive(true);
    }
    public void QuitGame()
    {//Close application
        Application.Quit();
    }
    public void CloseCharacterSelect()
    {
        characterSelected = "";
        characterSelect.SetActive(false);
    }
    public void CloseMapSelect()
    {
        mapSelected = "";
        mapSelect.SetActive(false);
    }
    public void Character1()
    {
        characterSelected = "HeroKnight1";
    }
    public void Character2()
    {
        characterSelected = "HeroKnight2";
    }
    public void Character3()
    {
        characterSelected = "MartialHero";
    }
    public void Character4()
    {
        characterSelected = "MedievalWarrior1";
    }
    public void Character5()
    {
        characterSelected = "MedievalWarrior2";
    }
    public void Map1()
    {
        mapSelected = "Map1";
    }
    public void Map2()
    {
        mapSelected = "Map2";
    }
    public void Map3()
    {
        mapSelected = "Map3";
    }
    public void UnloadScene()
    {//Unload the additive scene
        PlayerPrefs.SetInt("NextScene", 0);
        SceneManager.UnloadSceneAsync(1);
    }
    public void BattleScene()
    {
        myAudio.Stop();
        sound.playClip("encounter");        
        tileColour = "";
        deactivateAll();
        this.GetComponent<TransitionScene>().resetPositions();    
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
    }
    public void ChestScene()
    {
        sound.playClip("chestOpen");
        chestCanvas.SetActive(true);
        tileColour = "";
        PlayerPrefs.DeleteKey("CurrentTile");      
    }
    public void BuffScene()
    {
        sound.playClip("shrine");
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
    {//Return to the starting scene
        SceneManager.LoadScene("StartScene");
    }
    public void deleteAllKeys()
    {
        string mapW = PlayerPrefs.GetString("MapWins");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("MapWins", mapW);
    }
    public void showRoll()
    {
        playerM.showRoll = true;
    }
    public void ShowMenu()
    {
        menuCanvas.SetActive(true);
        sound.playClip("menuPause");
        Time.timeScale = 0;
        enemiesDefeated.text = PlayerPrefs.GetInt("EnemiesDefeated").ToString();
    }
    public void CloseMenu()
    {
        menuCanvas.SetActive(false);
        sound.playClip("menuUnpause");
        Time.timeScale = 1;
    }
    public void ShowHelp()
    {
        if(!helpCanvas.activeSelf)
        {
            helpCanvas.SetActive(true);
            sound.playClip("menuPause");
            Time.timeScale = 0;
        }
        else
        {
            helpCanvas.SetActive(false);
            sound.playClip("menuUnpause");
            Time.timeScale = 1;
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

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mapSelected);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
