using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject colorPanel;

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
        correctAnswer[0] = 4;
        correctAnswer[1] = 1;
        correctAnswer[2] = 2;
        correctAnswer[3] = 3;
        correctAnswer[4] = 0;
    }

    public void CheckAnswer(int penNumber, int answer)
    {
        print(penNumber);
        print(answer);
        if (correctAnswer[penNumber] == answer)
        {
            print("you are right");
        }else{
            print("you are wrong");
        }
    }
}
