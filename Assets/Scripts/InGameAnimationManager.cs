using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameAnimationManager : MonoBehaviour
{
    public static InGameAnimationManager shared;


    private void Awake()
    {
        if(shared == null)
        {
            shared = this;
        }
    }

    private InGameAnimationManager()
    {

    }

    bool isAnimating = false;

    public void StartRandomizeLetterAnimation(List<GameObject> letters)
    {

        if (!isAnimating)
        {
            isAnimating = true;

            List<Vector3> positions = new List<Vector3>();

            foreach (var obj in letters)
            {
                positions.Add(obj.transform.position);
            }
            Queue<Vector3> randomPositions = new Queue<Vector3>();

            while (randomPositions.Count != positions.Count)
            {
                int randomIndex = Random.Range(0, positions.Count);
                Vector3 randomPos = positions[randomIndex];

                if (!randomPositions.Contains(randomPos))
                {
                    randomPositions.Enqueue(randomPos);
                }

            }
            StartCoroutine(RandomizeLetterAnimation(letters, randomPositions));
        }
       
    }


    IEnumerator RandomizeLetterAnimation(List<GameObject> letters , Queue<Vector3> randomPositions)
    {

        Vector3 firstDestinationPoint = new Vector3(0, -2, 0);

        foreach (var letter in letters)
        {
            var mover = letter.transform;

            while (mover.position != firstDestinationPoint)
            {
                mover.position = Vector3.MoveTowards(
                    mover.position,
                    firstDestinationPoint,
                    15f * Time.deltaTime);
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.3f);

        foreach (var letter in letters)
        {
            var mover = letter.transform;
            var destination = randomPositions.Dequeue();
            while (mover.position != destination)
            {
                mover.position = Vector3.MoveTowards(
                    mover.position,
                    destination,
                    15f * Time.deltaTime);
                yield return null;
            }
        }


        isAnimating = false;

    }

}
