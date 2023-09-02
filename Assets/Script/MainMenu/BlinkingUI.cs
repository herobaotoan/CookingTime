using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BlinkingUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    void Start()
    {
        StartCoroutine("Blink");
    }

    IEnumerator Blink()
    {
		
        while (true)
        {
            textMesh.text = "";
            yield return new WaitForSeconds (0.3f);
            textMesh.text = "- Tap Anywhere To Start -";
            yield return new WaitForSeconds (0.5f);
        }
	}

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(1);
    }

    public void OnClickStartGame(){
        StartCoroutine("StartGame");
    }
}
