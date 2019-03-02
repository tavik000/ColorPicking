using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPanel : MonoBehaviour
{
    public GameObject colorPanel;

    public Button pinkButton;
    public Button purpleButton;
    //public Button pinkButton;
    //public Button pinkButton;
    //public Button pinkButton;

    void Start()
    {
        print("runing");

        pinkButton.onClick.AddListener(delegate { ButtonOnclick(0); });
        purpleButton.onClick.AddListener(delegate { ButtonOnclick(1); });
    }


    public void OpenColorPanel()
    {
        colorPanel.SetActive(true);
    }

    public void ButtonOnclick(int buttonID)
    {
        print(buttonID);
        colorPanel.SetActive(false);
    }

}
