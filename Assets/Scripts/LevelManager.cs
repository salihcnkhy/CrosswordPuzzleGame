using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    int difficulty = 0;


    public void loadScene(string mode)
    {
        PlayerPrefs.SetInt("difficulty", difficulty);

        switch (mode)
        {
            case "infinite":
                SceneManager.LoadScene("InfiniteModeScene");
                break;
            case "keepAlive":
                SceneManager.LoadScene("KeepAliveModeScene");
                break;
            case "againstTime":
                SceneManager.LoadScene("AgainstTimeModeScene");
                break;
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
    }
}