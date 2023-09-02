using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ButtonClick : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite Sprite;

    public void OnClick()
    {
        Debug.Log("Clicked");
        StartCoroutine("Wait");
    }

    IEnumerator Wait()
    {
		yield return new WaitForSeconds (0.3f);
        image.sprite = Sprite;
	}

    public void LoadMainMenu()
    {
        PhotonNetwork.LeaveRoom(true);
        SceneManager.LoadScene(0);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
