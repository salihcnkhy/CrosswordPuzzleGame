using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    string levelMode;


    public void loadScene(int difficulty)
    {
        PlayerPrefs.SetInt("difficulty", difficulty);

        switch (levelMode)
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


    public void setLevelMode(string mode)
    {
        levelMode = mode;
    }
}