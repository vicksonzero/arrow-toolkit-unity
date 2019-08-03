﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BController : MonoBehaviour
{

    public Text gameOverText;
    public Text gameOverSummary;
    public Text uiText;
    public Text licenseText;

    public bool gameIsOver = false;
    public AudioClip gameOverSound;
    AudioSource sound;
    BUI ui;
    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        gameOverSummary.gameObject.SetActive(false);

        ui = FindObjectOfType<BUI>();
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetButton("Submit"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (Input.GetButtonDown("License"))
            {
                gameOverText.gameObject.SetActive(false);
                gameOverSummary.gameObject.SetActive(false);
                licenseText.gameObject.SetActive(true);
            }
        }
    }

    public void GameOver(BPlayer player)
    {
        CleanUp();


        uiText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        gameOverSummary.gameObject.SetActive(true);

        gameOverSummary.text = "Time: " + "" +
            "\nMax Combo: " + ui.MaxCombo +
            "\n\n Press Enter to restart" +
            "\nL=License"
            ;

        sound.PlayOneShot(gameOverSound);

        gameIsOver = true;
    }

    void CleanUp()
    {
        var spawners = FindObjectsOfType<BSpawner>();
        foreach (var spawner in spawners)
        {
            spawner.gameObject.SetActive(false);
        }
    }
}
