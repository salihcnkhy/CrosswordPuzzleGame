using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    private UIManager ui;
    private string hittedLetter = "";
    private const int maxCountDown = 20;
    private int countDown = 20;
    private int misstakeCount = 0;
    private int scorePoint = 0;
    private List<GameObject> bullets = new List<GameObject>();
    private int subLevel = 1;
    private int difficulty;

    private int puzzleSize;
    private int letterSize;

    public string HittedLetter
    {
        get
        {
            return hittedLetter;
        }
        set
        {
            hittedLetter = value;
            OnLetterUpdate?.Invoke(hittedLetter);
        }
    }
    public int CountDown
    {
        get
        {
            return countDown;
        }
        set
        {
            countDown = value;
            OnCountDownUpdate?.Invoke(countDown);
        }
    }

    public int ScorePoint
    {
        get
        {
            return scorePoint;
        }
        set
        {
            scorePoint = value;
            OnScorePointUpdate?.Invoke(scorePoint);

        }
    }


    public List<string> words; // Lv içersinde ilerlerken wordsten added wordsleri çıkarman lazım
    private List<string> addedWords;
    public event OnLetterUpdateDelegate OnLetterUpdate;
    public event OnCountDownUpdateDelegate OnCountDownUpdate;
    public event OnScorePointUpdateDelegate OnScorePointUpdate;


    public delegate void OnLetterUpdateDelegate(string lastHitted);
    public delegate void OnCountDownUpdateDelegate(int countDown);
    public delegate void OnScorePointUpdateDelegate(int scorePoint);


    private void Start()
    {
        difficulty = PlayerPrefs.GetInt("difficulty");
        letterSize = difficulty + 4;

        ui = GameObject.Find("UI").GetComponent<UIManager>();

        words = ui.toUpperCase(words);

        OnLetterUpdate += ui.OnHittedLetterUpdate;
        OnScorePointUpdate += ui.OnScorePoointUpdate;
        OnCountDownUpdate += ui.OnCountDownUpdate;

        showNewLevel();
        StartCoroutine(StartCountDown());
    }

    private void showNewLevel()
    {
        puzzleSize = subLevel < 4 ? subLevel + 2 : 6;
        addedWords = ui.Create(words, puzzleSize, letterSize);
        removeAddedFromWords();
        ScorePoint = 0;
        CountDown = maxCountDown;
        foreach(var word in addedWords)
        {
            print(word);
        }
        print("Seviye : " + subLevel.ToString());

    }

    private void removeAddedFromWords()
    {
        foreach (var added in addedWords)
        {
            words.Remove(added);
        }
    }

    private void checkIsLevelOver()
    {
        if(addedWords.Count == 0)
        {

            var player = new Player();
            player.load();

            var lastMaxScore = 0;
            switch (difficulty)
            {
                case 0:
                     lastMaxScore = player.easy[subLevel - 1];
                    player.easy[subLevel - 1] = lastMaxScore < ScorePoint ? ScorePoint : lastMaxScore;
                    break;
                case 1:
                     lastMaxScore = player.medium[subLevel - 1];
                    player.medium[subLevel - 1] = lastMaxScore < ScorePoint ? ScorePoint : lastMaxScore;
                    break;
                case 2:
                     lastMaxScore = player.hard[subLevel - 1];
                    player.hard[subLevel - 1] = lastMaxScore < ScorePoint  ? ScorePoint : lastMaxScore;
                    break;

            }
            
            SaveSystem.SavePlayer(player);

            subLevel++;
            showNewLevel();
            //Go to next level of mode
        }
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "TryButton")
                {
                    if (addedWords.Contains(HittedLetter))
                    {
                        float timeFactor = (CountDown / maxCountDown) + 1;
                        float score = 10*HittedLetter.Length * timeFactor - 5*misstakeCount;
                        ScorePoint += Convert.ToInt32(score);
                        CountDown = maxCountDown;
                        misstakeCount = 0;
                        ui.openWord(HittedLetter);
                        addedWords.Remove(HittedLetter);
                        checkIsLevelOver();
                        HittedLetter = "";
                    }
                    else
                    {
                        // Screen Shake && Red Border ll show 
                        HittedLetter = "";
                        misstakeCount++;
                    }
                    foreach (var bul in bullets)
                    {
                        Destroy(bul);
                    }
                }
                else if (hit.collider.gameObject.name == "ResetButton")
                {
                    foreach (var bul in bullets)
                    {
                        Destroy(bul);
                    }
                }

            }
        }
       
    }



    public void OnLetterHit(GameObject bullet,GameObject hitted)
    {
        var bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = Vector2.zero;
        Destroy(bulletRb);
        bullet.transform.SetParent(hitted.transform);
        bullets.Add(bullet);
        print(hitted.transform.Find("LetterField").GetComponent<TextMesh>().text);
        HittedLetter += hitted.transform.Find("LetterField").GetComponent<TextMesh>().text;
        
    }

    private IEnumerator StartCountDown(int countDownValue = maxCountDown)
    {

        CountDown = countDownValue;
        while(CountDown != 0)
        {
            yield return new WaitForSeconds(1);
            CountDown--;
        }

    }


}
