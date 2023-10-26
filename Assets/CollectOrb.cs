using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectOrb : MonoBehaviour
{
    Material redOrb;
    Material greenOrb;
    Material blueOrb;
    Material playerColor;
    public int redEnergy;
    public int greenEnergy;
    public int blueEnergy;
    int maxEnergy = 5;
    bool isInRedForm;
    bool isInGreenForm;
    bool isInBlueForm;
    bool isInNormalForm;
    public int score;
    int scoreIncrement = 1;
    int energyIncrement = 1;
    bool isGreenPowerActive;
    bool isBluePowerActive;
    GameObject shield;
    GameObject shieldObject;
    float shieldY = 0.4f;
    float shieldZ = 0.5f;
    MovePlayer movePlayer;
    TextUpdate textUpdate;
    GameSceneChanger gameSceneChanger;
    AudioClip collectRedOrb;
    AudioClip collectGreenOrb;
    AudioClip collectBlueOrb;
    AudioClip hitObstacle;
    AudioClip activateRedPower;
    AudioClip activateGreenPower;
    AudioClip activateBluePower;
    AudioClip switchToRed;
    AudioClip switchToGreen;
    AudioClip switchToBlue;
    AudioClip errorSound;
    AudioClip gameSound;
    public AudioClip gameSound2;
    public AudioSource[] audioSource;
    // Start is called before the first frame update
    void Start()
    {
        movePlayer = GetComponent<MovePlayer>();
        textUpdate = GetComponent<TextUpdate>();
        gameSceneChanger = GetComponent<GameSceneChanger>();
        isInNormalForm = true;
        isInRedForm = false;
        isInGreenForm = false;
        isInBlueForm = false;
        isGreenPowerActive = false;
        isBluePowerActive = false;
        redOrb = Resources.Load("red orb", typeof(Material)) as Material;
        greenOrb = Resources.Load("green orb", typeof(Material)) as Material;
        blueOrb =Resources.Load("blue orb", typeof(Material)) as Material;
        playerColor = Resources.Load("player color", typeof(Material)) as Material;
        audioSource = GetComponents<AudioSource>();
        collectRedOrb = Resources.Load("collect_red_orb", typeof(AudioClip)) as AudioClip;
        collectGreenOrb = Resources.Load("collect_green_orb", typeof(AudioClip)) as AudioClip;
        collectBlueOrb = Resources.Load("collect_blue_orb", typeof(AudioClip)) as AudioClip;
        hitObstacle = Resources.Load("obstacle_hit", typeof(AudioClip)) as AudioClip;
        activateRedPower = Resources.Load("red_power", typeof(AudioClip)) as AudioClip;
        activateGreenPower = Resources.Load("green_power", typeof(AudioClip)) as AudioClip;
        activateBluePower = Resources.Load("blue_power", typeof(AudioClip)) as AudioClip;
        switchToRed = Resources.Load("switch_to_red", typeof(AudioClip)) as AudioClip;
        switchToGreen = Resources.Load("switch_to_green", typeof(AudioClip)) as AudioClip;
        switchToBlue = Resources.Load("switch_to_blue", typeof(AudioClip)) as AudioClip;
        errorSound = Resources.Load("error_sound", typeof(AudioClip)) as AudioClip;
        gameSound = Resources.Load("game_sound", typeof(AudioClip)) as AudioClip;
        gameSound2 = Resources.Load("game_sound2", typeof(AudioClip)) as AudioClip;
        shield = GameObject.FindGameObjectWithTag("shield");
        audioSource[4].clip = gameSound;
        audioSource[4].Play();
        audioSource[5].clip = gameSound2;
        audioSource[5].Play();
        audioSource[5].Pause();
        audioSource[4].mute = MainMenuSceneChanger.isMuted;
        audioSource[5].mute = MainMenuSceneChanger.isMuted;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchForm();
        CheckReturnToNormalForm();
        ActivatePower();
    }

    private void ActivatePower()
    {
        if (Input.GetKeyDown("space"))
        {
            if (isInRedForm && redEnergy > 0)
            {
                redEnergy -= 1;
                if (redEnergy == 0)
                {
                    isInRedForm = false;
                    isInNormalForm = true;
                    gameObject.GetComponent<Renderer>().material = playerColor;
                }
                useRedPower();
                
            }
            else if (isInGreenForm && greenEnergy > 0 && !isGreenPowerActive)
            {

                greenEnergy -= 1;
                if (greenEnergy == 0)
                {
                    isInGreenForm = false;
                    isInNormalForm = true;
                    gameObject.GetComponent<Renderer>().material = playerColor;
                    isGreenPowerActive = false;
                    deactivateGreenPower();
                }
                else
                {
                    isGreenPowerActive = true;
                    useGreenPower();
                    audioSource[0].clip = activateGreenPower;
                    audioSource[0].Play();
                }
            }
            else if (isInBlueForm && blueEnergy > 0 && !isBluePowerActive)
            {
                blueEnergy -= 1;
                if (blueEnergy == 0)
                {
                    isInBlueForm = false;
                    isInNormalForm = true;
                    gameObject.GetComponent<Renderer>().material = playerColor;
                    isBluePowerActive = false;
                    deactivateBluePower();
                }
                else
                {
                    isBluePowerActive = true;
                    useBluePower();
                    audioSource[0].clip = activateBluePower;
                    audioSource[0].Play();
                }
            }
            else
            {
                audioSource[3].clip = errorSound;
                audioSource[3].Play();
            }
        }
    }

    private void CheckReturnToNormalForm()
    {
        if (redEnergy == 0 && isInRedForm)
        {
            isInRedForm = false;
            isInNormalForm = true;
            gameObject.GetComponent<Renderer>().material = playerColor;
            audioSource[1].clip = switchToRed;
            audioSource[1].Play();
        }
        else if (greenEnergy == 0 && isInGreenForm)
        {
            isInGreenForm = false;
            isInNormalForm = true;
            gameObject.GetComponent<Renderer>().material = playerColor;
            audioSource[1].clip = switchToGreen;
            audioSource[1].Play();
        }
        else if (blueEnergy == 0 && isInBlueForm)
        {
            isInBlueForm = false;
            isInNormalForm = true;
            gameObject.GetComponent<Renderer>().material = playerColor;
            audioSource[1].clip = switchToBlue;
            audioSource[1].Play();
        }
    }

    private void SwitchForm()
    {
        if (Input.GetKeyDown("j"))
        {
            if (redEnergy == maxEnergy && !isInRedForm)
            {
                gameObject.GetComponent<Renderer>().material = redOrb;
                isInRedForm = true;
                isInNormalForm = false;
                isInGreenForm = false;
                isInBlueForm = false;
                redEnergy = redEnergy - 1;
                deactivateGreenPower();
                isGreenPowerActive = false;
                deactivateBluePower();
                isBluePowerActive = false;
                audioSource[1].clip = switchToRed;
                audioSource[1].Play();
            }
            else
            {
                audioSource[3].clip = errorSound;
                audioSource[3].Play();
            }
        }
        else if (Input.GetKeyDown("k"))
        {
            if (greenEnergy == maxEnergy && !isInGreenForm)
            {
                gameObject.GetComponent<Renderer>().material = greenOrb;
                isInRedForm = false;
                isInNormalForm = false;
                isInGreenForm = true;
                isInBlueForm = false;
                greenEnergy = greenEnergy - 1;
                deactivateBluePower();
                isBluePowerActive = false;
                audioSource[1].clip = switchToGreen;
                audioSource[1].Play();
            }
            else
            {
                audioSource[3].clip = errorSound;
                audioSource[3].Play();
            }
        }
        else if (Input.GetKeyDown("l"))
        {
            if (blueEnergy == maxEnergy && !isInBlueForm)
            {
                gameObject.GetComponent<Renderer>().material = blueOrb;
                isInRedForm = false;
                isInNormalForm = false;
                isInGreenForm = false;
                isInBlueForm = true;
                blueEnergy = blueEnergy - 1;
                deactivateGreenPower();
                isGreenPowerActive = false;
                audioSource[1].clip = switchToBlue;
                audioSource[1].Play();
            }
            else
            {
                audioSource[3].clip = errorSound;
                audioSource[3].Play();
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("red orb"))
        {
            Destroy(collision.gameObject);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            audioSource[2].clip = collectRedOrb;
            audioSource[2].Play();
            if (isInRedForm)
            {
                score = score + 2;
            }
            else
            {
                redEnergy = redEnergy + energyIncrement;
                redEnergy = redEnergy > maxEnergy ? maxEnergy : redEnergy;
                score = score + scoreIncrement;
            }
            if(isGreenPowerActive)
            {
                deactivateGreenPower();
                isGreenPowerActive = false;
            }
        }
        else if(collision.gameObject.CompareTag("green orb"))
        {
            Destroy(collision.gameObject);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            audioSource[2].clip = collectGreenOrb;
            audioSource[2].Play();
            if (isInGreenForm)
            {
                score = score + 2 * scoreIncrement;
            }
            else
            {
                if (isGreenPowerActive)
                {
                    score = score + 2 * scoreIncrement;
                }
                else
                {
                    greenEnergy = greenEnergy + 1;
                    greenEnergy = greenEnergy > maxEnergy ? maxEnergy : greenEnergy;
                    score = score + 1;
                }
            }
            if(isGreenPowerActive)
            {
                deactivateGreenPower();
                isGreenPowerActive = false;
            }
        }
        else if (collision.gameObject.CompareTag("blue orb"))
        {
            Destroy(collision.gameObject);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            audioSource[2].clip = collectBlueOrb;
            audioSource[2].Play();
            if (isInBlueForm)
            {
                score = score + 2;
            }
            else
            {
                blueEnergy = blueEnergy + energyIncrement;
                score = score + scoreIncrement;
                blueEnergy = blueEnergy > maxEnergy ? maxEnergy : blueEnergy;
            }
            if(isGreenPowerActive)
            {
                deactivateGreenPower();
                isGreenPowerActive = false;
            }
        }
        else if (collision.gameObject.CompareTag("obstacle"))
        {
            audioSource[2].clip = hitObstacle;
            audioSource[2].Play();
            if (isBluePowerActive)
            {
                Destroy(collision.gameObject);
                deactivateBluePower();
                isBluePowerActive = false;
            }
            else
            {
                if (!isInNormalForm)
                {
                    Destroy(collision.gameObject);
                    isInRedForm = false;
                    isInBlueForm = false;
                    isInGreenForm = false;
                    isInNormalForm = true;
                    gameObject.GetComponent<Renderer>().material = playerColor;
                    if(isGreenPowerActive)
                    {
                        deactivateGreenPower();
                        isGreenPowerActive = false;
                    }
                }
                else
                {
                    textUpdate.gameOverText.text = "Game Over!" + System.Environment.NewLine + "Final Score: " + score;
                    textUpdate.scoreText.text = "";
                    textUpdate.redEnergyText.text = "";
                    textUpdate.greenEnergyText.text = "";
                    textUpdate.blueEnergyText.text = "";
                    audioSource[4].Stop();
                    audioSource[5].Play();
                    gameSceneChanger.restartButton.gameObject.SetActive(true);
                    gameSceneChanger.mainMenuButton.gameObject.SetActive(true);
                    gameSceneChanger.silentButton.gameObject.SetActive(false);
                    gameSceneChanger.restartButton.transform.position = new Vector3(300, 175, 0);
                    gameSceneChanger.mainMenuButton.transform.position = new Vector3(600, 175, 0);
                    movePlayer.isGameOver = true;
                }
            }
        }
    }

    void useRedPower()
    {
        var destroyCount = 0;
        for(int i = 0; i < movePlayer.gameObjects.Count; i++)
        {
            if (movePlayer.gameObjects[i] != null)
            {
                if (movePlayer.gameObjects[i].CompareTag("obstacle"))
                {
                    if ((movePlayer.gameObjects[i].transform.position.z > transform.position.z) && (movePlayer.gameObjects[i].transform.position.z <= transform.position.z + 50))
                    {
                        Destroy(movePlayer.gameObjects[i]);
                        movePlayer.gameObjects.RemoveAt(i--);
                        destroyCount++;
                    }

                }
            }
        }
        if (destroyCount > 0)
        {
            audioSource[0].clip = activateRedPower;
            audioSource[0].Play();
        }
    }

    void useGreenPower()
    {
        scoreIncrement = 5;
        energyIncrement = 2;
    }

    void useBluePower()
    {
        shieldObject = Instantiate(shield, new Vector3(transform.position.x, shieldY, transform.position.z + shieldZ), Quaternion.identity);
        shieldObject.transform.parent = transform;
        //shieldObject.transform.Translate(Vector3.forward * movePlayer.speed * Time.deltaTime);
        //shieldObject.transform.Translate(Input.GetAxis("Horizontal") * movePlayer.horizontalSpeed * movePlayer.speed * Time.deltaTime, 0, 0);
    }

    void deactivateBluePower()
    {
        if(shieldObject != null)
        {
            Destroy(shieldObject);
        }
    }

    void deactivateGreenPower()
    {
        scoreIncrement = 1;
        energyIncrement = 1;
    }
}
