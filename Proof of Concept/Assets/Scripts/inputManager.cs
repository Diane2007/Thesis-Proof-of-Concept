using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    Camera mainCamera;
    //this stores the name player clicks on
    string rayHitName;
    RaycastHit2D rayHit;

    public static InputManager instance;

    //things on the desk
    [Header("Default Desk Objects")] public GameObject teaCup;
    public GameObject smallBrief;
    public GameObject closedBinder;
    public GameObject phone;

    [Header("Show only after clicked")]
    //only when the closed binder is clicked on, the giant document opens
    public GameObject govList;
    public GameObject legitNews;
    public GameObject protocols;

    [Space(10)] public GameObject closeButton;
    public GameObject protocolButton;
    public GameObject newspaperButton;
    public GameObject officialButton;

    [Header("Text Stuff")] public GameObject specialProtocol;
    public GameObject newsTitle, newsSubtitle, newsText, newsName;
    
    //set main camera before game starts
    void Awake()
    {
        mainCamera = Camera.main;

        if (!instance)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
        rayHit = Physics2D.GetRayIntersection
            (mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        
        
        //if player isn't clicking on a collider, do nothing
        if (!rayHit.collider)
        {
            return;
        }
        //show the object name in console
        rayHitName = rayHit.collider.gameObject.name;
        Debug.Log(rayHitName);
        DocumentControl();

    }

    void DocumentControl()
    {
        //if player clicks on the close button
        //close all open documents and show the newspaper text
        if (rayHitName == closeButton.name)
        {
            CloseAll();
            ResetDesk();
            
        }

        //if player clicks on the closed binder
        //open the big binder and hide the small one
        if (rayHitName == closedBinder.name)
        {
            closedBinder.SetActive(false);
            protocols.SetActive(true);
            specialProtocol.SetActive(true);
            OnOffNewspaperText(false);
            
            //show the close button!!
            closeButton.SetActive(true);
        }
    }

    //are we going to show all the newspaper text? or disable all of them?
    void OnOffNewspaperText(bool state)
    {
        newsSubtitle.SetActive(state);
        newsText.SetActive(state);
        newsTitle.SetActive(state);
        newsName.SetActive(state);
    }

    //close all open document stuff
    public void CloseAll()
    {
        closeButton.SetActive(false);
        protocolButton.SetActive(false);
        newspaperButton.SetActive(false);
        officialButton.SetActive(false);
        govList.SetActive(false);
        legitNews.SetActive(false);
        officialButton.SetActive(false);
        protocols.SetActive(false);
        specialProtocol.SetActive(false);
    }

    //show everything that's supposed to be on the desk
    void ResetDesk()
    {
        smallBrief.SetActive(true);
        closedBinder.SetActive(true);
        //show newspaper content
        OnOffNewspaperText(true);
    }
}
