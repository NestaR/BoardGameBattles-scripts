using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool canRoll, mouseEnabled;
    public string tileColour;
    public GameObject player, sceneObjects, battleObjects, diceObject, menuCanvas, chestCanvas, shopCanvas, buffCanvas;
    PlayerMovement playerM;
    void Start()
    {
        playerM = player.GetComponent<PlayerMovement>();
    }
    void Update()
    {      
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
        }
        if(PlayerPrefs.HasKey("NextScene") && PlayerPrefs.GetInt("NextScene") == 0)
        {
            if (this.GetComponent<TransitionScene>().animationFinished)
            {//Show the main scene
                SceneManager.UnloadSceneAsync("BattleScene");
                activateAll();
                canRoll = true;
                PlayerPrefs.DeleteKey("NextScene");
            }
            else
            {//Start the transition into the scene
                this.GetComponent<TransitionScene>().animateInScene();
            }
        }
        else if (PlayerPrefs.HasKey("NextScene") && PlayerPrefs.GetInt("NextScene") == 2)
        {
            if (this.GetComponent<TransitionScene>().animationFinished)
            {//Show the main scene
                SceneManager.UnloadSceneAsync("BattleScene");
                activateAll();
                canRoll = true;
                playerM.endReached = true;
                playerM.defeated = true;
                PlayerPrefs.DeleteKey("NextScene");
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
        else if (tileColour == "Yellow")
        {//Player can add a new attack to their moveset
            ChestScene();
        }
        else if (tileColour == "Blue")
        {//Player can receive a permament buff
            BuffScene();
        }
        else if (tileColour == "Black")
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
        buffCanvas.SetActive(true);
        tileColour = "";
        PlayerPrefs.DeleteKey("CurrentTile");
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
    public void deleteAllKeys()
    {
        PlayerPrefs.DeleteAll();
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
