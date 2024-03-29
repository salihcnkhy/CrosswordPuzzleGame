﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class CrossWordCreateManager : MonoBehaviour
{
    private char[,] wordsMatrix;
    private GameObject[,] letterSlotMatrix;
    private List<Word> addedWords;
    private List<string> words;
    private Queue<string> randomWordQueue;
    private int letterCountV;
    private int letterCountH;
    private RectTransform cellsField;
    private int rowCount = 75;
    public List<string> create(List<string> words, GameObject letterSlot,int puzzeSize,int letterSize)
    {

        this.words = words;
        addedWords = new List<Word>();
        randomWordQueue = new Queue<string>();
        cellsField = GameObject.Find("WordsField").GetComponent<RectTransform>();
        createWordsMatrix(puzzeSize,letterSize);
        createCells(letterSlot);

        var addedWordStrings = new List<string>();

        foreach(var word in addedWords)
        {
            addedWordStrings.Add(word.word);

        }
        return addedWordStrings;

    }
    public List<string> createWordList(List<string> words, int puzzeSize, int letterSize)
    {

        this.words = words;
        addedWords = new List<Word>();
        randomWordQueue = new Queue<string>();
        cellsField = GameObject.Find("WordsField").GetComponent<RectTransform>();
        createWordsMatrix(puzzeSize, letterSize);
      
        var addedWordStrings = new List<string>();

        foreach (var word in addedWords)
        {
            addedWordStrings.Add(word.word);

        }
        return addedWordStrings;

    }

    private Queue<T> getRandomQueueFromList<T>(List<T> list)
    {
        Queue<T> queue = new Queue<T>();
        for(int i = 0; i<list.Count/2; i++)
        {
            int random = UnityEngine.Random.Range(0, list.Count);
            while (random == i)
            {
                random = UnityEngine.Random.Range(0, list.Count);
            }

            var temp = list[random];
            list[random] = list[i];
            list[i] = temp;
            
        }
        foreach(var item in list)
        {
            queue.Enqueue(item);
        }
        return queue;

    }
    private bool checkLetters(string e , string addedWordString)
    {
        for (int i = 0; i < addedWordString.Length; i++)
        {
            if (!e.Contains(addedWordString[i]))
            {
                return false;
            }
                
        }

        return true;
    }

    private void createWordsMatrix(int puzzleSize,int letterSize)
    {       
             List<string> addedWordStrings = new List<string>();
             bool isFound = false;
        if (letterSize < 6)
        {
            words = words.FindAll(e => e.Length <= letterSize);

        }
        while (!isFound)
            {
            addedWordStrings.Clear();
            addedWords.Clear();
            wordsMatrix = new char[rowCount, rowCount];

            randomWordQueue = getRandomQueueFromList(words);
              
                while (randomWordQueue.Count != 0)
                {
                
                    setWordPlace();
                if (!addedWordStrings.Contains(addedWords[addedWords.Count - 1].word))
                {
                    addedWordStrings.Add(addedWords[addedWords.Count - 1].word);
                    var allLetters = GetAllLetters(addedWordStrings);
                    var tempWords = words.FindAll(delegate(string e){
                            return checkLetters(e, allLetters);
                        });

                    foreach(var word in addedWordStrings)
                    {
                        tempWords.Remove(word);
                    }

                    if(tempWords.Count+addedWordStrings.Count < puzzleSize)
                    {
                        break;
                    }

                    randomWordQueue = getRandomQueueFromList(tempWords);

                }
                if(letterSize == 6)
                {
                    if (addedWords.Count == puzzleSize && GetAllLetters(addedWordStrings).Length >= letterSize)
                    {

                        isFound = true;
                        break;
                    }  else if (GetAllLetters(addedWordStrings).Length > 12)
                    {
                        addedWordStrings.Clear();
                        addedWords.Clear();
                        wordsMatrix = new char[rowCount, rowCount];

                    }
                }
                else if (letterSize < 6)
                {
                    if (addedWords.Count == puzzleSize && GetAllLetters(addedWordStrings).Length == letterSize)
                    {

                        isFound = true;
                        break;
                    }else if(GetAllLetters(addedWordStrings).Length > letterSize)
                    {
                        addedWordStrings.Clear();
                        addedWords.Clear();
                        wordsMatrix = new char[rowCount, rowCount];

                    }
                }

                }
               
        }
           
      
    }


    public void deleteAllLetters(List<GameObject> slots)
    {
        if(letterSlotMatrix != null)
        foreach (var letter in letterSlotMatrix)
        {
            Destroy(letter);

        }
        foreach (var letter in slots)
        {
            Destroy(letter);

        }
        slots.Clear();
    }

    public void openWordChars(string openedWord)
    {
        foreach(var word in addedWords)
        {
            if(word.word == openedWord)
            {

                if(word.direction == Direction.Horizontal)
                {
                    for (int i = word.startIndex.y; i < word.startIndex.y + word.word.Length; i++)
                    {
                        letterSlotMatrix[word.startIndex.x, i].transform.Find("LetterField").GetComponent<TMPro.TextMeshPro>().gameObject.SetActive(true);
                    }
                }
                else if(word.direction == Direction.Vertical)
                {
                    for (int i = word.startIndex.x; i < word.startIndex.x + word.word.Length; i++)
                    {
                        letterSlotMatrix[i, word.startIndex.y].transform.Find("LetterField").GetComponent<TMPro.TextMeshPro>().gameObject.SetActive(true);
                    }
                }

            }
        }
    }

    private void updateAddedWordsIndex(Vector2Int startIndexes)
    {
        for(int i =  0; i<addedWords.Count; i++)
        {
            addedWords[i].startIndex -= startIndexes;
        }
    }

    private void createCells(GameObject letterSlot)
    {

        var indexesArray = getStartPoints();
        updateAddedWordsIndex(new Vector2Int(indexesArray[0].x, indexesArray[1].x));
        var cellSize = calculateCellSize(indexesArray);
        int row = 0;
        int column = 0;

        for (int i = indexesArray[0].x; i <= indexesArray[0].y; i++)
        {
            for (int j = indexesArray[1].x; j <= indexesArray[1].y; j++)
            {
                var vec = new Vector3(((j - indexesArray[1].x)*cellSize*0.9f),((i - indexesArray[0].x)*cellSize*-0.9f)+cellsField.position.y, 0);
                vec = Camera.main.ScreenToWorldPoint(vec);
                vec.z = 0;
                GameObject slot = Instantiate(letterSlot, vec,new Quaternion());
               
                letterSlotMatrix[row, column] = slot;
                slot.transform.Find("LetterField").GetComponent<TMPro.TextMeshPro>().gameObject.SetActive(false);
                slot.transform.Find("LetterField").GetComponent<TMPro.TextMeshPro>().text = wordsMatrix[i,j].ToString();
                if (wordsMatrix[i,j] == '\0')
                {
                    letterSlotMatrix[row, column].SetActive(false);
                }
                column++;
            }
            
            column = 0;
            row++;
        }
 
        var distanceX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0))
            - letterSlotMatrix[0, indexesArray[1].y - indexesArray[1].x].transform.position;
        distanceX.y = 0;
        distanceX.z = 0;

        var distanceY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height/2))
           - letterSlotMatrix[indexesArray[0].y - indexesArray[0].x, 0].transform.position;
        distanceY.x = 0;
        distanceY.z = 0;

        var sc = letterSlotMatrix[0, 1].transform.position - letterSlotMatrix[0, 0].transform.position;

        foreach (var letter in letterSlotMatrix)
        {
            if (letter != null)
            {
                
                letter.transform.position += distanceX / 2;
                letter.transform.position += distanceY / 2;
                
            }

            letter.transform.localScale = new Vector3(sc.x-0.1f, sc.x-0.1f, 1);
        }

      
    }


    public string GetAllLetters(List<string> words)
    {
        string allWords = "";

        foreach (var word in words)
        {
            allWords += word;
        }

        char[] allCharInWords = allWords.ToCharArray();

        string letters = "";

        for (int i = 0; i < allCharInWords.Length; i++)
        {
            if (!letters.Contains(allCharInWords[i].ToString()))
            {
                letters += allCharInWords[i].ToString();
            }
        }
        return letters;
    }

    private Vector2Int[] getStartPoints()
    {
        int firstRowIndex = -1;
        int lastRowIndex = -1;

        int firstColumnIndex = -1;
        int lastColumnIndex = -1;


        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                if (wordsMatrix[i, j] != '\0')
                {
                    if (firstRowIndex == -1)
                    {
                        firstRowIndex = i;
                    }
                    else
                    {
                        lastRowIndex = i;
                    }
                }

                if (wordsMatrix[j, i] != '\0')
                {
                    if (firstColumnIndex == -1)
                    {
                        firstColumnIndex = i;
                    }
                    else
                    {
                        lastColumnIndex = i;
                    }
                }
            }
        }

        Vector2Int[] indexArray = new Vector2Int[2];
        indexArray[0] = new Vector2Int(firstRowIndex, lastRowIndex);
        indexArray[1] = new Vector2Int(firstColumnIndex, lastColumnIndex);
        return indexArray;
    }

    private float calculateCellSize(Vector2Int[] indexsArray)
    {
         letterCountV = indexsArray[0].y - indexsArray[0].x + 1;
         letterCountH = indexsArray[1].y - indexsArray[1].x + 1;

         letterSlotMatrix = new GameObject[letterCountV, letterCountH];

        var cellSize = 50f;

        if (letterCountH > letterCountV)
        {
            cellSize = (cellsField.rect.width / letterCountH);

        }
        else
        {
            cellSize = (cellsField.rect.height / letterCountV);
         
        }
        return cellSize > (cellsField.rect.width*0.25f) ? (cellsField.rect.width * 0.25f) : cellSize;
    }


    private void setWordPlace()
    {
        string word = randomWordQueue.Dequeue();
        char[] randomWord = word.ToCharArray();
        if (addedWords.Count == 0)
        {
            int start = (rowCount - randomWord.Length) / 2;
            int count = 0;
            for (int i = start; i < start + randomWord.Length; i++)
            {
                wordsMatrix[rowCount/2, i] = randomWord[count];
                count++;
            }
            addedWords.Add(new Word(word,new Vector2Int(rowCount/2,start),Direction.Horizontal));
        }
        else
        {
            Queue<char> letterQueue = new Queue<char>();
            List<char> letters = new List<char>();
            foreach (char letter in randomWord)
            {
                if (!letters.Contains(letter))
                {
                    letters.Add(letter);
                }
            }

            while (letterQueue.Count != letters.Count)
            {
                int rand = UnityEngine.Random.Range(0, letters.Count);
                if (!letterQueue.Contains(letters[rand]))
                {
                    letterQueue.Enqueue(letters[rand]);
                }
            }
            bool isAdded = false;
            while (!isAdded && letterQueue.Count != 0)
            {
                var indexVectors = searchWordsMatrix(letterQueue.Dequeue());

                if (indexVectors.Count != 0)
                {

                    foreach (var index in indexVectors)
                    {
                        var direction = whichDirection(index);
                        isAdded = addWordIfCan(word, index, direction);
                        if (isAdded)
                        {
                            break;
                        }

                    }
                }
            }

        }

    }

    private char[,] cloneWordMatrix()
    {

        char[,] temp = new char[rowCount, rowCount];
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                temp[i, j] = wordsMatrix[i, j];
            }
        }
        return temp;
    }

    private bool addWordIfCan(string word, Vector2Int indexVector, Direction direction)
    {

        int offset = 0;

        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] != wordsMatrix[indexVector.x, indexVector.y])
            {
                offset++;
            }
            else
            {
                break;
            }
        }

        char[,] tempList = cloneWordMatrix();



        if (direction == Direction.Horizontal)
        {
            int start = indexVector.y - offset;
            int count = 0;
            for (int i = start; i < word.Length + start; i++)
            {
                if (indexVector.y == i)
                {
                    count++;
                    continue;
                }
                if (tempList[indexVector.x - 1, i] != '\0' || tempList[indexVector.x + 1, i] != '\0')
                {
                    return false;
                }
                if (count == 0)
                {
                    if (tempList[indexVector.x, i - 1] != '\0')
                    {
                        return false;
                    }

                }
                else if (count == word.Length - 1)
                {
                    if (tempList[indexVector.x, i + 1] != '\0')
                    {
                        return false;
                    }

                }

                tempList[indexVector.x, i] = word[count];
                count++;
            }
            wordsMatrix = tempList;
            addedWords.Add(
                new Word(word,
                new Vector2Int(indexVector.x, start),
               direction));

            return true;


        }
        else if (direction == Direction.Vertical)
        {
            int start = indexVector.x - offset;
          
            int count = 0;
            for (int i = start; i < word.Length + start; i++)
            {
                
                if (indexVector.x == i)
                {
                    count++;
                    continue;
                }


                if (tempList[i, indexVector.y - 1] != '\0' || tempList[i, indexVector.y + 1] != '\0')
                {
                    return false;
                }
                if (count == 0)
                {
                    if (tempList[i - 1, indexVector.y] != '\0')
                    {
                        return false;
                    }

                }
                else if (count == word.Length - 1)
                {
                    if (tempList[i + 1, indexVector.y] != '\0')
                    {
                        return false;
                    }


                }

                tempList[i, indexVector.y] = word[count];
                count++;
            }
            wordsMatrix = tempList;
            addedWords.Add(
                new Word(word,
                new Vector2Int(start, indexVector.y),
                direction));

            return true;

        }
        return false;

    }

    private Queue<Vector2Int> searchWordsMatrix(char element)
    {

        var temp = new Queue<Vector2Int>();
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                if (wordsMatrix[i, j] != '\0')

                    if (wordsMatrix[i, j] == element)
                    {
                        temp.Enqueue(new Vector2Int(i, j));
                    }
            }
        }
        return temp;
    }

    private Direction whichDirection(Vector2Int indexVector)
    {

        if ((wordsMatrix[indexVector.x, indexVector.y - 1] != '\0' || wordsMatrix[indexVector.x, indexVector.y + 1] != '\0') &&
            (wordsMatrix[indexVector.x - 1, indexVector.y] == '\0' && wordsMatrix[indexVector.x + 1, indexVector.y] == '\0'))
        {
            return Direction.Vertical;
        }
        else if ((wordsMatrix[indexVector.x - 1, indexVector.y] != '\0' || wordsMatrix[indexVector.x + 1, indexVector.y] != '\0') &&
            (wordsMatrix[indexVector.x, indexVector.y - 1] == '\0' && wordsMatrix[indexVector.x, indexVector.y + 1] == '\0'))
        {
            return Direction.Horizontal;
        }
        return Direction.Unknown;

    }





}
internal enum Direction
{
    Vertical,
    Horizontal,
    Unknown
}

[SerializeField]
internal class Word
{
   public string word;
   public Vector2Int startIndex;
   public Direction direction;


    public Word(string word , Vector2Int startIndex , Direction direction)
    {
        this.word = word;
        this.startIndex = startIndex;
        this.direction = direction;
    }

}