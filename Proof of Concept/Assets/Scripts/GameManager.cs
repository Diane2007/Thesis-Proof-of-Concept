using System;
using System.IO;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int newsCount;
    
    //connect all text files
    public TextMeshProUGUI newsTitle, newsSubtitle, newspaper, newsText;

    //load text stuff
    const string TEXT_NAME = "textNum.txt";
    const string TEXT_DIR = "/Resources/Texts/";
    string TEXT_PATH;
    string newTextPath;

    //int lineIndex = 0;
    string[] fileLines;
    
    //check if there is a special protocol
    bool specialProtocolExists = false;
    
    //answer and player choice related variables
    List<int> choice_Violation, answer_Violation;
    bool choice_YesNo, answer_YesNo;
    
    //connect the dropdown menus here
    [Header("Dropdown Menus")] public TMP_Dropdown boolDropdown;
    public TMP_Dropdown protocolDropdown;

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
    public TextMeshProUGUI specialProtocolText;

    //calculate player's sus level
    int susLv;
    public int SusLv
    {
        get { return susLv; }
        set
        {
            susLv = value;
            susLv++;
        }
    }

    void Start()
    {
        //these things shouldn't appear at game start
        InputManager.instance.CloseAll();
        //and don't show the special protocol text
        specialProtocolText.text = "";
        
        //right now we have a news count of 3.
        newsCount = 3;
        
        //init file path
        TEXT_PATH = Application.dataPath + TEXT_DIR + TEXT_NAME;
        
        //clear all text at scene starts
        ClearNews();
        
        //init the list
        choice_Violation = new List<int>();
        answer_Violation = new List<int>();

        //now load the first newspaper
        LoadNews();
    }

    int currentNewsFile = 0;

    public int CurrentNewsFile
    {
        get { return currentNewsFile; }
        set
        {
            currentNewsFile = value;
            
            //we only have this many news every day, so this is to ensure we only load this much
            if (currentNewsFile < newsCount)
            {
                //reset player answer's bool and list
                choice_Violation.Clear();
                choice_YesNo = false;
                
                //load next news
                LoadNews();
            }
            else
            {
                Debug.Log("No more news today!");
            }
        }
    }

    void LoadNews()
    {
        //init the new file path
        newTextPath = TEXT_PATH.Replace("Num", currentNewsFile + "");
        
        //put each line of that text file into one array
        fileLines = File.ReadAllLines(newTextPath);

        //and show the news text
        ShowNewsText();

    }

    void ShowNewsText()
    {
        string debugString = "";
        
        //the reading the file lines for loop stuff
        //lineIndex = 0;  //start with line index 0
        
        //clear the previous news just in case
        ClearNews();

        Debug.Log("The number of lines in this document is: " + fileLines.Length);
        for (int lineNum = 0; lineNum < fileLines.Length; lineNum++)
        {
            //every line of content is here
            string lineContent = fileLines[lineNum];

            //the first line is the news title
            if (lineNum == 0)
            {
                newsTitle.text = lineContent;
            }
            //the second line is the subtitle
            else if (lineNum == 1)
            {
                newsSubtitle.text = lineContent;
            }
            //the third line is the publisher
            else if (lineNum == 2)
            {
                newspaper.text = lineContent;
            }
            //the fourth line is always empty, we don't want that line
            //line 5 is the news story
            else if(lineNum == 4)
            {
                newsText.text = lineContent;
            }
            //line 6 is empty and we don't want that
            //line 7 is if the answer of whether there is news violation in this news
            else if (lineNum == 6)
            {
                //if that line says "t", then answer of if there is violation is true
                if (lineContent == "t")
                {
                    answer_YesNo = true;
                }
                //if that line says "f", there is no violation
                else if (lineContent == "f")
                {
                    answer_YesNo = false;
                }
                Debug.Log("Protocol violation in this article? " + answer_YesNo);
            }
            //line 8 is the protocols this news violated (if there is violation)
            else if (lineNum == 7)
            {
                //parse that line into an array, separated by comma
                //but before that don't forget to trim all the spaces
                string[] answer_ProtocolArray = lineContent.Trim().Split(',');
                    
                //now pars this string array into an int array
                for (int i = 0; i < answer_ProtocolArray.Length; i++)
                {
                     answer_Violation.Add(Int32.Parse(answer_ProtocolArray[i]));
                     debugString += answer_ProtocolArray[i] + " ";
                }
                Debug.Log("The answer for this news is " + debugString);
                    
            }
        }
    }

    void ClearNews()
    {
        newsTitle.text = string.Empty;
        newsSubtitle.text = string.Empty;
        newspaper.text = string.Empty;
        newsText.text = string.Empty;
    }

    //player's first choice: is there protocol violation?
    public void YesOrNoDropdown()
    {
        int index = boolDropdown.value;
        switch (index)
        {
            case 0: //text is no
                choice_YesNo = false;
                break;
            case 1: //text is yes
                choice_YesNo = true;
                break;
        }
    }
    
    //TODO: think about when to clear this list!! when to compare this list!!
    //player's multiple choice: if there is violation, what protocol(s) does it violate?
    
    public void ViolationDropdown()
    {
        int index = protocolDropdown.value;
        switch (index)
        {
            case 0:     //default: no choice
                choice_Violation.Add(0);
                Debug.Log("Player chooses protocol " + choice_Violation[0] + "for the violation!");
                break;
            case 1:     //protocol 1
                choice_Violation.Add(1);
                break;
            case 2:     //protocol 2
                choice_Violation.Add(2);
                break;
            case 3:     //protocol 3
                choice_Violation.Add(3);
                break;
            case 4:     //special protocol (4)
                choice_Violation.Add(4);
                break;
        }
    }

    public void DropdownMenuControl()
    {
        int boolChoice = boolDropdown.value;
        int protocolChoice = protocolDropdown.value;

        switch (boolChoice)
        {
            case 0: //text is no
                choice_YesNo = false;
                Debug.Log("Player thinks if there is violation: " + choice_YesNo);
                break;
            case 1: //text is yes
                choice_YesNo = true;
                Debug.Log("Player thinks if there is violation: " + choice_YesNo);
                break;
        }

        switch (protocolChoice)
        {
            case 0: //default: no selection
                choice_Violation.Add(0);
                break;
            case 1:
                choice_Violation.Add(1);
                break;
            case 2:
                choice_Violation.Add(2);
                break;
            case 3:
                choice_Violation.Add(3);
                break;
            case 4:
                choice_Violation.Add(4);
                break;
        }
        Debug.Log("Player chooses protocol number: " + protocolChoice);
    }

    public void JudgeButton()
    {
        string playerAnswerDebug = "";
        //choice_Violation.ToString().Trim();

        for (int i = 0; i < choice_Violation.Count; i++)
        {
            playerAnswerDebug += choice_Violation[i].ToString();
        }
        Debug.Log("Is violation?" + choice_YesNo + " Violation code is: " + playerAnswerDebug);
        

        //if the yes no player choice matches our answer
        //start checking if the protocol numbers match as well
        if (answer_YesNo == choice_YesNo)
        {
            //if the protocol numbers are correct, regardless the order
            if (choice_Violation.Intersect(answer_Violation).Any())
            {
                CurrentNewsFile++;  //load next news
            }
            else
            {
                //TODO: make a punishment sheet
                Debug.Log("You make a boo-boo!");
            }
        }
        //if player's yes no already doesn't match
        else
        {
            //TODO: make a punishment sheet
            Debug.Log("You make a boo-boo!");
        }
        
    }
    

}
