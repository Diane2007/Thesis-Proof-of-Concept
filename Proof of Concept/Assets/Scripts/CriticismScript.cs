using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CriticismScript : MonoBehaviour
{
    //warning text replacement stuff
    public TextMeshProUGUI warningText, copyText;
    public TMP_InputField inputText;
    string violationWarning;
    
    void Start()
    {
        //get the violation text from Game Manager
        violationWarning = GameManager.instance.replaceText;
        //replace the <MistakeText> with the violation warning text.
        warningText.text = warningText.text.Replace("<MistakeText>", violationWarning);
    }

    public void SubmitButton()
    {
        //check if player's input text is exactly same as what we tell them to copy
        if (copyText.text == inputText.text)
        {
            Debug.Log("Player typed in the exact same text!");
            //if it's the first (0) piece of news
            if (GameManager.instance.CurrentNewsFile == 0)
            {
                //the first apology correct bool (0) is true
                GameManager.instance.apologyCorrect0 = true;
            }
            else if (GameManager.instance.CurrentNewsFile == 1)
            {
                GameManager.instance.apologyCorrect1 = true;
            }
            else if (GameManager.instance.CurrentNewsFile == 2)
            {
                GameManager.instance.apologyCorrect2 = true;
            }
        }
        else
        {
            Debug.Log("Player typed in different text!");
        }

        //when we go back, we are going to load the new file
        GameManager.instance.CurrentNewsFile++;

        //move back to main scene
        GameManager.instance.ChangeScene(0);
    }
    
}
