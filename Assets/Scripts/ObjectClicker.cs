using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectClicker : MonoBehaviour
{
    public GameObject colorPanel;

    GameManager gm;


    // For Canvas Click
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;


    private void Start()
    {
        gm = GameManager.Instance;

        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            //foreach (RaycastResult result in results)
            //{
            //    Debug.Log("Hit " + result.gameObject.name);
            //}



            // If it is not a UI Click then Detect 3D Object Click
            if ( results.Count == 0 )
            {

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.transform.gameObject.tag == "Pen")
                    {
                        OpenPanel(hit.transform.gameObject);
                    }
                }
                else
                {
                    CloseColorPanel();
                }

            }




        }

    }

    private void OpenPanel(GameObject clickObject)
    {
        int currentPenID = -1;


        for (int i = gm.pen.Length - 1; i >= 0; i--)
        {
            if (clickObject == gm.pen[i])
            {
                currentPenID = i;
            }
        }

        if (!gm.penIsClicked[currentPenID])
        {
            colorPanel.GetComponent<ColorPanel>().OpenColorPanel(currentPenID);
        }

    }

    private void CloseColorPanel()
    {
        //print("close by other");
        colorPanel.GetComponent<ColorPanel>().CloseColorPanel();
    }
}
