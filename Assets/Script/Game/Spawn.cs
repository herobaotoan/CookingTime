using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public new SpriteRenderer renderer;
    public new BoxCollider2D collider;
    GameObject IngredientBox;
    public string ingredientName;
    private bool firstClick = true;

    // Start is called before the first frame update
    void Start()
    {
        IngredientBox = GameObject.Find(ingredientName);
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(2.5f, 2.5f);
    }

    //Make object visible on first click
    private void OnMouseDown()
    {
        if (firstClick){
            TurnVisible();
        }
    }
    private void TurnVisible(){
        if (firstClick){
            renderer.enabled = true;
            collider.size = new Vector2(1f, 1f);
            //Respawn ingredient
            Debug.Log(IngredientBox.name);
            IngredientBox.GetComponent<CreateIngredient>().PunSpawnIngredient();
            firstClick = false;
        }
    }
}
