using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField] private Animator myChest1, myChest2;
    [SerializeField] private string openChest = "Open", closeChest = "Close";
    public bool chestOpened1, chestOpened2;
    void Update()
    {
        if(PlayerPrefs.GetString("CurrentTile").Contains("Yellow1") && !chestOpened1)
        {
            myChest1.Play(openChest, 0, 0.0f);
            chestOpened1 = true;
        }
        else if (PlayerPrefs.GetString("CurrentTile").Contains("Yellow2") && !chestOpened2)
        {
            myChest2.Play(openChest, 0, 0.0f);
            chestOpened2 = true;
        }
    }
    public void chestAnimation()
    {
        myChest1.Play(openChest, 0, 0.0f);
        chestOpened1 = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && chestOpened1)
        {
            myChest1.Play(closeChest, 0, 0.0f);
            chestOpened1 = false;
        }
        else if (other.CompareTag("Player") && chestOpened2)
        {
            myChest2.Play(closeChest, 0, 0.0f);
            chestOpened2 = false;
        }
    }
}
