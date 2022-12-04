using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePosition : MonoBehaviour
{
    public Image pointer;
    public Image pointerPressed;
    public bool isPressed;

    public float cameraToCanvasDistance;

    public bool show;

    public Camera cam;

    void Start()
    {
        show = false;
        
        
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!show)
            {
                show=true;
                Cursor.visible = false;
               
            }
            else{
            show=false;
            Cursor.visible = true;
            }

            
        }

        if (Input.GetMouseButtonDown(0))
            isPressed = true;

        if (Input.GetMouseButtonUp(0))
            isPressed = false;

        if (isPressed && show)
        {
            pointer.gameObject.SetActive(false);
            pointerPressed.gameObject.SetActive(true);
        }
        else if (!isPressed && show)
        {
            pointer.gameObject.SetActive(true);
            pointerPressed.gameObject.SetActive(false);
        }
        else
        {
            pointer.gameObject.SetActive(false);
            pointerPressed.gameObject.SetActive(false);
        }

        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = cameraToCanvasDistance; //distance of the plane from the camera
        pointer.gameObject.transform.position = cam.ScreenToWorldPoint(screenPoint);
        pointerPressed.gameObject.transform.position = cam.ScreenToWorldPoint(screenPoint);
    }
}