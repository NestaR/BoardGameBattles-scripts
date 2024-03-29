using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    public GameObject dice, fixedPosObj;
    public bool fixDice;
    Vector3 fixedPosition;
    void Start()
    {
        fixedPosition = fixedPosObj.transform.position;
    }
    void Update()
    {
        if(fixDice)
        {//If the dice gets stuck it gets reset
            dice = GameObject.Find("d6(Clone)");
            dice.transform.position = Vector3.Lerp(dice.transform.position, fixedPosition, 1f);
            fixDice = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dice"))
        {//Reposition the dice
            Debug.Log("Fix Dice");
            fixDice = true;
        }
    }
}
