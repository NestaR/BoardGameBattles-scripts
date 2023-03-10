using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDiceRoll : MonoBehaviour
{
    public int MoveAmount;
    public int checkRolling;
    private const string PlayerDiceRoll = "PlayerDiceRoll", FinishedRolling = "FinishedRolling";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkRolling = PlayerPrefs.GetInt(FinishedRolling);

        if ((Input.GetButtonUp("Fire1") || Input.GetMouseButtonUp(0)) && checkRolling == 1)
        {
            MoveAmount = PlayerPrefs.GetInt(PlayerDiceRoll);
            Debug.Log("You rolled a " + MoveAmount);
            PlayerPrefs.SetInt(FinishedRolling, 2);
        }
        if ((Input.GetMouseButtonUp(1)) && checkRolling >= 1)
        {
            PlayerPrefs.DeleteAll();
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
}
