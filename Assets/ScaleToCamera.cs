using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScaleToCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start() {
        var spriteBounds = spriteRenderer.bounds; // Sprite to fit viewport
        var worldScale = mainCamera.ViewportToWorldPoint(Vector3.one);
        var localScale = 2 * Math.Max(
            worldScale.x / spriteBounds.size.x * transform.localScale.x,
            worldScale.y / spriteBounds.size.y * transform.localScale.y);

        transform.localScale = new Vector3(localScale, localScale, 1);
    }
}
