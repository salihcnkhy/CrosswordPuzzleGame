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

    bool isLevelEnd = false;

    public List<string> LevelWords; // TODO: static classtaki fonksiyona eşitleyeceğin kelimeler bunlar
    private List<string> addedWords;
    public event OnLetterUpdateDelegate OnLetterUpdate;
    public event OnCountDownUpdateDelegate OnCountDownUpdate;
    public event OnScorePointUpdateDelegate OnScorePointUpdate;


    public delegate void OnLetterUpdateDelegate(string lastHitted);
    public delegate void OnCountDownUpdateDelegate(int countDown);
    public delegate void OnScorePointUpdateDelegate(int scorePoint);
    
    [Serializable]
    public class Word
    {
        public List<Easy> easy;

        public List<Medium> medium;

        public List<Hard> hard;
    }
    [Serializable]
    public class Easy
    {

        public List<string> level0;

        public List<string> level1;

        public List<string> level2;

        public List<string> level3;

        public List<string> level4; 

        public List<string> level5; 
    }
    [Serializable]
    public class Medium
    {

        public List<string> level0;

        public List<string> level1;

        public List<string> level2;

        public List<string> level3;

        public List<string> level4;

        public List<string> level5;
    }
    [Serializable]
    public class Hard
    {

        public List<string> level0;

        public List<string> level1;

        public List<string> level2; 

        public List<string> level3; 

        public List<string> level4; 

        public List<string> level5;
    }
    
    private void Start()
    {
        difficulty = PlayerPrefs.GetInt("difficulty");
        letterSize = difficulty + 4;

        ui = GameObject.Find("UI").GetComponent<UIManager>();

        OnLetterUpdate += ui.OnHittedLetterUpdate;
        OnScorePointUpdate += ui.OnScorePointUpdate;
        OnCountDownUpdate += ui.OnCountDownUpdate;

        showNewLevel();
        StartCoroutine(StartCountDown());
    }
    
    public List<string> GetlevelWords(int diffucult, int subLevel)
    {
        List<string> ListWords = null;
                                                     //BU KISMI DEĞİŞTİRMEN LAZIM JSON DOSYASI NERDEYSE  !!!
        using (StreamReader read = new StreamReader("C:\\Users\\Ege\\source\\repos\\JsonSerialize\\JsonSerialize\\words.json"))
        {
            string json = read.ReadToEnd();
            Word word = JsonUtility.FromJson<Word>(json);

            if (difficulty == 0)
            {
                switch (subLevel)
                {
                    case 0:
                        ListWords = word.easy[0].level0;
                        break;
                    case 1:
                        ListWords = word.easy[0].level1;
                        break;
                    case 2:
                        ListWords = word.easy[0].level2;
                        break;
                    case 3:
                        ListWords = word.easy[0].level3;
                        break;
                    case 4:
                        ListWords = word.easy[0].level4;
                        break;
                    case 5:
                        ListWords = word.easy[0].level5;
                        break;
                    default:
                        break;
                }

            }
            else if (difficulty == 1)
            {

                switch (subLevel)
                {
                    case 0:
                        ListWords = word.medium[0].level0;
                        break;
                    case 1:
                        ListWords = word.medium[0].level1;
                        break;
                    case 2:
                        ListWords = word.medium[0].level2;
                        break;
                    case 3:
                        ListWords = word.medium[0].level3;
                        break;
                    case 4:
                        ListWords = word.medium[0].level4;
                        break;
                    case 5:
                        ListWords = word.medium[0].level5;
                        break;
                    default:
                        break;
                }
            }
            else if (difficulty == 2)
            {

                switch (subLevel)
                {
                    case 0:
                        ListWords = word.hard[0].level0;
                        break;
                    case 1:
                        ListWords = word.hard[0].level1;
                        break;
                    case 2:
                        ListWords = word.hard[0].level2;
                        break;
                    case 3:
                        ListWords = word.hard[0].level3;
                        break;
                    case 4:
                        ListWords = word.hard[0].level4;
                        break;
                    case 5:
                        ListWords = word.hard[0].level5;
                        break;
                    default:
                        break;
                }
            }

        }

        return ListWords;
    }

    private void showNewLevel()
    {
        // TODO: burada eşitleme işini yap
        // subLevel değişkeni 1 den başlıyor !!
        LevelWords = GetlevelWords(difficulty , subLevel-1);

        puzzleSize = subLevel < 4 ? subLevel + 2 : 6;

        LevelWords = ui.toUpperCase(LevelWords);
        addedWords = ui.Create(LevelWords, puzzleSize, letterSize);
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
            LevelWords.Remove(added);
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
            StartCoroutine(MoveToZero());
            StopCoroutine(StartCountDown());
            //Go to next level of mode
        }
    }

    public void PressedTryButton()
    {

        if (addedWords.Contains(HittedLetter))
        {
            float timeFactor = (CountDown / maxCountDown) + 1;
            float score = 10 * HittedLetter.Length * timeFactor - 5 * misstakeCount;
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
            ui.triggerAnimation("Uncorrect");
            // Screen Shake && Red Border ll show 
            HittedLetter = "";
            misstakeCount++;
        }
        foreach (var bul in bullets)
        {
            Destroy(bul);
        }
    }

    public void PressedRandomizeButton()
    {
        foreach (var bul in bullets)
        {
            Destroy(bul);
        }
    }

  
    public void OnLetterHit(GameObject bullet,GameObject hitted)
    {
        var bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = Vector2.zero;
        Destroy(bulletRb);
        bullet.transform.SetParent(hitted.transform);
        bullets.Add(bullet);
        print(hitted.transform.Find("LetterField").GetComponent<TMPro.TextMeshPro>().text);
        HittedLetter += hitted.transform.Find("LetterField").GetComponent<TMPro.TextMeshPro>().text;
        
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

    IEnumerator MoveToZero()
    {
        // This looks unsafe, but Unity uses
        // en epsilon when comparing vectors.
        var destination = new Vector3(0, 0, 0);

        foreach(var letter in ui.letters)
        {
            var mover = letter.transform;

            while (mover.position != destination)
            {
                mover.position = Vector3.MoveTowards(
                    mover.position,
                    destination,
                    5f * Time.deltaTime);
                // Wait a frame and move again.
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);


        }

        showNewLevel();

    }

}
