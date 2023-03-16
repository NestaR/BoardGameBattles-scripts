using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOptions : MonoBehaviour
{
    public string[] attackName;
    public GameObject attackCanvas;
    public GameObject[] getButtons;
    public GameObject[] getButtonsText;
    // Start is called before the first frame update
    void Start()
    {
        attackName = new string[4];
        getButtons = new GameObject[4];
        getButtonsText = new GameObject[4];
    }

    // Update is called once per frame
    void Update()
    {
        if (attackName[0] == null)
        {
            attackName[0] = "Simple Strike";
            attackName[1] = "";
            attackName[2] = "";
            attackName[3] = "";
        }
        //GameObject[] getButtons;
        //GameObject[] getButtonsText;
        //if(getButtons == null)
        //{
        //getButtons = GameObject.FindGameObjectsWithTag("AttackButtons");
        //getButtonsText = GameObject.FindGameObjectsWithTag("AttackText");
        attackCanvas = GameObject.Find("AttackBG");
        //}
        //else
        //{
        if(attackCanvas != null)
        {
            if (attackCanvas.activeSelf == true)
            {
                getButtons = GameObject.FindGameObjectsWithTag("AttackButtons");
                getButtonsText = GameObject.FindGameObjectsWithTag("AttackText");
                for (int i = 0; i < 4; i++)
                {
                    if (attackName[i] != "")
                    {
                        getButtons[i].GetComponent<Button>().interactable = true;
                        getButtonsText[i].GetComponent<Text>().text = attackName[i];
                    }
                    else
                    {
                        getButtons[i].GetComponent<Button>().interactable = false;
                        getButtonsText[i].GetComponent<Text>().text = "";
                    }
                }
            }
        }


       //}
    }
}
