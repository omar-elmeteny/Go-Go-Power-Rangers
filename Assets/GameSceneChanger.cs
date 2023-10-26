using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneChanger : MonoBehaviour
{
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;
    public Button muteButton;
    public Button silentButton;
    MovePlayer movePlayer;
    CollectOrb collectOrb;
    bool isSilent = false;
    // Start is called before the first frame update
    void Start()
    {
        movePlayer = GetComponent<MovePlayer>();
        collectOrb = GetComponent<CollectOrb>();
        resumeButton.onClick.AddListener(ResumeButton);
        restartButton.onClick.AddListener(RestartButton);
        mainMenuButton.onClick.AddListener(MainMenuButton);

        resumeButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        muteButton.gameObject.SetActive(true);
        muteButton.onClick.AddListener(MuteButton);
        silentButton.gameObject.SetActive(true);
        silentButton.onClick.AddListener(SilentButton);

    }

    void SilentButton()
    {
        isSilent = !isSilent;
        collectOrb.audioSource[0].mute = isSilent;
        collectOrb.audioSource[1].mute = isSilent;
        collectOrb.audioSource[2].mute = isSilent;
        collectOrb.audioSource[3].mute = isSilent;
        silentButton.GetComponentInChildren<TMP_Text>().text = isSilent ? "Unmute SFX" : "Mute SFX";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResumeButton()
    {
        movePlayer.pauseResumeGame();
    }
    void RestartButton()
    {
        var buildindex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(buildindex);        
    }

    void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    void MuteButton()
    {
        MainMenuSceneChanger.isMuted = !MainMenuSceneChanger.isMuted;
        collectOrb.audioSource[4].mute = MainMenuSceneChanger.isMuted;
        collectOrb.audioSource[5].mute = MainMenuSceneChanger.isMuted;
        muteButton.GetComponentInChildren<TMP_Text>().text = MainMenuSceneChanger.isMuted ? "Unmute Music" : "Mute Music";
    }
}
