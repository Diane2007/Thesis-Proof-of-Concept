using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputManager : MonoBehaviour
{
    Camera _mainCamera;

    //set main camera before game starts
    void Awake()
    {
        _mainCamera = Camera.main;
    }

    //check we are clicking a game object
    public void OnClick(InputAction.CallbackContext docClick)
    {
        //do nothing if player isn't clicking
        if (!docClick.started)
        {
            return;
        }
        
        //define and init our ray
        RaycastHit2D rayHit = Physics2D.GetRayIntersection
            (_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        
        //if player isn't clicking on an object, do nothing
        if (!rayHit.collider)
        {
            return;
        }
        //show the object name in console
        Debug.Log(rayHit.collider.gameObject.name);
    }
}
