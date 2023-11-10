using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CriticismScript : MonoBehaviour
{
    //warning text replacement stuff
    public TextMeshProUGUI warningText;
    string violationWarning;
    
    void Start()
    {
        //get the violation text from Game Manager
        violationWarning = GameManager.instance.replaceText;
        //replace the <MistakeText> with the violation warning text.
        warningText.text = warningText.text.Replace("<MistakeText>", violationWarning);
    }
}
