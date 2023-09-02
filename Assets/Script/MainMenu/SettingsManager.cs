using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    Button muteButton;
    [SerializeField]
    Sprite muteNormal;
    [SerializeField]
    Sprite muteSelected;
    // Start is called before the first frame update
    public void OnClickMuteSound()
    {
        if (AudioListener.volume == 1)
        {
            muteButton.GetComponent<Image>().sprite = muteSelected;
            AudioListener.volume = 0;
        } else {
            muteButton.GetComponent<Image>().sprite = muteNormal;
            AudioListener.volume = 1;
        }
    }
}
