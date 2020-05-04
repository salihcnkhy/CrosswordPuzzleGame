using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour
{



    private PlayerProfile profile;
    int scrollCount = 0;

    private void Start()
    {
       profile = transform.parent.GetComponent<MainMenuManager>().profile;
       transform.Find("PlayerName").GetComponent<TMPro.TextMeshProUGUI>().text = profile.playerName;
       setLvScores();
    }

    public void setLvScores()
    {
        print(scrollCount);

        int[] scores = new int[6];
        var modeText = transform.Find("ModeText").GetComponent<TMPro.TextMeshProUGUI>();
        if(scrollCount%3 == 0 )
        {
            modeText.text = "Easy";
            scores = profile.easy;
            scrollCount = 0;
        }else if(scrollCount == 1 || scrollCount == -2)
        {
            modeText.text = "Medium";

            scores = profile.medium;
            scrollCount = 1;
        }else if(scrollCount == 2 || scrollCount == -1)
        {
            modeText.text = "Hard";

            scores = profile.hard;
            scrollCount = 2;
        }

        for (int i = 0; i < scores.Length; i++)
        {
            var scoreText = transform
                .Find("ScoreScrollView")
                .Find("Viewport")
                .Find("Content")
                .Find("Lv." + (i + 1).ToString())
                .Find("ScoreText")
                .GetComponent<TMPro.TextMeshProUGUI>();
            scoreText.text = scores[i].ToString();
        }
    }

    public void PressedPreviousButton()
    {
        scrollCount--;
        setLvScores();

    }
    public void PressedNextButton()
    {
        scrollCount++;
        setLvScores();

    }
}
