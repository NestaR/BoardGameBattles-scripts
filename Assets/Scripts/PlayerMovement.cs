using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int numTiles, counter = 0, moveAmount, currentTileNumber = 0, flip1, flip2;
    public GameObject player, gameManager, victoryCanvas;
    public GameObject[] tilePositions;
    public string currentTileColour;
    public int[] destination;
    public Vector3 startPos, nextPos;
    public bool movingPlayer, finishedMoving, endReached, defeated, victory;
    public float moveTimer;
    bool flipped;
    private const string PlayerDiceRoll = "PlayerDiceRoll", FinishedRolling = "FinishedRolling";
    Animator playerAnimator;
    void Start()
    {
        PlayerPrefs.DeleteKey(PlayerDiceRoll);
        startPos = transform.position;
        tilePositions = new GameObject[numTiles];
        for (int i = 0; i < numTiles; i++)
        {//Store the position of all the tiles in an array for traversing the map
            tilePositions[i] = GameObject.Find("Tile (" + i + ")");
        }
        playerAnimator = this.transform.GetChild(1).GetComponent<Animator>();
    }


    void Update()
    {
        if(PlayerPrefs.HasKey(PlayerDiceRoll) && !movingPlayer)
        {
            moveAmount = PlayerPrefs.GetInt(PlayerDiceRoll);
            //Move player by amount rolled
            FindNextPosition();
            counter += 1;
        }

        if(endReached && defeated)
        {//Being defeated brings you back to the start
            playerLost();
        }
        else if(endReached && victory)
        {
            playerWin();
        }
        else if(defeated)
        {
            playerLost();       
        }
        if (Vector3.Distance(player.transform.position, nextPos) < 0.001f)
        {         
            movingPlayer = false;
            if (counter == moveAmount)
            {//Stop moving when reached the correct amount
                finishedMoving = true;
                playerAnimator.SetBool("Running", false);
                PlayerPrefs.DeleteKey(PlayerDiceRoll);
                PlayerPrefs.SetString("CurrentTile", currentTileColour);
                counter = 0;
                gameManager.GetComponent<GameManager>().canRoll = true;
                gameManager.GetComponent<GameManager>().tileColour = currentTileColour;
                gameManager.GetComponent<GameManager>().GetComponent<TransitionScene>().resetPositions();
            }
        }
        else if (movingPlayer)
        {
            //player.transform.position = Vector3.Lerp(player.transform.position, nextPos, 0.1f);
            player.transform.position = Vector3.MoveTowards(player.transform.position, nextPos, Time.deltaTime * 3f);
            playerAnimator.SetBool("Running", true);
        }
        if(currentTileNumber >= (numTiles - 1))
        {
            gameManager.GetComponent<GameManager>().canRoll = false;
        }
        if (currentTileNumber == flip1 && !flipped && !movingPlayer)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            flipped = true;
        }
        else if (currentTileNumber == flip2 && !flipped && !movingPlayer)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            flipped = true;
        }
        if (player.transform.position == startPos)
        {
            defeated = false;
            endReached = false;
            //gameManager.GetComponent<GameManager>().canRoll = true;
        }
    }
    void FindNextPosition()
    {//Move the player tile by tile by the amount rolled on the dice
        PlayerPrefs.DeleteKey("CurrentTile");
        movingPlayer = true;
        finishedMoving = false;
        
        if (currentTileNumber >= (numTiles - 1))
        {//If the player rolls past the last tile stop them at the end
            nextPos = new Vector3(tilePositions[numTiles - 1].transform.position.x, 0.3864222f, tilePositions[numTiles - 1].transform.position.z);
            endReached = true;
            gameManager.GetComponent<GameManager>().canRoll = false;
            //0.6135788
        }
        else
        {
            currentTileNumber += 1;
            nextPos = new Vector3(tilePositions[currentTileNumber].transform.position.x, 0.3864222f, tilePositions[currentTileNumber].transform.position.z);
            endReached = false;
            flipped = false;
        }
    }

    public void playerLost()
    {
        player.transform.position = startPos;
        currentTileNumber = 0;
        PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("MaxHealthRating"));
        gameManager.GetComponent<GameManager>().canRoll = true;
    }
    public void playerWin()
    {
        Debug.Log("PLAYER VICTORY!");
        player.transform.position = GameObject.Find("Tile (End)").transform.position;
        currentTileNumber = 0;
        victoryCanvas.SetActive(true);
        //PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("MaxHealthRating"));
        //gameManager.GetComponent<GameManager>().canRoll = true;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Flip")
        {
            //Vector3 newScale = transform.localScale;
            //newScale.x *= -1;
            //transform.localScale = newScale;
            //Debug.Log("Flipped");
        }
        else if (collision.gameObject.tag != "Plane")
        {
            currentTileColour = collision.gameObject.tag;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        //currentTileColour = "";
    }
    public void SetInt(string KeyName, int Value)
    {
        PlayerPrefs.SetInt(KeyName, Value);
    }
    public int GetInt(string KeyName)
    {
        return PlayerPrefs.GetInt(KeyName);
    }
    public void SetString(string KeyName, string Value)
    {
        PlayerPrefs.SetString(KeyName, Value);
    }
    public string GetString(string KeyName)
    {
        return PlayerPrefs.GetString(KeyName);
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
