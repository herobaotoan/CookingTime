using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public Sprite currentSprite;
    public bool isProcessing;
    public bool isChangable;
    public bool isProtected;
    public GameObject ingredient;
    public GameObject collidedIngredient;
    public AudioClip eggAudio;
    public AudioClip veggieAudio;
    public AudioClip meatAudio;
    public AudioClip commonAudio;
    new AudioSource audio;

    bool isBonus = false;
    
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSprite = spriteRenderer.sprite;
        isProcessing = false;
        isProtected = false;
    }

    public void Change(bool isCutting, string name)
    {
        if (isCutting)
        {
            CheckAudio(name);
            if (isChangable)
            {
                spriteRenderer.sprite = newSprite;
            }
            isProcessing = true;
            ingredient = collidedIngredient;
            audio.Play();
        } else 
        { 
            CheckAudio(null);
            if (isChangable)
            {
                spriteRenderer.sprite = currentSprite;
            }
            isProcessing = false;
            ingredient = null;
            if (name != "trash")
            {
                audio.Stop();
            }
        }
    }
    public bool GetState()
    {
        return isProcessing;
    }
    public GameObject GetIngredient()
    {
        return ingredient;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        collidedIngredient = col.gameObject;
    }

    public void SetIsBonus(bool state)
    {
        isBonus = state;
    }
    public bool GetIsBonus()
    {
        return isBonus;
    }

    private void CheckAudio(string name)
    {
        if (name != null)
        {
            if (name.Contains("Egg"))
            {
                audio.clip = eggAudio;
            }
            if (name.Contains("Letture") || name.Contains("Tomato") || name.Contains("Potato"))
            {
                audio.clip = veggieAudio;
            }
            if (name.Contains("Fish") || name.Contains("Meat"))
            {
                audio.clip = meatAudio;
            }
        }
        Debug.Log(audio.clip);

        if (audio.clip == null)
        {
            audio.clip = commonAudio;
        }
    }

    public void SetIsProtected(bool status)
    {
        isProtected = status;
    }
    public bool GetIsProtected()
    {
        return isProtected;
    }
}
