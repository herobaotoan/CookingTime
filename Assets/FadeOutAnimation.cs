using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAnimation : MonoBehaviour
{
    SpriteRenderer img;
    public float opacity = 0.15f;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<SpriteRenderer>();
        img.color = new Color(1,1,1,0);
    }

    public void Fade(bool fade)
    {
        StartCoroutine("FadeImage", fade);
    }

    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = opacity; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= opacity; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
}
