using System.IO;
using TMPro;
using UnityEngine;

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
        
        //now load the first newspaper
        LoadNews();
    }

    int currentNewsFile = 0;

    //TODO: after the brief related stuff is done, add a button that calls CurrentNewsFile++!!!
    public int CurrentNewsFile
    {
        get { return currentNewsFile; }
        set
        {
            currentNewsFile = value;
            
            //we only have this many news every day, so this is to ensure we only load this much
            if (currentNewsFile < newsCount)
            {
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
        //the reading the file lines for loop stuff
        //lineIndex = 0;  //start with line index 0
        
        //clear the previous news just in case
        ClearNews();

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
            
        }
        
    }

    void ClearNews()
    {
        newsTitle.text = string.Empty;
        newsSubtitle.text = string.Empty;
        newspaper.text = string.Empty;
        newsText.text = string.Empty;
    }

}
