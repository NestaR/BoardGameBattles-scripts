using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    // Start is called before the first frame update
    void Start()
    {//Get moves from a text file and store them
        chestSelection = new string[3];
        playerOptions = this.GetComponent<PlayerOptions>();
        GetAllMoves();
        GetNewMoves();
    }
    // Update is called once per frame
    void Update()
    {
        if(chestCanvas != null)
        {
            if ((chestCanvas.activeSelf == true || moveReplacePanel.activeSelf == true) && movePicked)
            {
                chestCanvas.SetActive(false);
                moveReplacePanel.SetActive(false);
                Time.timeScale = 1;
            }
            else if (chestCanvas.activeSelf == true)
            {//If the player lands on a chest tile give them a selection of random moves
                Time.timeScale = 0;
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
                    }
                }
            }
            if (moveReplacePanel.activeSelf == true)
            {//If the player wants to replace a move they can
                Time.timeScale = 0;
                optionButton[0].GetComponentInChildren<Text>().text = playerOptions.attackName[1];
                optionButton[1].GetComponentInChildren<Text>().text = playerOptions.attackName[2];
                optionButton[2].GetComponentInChildren<Text>().text = playerOptions.attackName[3];
                replace = true;
            }
        }
    }
    public void Option1()
    {
        if(replace)
        {
            ReplaceMove(optionButton[0].GetComponentInChildren<Text>().text, 1);
        }
        else
        {
            ApplyMove(optionButton[0].GetComponentInChildren<Text>().text);
            movePicked = true;
            chestCanvas.SetActive(false);
        }

    }
    public void Option2()
    {
        if (replace)
        {
            ReplaceMove(optionButton[1].GetComponentInChildren<Text>().text, 2);
        }
        else
        {
            ApplyMove(optionButton[1].GetComponentInChildren<Text>().text);
            movePicked = true;
            chestCanvas.SetActive(false);
        }
    }
    public void Option3()
    {
        if (replace)
        {
            ReplaceMove(optionButton[2].GetComponentInChildren<Text>().text, 3);
        }
        else
        {
            ApplyMove(optionButton[2].GetComponentInChildren<Text>().text);
            movePicked = true;
            chestCanvas.SetActive(false);
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
            // Now let's find the path.
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
            if (moveNames[index] != "")
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
        for(int a = 1; a < 4; a++)
        {
            if (playerOptions.attackName[a] == "")
            {
                playerOptions.attackName[a] = moveSelected;
                break;
            }
            if (playerOptions.attackName[a] != "")
            {
                moveCounter += 1;
            }
        }
        if(moveCounter == 3)
        {//If the moveset is full ask the player if they want to replace a move
            moveReplacePanel.SetActive(true);
        }
        Time.timeScale = 1;
    }

    public void ReplaceMove(string moveSelected, int a)
    {
        movePicked = true;
        playerOptions.attackName[a] = moveSelected;
        Time.timeScale = 1;
        ReplaceExit();
    }
    public void ReplaceExit()
    {//Close chest canvas
        moveReplacePanel.SetActive(false);
        chestCanvas.SetActive(false);
        replace = false;
    }
}
