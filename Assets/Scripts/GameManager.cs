using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject colorPanel;


    public GameObject[] pen;


    public bool[] penIsClicked = new bool[5];
    bool gameOver = false;

    int score = 0;
    int combo = 0;

    int [] correctAnswer = new int[5];

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameStart()
    {
        gameOver = false;

        colorPanel.GetComponent<ColorPanel>().Init();

        // Generate 5 pens and brand of "Cantsee"

        GeneratePen();
    }

    public void GameOver()
    {
        gameOver = true;
        score = 0;
    }

    void GeneratePen()
    {


        // Init Value
        for (var i = penIsClicked.Length - 1; i >= 0; i--)
        {
            penIsClicked[i] = false;
        }

        correctAnswer[0] = 4;
        correctAnswer[1] = 1;
        correctAnswer[2] = 2;
        correctAnswer[3] = 3;
        correctAnswer[4] = 0;

        // Shuffle

        for (var i = correctAnswer.Length - 1; i >= 0; i--)
        {
            var r = Random.Range(0, i);
            var temp = correctAnswer[i];
            correctAnswer[i] = correctAnswer[r];
            correctAnswer[r] = temp;
        }

        //foreach (var answer in correctAnswer)
        //{
        //    Debug.Log(answer);
        //}

        // Display the Image of Penbody

        for (int i = pen.Length - 1; i >= 0; i--)
        {
            foreach (Transform penPic in pen[i].GetComponent<PenHandler>().penBodyPic)
            {
                penPic.gameObject.active = false;
            }
            pen[i].GetComponent<PenHandler>().cantSee.gameObject.active = true;
            pen[i].GetComponent<PenHandler>().penBodyPic[correctAnswer[i]].gameObject.active = true;
        }

    }

    public void CheckAnswer(int penNumber, int answer)
    {
        print(penNumber);
        print(answer);


        penIsClicked[penNumber] = true;
        pen[penNumber].GetComponent<PenHandler>().cantSee.gameObject.active = false;

        if (correctAnswer[penNumber] == answer)
        {
            print("you are right");
        }else{
            print("you are wrong");
        }
    }



}
