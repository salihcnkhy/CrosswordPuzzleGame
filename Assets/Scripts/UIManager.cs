using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject latterField;
    public GameObject wordLetterField;

    private GameObject lettersField;
    private Text dragText;
    private CrossWordCreateManager cwc;
    private GameObject canvas;
    private GameObject toolBar;
    public List<GameObject> letters;

    private void Awake()
    {
        letters = new List<GameObject>();

        canvas = transform.Find("Canvas").gameObject;
        toolBar = canvas.transform.Find("ToolBar").gameObject;
        lettersField = transform.Find("LettersField").gameObject;
        dragText = canvas.transform.Find("DraggedText").gameObject.GetComponent<Text>();
        cwc = GetComponent<CrossWordCreateManager>();

    }

    public List<string> Create(List<string> words,int puzzleSize,int letterSize)
    {
        cwc.deleteAllLetters(letters);
        var addedWords = cwc.create(words, wordLetterField, puzzleSize,letterSize);
        CreateLetterPoints(addedWords);
        return addedWords;
    }


   
    public void openWord(string word)
    {
        cwc.openWordChars(word);
    }

    public void OnHittedLetterUpdate(string letter)
    {
        dragText.text = letter;

    }
    public void OnScorePoointUpdate(int scorePoint)
    {
        toolBar.transform.Find("ScoreContainer").Find("ScoreText").GetComponent<TMPro.TextMeshProUGUI>().text = scorePoint.ToString();

    }

    public void OnCountDownUpdate(int countDown)
    {
        toolBar.transform.Find("TimeContainer").Find("TimeText").GetComponent<TMPro.TextMeshProUGUI>().text = countDown.ToString();
    }



    #region ForLetters

    public List<string> toUpperCase(List<string> words)
    {
        List<string> capitalWords = new List<string>();
        for (int i = 0; i < words.Count; i++)
        {
            capitalWords.Add(words[i].ToUpper());
        }
        return capitalWords;
    }




    private void CreateLetterPoints(List<string> words)
    {

        float x;
        float y;
        float z = 0f;
        float angle = -45;
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

            obj.transform.SetParent(lettersField.transform);
            obj.transform.position = lettersField.transform.position;

            obj.transform.Find("LetterField").GetComponent<TextMesh>().text = randomChars.Dequeue().ToString();


            x = Mathf.Sin(Mathf.Deg2Rad * angle) * (lettersField.GetComponent<SpriteRenderer>().bounds.size.x / 2f);
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * (lettersField.GetComponent<SpriteRenderer>().bounds.size.x / 2f);

            obj.transform.position += new Vector3(x, y, z);

            this.letters.Add(obj);

            angle += (90f / (letter.Length-1));
        }
    }
    #endregion
    private void setAlpha(GameObject gm , float alpha)
    {
        var spriteRenderer = gm.GetComponent<SpriteRenderer>();
        var color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

 /*  private void DraggingProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100))
            {
                var gm = hit.transform.gameObject;
                
                var vec = gm.GetComponent<Renderer>().bounds.center;
                vec.z = -5;

                passedOnLetters.Add(gm);

                var textComp = gm.transform.Find("LetterField").GetComponent<TextMesh>();

                setAlpha(gm, 1f);

                dragText.text += textComp.text;

                lr.positionCount = passedOnLetters.Count + 1;

                lr.SetPosition(passedOnLetters.Count - 1, vec);

                lr.SetPosition(passedOnLetters.Count, vec);

                lr.enabled = true;
                isDragging = true;
            }

        }
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec.z = -5;
            if (Physics.Raycast(ray, out hit, 100))
            {
                var gm = hit.transform.gameObject;

                if (!passedOnLetters.Contains(gm))
                {
                    passedOnLetters.Add(gm);
                    var textComp = gm.transform.Find("LetterField").GetComponent<TextMesh>();
                    dragText.text += textComp.text;

                    lr.positionCount = passedOnLetters.Count + 1;

                    vec = gm.GetComponent<Renderer>().bounds.center;
                    vec.z = -5;

                    setAlpha(gm, 1f);
                    lr.SetPosition(passedOnLetters.Count, vec);
                }
            }
            else
            {
                lr.SetPosition(passedOnLetters.Count, vec);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            foreach(var letter in passedOnLetters)
            {
                setAlpha(letter, 0.1f);
            }

            if (words.Contains(dragText.text))
            {
                crossWordsField.GetComponent<CrossWordCreateManager>().openWordChars(dragText.text);
            }

            passedOnLetters.Clear();
            dragText.text = "";
            lr.positionCount = 0;
            lr.enabled = false;
            isDragging = false;
        }
    }*/

}
