using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhoneConvoScript : MonoBehaviour
{
    public TextMeshProUGUI phoneText;
    int convoLine = 0;

    void Start()
    {
        Invoke("NextLine", 1);
    }

    void NextLine()
    {
        if(GameManager.instance.PhoneTurn == 0)
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
                    phoneText.text = "I am calling to inform you that I will be your manager, starting today.";
                    break;
                case 4:
                    phoneText.text = "You'd better not hold any grudges.";
                    break;
                case 5:
                    phoneText.text =
                        "You don't have parents who could arrange and keep a high position for you in the government.";
                    break;
                
            }
        }
    }
}
