using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private UIManager ui;
    private string hittedLetter = "";
    private int misstakeCount = 0;
    private int scorePoint = 0;
    private List<GameObject> bullets = new List<GameObject>();
    private int subLevel;
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
    public double passedTime = 0;
   
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

    private Player Player;

    public List<string> LevelWords; 
    private List<string> addedWords;
    public event OnLetterUpdateDelegate OnLetterUpdate;
    public event OnScorePointUpdateDelegate OnScorePointUpdate;


    public delegate void OnLetterUpdateDelegate(string lastHitted);
    public delegate void OnScorePointUpdateDelegate(int scorePoint);

    #region JSON Serializable

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


    public List<string> GetlevelWords(int diffucult, int subLevel)
    {
        List<string> ListWords = null;

        TextAsset jsonAsset = Resources.Load<TextAsset>("words");

         
         string json = jsonAsset.text;

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

        

        return ListWords;
    }

    #endregion

    private void Start()
    {
        difficulty = PlayerPrefs.GetInt("difficulty");
        subLevel = PlayerPrefs.GetInt("subLevel");

        
        Player = new Player();
        Player.load();

        Player.lastLevels[difficulty] = subLevel;

        letterSize = difficulty + 4;

        ui = GameObject.Find("UI").GetComponent<UIManager>();

        OnLetterUpdate += ui.OnHittedLetterUpdate;
        OnScorePointUpdate += ui.OnScorePointUpdate;

        StartShowingLevel(0);
        StartCoroutine(StartCount());
    }

    public void StartShowingLevel(int cmd)
    {
        StartCoroutine(ShowLevel(cmd));
    }

    private IEnumerator ShowLevel(int cmd)
    {
        if(cmd == 3)
        {
            GameObject.Find("UI").transform.Find("Canvas").GetComponent<Animator>().SetTrigger("RemovePausePanel");

        }
        else if(cmd == 1 || cmd == 2)
        {
            GameObject.Find("UI").transform.Find("Canvas").GetComponent<Animator>().SetTrigger("RemoveLevelPassedPanel");
           

            if(cmd == 1)
            {
                subLevel++;
               
            }
            saveDataFromLastLevel();

        }
        yield return new WaitForSeconds(1); 

        LevelWords = GetlevelWords(difficulty, subLevel);

        puzzleSize = subLevel < 3 ? subLevel + 3 : 6;

        LevelWords = ui.toUpperCase(LevelWords);

        ScorePoint = 0;
        passedTime = 0;

        switch (difficulty)
        {
            case 0:
                ui.setLeaderScoreText(Player.easy[subLevel]);
                break;
            case 1:
                ui.setLeaderScoreText(Player.medium[subLevel]);

                break;
            case 2:
                ui.setLeaderScoreText(Player.hard[subLevel]);

                break;
        }
        ui.setLevelText(subLevel+1);

        addedWords = ui.Create(LevelWords, puzzleSize, letterSize);

        removeAddedFromWords();

        foreach (var word in addedWords)
        {
            print(word);
        }

    }

    private void saveDataFromLastLevel()
    {
       

        var lastMaxScore = 0;
        switch (difficulty)
        {
            case 0:
                lastMaxScore = Player.easy[subLevel-1];
                Player.easy[subLevel-1] = lastMaxScore < ScorePoint ? ScorePoint : lastMaxScore;
                break;
            case 1:
                lastMaxScore = Player.medium[subLevel-1];
                Player.medium[subLevel-1] = lastMaxScore < ScorePoint ? ScorePoint : lastMaxScore;
                break;
            case 2:
                lastMaxScore = Player.hard[subLevel-1];
                Player.hard[subLevel-1] = lastMaxScore < ScorePoint ? ScorePoint : lastMaxScore;
                break;

        }
        subLevel = subLevel == 6 ? 5 : subLevel;
        Player.lastLevels[difficulty] = subLevel;


        SaveSystem.SavePlayer(Player);
    }

    public void LoadMainMenu(bool isLevelPassed)
    {
        if (isLevelPassed)
        {
            subLevel++;
            saveDataFromLastLevel();
        }
        SceneManager.LoadScene(0);
    }

    /*public TextAsset txtFile;

    private void showNewLevel()
    {

        var player = new Player();
        player.load();

        //LevelWords = GetlevelWords(difficulty , subLevel-1);

        puzzleSize = subLevel < 4 ? subLevel + 2 : 6;

        var stringOfTxt = txtFile.text;
        LevelWords = stringOfTxt.Split('\n').ToList();
        //LevelWords = ui.toUpperCase(LevelWords);
      
        ScorePoint = 0;
        CountDown = maxCountDown;

        switch (difficulty)
        {
            case 0:
                ui.setLeaderScoreText(player.easy[subLevel-1]);
                break;
            case 1:
                ui.setLeaderScoreText(player.medium[subLevel-1]);

                break;
            case 2:
                ui.setLeaderScoreText(player.hard[subLevel-1]);

                break;
        }
        ui.setLevelText(subLevel);
        List<string> allTemplates = new List<string>();


        for (int k = 0; k < 50; k++)
        {
            LevelWords = stringOfTxt.Split('\n').ToList();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {

                    puzzleSize = (j + 1) < 4 ? (j + 1) + 2 : 6;
                    letterSize = i + 4;
                    addedWords = ui.Create2(LevelWords, puzzleSize, letterSize);

                    allTemplates.Add("Zorluk : " + i + " Seviye : " + (j + 1));
                    foreach (var word in addedWords)
                    {
                        allTemplates.Add(word);
                    }
                    removeAddedFromWords();

                }

            }
            
            var path = @"/Users/salihcnkhy/KelimeKalıpları/Words_"+k+".txt";

            File.WriteAllLines(path, allTemplates);
            allTemplates.Clear();
        }
        foreach (var word in addedWords)
        {
            print(word);
        }
   
    }
    */
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

            var bestScore = 0;
            switch (difficulty)
            {
                case 0:
                    bestScore = Player.easy[subLevel];
                    break;
                case 1:
                    bestScore = Player.medium[subLevel];
                    Player.medium[subLevel] = bestScore < ScorePoint ? ScorePoint : bestScore;
                    break;
                case 2:
                    bestScore = Player.hard[subLevel];
                    break;

            }
            ui.showLevelPassedPanel(ScorePoint, bestScore,subLevel);
        }
    }
    

    public void PressedTryButton()
    {

        if (addedWords.Contains(HittedLetter))
        {
            double timeFactor = 1 / ((passedTime/7) + Convert.ToDouble(misstakeCount));
            double score = (10 * HittedLetter.Length) * timeFactor;
            print(score);
            ScorePoint += Convert.ToInt32(score);
            passedTime = 0;
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
        HittedLetter = "";
        InGameAnimationManager.shared.StartRandomizeLetterAnimation(ui.letters);
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
        HittedLetter += hitted.transform.Find("LetterField").GetComponent<TMPro.TextMeshPro>().text;
        
    }

    private IEnumerator StartCount()
    {
        passedTime = 0;
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            passedTime += 0.1f;
        }

    }



}
