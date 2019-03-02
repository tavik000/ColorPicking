using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPanel : MonoBehaviour
{
    public GameObject colorPanel;

    public void OpenColorPanel()
    {
        colorPanel.SetActive(true);
    }
}
