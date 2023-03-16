using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleMenu : MonoBehaviour
{
    public GameObject attackButton, playerAttackButton1, playerAttacksPanel, menuPanel;   
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(attackButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void checkLog()
    {
        Debug.Log("Check pressed");
    }
    public void showPlayerAttacks()
    {
        playerAttacksPanel.SetActive(true);
        menuPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(playerAttackButton1);
    }
    public void showBattleMenu()
    {
        playerAttacksPanel.SetActive(false);
        menuPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(attackButton);
    }
}
