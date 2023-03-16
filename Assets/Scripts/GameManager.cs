using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool canRoll, mouseEnabled;
    public string tileColour;
    public GameObject sceneObjects, battleObjects, diceObject, menuCanvas, chestCanvas, shopCanvas, buffCanvas;
    PlayerMovement playerM;
    void Start()
    {
        //PlayerPrefs.DeleteKey("CurrentTile");
        //tileColour = "";
    }
    void Update()
    {
        
        if (Input.GetKeyDown("m") && !menuCanvas.activeSelf)
        {//Open menu panel
            menuCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown("m") && menuCanvas.activeSelf)
        {
            menuCanvas.SetActive(false);
            Time.timeScale = 1;
        }
        if (Input.GetMouseButtonUp(1))
        {
            if(canRoll)
            {
                canRoll = false;
            }
        }
        if(PlayerPrefs.HasKey("NextScene") && PlayerPrefs.GetInt("NextScene") == 0)
        {
            activateAll();
            canRoll = true;
            PlayerPrefs.DeleteKey("NextScene");
        }
        //if(PlayerPrefs.HasKey("CurrentTile"))
        //{
        //    tileColour = PlayerPrefs.GetString("CurrentTile");
        //}
        //else
        //{
        //    tileColour = "";
        //}
        //switch (tileColour)
        //{
        //    case "Green":
        //        BattleScene();
        //        break;
        //    case "Red":
        //        BattleScene();
        //        break;
        //    case "Yellow":
        //        ChestScene();
        //        break;
        //    case "Blue":
        //        BuffScene();
        //        break;
        //    case "Orange":
        //        break;
        //    default:
        //        PlayerPrefs.DeleteKey("CurrentTile");
        //        break;
        //}
        if (tileColour == "Green")
        {
            BattleScene();
        }
        else if (tileColour == "Red")
        {
            BattleScene();
        }
        else if (tileColour == "Yellow")
        {
            ChestScene();
        }
        else if (tileColour == "Blue")
        {
            BuffScene();
        }
        else if (tileColour == "Black")
        {
            BattleScene();
        }
    }
    public void UnloadScene()
    {
        //diceObject = GameObject.Find("AllObjects");
        //activateAll();
        PlayerPrefs.SetInt("NextScene", 0);
        SceneManager.UnloadSceneAsync(1);
    }
    public void BattleScene()
    {
        deactivateAll();
        tileColour = "";
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
    }
    public void OtherScene()
    {
        //canRoll = true;
        diceObject = GameObject.Find("d6(Clone)");
        GameObject.Destroy(diceObject);
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
