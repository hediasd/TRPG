
using UnityEngine;
using System.Collections.Generic;

public class DragCamera : MonoBehaviour
{


    Vector3 ResetCamera; // original camera position
    Vector3 Origin; // place where mouse is first pressed
    Vector3 Diference; // change in position of mouse relative to origin

    void Start()
    {
        ResetCamera = Camera.main.transform.position;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Origin = MousePos();
        }

        if (Input.GetMouseButton(0))
        {
            Diference = MousePos() - transform.position;
            transform.position = Origin - Diference;
        }
		
        if (Input.GetMouseButton(1)) // reset camera to original position
        {
            transform.position = ResetCamera;
        }
    }
    // return the position of the mouse in world coordinates (helper method)
    Vector3 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}