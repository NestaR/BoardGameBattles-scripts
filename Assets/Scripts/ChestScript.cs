using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChestScript : MonoBehaviour
{
    public TextAsset figtherMoves, magicMoves;
    public GameObject chestCanvas, moveReplacePanel;
    public string optionName;
    public GameObject[] optionObjects;
    public Button[] optionButton, replaceOptions;
    public PlayerOptions playerOptions;
    public string[] moveNames, chestSelection;
    int counter, index, pindex;
    public bool replace, movePicked;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {//Get moves from a text file and store them
        chestSelection = new string[3];
        playerOptions = this.GetComponent<PlayerOptions>();
        GetAllMoves();
        GetNewMoves();
        player = GameObject.FindWithTag("Player");
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if(chestCanvas != null)
        {
            moveNames = moveNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if ((chestCanvas.activeSelf == true || moveReplacePanel.activeSelf == true) && movePicked)
            {//Close the chest options
                movePicked = false;
                chestCanvas.SetActive(false);
                moveReplacePanel.SetActive(false);
                player.GetComponent<PlayerMovement>().movingPlayer = true;
                player.GetComponent<PlayerMovement>().stopped = false;
            }
            else if (chestCanvas.activeSelf == true)
            {//If the player lands on a chest tile give them a selection of 3 random moves
                //Time.timeScale = 0;
                player.GetComponent<PlayerMovement>().movingPlayer = false;
                player.GetComponent<PlayerMovement>().stopped = true;
                //currentPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                //player.transform.position = currentPos;
                optionButton[0].GetComponentInChildren<Text>().text = chestSelection[0];
                optionButton[1].GetComponentInChildren<Text>().text = chestSelection[1];
                optionButton[2].GetComponentInChildren<Text>().text = chestSelection[2];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (playerOptions.attackName[i] == optionButton[j].GetComponentInChildren<Text>().text)
                        {//If the player already has one of the moves they get replaced
                            GetNewMoves();
                        }
                        else if (optionButton[j].GetComponentInChildren<Text>().text == "" || optionButton[j].GetComponentInChildren<Text>().text == null)
                        {//If the player already has one of the moves they get replaced
                            GetNewMoves();
                        }
                    }
                }
            }
            if (moveReplacePanel.activeSelf == true)
            {//If the player wants to replace a move they can
                optionButton[0].GetComponentInChildren<Text>().text = playerOptions.attackName[1];
                optionButton[1].GetComponentInChildren<Text>().text = playerOptions.attackName[2];
                optionButton[2].GetComponentInChildren<Text>().text = playerOptions.attackName[3];
                replace = true;
                player.GetComponent<PlayerMovement>().movingPlayer = false;
                player.GetComponent<PlayerMovement>().stopped = true;
            }
        }
    }
    public void Option1()
    {
        if(replace)
        {
            ReplaceMove(optionName, 1);
        }
        else
        {
            ApplyMove(optionButton[0].GetComponentInChildren<Text>().text);
        }

    }
    public void Option2()
    {
        if (replace)
        {
            ReplaceMove(optionName, 2);
        }
        else
        {
            ApplyMove(optionButton[1].GetComponentInChildren<Text>().text);
        }
    }
    public void Option3()
    {
        if (replace)
        {
            ReplaceMove(optionName, 3);
        }
        else
        {
            ApplyMove(optionButton[2].GetComponentInChildren<Text>().text);
        }
    }
    public void GetAllMoves()
    {
        counter = 0;
        StreamReader fighter = File.OpenText("Assets/Resources/FighterBattleMoves.txt");
        string line;
        while ((line = fighter.ReadLine()) != null)
        {
            string[] items = line.Split(',');
            moveNames = new string[items.Length];
            //string path = null;
            foreach (string item in items)
            {
                moveNames[counter] = item;
                counter++;
            }
        }
    }
    public void GetNewMoves()
    {//Reroll the move selection
        Array.Clear(chestSelection, 0, chestSelection.Length);
        GetAllMoves();
        for (int a = 0; a < 3; a++)
        {
            index = Random.Range(0, moveNames.Length);
            pindex = index;
            if (moveNames[index] != "" && moveNames[index] != null)
            {
                chestSelection[a] = moveNames[index];
                moveNames[index] = "";              
            }
        }
    }
    public void ApplyMove(string moveSelected)
    {//Add the selected move to the players moveset
        Array.Clear(chestSelection, 0, chestSelection.Length);
        int moveCounter = 0;

        for (int a = 1; a < 4; a++)
        {
            if (playerOptions.attackName[3] != "")
            {//If the moveset is full ask the player if they want to replace a move
                optionName = moveSelected;
                moveReplacePanel.SetActive(true);
                break;
            }
            if (playerOptions.attackName[a] == "")
            {
                playerOptions.attackName[a] = moveSelected;
                movePicked = true;
                break;
            }
        }
        //Time.timeScale = 1;    
    }

    public void ReplaceMove(string moveSelected, int a)
    {
        movePicked = true;
        playerOptions.attackName[a] = moveSelected;
        replace = false;
    }
    public void ReplaceExit()
    {//Close chest canvas
        moveReplacePanel.SetActive(false);
        chestCanvas.SetActive(false);
        replace = false;
        player.GetComponent<PlayerMovement>().movingPlayer = true;
        player.GetComponent<PlayerMovement>().stopped = false;
    }
}
