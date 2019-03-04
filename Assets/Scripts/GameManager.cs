using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject colorPanel;
    public GameObject askQuestionPanel;

    public GameObject gameOverPage;

    public AudioSource correctSound;
    public AudioSource wrongSound;

    public Text scoreEquationText;
    public Text comboText;
    public Text highscore;
    public Text popupText;
    

    [SerializeField] Image crossImage;
    [SerializeField] Image tickImage;

    public GameObject[] pen;


    public bool[] penIsClicked = new bool[5];


    public int questionAnsSamePenColorCount;
    public int questionAnsPairPen;

    // Game Balance Value
    public float initRandomScore;

    public bool gameOver;

    bool isLastPenEventGeneratePen = false; // is in the last pen event create new pen or not
    int lastPenEventGeneratePenCount;


    int score = 0;
    int combo = 1;
    int guessTime = 0;

    int [] correctAnswer = new int[5];

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        colorPanel.GetComponent<ColorPanel>().Init();

        GameStart();
    }

    void GameStart()
    {
        gameOver = true; // gameOver set false after asked question

        gameOverPage.SetActive(false);

        ValueInit();


        // Generate 5 pens and brand of "Cantsee"

        GeneratePen();
        PrepareQuestionAnswer();

        StartAskQuestion();
    }

    public void Restart()
    {
        GameStart();
    }

    void PrepareQuestionAnswer()
    {
        for (var i = correctAnswer.Length - 1; i >= 0; i--)
        {
            if (correctAnswer[i] == i)
            {
                questionAnsSamePenColorCount++;
            }
            else if (correctAnswer[correctAnswer[i]] == i )
            {
                questionAnsPairPen++;
            }
        }
        questionAnsPairPen /= 2;

        //print("questionAnsSamePenColorCount: " + questionAnsSamePenColorCount);
        //print("questionAnsPairPen: " + questionAnsPairPen);
    }


    void StartAskQuestion()
    {
        askQuestionPanel.SetActive(true);
    }

    public void AskSamePenColorCount()
    {
        popupText.text = "有 " + questionAnsSamePenColorCount + "枝 同色筆";
        popupText.gameObject.SetActive(true);
        StartCoroutine(CountdownCleanPopupText(5.0f));
        askQuestionPanel.SetActive(false);

        gameOver = false;

    }

    public void AskPairPenCount()
    {
        popupText.text = "有 " + questionAnsPairPen + "對 對色筆";
        popupText.gameObject.SetActive(true);
        StartCoroutine(CountdownCleanPopupText(5.0f));
        askQuestionPanel.SetActive(false);

        gameOver = false;

    }




    void ValueInit()
    {
        // Init Value
        for (var i = penIsClicked.Length - 1; i >= 0; i--)
        {
            penIsClicked[i] = false;
        }

        questionAnsSamePenColorCount = 0;
        questionAnsPairPen = 0;

        score = 0;
        combo = 1;
        guessTime = 0;
        lastPenEventGeneratePenCount = 0;
        isLastPenEventGeneratePen = false;

        scoreEquationText.text = "Score: Random(5000)";
        comboText.text = "";
    }

    public void GameOver()
    {

        scoreEquationText.text = scoreEquationText.text + " = " + score.ToString();



        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highscore.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
        }

        gameOver = true;
        score = 0;

        gameOverPage.SetActive(true);
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
            var r = Random.Range(0, i + 1);
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
            //print("you are right");

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

            //print("you are wrong");

            wrongSound.Play();
            crossImage.enabled = true;
            StartCoroutine("CountdownCloseImage");

            comboText.text = "";

            adding = -500;
            combo = 1;
            AddScore(adding);

        }


        if (isLastPenEventGeneratePen)
        {
            if (guessTime >= lastPenEventGeneratePenCount)
            {
                // GameOver
                GameOver();
            }
        }
        else if (guessTime >= pen.Length)
        {
            // Check Player Finish

            AddInitScore();

            // Last Pen Event
            LastPenEvent(correctAnswer[penNumber]);

        }
    }

    void LastPenEvent(int lastPenID)
    {
        switch (lastPenID)
        {
            case 0:
                score += 4000;
                scoreEquationText.text = scoreEquationText.text + " <color=magenta>+ " + "4000" + "</color>";

                // GameOver
                GameOver();
                break;
            case 1:
                score -= 2000;
                scoreEquationText.text = scoreEquationText.text + " <color=purple>- " + "2000" + "</color>";

                // GameOver
                GameOver();
                break;
            case 2:
                lastPenEventGeneratePenCount = 2;
                LastPenEventGeneratePen(lastPenEventGeneratePenCount);
                break;
            case 3:
                score = Mathf.RoundToInt(score * 1.75f);

                string x = scoreEquationText.text.Replace("Score:", string.Empty);

                scoreEquationText.text = "Score: <color=lime>(</color>" + x + " <color=lime>) x " + "1.75" + "</color>";

                // GameOver
                GameOver();
                break;
            case 4:
                lastPenEventGeneratePenCount = 4;
                LastPenEventGeneratePen(lastPenEventGeneratePenCount);
                break;
            default:
                Debug.Log("Error lastPenID: " + lastPenID);
                break;
        }
    }

    void LastPenEventGeneratePen(int genPenCount)
    {
        isLastPenEventGeneratePen = true;
        guessTime = 0;



        for (var i = genPenCount - 1; i >= 0; i--)
        {
            penIsClicked[i] = false;
        }

        if (genPenCount == 2)
        {
            correctAnswer[0] = 1;
            correctAnswer[1] = 0;
            popupText.text = "<color=blue>再猜" + genPenCount + "枝筆</color>";
        }

        if (genPenCount == 4)
        {
            correctAnswer[0] = 1;
            correctAnswer[1] = 3;
            correctAnswer[2] = 2;
            correctAnswer[3] = 0;
            popupText.text = "<color=orange>再猜" + genPenCount + "枝筆</color>";
        }
       
        popupText.gameObject.SetActive(true);
        
        StartCoroutine(CountdownCleanPopupText(1.5f));

        // Shuffle


        for (var i = genPenCount - 1; i >= 0; i--)
        {
        
            var r = Random.Range(0, i + 1);

            //print("i: " + i);
            //print("r: " + r);

            var temp = correctAnswer[i];
            correctAnswer[i] = correctAnswer[r];
            correctAnswer[r] = temp;
        }


        for (int i = pen.Length - 1; i >= 0; i--)
        {
            foreach (Transform penPic in pen[i].GetComponent<PenHandler>().penBodyPic)
            {
                penPic.gameObject.active = false;
            }
            if ( i <= genPenCount - 1)
            {
                pen[i].GetComponent<PenHandler>().cantSee.gameObject.active = true;
                pen[i].GetComponent<PenHandler>().penBodyPic[correctAnswer[i]].gameObject.active = true;
            }
        }

    }

    void AddInitScore()
    {
        int randomScore = Mathf.RoundToInt(Random.Range(0.0f, initRandomScore));
        score = score + randomScore;
        print("randomScore: " + randomScore);
    }


    IEnumerator CountdownCloseImage()
    {
        
        yield return new WaitForSeconds(0.75f);

        tickImage.enabled = false;
        crossImage.enabled = false;

    }

    IEnumerator CountdownCleanPopupText(float time)
    {
        yield return new WaitForSeconds(time);

        popupText.text = "";
        popupText.gameObject.SetActive(false);

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


    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
