﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicker : MonoBehaviour
{
    public GameObject colorPanel;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null && hit.transform.gameObject.tag == "Pen")
            {
                OpenPanel(hit.transform.gameObject);
            }

        }

    }

    private void OpenPanel(GameObject clickObject)
    {
        int currentPenID = -1;

        if (clickObject.name == "Pen1")
        {
            currentPenID = 0;
        }
        colorPanel.GetComponent<ColorPanel>().OpenColorPanel(currentPenID);
    }
}
