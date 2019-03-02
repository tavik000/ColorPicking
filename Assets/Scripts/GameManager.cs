using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    bool gameOver = false;

    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameStart()
    {
        gameOver = false;

        // Generate 5 pens and brand of "Cantsee"
    }

    public void GameOver()
    {
        gameOver = true;
        score = 0;
    }
}
