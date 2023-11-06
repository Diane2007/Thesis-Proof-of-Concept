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

    int susLv;
    public TextMeshProUGUI specialProtocolText;

    //calculate player's sus level
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
    }

}
