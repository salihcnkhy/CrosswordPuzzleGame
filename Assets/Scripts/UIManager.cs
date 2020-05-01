using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject latterField;
    public GameObject wordLetterField;

    private GameObject lettersField;
    private TMPro.TextMeshProUGUI dragText;
    private CrossWordCreateManager cwc;
    private GameObject canvas;
    public List<GameObject> letters;

    private void Awake()
    {
        letters = new List<GameObject>();

        canvas = transform.Find("Canvas").gameObject;
        lettersField = transform.Find("LettersField").gameObject;
        dragText = canvas.transform.Find("ShootedText").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        cwc = GetComponent<CrossWordCreateManager>();

    }

    public List<string> Create(List<string> words,int puzzleSize,int letterSize)
    {
        cwc.deleteAllLetters(letters);
        var addedWords = cwc.create(words, wordLetterField, puzzleSize,letterSize);
        CreateLetterPoints(addedWords);
        return addedWords;
    }

    public List<string> Create2(List<string> words, int puzzleSize, int letterSize)
    {
        var addedWords = cwc.createWordList(words, puzzleSize, letterSize);
        return addedWords;
    }

    public void setLevelText(int level)
    {
        canvas.transform.Find("LevelText")
            .GetComponent<TMPro.TextMeshProUGUI>()
            .text = "Level " + level.ToString();
    }
    public void setLeaderScoreText(int maxScore)
    {
        canvas.transform.Find("LeaderScoreField")
           .Find("LeaderScoreText")
           .GetComponent<TMPro.TextMeshProUGUI>()
           .text = maxScore.ToString();
    }

   
    public void openWord(string word)
    {
        cwc.openWordChars(word);
    }

    public void OnHittedLetterUpdate(string letter)
    {
        dragText.text = letter;

    }
    public void OnScorePointUpdate(int scorePoint)
    {
        canvas.transform.Find("CurrentScoreField").Find("CurrentScoreText").GetComponent<TMPro.TextMeshProUGUI>().text = scorePoint.ToString();

    }

    public void OnCountDownUpdate(int countDown)
    {
        //canvas.transform.Find("TimeContainer").Find("TimeText").GetComponent<TMPro.TextMeshProUGUI>().text = countDown.ToString();
    }
    public void triggerAnimation(string animation) {

          foreach(var letter in letters)
        {
            letter.GetComponent<Animator>().SetTrigger(animation);
        }

    }



    #region ForLetters

    public List<string> toUpperCase(List<string> words)
    {
        List<string> capitalWords = new List<string>();
        for (int i = 0; i < words.Count; i++)
        {
            capitalWords.Add(words[i].ToUpper(new CultureInfo("tr-TR", false)));
        }
        return capitalWords;
    }




    private void CreateLetterPoints(List<string> words)
    {

        float x;
        float y;
        float z = 0f;
        float angle = -50;
        words = toUpperCase(words);
        var letters = cwc.GetAllLetters(words);

        char[] letter = letters.ToCharArray();
        Queue<char> randomChars = new Queue<char>();

        while (randomChars.Count != letter.Length)
        {
            int rand = Random.Range(0, letter.Length);
            if (!randomChars.Contains(letter[rand]))
            {
                randomChars.Enqueue(letter[rand]);
            }
        }

        for (int i = 0; i < letter.Length; i++)
        {

            var obj = Instantiate(latterField);

            obj.transform.position = lettersField.transform.position;

            obj.transform.Find("LetterField").GetComponent<TMPro.TextMeshPro>().text = randomChars.Dequeue().ToString();


            x = Mathf.Sin(Mathf.Deg2Rad * angle) * (lettersField.GetComponent<SpriteRenderer>().bounds.size.x / 2f);
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * (lettersField.GetComponent<SpriteRenderer>().bounds.size.x / 2f);

            obj.transform.position += new Vector3(x, y, z);

            this.letters.Add(obj);

            angle += (100f / (letter.Length-1));
        }
        var scale = Vector3.Distance(this.letters[0].transform.position, this.letters[1].transform.position);
        scale = scale > 1 ? 1f : scale;
        foreach(var obj in this.letters)
        {
            obj.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
    #endregion

}
