using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSceneChanger : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button muteButton;
    public Button optionsButton;
    public Button howToPlayButton;
    public Button creditsButton;
    public Button backButton;
    public GameObject gameDescriptionPanel;
    public GameObject creditsPanel;
    public TMP_Text howToPlay;
    public TMP_Text credits;
    AudioSource audioSource;
    AudioClip menuSound;
    public static bool isMuted = false;
    bool isInOptions = false;
    // Start is called before the first frame update
    void Start()
    {
        gameDescriptionPanel = GameObject.Find("how_to_play_panel");
        creditsPanel = GameObject.Find("credits_panel");
        gameDescriptionPanel.SetActive(false);
        creditsPanel.SetActive(false);
        menuSound = Resources.Load("game_sound2", typeof(AudioClip)) as AudioClip;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = menuSound;
        audioSource.Play();
        audioSource.mute = isMuted;
        muteButton.GetComponentInChildren<TMP_Text>().text = isMuted ? "Unmute Music" : "Mute Music";
        startButton.onClick.AddListener(StartButton);
        quitButton.onClick.AddListener(QuitButton);
        muteButton.onClick.AddListener(MuteButton);
        optionsButton.onClick.AddListener(OptionsButton);
        howToPlayButton.onClick.AddListener(HowToPlayButton);
        creditsButton.onClick.AddListener(CreditsButton);
        backButton.onClick.AddListener(BackButton);
        howToPlay.text = "";
        credits.text = "";
        howToPlayButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    void BackButton()
    {
        if (isInOptions)
        {
            howToPlayButton.gameObject.SetActive(true);
            creditsButton.gameObject.SetActive(true);
            gameDescriptionPanel.SetActive(false);
            creditsPanel.SetActive(false);
            isInOptions = false;
        }
        else
        {
            howToPlayButton.gameObject.SetActive(false);
            creditsButton.gameObject.SetActive(false);
            optionsButton.gameObject.SetActive(true);
            startButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
        }
    }

    void CreditsButton()
    {
        howToPlayButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        credits.text = ReadString("Assets/Resources/credits.txt");
        creditsPanel.SetActive(true);
        backButton.gameObject.SetActive(true);
        isInOptions = true;
    }

    void HowToPlayButton()
    {
        howToPlayButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        howToPlay.text = ReadString("Assets/Resources/game_description.txt");
        gameDescriptionPanel.SetActive(true);
        backButton.gameObject.SetActive(true);
        isInOptions = true;
    }

    void OptionsButton()
    {
        startButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        muteButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(false);
        howToPlayButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    void QuitButton()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    void MuteButton()
    {
        isMuted = !isMuted;
        if (isMuted)
        {
            muteButton.GetComponentInChildren<TMP_Text>().text = "Unmute Music";
        }
        else
        {
            muteButton.GetComponentInChildren<TMP_Text>().text = "Mute Music";
        }
        audioSource.mute = isMuted;
    }
    string ReadString(string path)
    {
        StreamReader reader = new StreamReader(path);
        return reader.ReadToEnd();
    }
}
