using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

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

    [Header("Text Stuff")] public GameObject specialProtocol;
    public GameObject newsTitle, newsSubtitle, newsText, newsName;
    
    //I know it's crazy man but here are the handbook buttons
    [Header("Handbook Buttons")] public GameObject protocolButtonLeft;
    public GameObject newspaperButtonLeft, officialButtonLeft;
    [Space(10)] public GameObject newspaperButtonRight;
    public GameObject officialButtonRight;
    [Space(10)] public GameObject closeButton;
    
    //colliders for teacup and phone
    Collider2D teacupCol, phoneCol;
    
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

    void Start()
    {
        //init variables
        teacupCol = teaCup.GetComponent<Collider2D>();
        phoneCol = phone.GetComponent<Collider2D>();
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
            Debug.Log("Not clicking on any collider!");
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
            //turn off the newspaper text first
            ShowNewspaperText(false);
            
            //turn off the collider for teacup and phone
            //because we don't want to click on them right now
            teacupCol.enabled = false;
            phoneCol.enabled = false;

            //turn off the binder and show the protocol page
            closedBinder.SetActive(false);
            ShowProtocol(true);
            
        }

        //if player clicks on the brief, bring it to front
        if (rayHitName == smallBrief.name)
        {
            SpriteRenderer rend = rayHit.collider.gameObject.GetComponent<SpriteRenderer>();
            rend.sortingOrder = 2;
            smallBrief.GetComponent<SpriteRenderer>().sortingOrder = rend.sortingOrder;
        }

        //if player clicks on the newspaper list button
        if (rayHitName == newspaperButtonLeft.name || rayHitName == newspaperButtonRight.name)
        {
            //turn off newspaper text
            //ShowNewspaperText(false);
            
            //show the newspaper list page and buttons
            ShowNewsList(true);
        }
        
        //if player clicks on the protocol button
        if (rayHitName == protocolButtonLeft.name)
        {
            //turn off newspaper text
            //ShowNewspaperText(false);
            
            //show the protocol page
            ShowProtocol(true);
        }
        
        //if player clicks on the gov official list button
        if (rayHitName == officialButtonLeft.name || rayHitName == officialButtonRight.name)
        {
            //turn off newspaper text
            //ShowNewspaperText(false);
            
            //show the gov official list
            ShowOfficialList(true);
        }
    }

    //are we going to show all the newspaper text? or disable all of them?
    void ShowNewspaperText(bool state)
    {
        newsSubtitle.SetActive(state);
        newsText.SetActive(state);
        newsTitle.SetActive(state);
        newsName.SetActive(state);
    }

    //close all open document stuff
    public void CloseAll()
    {
        //close all the buttons
        closeButton.SetActive(false);
        protocolButtonLeft.SetActive(false);
        newspaperButtonLeft.SetActive(false);
        newspaperButtonRight.SetActive(false);
        officialButtonLeft.SetActive(false);
        officialButtonRight.SetActive(false);
        
        //close all handbook content
        govList.SetActive(false);
        legitNews.SetActive(false);
        protocols.SetActive(false);
        specialProtocol.SetActive(false);
    }
    
    void ShowProtocol(bool state)
    {

        //turn on the protocol page
        if (state)
        {
            //if the other pages are active, hide them
            if (legitNews.activeSelf)
            {
                ShowNewsList(false);
            }
            if (govList.activeSelf)
            {
                ShowOfficialList(false);
            }
            
            //show the protocol page and text
            protocols.SetActive(true);
            specialProtocol.SetActive(true);

            //show the protocol page buttons
            closeButton.SetActive(true);
            protocolButtonLeft.SetActive(true);
            newspaperButtonRight.SetActive(true);
            officialButtonRight.SetActive(true);
        }
        //turn off the protocol page
        else
        {
            //hide the protocol page and text
            protocols.SetActive(false);
            specialProtocol.SetActive(false);
            
            //hide the protocol page buttons
            closeButton.SetActive(false);
            protocolButtonLeft.SetActive(false);
            newspaperButtonRight.SetActive(false);
            officialButtonRight.SetActive(false);
        }
    }

    void ShowNewsList(bool state)
    {
        //show legitimate newspaper list
        if (state)
        {
            //hide protocol and official pages if they are active
            if (protocols.activeSelf)
            {
                ShowProtocol(false);
            }

            if (govList.activeSelf)
            {
                ShowOfficialList(false);
            }
            
            //turn on the legitimate news page
            legitNews.SetActive(true);
            
            //turn on all the news list page buttons
            closeButton.SetActive(true);
            protocolButtonLeft.SetActive(true);
            newspaperButtonLeft.SetActive(true);
            officialButtonRight.SetActive(true);
        }
        //turn off the legitimate newspaper list
        else
        {
            //hide the news list page
            legitNews.SetActive(false);
            
            //turn off all the news list page buttons
            closeButton.SetActive(false);
            protocolButtonLeft.SetActive(false);
            newspaperButtonLeft.SetActive(false);
            officialButtonRight.SetActive(false);
        }
    }

    void ShowOfficialList(bool state)
    {
        //turn official list on
        if (state)
        {
            //hide official and protocol list if they are on
            if (legitNews.activeSelf)
            {
                legitNews.SetActive(false);
            }

            if (protocols.activeSelf)
            {
                protocols.SetActive(false);
            }
            
            //turn on the gov official list
            govList.SetActive(true);
            
            //turn on all the news page buttons
            closeButton.SetActive(true);
            protocolButtonLeft.SetActive(true);
            officialButtonLeft.SetActive(true);
            newspaperButtonLeft.SetActive(true);
        }

        //hide official list  
        else
        {
            govList.SetActive(false);
            closeButton.SetActive(false);
            protocolButtonLeft.SetActive(false);
            officialButtonLeft.SetActive(false);
            newspaperButtonLeft.SetActive(false);
        }
    }
    

    //show everything that's supposed to be on the desk
    void ResetDesk()
    {
        //turn on the teacup and phone colliders
        teacupCol.enabled = true;
        phoneCol.enabled = true;
        
        //turn on the small closed binder
        closedBinder.SetActive(true);
        //show newspaper content
        ShowNewspaperText(true);
    }
}