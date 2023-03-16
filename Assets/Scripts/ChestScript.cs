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
    public GameObject chestCanvas;
    public string optionName;
    public GameObject[] optionObjects;
    public Button[] optionButton;
    public PlayerOptions playerOptions;
    public string[] moveNames, chestSelection;
    int counter, index, pindex;
    // Start is called before the first frame update
    void Start()
    {
        chestSelection = new string[3];
        playerOptions = this.GetComponent<PlayerOptions>();
        GetAllMoves();
        GetNewMoves();
    }
    // Update is called once per frame
    void Update()
    {

        if (chestCanvas.activeSelf == true)
        {
            optionButton[0].GetComponentInChildren<Text>().text = chestSelection[0];
            optionButton[1].GetComponentInChildren<Text>().text = chestSelection[1];
            optionButton[2].GetComponentInChildren<Text>().text = chestSelection[2];
            //playerOptions = this.GetComponent<PlayerOptions>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (playerOptions.attackName[i] == optionButton[j].GetComponentInChildren<Text>().text)
                    {
                        GetNewMoves();
                    }
                }
            }
        }
    }
    public void Option1()
    {
        ApplyMove(optionButton[0].GetComponentInChildren<Text>().text);
        chestCanvas.SetActive(false);
    }
    public void Option2()
    {
        ApplyMove(optionButton[1].GetComponentInChildren<Text>().text);
        chestCanvas.SetActive(false);
    }
    public void Option3()
    {
        ApplyMove(optionButton[2].GetComponentInChildren<Text>().text);
        chestCanvas.SetActive(false);
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
            string path = null;
            foreach (string item in items)
            {
                moveNames[counter] = item;
                counter++;
            }
        }
    }
    public void GetNewMoves()
    {
        Array.Clear(chestSelection, 0, chestSelection.Length);
        GetAllMoves();
        for (int a = 0; a < 3; a++)
        {
            index = Random.Range(0, moveNames.Length);
            pindex = index;
            if (moveNames[index] != "")
            {
                //optionButton[a].GetComponentInChildren<Text>().text = moveNames[index];
                chestSelection[a] = moveNames[index];
                moveNames[index] = "";
            }
        }
    }
    public void ApplyMove(string moveSelected)
    {
        Array.Clear(chestSelection, 0, chestSelection.Length);
        if (playerOptions.attackName[1] == "")
        {
            playerOptions.attackName[1] = moveSelected;
        }
        else if (playerOptions.attackName[2] == "")
        {
            playerOptions.attackName[2] = moveSelected;
        }
        else if (playerOptions.attackName[3] == "")
        {
            playerOptions.attackName[3] = moveSelected;
        }
    }
}
