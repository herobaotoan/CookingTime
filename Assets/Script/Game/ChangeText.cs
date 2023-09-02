using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeText : MonoBehaviour
{
    public TextMeshPro text;
    public TextMeshProUGUI textUI;
    // Start is called before the first frame update
    // void Start()
    // {
    //     text.GetComponent<TextMeshPro>();
    // }

    public void Change(string newText)
    {
        if (text != null)
        {
            text.text = newText;
        } else {
            textUI.text = newText;
        }
    }
}
