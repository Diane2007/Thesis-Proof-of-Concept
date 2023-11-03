using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake()
    {
        if (!instance)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;    //set instance to this gameObject
        }
        else
        {
            Destroy(gameObject);
        }
    }

    int _susLv;

    //calculate player's sus level
    public int SusLv
    {
        get { return _susLv; }
        set
        {
            _susLv = value;
            _susLv++;
        }
    }
    
    void Start()
    {
        //these things shouldn't appear at game start
        InputManager.instance.CloseAll();
        //and don't show the special protocol text
        
    }

    /*
    public void OpenCloseDocuments()
    {
        //if player clicks on the close button
        //close all open documents
        if (InputManager.instance.rayHit.collider.gameObject.name == closeButton.name)
        {
            Debug.Log("Now hitting closeButton!");
            CloseAll();
        }
        //if player clicks on the closed handbook
        //open the protocol page and hide the closed binder
        else if (InputManager.instance.rayHit.collider.gameObject.name == closedBinder.name)
        {
            Debug.Log("Now hitting closedBinder!");
            protocols.SetActive(true);
            closedBinder.SetActive(false);
        }
    }
    */


}
