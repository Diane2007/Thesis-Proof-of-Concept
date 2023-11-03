using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public static GameManager Instance;

    void Awake()
    {
        if (!Instance)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;    //set instance to this gameObject
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        //these things shouldn't appear at game start
        closeButton.SetActive(false);
        protocolButton.SetActive(false);
        newspaperButton.SetActive(false);
        officialButton.SetActive(false);
        govList.SetActive(false);
        legitNews.SetActive(false);
        officialButton.SetActive(false);
        protocols.SetActive(false);
    }

    public void OpenCloseDocuments()
    {
        
    }
    
}
