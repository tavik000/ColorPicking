using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPanel : MonoBehaviour
{
    public GameObject colorPanel;
    GameManager gm;

    public Button pinkButton;
    public Button purpleButton;
    public Button blueButton;
    public Button lightGreenButton;
    public Button orangeButton;

    private int currentPenID;

   
    public void Init()
    {
        gm = GameManager.Instance;
        pinkButton.onClick.AddListener(delegate { ButtonOnclick(0); });
        purpleButton.onClick.AddListener(delegate { ButtonOnclick(1); });
        blueButton.onClick.AddListener(delegate { ButtonOnclick(2); });
        lightGreenButton.onClick.AddListener(delegate { ButtonOnclick(3); });
        orangeButton.onClick.AddListener(delegate { ButtonOnclick(4); });
    }


    public void OpenColorPanel(int penID)
    {
        currentPenID = penID;


        //print(currentPenID);

        // Adjust Panel Position

        Vector3 penPos = new Vector3(
                                    gm.pen[currentPenID].transform.position.x - 1, 
                                    gm.pen[currentPenID].transform.position.y + 3,
                                    gm.pen[currentPenID].transform.position.z
                                    );

        Vector3 panelPos = Camera.main.WorldToScreenPoint(penPos);


        colorPanel.transform.position = panelPos;

        colorPanel.SetActive(true);

    }

    public void ButtonOnclick(int buttonID)
    {
        CloseColorPanel();
        gm.CheckAnswer(currentPenID, buttonID);
    }

    public void CloseColorPanel()
    {
        colorPanel.SetActive(false);
    }

}
