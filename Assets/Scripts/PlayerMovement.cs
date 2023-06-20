using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public int numTiles, counter = 0, moveAmount, currentTileNumber = 0, flip1, flip2, flip3, rotate1, rotate2, rotate3;
    public GameObject player, gameManager, victoryCanvas, cameraRollPosition, cameraPlayerPosition, directionCanvas, chestAnim;
    public GameObject[] tilePositions;
    public string currentTileColour, movementDirection;
    public int[] destination;
    public Vector3 startPos, nextPos, cameraPos;
    public bool movingPlayer, finishedMoving, stopped, endReached, defeated, victory, showRoll;
    public float moveTimer;
    bool flipped1, flipped2, flipped3;
    float timer;
    public Text rollNum;
    private const string PlayerDiceRoll = "PlayerDiceRoll", FinishedRolling = "FinishedRolling";
    Animator playerAnimator;
    SoundScript soundScript;
    void Start()
    {
        PlayerPrefs.DeleteKey(PlayerDiceRoll);
        startPos = transform.position;
        soundScript = gameManager.GetComponent<SoundScript>();
        tilePositions = new GameObject[numTiles];
        for (int i = 0; i < numTiles; i++)
        {//Store the position of all the tiles in an array for traversing the map
            tilePositions[i] = GameObject.Find("Tile (" + i + ")");
        }

        playerAnimator = this.transform.GetChild(1).GetComponent<Animator>();
        cameraPos = mainCamera.transform.position;
    }

    void Update()
    {
        Invoke("flipPlayer", 1f);

        if (showRoll)
        {//Move camera to dice roll position
            rollNum.text = "";
            Vector3 newRotation = new Vector3(16, 0, 0);
            if (Vector3.Distance(mainCamera.transform.position, cameraRollPosition.transform.position) > 0.001f)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraRollPosition.transform.position, Time.deltaTime * 0.3f);
                mainCamera.transform.eulerAngles = Vector3.Lerp(mainCamera.transform.eulerAngles, newRotation, 0.01f);
            }

            mainCamera.transform.eulerAngles = newRotation;
        }
        if (PlayerPrefs.HasKey(PlayerDiceRoll) && !showRoll)
        {//Move camera to players position when they move
            rollNum.text = "You rolled a: " + PlayerPrefs.GetInt(PlayerDiceRoll);
            if (Vector3.Distance(mainCamera.transform.position, cameraPlayerPosition.transform.position) > 0.001f)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPlayerPosition.transform.position, 0.3f);
                mainCamera.transform.eulerAngles = Vector3.Lerp(mainCamera.transform.eulerAngles, transform.eulerAngles, 0.3f);
            }
            if (mainCamera.transform.position.y < 0.9f)
            {
                Vector3 clamp = new Vector3(mainCamera.transform.position.x, 0.9f, mainCamera.transform.position.z);
                mainCamera.transform.position = clamp;
            }
        }
        if (PlayerPrefs.HasKey(PlayerDiceRoll) && !movingPlayer)
        {
            showRoll = false;
            moveAmount = PlayerPrefs.GetInt(PlayerDiceRoll);
            //Move player by amount rolled
            if (PlayerPrefs.GetString("MapSelected").Contains("2") && movementDirection == "")
            {
                showDirectionOption();
            }
            else if(stopped)
            {
                playerAnimator.SetBool("Running", false);
            }
            else
            {
                FindNextPosition();
                counter += 1;
            }

        }    
        if (endReached && defeated)
        {//Being defeated brings you back to the start
            playerLost();
        }
        else if (endReached && victory)
        {//Defeating the boss shows victory screen
            playerWin();
        }
        else if (defeated)
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
                GameObject.FindWithTag("Upgrades").GetComponent<ChestScript>().movePicked = false;
                setTileColour();
            }
        }
        else if (movingPlayer)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, nextPos, Time.deltaTime * 3f);
            playerAnimator.SetBool("Running", true);
        }
        if (currentTileNumber >= (numTiles - 1))
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
    {//Determine which way the player should rotate depending on the position
        if (currentTileNumber >= flip3)
        {
            Vector3 newRotation = new Vector3(0, rotate3, 0);
            transform.eulerAngles = newRotation;
            flipped1 = false;
            flipped2 = false;
            flipped3 = true;
        }
        else if (currentTileNumber == flip2)
        {
            Vector3 newRotation = new Vector3(0, rotate2, 0);
            transform.eulerAngles = newRotation;
            if(movementDirection == "Right" && !flipped2)
            {
                Vector3 newScale = transform.localScale;
                newScale.x *= -1;
                transform.localScale = newScale;
            }
            flipped1 = false;
            flipped2 = true;
            flipped3 = false;
        }
        else if (currentTileNumber == flip1)
        {
            Vector3 newRotation = new Vector3(0, rotate1, 0);
            transform.eulerAngles = newRotation;
            if (!flipped1)
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
    {//Determine which way the camera should rotate depending on the position
        if (flipped1)
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
    {//Check which tile the player landed on
        finishedMoving = true;
        gameManager.GetComponent<GameManager>().canRoll = true;
        PlayerPrefs.SetString("CurrentTile", currentTileColour);
        gameManager.GetComponent<GameManager>().tileColour = currentTileColour;
        gameManager.GetComponent<GameManager>().GetComponent<TransitionScene>().resetPositions();
        counter = 0;
    }
    void showDirectionOption()
    {
        directionCanvas.SetActive(true);
    }
    public void pickDirectionLeft()
    {
        movementDirection = "Left";
        directionCanvas.SetActive(false);
        Vector3 newRotation = new Vector3(0, 45, 0);
        transform.eulerAngles = newRotation;
        Vector3 newRotationC = new Vector3(0, 45, 0);
        mainCamera.transform.eulerAngles = newRotationC;
        chestAnim.GetComponent<TriggerAnimation>().chestAnimation();
        soundScript.playClip("chestOpen");
    }
    public void pickDirectionRight()
    {
        movementDirection = "Right";
        currentTileNumber = numTiles;
        directionCanvas.SetActive(false);
        Vector3 newRotation = new Vector3(0, -45, 0);
        transform.eulerAngles = newRotation;
        Vector3 newRotationC = new Vector3(0, -45, 0);
        mainCamera.transform.eulerAngles = newRotationC;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        soundScript.playClip("shrine");
    }
    void FindNextPosition()
    {//Move the player tile by tile by the amount rolled on the dice
        PlayerPrefs.DeleteKey("CurrentTile");
        movingPlayer = true;
        finishedMoving = false;
        if (PlayerPrefs.GetString("MapSelected").Contains("1"))
        {
            if (currentTileNumber >= (numTiles - 1))
            {//If the player rolls past the last tile stop them at the end
                nextPos = new Vector3(tilePositions[numTiles - 1].transform.position.x, 0.3864222f, tilePositions[numTiles - 1].transform.position.z);
                endReached = true;
                gameManager.GetComponent<GameManager>().canRoll = false;
            }
            else
            {
                currentTileNumber += 1;
                nextPos = new Vector3(tilePositions[currentTileNumber].transform.position.x, 0.3864222f, tilePositions[currentTileNumber].transform.position.z);
                endReached = false;
            }
        }
        else if (PlayerPrefs.GetString("MapSelected").Contains("2"))
        {
            if (currentTileNumber == (8))
            {//If the player rolls past the last tile stop them at the end
                nextPos = new Vector3(tilePositions[8].transform.position.x, 0.3864222f, tilePositions[8].transform.position.z);
                endReached = true;
                gameManager.GetComponent<GameManager>().canRoll = false;
            }
            else if(movementDirection.Contains("Left"))
            {
                currentTileNumber += 1;
                nextPos = new Vector3(tilePositions[currentTileNumber].transform.position.x, 0.3864222f, tilePositions[currentTileNumber].transform.position.z);
                endReached = false;
            }
            else if (movementDirection.Contains("Right"))
            {
                currentTileNumber -= 1;
                nextPos = new Vector3(tilePositions[currentTileNumber].transform.position.x, 0.3864222f, tilePositions[currentTileNumber].transform.position.z);
                endReached = false;
            }
        }
        else if (PlayerPrefs.GetString("MapSelected").Contains("3"))
        {
            movementDirection = "Right";
            if (currentTileNumber >= (numTiles - 1))
            {//If the player rolls past the last tile stop them at the end
                nextPos = new Vector3(tilePositions[numTiles - 1].transform.position.x, 0.3864222f, tilePositions[numTiles - 1].transform.position.z);
                endReached = true;
                gameManager.GetComponent<GameManager>().canRoll = false;
            }
            else if (currentTileNumber == (7))
            {//Trigger buff canvas
                nextPos = new Vector3(tilePositions[8].transform.position.x, 0.3864222f, tilePositions[8].transform.position.z);
                counter -= 1;
                endReached = false;
                currentTileNumber += 1;
            }
            else if (currentTileNumber == (14))
            {//Trigger buff canvas
                nextPos = new Vector3(tilePositions[15].transform.position.x, 0.3864222f, tilePositions[15].transform.position.z);
                counter -= 1;
                endReached = false;
                currentTileNumber += 1;
            }
            else
            {
                currentTileNumber += 1;
                nextPos = new Vector3(tilePositions[currentTileNumber].transform.position.x, 0.3864222f, tilePositions[currentTileNumber].transform.position.z);
                endReached = false;
            }
        }
    }

    public void playerLost()
    {//Restart run after player loss
        player.transform.position = startPos;
        currentTileNumber = 0;
        PlayerPrefs.SetInt("HealthRating", PlayerPrefs.GetInt("MaxHealthRating"));
        PlayerPrefs.SetInt("ManaRating", PlayerPrefs.GetInt("MaxManaRating"));
        gameManager.GetComponent<GameManager>().canRoll = true;
        showRoll = true;
        movementDirection = "";
        if (flipped1 || flipped2 || flipped3)
        {//Fix the players rotation on respawn
            Vector3 newRotation = new Vector3(0, 0, 0);
            transform.eulerAngles = newRotation;
            mainCamera.transform.eulerAngles = newRotation;
        }
        if(transform.position.x < 0 && !PlayerPrefs.GetString("MapSelected").Contains("3"))
        {
            Vector3 resetPos = new Vector3(transform.position.x * -1, transform.position.y, transform.position.z);
            transform.position = resetPos;
        }
        else if (transform.position.x > 0 && PlayerPrefs.GetString("MapSelected").Contains("3"))
        {
            Vector3 resetPos = new Vector3(transform.position.x * -1, transform.position.y, transform.position.z);
            transform.position = resetPos;
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
        if (collision.gameObject.tag == "Yellow" && !GameObject.FindWithTag("Upgrades").GetComponent<ChestScript>().movePicked)
        {
            //chestAnim.GetComponent<TriggerAnimation>().chestAnimation();
            //Time.timeScale = 0;
            gameManager.GetComponent<GameManager>().ChestScene();
        }
        else if (collision.gameObject.tag == "Blue")
        {
            if (PlayerPrefs.GetString("MapSelected").Contains("3") && (currentTileNumber == (7) || currentTileNumber == (14)))
            {
                gameManager.GetComponent<GameManager>().BuffScene();
            }
            else if(PlayerPrefs.GetString("MapSelected").Contains("1") || PlayerPrefs.GetString("MapSelected").Contains("2"))
            {
                gameManager.GetComponent<GameManager>().BuffScene();
            }
        }
        //else if (PlayerPrefs.GetString("MapSelected").Contains("Map3") && collision.gameObject.tag.Contains("Blue"))
        //{
        //    //currentTileColour = collision.gameObject.tag;
        //    gameManager.GetComponent<GameManager>().BuffScene();
        //}
        else if (collision.gameObject.tag != "Plane")
        {
            currentTileColour = collision.gameObject.tag;
        }

    }
    private void OnTriggerExit(Collider collision)
    {

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
