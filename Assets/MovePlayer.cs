using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MovePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 6f;
    float maxLeft = -7.5f;
    float maxRight = 7.5f;
    GameObject mainCamera;
    GameObject[] street;
    public float horizontalSpeed = 2.0f;
    GameObject redOrb;
    GameObject greenOrb;
    GameObject blueOrb;
    GameObject obstacle;
    int leftX = -5;
    int middleX = 0;
    int rightX = 5;
    float y = 0.5f;
    float zStart = -5f;
    float zIncrement = 7.5f;
    float timer = 1f;
    bool leftCreated = false;
    bool middleCreated = false;
    bool rightCreated = false;
    bool twoObstaclesCreated = false;
    public List<GameObject> gameObjects;
    TextUpdate textUpdate;
    GameSceneChanger gameSceneChanger;
    CollectOrb collectOrb;
    public bool isGamePaused = false;
    public bool isGameOver = false;
    void Start()
    {
        gameSceneChanger = GetComponent<GameSceneChanger>();
        textUpdate = GetComponent<TextUpdate>();
        collectOrb = GetComponent<CollectOrb>();
        gameObjects = new List<GameObject>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        street = GameObject.FindGameObjectsWithTag("street");
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        mainCamera.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        for (int i = 0; i < street.Length; i++)
        {
            street[i].transform.Translate(Vector3.forward * speed * Time.deltaTime);

        }
        redOrb = GameObject.FindGameObjectWithTag("red orb");
        greenOrb = GameObject.FindGameObjectWithTag("green orb");
        blueOrb = GameObject.FindGameObjectWithTag("blue orb");
        obstacle = GameObject.FindGameObjectWithTag("obstacle");

        isGamePaused = false;
        Time.timeScale = 1;
        gameSceneChanger.resumeButton.gameObject.SetActive(false);
        gameSceneChanger.restartButton.gameObject.SetActive(false);
        gameSceneChanger.mainMenuButton.gameObject.SetActive(false);
        gameSceneChanger.muteButton.GetComponentInChildren<TMP_Text>().text = MainMenuSceneChanger.isMuted
            ? "Unmute Music" : "Mute Music";
        
        //collectOrb.audioSource[4].mute = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            pauseResumeGame();
        }
        if (isGamePaused || isGameOver)
        {
            return;
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        mainCamera.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        for(int i = 0; i < street.Length; i++)
        {
            street[i].transform.Translate(Vector3.forward * speed * Time.deltaTime);

        }
        transform.Translate(Input.GetAxis("Horizontal") * horizontalSpeed * speed * Time.deltaTime, 0, 0);
        mainCamera.transform.Translate(Input.GetAxis("Horizontal") * horizontalSpeed * speed * Time.deltaTime, 0, 0);
        if(transform.position.x <= maxLeft || transform.position.x >= maxRight)
        {
            textUpdate.gameOverText.text = "Game Over!" + System.Environment.NewLine + "Final Score: " + textUpdate.scoreText.text;
            textUpdate.scoreText.text = "";
            textUpdate.redEnergyText.text = "";
            textUpdate.greenEnergyText.text = "";
            textUpdate.blueEnergyText.text = "";
            collectOrb.audioSource[4].Stop();
            collectOrb.audioSource[5].Play();
            gameSceneChanger.restartButton.gameObject.SetActive(true);
            gameSceneChanger.mainMenuButton.gameObject.SetActive(true);
            gameSceneChanger.silentButton.gameObject.SetActive(false);
            gameSceneChanger.restartButton.transform.position = new Vector3(300, 175, 0);
            gameSceneChanger.mainMenuButton.transform.position = new Vector3(600, 175, 0);
            isGameOver = true;
        }
        //create orbs and obstacles at random positions starting at zStart and incrementing by zIncrement and make sure not to create 3 obstacles in a row
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 1f;
            int numberOfObstacles = 0;
            while(createHowManyObject())
            {
                int xPosition = getXPosition();
                GameObject gameObject = GetObject();
                if(gameObject.Equals(obstacle))
                {
                    numberOfObstacles++;
                    if(numberOfObstacles > 2)
                    {
                        twoObstaclesCreated = true;
                    }
                }
                if(xPosition == leftX && !leftCreated && createObject(gameObject))
                {
                    GameObject gameObj = Instantiate(gameObject, new Vector3(xPosition, y, zStart), Quaternion.identity);
                    gameObjects.Add(gameObj);
                    leftCreated = true;
                }
                else if(xPosition == middleX && !middleCreated && createObject(gameObject))
                {
                    GameObject gameObj = Instantiate(gameObject, new Vector3(xPosition, y, zStart), Quaternion.identity);
                    gameObjects.Add(gameObj);
                    middleCreated = true;
                }
                else if(xPosition == rightX && !rightCreated && createObject(gameObject))
                {
                    GameObject gameObj = Instantiate(gameObject, new Vector3(xPosition, y, zStart), Quaternion.identity);
                    gameObjects.Add(gameObj);
                    rightCreated = true;
                }
            }
            leftCreated = false;
            middleCreated = false;
            rightCreated = false;
            twoObstaclesCreated = false;
            zStart += zIncrement;
        }
        destroyObjects();
    }
    int getXPosition()
    {
        int[] positionsX = { leftX, middleX, rightX };
        int index = Random.Range(0, positionsX.Length);
        return positionsX[index];
    }

    GameObject GetObject()
    {
        GameObject[] objects = { redOrb, greenOrb, blueOrb, obstacle};
        int index = Random.Range(0, objects.Length);
        return objects[index];
    }

    bool createObject(GameObject gameObject)
    {
        return (!twoObstaclesCreated && gameObject.Equals(obstacle)) || (!gameObject.Equals(obstacle));
    }
    bool createHowManyObject()
    {
        bool[] bools = { 
            !leftCreated, 
            !middleCreated,
            !rightCreated,
            !leftCreated || !middleCreated,
            !leftCreated || !rightCreated,
            !middleCreated || !rightCreated,
            !leftCreated || !middleCreated || !rightCreated};
        int index = Random.Range(0, bools.Length);
        return bools[index];
    }

    void destroyObjects()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] != null)
            {
                if (gameObjects[i].transform.position.z + zIncrement < transform.position.z)
                {
                    Destroy(gameObjects[i]);
                    gameObjects.Remove(gameObjects[i]);
                }
            }
            
        }
    }

    public void pauseResumeGame()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            gameSceneChanger.resumeButton.gameObject.SetActive(true);
            gameSceneChanger.restartButton.gameObject.SetActive(true);
            gameSceneChanger.mainMenuButton.gameObject.SetActive(true);
            gameSceneChanger.silentButton.gameObject.SetActive(false);
            collectOrb.audioSource[4].Pause();
            collectOrb.audioSource[5].UnPause();
            //Time.timeScale = 0;
        }
        else
        {
            isGamePaused = false;
            gameSceneChanger.resumeButton.gameObject.SetActive(false);
            gameSceneChanger.restartButton.gameObject.SetActive(false);
            gameSceneChanger.mainMenuButton.gameObject.SetActive(false);
            gameSceneChanger.silentButton.gameObject.SetActive(true);
            collectOrb.audioSource[4].UnPause();
            collectOrb.audioSource[5].Pause();
            //Time.timeScale = 1;
        }
    }


}
