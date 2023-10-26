using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text redEnergyText;
    public TMP_Text greenEnergyText;
    public TMP_Text blueEnergyText;
    public TMP_Text gameOverText;
    public TMP_Text pauseText;
    // Start is called before the first frame update
    CollectOrb collectOrb;
    void Start()
    {
        collectOrb = GetComponent<CollectOrb>();
        scoreText.text = "Score: " + collectOrb.score;
        redEnergyText.text = "Red Energy: " + collectOrb.redEnergy;
        greenEnergyText.text = "Green Energy: " + collectOrb.greenEnergy;
        blueEnergyText.text = "Blue Energy: " + collectOrb.blueEnergy;
        gameOverText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + collectOrb.score;
        redEnergyText.text = "Red Energy: " + collectOrb.redEnergy;
        greenEnergyText.text = "Green Energy: " + collectOrb.greenEnergy;
        blueEnergyText.text = "Blue Energy: " + collectOrb.blueEnergy;
    }
    
}
