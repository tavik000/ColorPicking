using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject colorPanel;

    public AudioSource correctSound;
    public AudioSource wrongSound;

    public Text scoreEquationText;
    public Text comboText;
    public Text highscore;

    [SerializeField] private Image crossImage;
    [SerializeField] private Image tickImage;

    public GameObject[] pen;


    public bool[] penIsClicked = new bool[5];


    // Game Balance Value
    public float initRandomScore;

    bool gameOver = false;

    int score = 0;
    int combo = 1;
    int guessTime = 0;

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

        ValueInit();

        colorPanel.GetComponent<ColorPanel>().Init();

        // Generate 5 pens and brand of "Cantsee"

        GeneratePen();
    }

    void ValueInit()
    {
        // Init Value
        for (var i = penIsClicked.Length - 1; i >= 0; i--)
        {
            penIsClicked[i] = false;
        }

        score = 0;
        combo = 1;
        guessTime = 0;

        gameOver = false;
    }

    public void GameOver()
    {
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highscore.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
        }

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
        int adding; // Add score

        //print(penNumber);
        //print(answer);

        guessTime++;
        penIsClicked[penNumber] = true;
        pen[penNumber].GetComponent<PenHandler>().cantSee.gameObject.active = false;

        if (correctAnswer[penNumber] == answer)
        {
            print("you are right");

            correctSound.Play();
            tickImage.enabled = true;
            StartCoroutine("CountdownCloseImage");

            if ( combo >= 2)
            {
                comboText.text = "COMBO " + combo.ToString();
            }

            adding = 1000 * combo;
            combo++;
            AddScore(adding);

        }else{

            print("you are wrong");

            wrongSound.Play();
            crossImage.enabled = true;
            StartCoroutine("CountdownCloseImage");

            comboText.text = "";

            adding = -500;
            combo = 1;
            AddScore(adding);

        }


        // Check Gameover
        if (guessTime >= pen.Length)
        {
            //GameOver
            score = score + Mathf.RoundToInt(Random.Range(0.0f, initRandomScore));
            scoreEquationText.text = scoreEquationText.text + " = " + score.ToString();
            GameOver();
        }
    }

    IEnumerator CountdownCloseImage()
    {
        
        yield return new WaitForSeconds(0.75f);

        tickImage.enabled = false;
        crossImage.enabled = false;
    }


    void AddScore(int adding)
    {
        score = score + adding;
        print("Adding: " + adding);
        print("Score: " + score);

        if( adding > 0)
        {
            scoreEquationText.text = scoreEquationText.text + " <color=green>+ " + adding.ToString() + "</color>";
        }
        else
        {
            scoreEquationText.text = scoreEquationText.text + " <color=red>- " + Mathf.Abs(adding).ToString() + "</color>";
        }

    }


}
