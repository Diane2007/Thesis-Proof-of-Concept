using System;
using System.IO;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int newsCount;

    //connect all text files
    public TextMeshProUGUI newsTitle, newsSubtitle, newspaper, newsText;

    //load text stuff
    const string TEXT_NAME = "textNum.txt";
    const string TEXT_DIR = "/Resources/";
    string TEXT_PATH;
    string newTextPath;
    TextAsset readNewsFile;

    //int lineIndex = 0;
    string[] fileLines;
    
    //check if there is a special protocol
    //bool specialProtocolExists = false;
    
    //answer and player choice related variables
    List<int> choice_Violation, answer_Violation;
    bool choice_YesNo, answer_YesNo;
    string debugProtocolChoice;
    bool areEqual;
    
    //connect the dropdown menus here
    [Header("Dropdown Menus")] public TMP_Dropdown boolDropdown;
    //public TMP_Dropdown protocolDropdown1, protocolDropdown2, protocolDropdown3;

    //Self-Criticism related stuff
    [Header("Self-Criticism Page")] public string replaceText;
    //if player copies exactly as is, these bools are true each time
    public bool apologyCorrect1, apologyCorrect2, apologyCorrect0;
    //check if player has made a mistake
    //bool wrongAnswer;
    public TextMeshProUGUI warningText, copyText;
    public TMP_InputField inputText;

    [Header("Phone related")] public GameObject phone;
    public GameObject dialogueBox;
    int convoLine = 0;
    public TextMeshProUGUI phoneText;
    //checking if we are having convo with florian
    bool isTalking;

    //control when phone rings and which phone text should show
    int phoneTurn = 0;

    public int PhoneTurn
    {
        get { return phoneTurn; }
        set
        {
            phoneTurn = value;
            phoneTurn++;
        }
    }
    
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
        phone.SetActive(false);
        dialogueBox.SetActive(false);
        phoneText.text = "";
        
        //and don't show the special protocol text
        specialProtocolText.text = "Any news relating to Florian Wilman's alleged sexual assault must be censored.";
        
        //right now we have a news count of 3.
        newsCount = 3;
        
        //init file path
        //TEXT_PATH = Application.dataPath + TEXT_DIR + TEXT_NAME;
        
        
        //clear all text at scene starts
        ClearNews();
        
        //init the list
        choice_Violation = new List<int>();
        answer_Violation = new List<int>();

        //now load the first newspaper
        LoadNews();
        
        //phone rings
        PickUpPhone();
        
    }

    int currentNewsFile = 0;

    public int CurrentNewsFile
    {
        get { return currentNewsFile; }
        set
        {
            //reset the two lists before each news starts
            answer_Violation.Clear();
            choice_Violation.Clear();
            
            currentNewsFile = value;
            
            //phone rings when:
            //1- player just starts reading the first news, this function is achieved in Start()
            //2- after player just starts reading the third news. this function is achieved here.
            if (currentNewsFile == 2)
            {
                PickUpPhone();
            }
            
            //we only have this many news every day, so this is to ensure we only load this much
            if (currentNewsFile < newsCount)
            {
                Debug.Log("Current news number:" + currentNewsFile);
                //reset player answer's bool and list
                choice_Violation.Clear();
                choice_YesNo = false;
                
                //reset the replaceText to empty
                replaceText = "";
                
                //load next news
                LoadNews();
            }
            else
            {
                ChangeScene(2);
            }
        }
    }

    void LoadNews()
    {
        //init the new file path
        newTextPath = "textNum".Replace("Num", currentNewsFile + "");
        //newTextPath = TEXT_PATH.Replace("Num", currentNewsFile + "");
        readNewsFile = Resources.Load(newTextPath) as TextAsset;


        //put each line of that text file into one array
        //fileLines = File.ReadAllLines(newTextPath);
        fileLines = readNewsFile.text.Split('\n');

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
                //Debug.Log("Protocol violation in this article? " + answer_YesNo);
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

    //TODO: think about when to clear this list!! when to compare this list!!
    //player's multiple choice: if there is violation, what protocol(s) does it violate?

    public void YesNoDropdown()
    {
        int boolChoice = boolDropdown.value;
        //player choice: is there a violation?
        switch (boolChoice)
        {
            case 0: //text is no
                choice_YesNo = false;
                //Debug.Log("Player thinks if there is violation: " + choice_YesNo);
                break;
            case 1: //text is yes
                choice_YesNo = true;
                //Debug.Log("Player thinks if there is violation: " + choice_YesNo);
                break;
        }

    }
    
    /*
    public void ProtocolMenuControl()
    {
        int protocolChoice1 = protocolDropdown1.value;
        int protocolChoice2 = protocolDropdown2.value;
        int protocolChoice3 = protocolDropdown3.value;
        
        //player choice: what protocols did it violate?
        switch (protocolChoice1)
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
        
        switch (protocolChoice2)
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
        
        switch (protocolChoice3)
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

        debugProtocolChoice = "" + protocolChoice1 + protocolChoice2 + protocolChoice3;

    }
    */

    public void JudgeButton()
    {
        string playerAnswerDebug = "";
        //choice_Violation.ToString().Trim();

        for (int i = 0; i < choice_Violation.Count; i++)
        {
            playerAnswerDebug += choice_Violation[i].ToString();
        }
        Debug.Log("Player choice -- Is violation? " + choice_YesNo + " Violation code is: " + playerAnswerDebug);

        areEqual = choice_Violation.OrderBy(x => x).SequenceEqual(answer_Violation.OrderBy(x => x));

        //if the yes no player choice matches our answer
        //start checking if the protocol numbers match as well
        if (answer_YesNo == choice_YesNo)
        {
            // if (areEqual)
            // {
            //     CurrentNewsFile++;
            // }
            // else
            // {
            //     Debug.Log("Protocol number doesn't match");
            //     //give player the self-criticism page
            //     //Debug.Log("Player chose protocols: " + debugProtocolChoice);
            //     SelfCriticism();
            // }
            CurrentNewsFile++;

        }
        //if player's yes no already doesn't match
        else
        {
            Debug.Log("IsViolation doesn't match");
            SelfCriticism();
        }
        
        
    }

    public void ChangeScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    void SelfCriticism()
    {
        //indicate player has made a wrong answer
        //wrongAnswer = true;
        
        //check what protocol related mistakes player made
        PlayerMistake();
        
        //leave nothing but the cup and self criticism on the desk
        InputManager.instance.ClearDesk();
        InputManager.instance.ShowSelfCriticism(true);


    }

    void PlayerMistake()
    {

        //if there is news violation and player thinks there aren't
        if (answer_YesNo && !choice_YesNo)
        {
            replaceText += "The news article contains violations.\n";
        }
        //if there is no news violation and player thinks there is
        else if (!answer_YesNo && choice_YesNo)
        {
            replaceText += "The news article does not contain violations.\n";
        }

        //check each answer in choice_Violation against the list of answer_Violation
        //and see if player chose a wrong protocol
        // for (int i = 0; i < choice_Violation.Count; i++)
        // {
        //     if (!answer_Violation.Contains(choice_Violation[i]))
        //     {
        //         int wrongProtocol = i + 1;
        //         replaceText += "Protocol " + wrongProtocol + " was not violated. ";
        //     }
        // }

        //check each answer in answer_Violation against the list of choice_violation
        //And see if player missed any protocols
        // for (int i = 0; i < answer_Violation.Count; i++)
        // {
        //     if (!choice_Violation.Contains(answer_Violation[i]))
        //     {
        //         int missedProtocol = i + 1;
        //         replaceText += "Protocol " + missedProtocol + " was violated. ";
        //     }
        // }
        
        
        //check if player missed any protocol violation
        // for (int i = 0; i < answer_Violation.Count; i++)
        // {
        //
        //     if (MissingProtocol(i))
        //     {
        //         replaceText += "Protocol " + i + " was violated. ";
        //
        //     } 
        // }
        //
        // //check if player thought a protocol is violated but it didn't
        // for (int i = 0; i < choice_Violation.Count; i++)
        // {
        //     if (WrongProtocol(i))
        //     {
        //         replaceText += "Protocol " + i + " was not violated. ";
        //     }
        // }
        
        //replace the <MistakeText> with the violation warning text.
        warningText.text = warningText.text.Replace("<MistakeText>", replaceText);
        
    }
    
    public void SubmitButton()
    {
        //check if player's input text is exactly same as what we tell them to copy
        if (copyText.text == inputText.text)
        {
            Debug.Log("Player typed in the exact same text!");
            //if it's the first (0) piece of news
            if (CurrentNewsFile == 0)
            {
                //the first apology correct bool (0) is true
                apologyCorrect0 = true;
            }
            else if (CurrentNewsFile == 1)
            {
                apologyCorrect1 = true;
            }
            else if (CurrentNewsFile == 2)
            {
                apologyCorrect2 = true;
            }
        }
        else
        {
            Debug.Log("Player typed in different text!");
        }

        //when we go back, we are going to load the new file
        CurrentNewsFile++;
        //close the page
        InputManager.instance.ShowSelfCriticism(false);
        InputManager.instance.ResetDesk();
        

    }


    // bool MissingProtocol(int chosenProtocol)
    // {
    //     //player didn't identify protocol breach
    //     if (!choice_Violation.Contains(chosenProtocol) && answer_Violation.Contains(chosenProtocol))
    //     {
    //         return true;
    //     }
    //     //player did identify protocol breach
    //     else if (choice_Violation.Contains(chosenProtocol) && answer_Violation.Contains(chosenProtocol))
    //     {
    //         return false;
    //     }
    //
    //     return false;
    // }
    //
    // bool WrongProtocol(int chosenProtocol)
    // {
    //     //player chose a wrong protocol
    //     if (choice_Violation.Contains(chosenProtocol) && !answer_Violation.Contains(chosenProtocol))
    //     {
    //         return true;
    //     }
    //
    //     return false;
    // }

    void PickUpPhone()
    {
        //phone rings
        GetComponent<AudioSource>().Play();
        
        Invoke("ShowDialogue", 2);
        
    }

    void ShowDialogue()
    {
        //show nothing but the tea cup
        InputManager.instance.ClearDesk();
        
        //show the phone
        phone.SetActive(true);
        isTalking = true;
    }

    void Update()
    {
        //if we've started talking with Florian
        if (isTalking)
        {
            //when player left clicks
            if (Input.GetMouseButtonDown(0))
            {
                //if the phone is ringing, stop it
                if (GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().Stop();
                }
                //show the dialogue box
                dialogueBox.SetActive(true);
                
                //start running the dialogue lines
                NextLine();
                convoLine++;
            }
        }
    }

    void NextLine()
    {
        //the first interaction with Florian
        if (phoneTurn == 0)
        {
            switch (convoLine)
            {
                case 0:
                    phoneText.text = "Good morning, censor!";
                    break;
                case 1:
                    phoneText.text = "Obviously you remember me. We went to the same high school!";
                    break;
                case 2:
                    phoneText.text =
                        "And yes, I did bully you back then, but we all are past adolescence, and no harm was done anyway!";
                    break;
                case 3:
                    phoneText.text =
                        "I am calling to inform you that I will be your manager, starting today.";
                    break;
                case 4:
                    phoneText.text = "You'd better not hold any grudges.";
                    break;
                case 5:
                    phoneText.text =
                        "You don't have parents who could arrange and keep a high position for you in the government.";
                    //stop the convo
                    break;
                case 6:
                    phoneText.text = "Now, get back to work.";
                    break;
                case 7:
                    //reset the desk and get back to work
                    phone.SetActive(false);
                    dialogueBox.SetActive(false);
                    InputManager.instance.ResetDesk();
                    isTalking = false;
                    //the next time convo happens, it is the second time we talk to Florian
                    phoneTurn++;
                    convoLine = -1;
                    break;
                
            }
        }
        else if (phoneTurn == 1)
        {
            switch (convoLine)
            {
                case 0:
                    phoneText.text = "Just forgot to mention...";
                    break;
                case 1:
                    phoneText.text = "You probably heard some rumors of my cousin, Declan Denis.";
                    break;
                case 2:
                    phoneText.text = "Keep an eye on those articles, will ya?";
                    break;
                case 3:
                    phoneText.text = "Those newspaper people need to know who really is in charge.";
                    break;
                case 4:
                    //reset the desk and get back to work
                    phone.SetActive(false);
                    dialogueBox.SetActive(false);
                    InputManager.instance.ResetDesk();
                    specialProtocolText.text =
                        "Any news relating to Florian Wilman's alleged sexual assault must be censored.";
                    isTalking = false;
                    convoLine = 0;
                    break;
            }
        }
    }

}
