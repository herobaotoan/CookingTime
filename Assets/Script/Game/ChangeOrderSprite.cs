using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOrderSprite : MonoBehaviour
{
    public new SpriteRenderer renderer;
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    public void ChangeSprite(Sprite newSprite)
    {
        renderer.sprite = newSprite;
    }
}
