using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public int numTiles, counter = 0, moveAmount, currentTileNumber = 0, flip1, flip2, flip3, rotate1, rotate2, rotate3;
    public GameObject player, gameManager, victoryCanvas, cameraRollPosition, cameraPlayerPosition;
    public GameObject[] tilePositions;
    public string currentTileColour;
    public int[] destination;
    public Vector3 startPos, nextPos, cameraPos;
    public bool movingPlayer, finishedMoving, endReached, defeated, victory, showRoll;
    public float moveTimer;
    bool flipped1, flipped2, flipped3;
    float timer;
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
        cameraPos = mainCamera.transform.position;
    }

    void Awake()
    {

    }
    void Update()
    {
        Invoke("flipPlayer", 1f);

        if(showRoll)
        {
            Vector3 newRotation = new Vector3(16, 0, 0);
            if (Vector3.Distance(mainCamera.transform.position, cameraRollPosition.transform.position) > 0.001f)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraRollPosition.transform.position, Time.deltaTime * 0.5f);
                mainCamera.transform.eulerAngles = Vector3.Lerp(mainCamera.transform.eulerAngles, newRotation, 0.01f);
            }

            mainCamera.transform.eulerAngles = newRotation;
        }
        if (PlayerPrefs.HasKey(PlayerDiceRoll) && !movingPlayer)
        {
            showRoll = false;
            moveAmount = PlayerPrefs.GetInt(PlayerDiceRoll);
            //Move player by amount rolled
            FindNextPosition();
            counter += 1;
        }
        else if (PlayerPrefs.HasKey(PlayerDiceRoll))
        {
            if (Vector3.Distance(mainCamera.transform.position, cameraPlayerPosition.transform.position) > 0.001f)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPlayerPosition.transform.position, 0.1f);
                mainCamera.transform.eulerAngles = Vector3.Lerp(mainCamera.transform.eulerAngles, transform.eulerAngles, 0.1f);
            }
            if(mainCamera.transform.position.y < 0.9f)
            {
                Vector3 clamp = new Vector3(mainCamera.transform.position.x, 0.9f, mainCamera.transform.position.z);
                mainCamera.transform.position = clamp;
            }
        }
        if (endReached && defeated)
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
                
                playerAnimator.SetBool("Running", false);
                PlayerPrefs.DeleteKey(PlayerDiceRoll);
                setTileColour();
            }
        }
        else if (movingPlayer)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, nextPos, Time.deltaTime * 3f);
            playerAnimator.SetBool("Running", true);
        }
        if(currentTileNumber >= (numTiles - 1))
        {
            gameManager.GetComponent<GameManager>().canRoll = false;
        }
        
        if (player.transform.position == startPos)
        {
            defeated = false;
            endReached = false;
        }
    }
    void flipPlayer()
    {
        if (currentTileNumber >= flip3)
        {
            Vector3 newRotation = new Vector3(0, rotate3, 0);
            transform.eulerAngles = newRotation;
            flipped1 = false;
            flipped2 = false;
            flipped3 = true;
        }
        else if (currentTileNumber >= flip2)
        {
            Vector3 newRotation = new Vector3(0, rotate2, 0);
            transform.eulerAngles = newRotation;
            flipped1 = false;
            flipped2 = true;
            flipped3 = false;
        }
        else if (currentTileNumber >= flip1)
        {
            Vector3 newRotation = new Vector3(0, rotate1, 0);
            transform.eulerAngles = newRotation;
            if(!flipped1)
            {
                Vector3 newScale = transform.localScale;
                newScale.x *= -1;
                transform.localScale = newScale;
            }
            flipped1 = true;
            flipped2 = false;
            flipped3 = false;
        }
        else
        {
            flipped1 = false;
            flipped2 = false;
            flipped3 = false;
        }
    }
    void flipCamera()
    {
        if(flipped1)
        {
            Vector3 newRotation = new Vector3(0, rotate1, 0);
            mainCamera.transform.eulerAngles = newRotation;
        }
        else if (flipped2)
        {
            Vector3 newRotation = new Vector3(0, rotate2, 0);
            mainCamera.transform.eulerAngles = newRotation;
        }
        else if (flipped3)
        {
            Vector3 newRotation = new Vector3(0, rotate3, 0);
            mainCamera.transform.eulerAngles = newRotation;
        }
        else
        {
            Vector3 newRotation = new Vector3(0, 0, 0);
            mainCamera.transform.eulerAngles = newRotation;
        }
    }
    void setTileColour()
    {      
        finishedMoving = true;
        gameManager.GetComponent<GameManager>().canRoll = true;
        PlayerPrefs.SetString("CurrentTile", currentTileColour);
        gameManager.GetComponent<GameManager>().tileColour = currentTileColour;
        gameManager.GetComponent<GameManager>().GetComponent<TransitionScene>().resetPositions();
        counter = 0;
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
        }
    }

    public void playerLost()
    {
        player.transform.position = startPos;
        currentTileNumber = 0;
        PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("MaxHealthRating"));
        gameManager.GetComponent<GameManager>().canRoll = true;

        if(player.transform.eulerAngles.y != 0)
        {//Fix the players rotation on respawn
            Vector3 newRotation = new Vector3(0, 0, 0);
            transform.eulerAngles = newRotation;
            mainCamera.transform.eulerAngles = newRotation;
        }
    }
    public void playerWin()
    {
        Debug.Log("PLAYER VICTORY!");
        player.transform.position = GameObject.Find("Tile (End)").transform.position;
        currentTileNumber = 0;
        victoryCanvas.SetActive(true);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Plane")
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
