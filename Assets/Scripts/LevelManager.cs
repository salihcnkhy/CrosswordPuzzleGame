using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    int difficulty = 0;
    Player player;

    private void Start()
    {

        player = new Player();
        player.load();
        checkContinue();

    }

    public void loadScene(bool isNew)
    {
        PlayerPrefs.SetInt("difficulty", difficulty);

        if (isNew)
        {
            PlayerPrefs.SetInt("subLevel", 0);
            player.lastLevels[difficulty] = 0;
            SaveSystem.SavePlayer(player);
            SceneManager.LoadScene(1);

        }
        else
        {
            PlayerPrefs.SetInt("subLevel", player.lastLevels[difficulty]);
            SceneManager.LoadScene(1);
        }

    }


    public void PressedNextDifficulty()
    {
        difficulty++;
        updateDifficulty();
    }

    public void PressedPreviousDifficulty()
    {
        difficulty--;
        updateDifficulty();
    }

    private void checkContinue()
    {
        if (player.lastLevels[difficulty] == 0)
        {
            transform.Find("ContinueButton").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("ContinueButton").gameObject.SetActive(true);

        }
        print(player.lastLevels[difficulty]);
      
    }

    private void updateDifficulty()
    {

        var textMesh = transform.Find("DifficultyText").GetComponent<TMPro.TextMeshProUGUI>();
        if(difficulty%3 == 0)
        {
            textMesh.text = "EASY";
            difficulty = 0;

        }   else if(difficulty == -2 || difficulty == 1)
        {
            textMesh.text = "MEDIUM";
            difficulty = 1;
        }
        else if(difficulty == -1 || difficulty == 2)
        {
            textMesh.text = "HARD";
            difficulty = 2;
        }
        checkContinue();
    }
}