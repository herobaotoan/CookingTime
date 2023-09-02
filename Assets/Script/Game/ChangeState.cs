using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Pun;
using Photon.Realtime;

public class ChangeState : Photon.Pun.MonoBehaviourPun
{
    public Rigidbody2D rb;
    public new SpriteRenderer renderer;

    // Pan = 1, Board = 2
    public int applianceCollided = 0;
    public GameObject applianceCollidedObject;
    public GameObject previousApplianceCollidedObject;
    int toolNo = 0;
    //Sprites and states
    public float state = 0;
    public Sprite cutSprite;
    public Sprite cookedSrpite;
    public Sprite finalSprite;
    //Fish, Egg and Vegetable can't be cooked directly
    public bool isCookable;
    public bool isCookableAfterCut;
    public bool isCutable;
    //Combination
    public string[] combinables;
    public float[] conditions;
    public float[] selfConditions;
    public string[] meals;

    // public bool isMouseDown = false;
    Vector3 appliancePosition;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // private void OnMouseDown()
    // {
    //     Debug.Log("Mouse Down");
    //     isMouseDown = true;
    // }

    private void OnMouseUp()
    {
        //Check if the appliance is busy
        if (applianceCollidedObject != null)
        {
            if (applianceCollidedObject.GetComponent<ChangeSprite>().GetState() == false)
            {
                // If collided with Pan
                if (applianceCollided == 1)
                {
                    if (isCookable)
                    {
                        StartCoroutine("Cook");
                    } else if (isCookableAfterCut && state == 0.5)
                    {
                        StartCoroutine("Cook");
                    } else {
                        StartCoroutine("MoveOut");
                    }
                }

                // If collided with Board
                if (applianceCollided == 2)
                {
                    if (isCutable)
                    {
                        StartCoroutine("Cut");
                    }else {
                        StartCoroutine("MoveOut");
                    }
                }
                // If collided with Station
                if (applianceCollided == 3)
                {
                    Combine();
                }
                // If collided with order board
                if (applianceCollided == 4)
                {
                    OrderUp();
                }
                // If collided with Trashcan
                if (applianceCollided == 5)
                {
                    applianceCollidedObject.GetComponent<ChangeSprite>().Change(true, null);
                    StartCoroutine("ShrinkAndDestroy");
                    applianceCollidedObject.GetComponent<ChangeSprite>().Change(false, "trash");
                }
            }
            else {
                StartCoroutine("MoveOut");
            }
        }
        // isMouseDown = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Pan"))
        {
            SetCollidedAppliance(col.gameObject, 0);
        }
        if (col.gameObject.CompareTag("Pan1"))
        {
            SetCollidedAppliance(col.gameObject, 1);
        }
        if (col.gameObject.CompareTag("Board"))
        {
            SetCollidedAppliance(col.gameObject, 0);
        }
        if (col.gameObject.CompareTag("Board1"))
        {
            SetCollidedAppliance(col.gameObject, 1);
        }
        if (col.gameObject.CompareTag("Portal"))
        {
            photonView.RPC("SyncState", RpcTarget.All, state);
        }
        if (col.gameObject.CompareTag("Station"))
        {
            SetCollidedAppliance(col.gameObject, 0);
        }
        if (col.gameObject.tag.Contains("Order"))
        {
            SetCollidedAppliance(col.gameObject, 0);
        }
        if (col.gameObject.CompareTag("Trash"))
        {
            SetCollidedAppliance(col.gameObject, 0);
        }
    }
    //To know which appliance will be used
    private void SetCollidedAppliance(GameObject appliance, int tool)
    {
        if (appliance.tag.Contains("Pan"))
        {
            applianceCollided = 1;
        } else if (appliance.tag.Contains("Board"))
        {
            applianceCollided = 2;
        } else if (appliance.tag.Contains("Station")) 
        {
            applianceCollided = 3;
        } else if (appliance.tag.Contains("Order")) 
        {
            applianceCollided = 4;
        } else {
            applianceCollided = 5;
        }
        applianceCollidedObject = appliance;
        appliancePosition = appliance.transform.position;
        previousApplianceCollidedObject = appliance;
        toolNo = tool;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag.Contains("Pan") || col.gameObject.tag.Contains("Board") || col.gameObject.tag.Contains("Station") || col.gameObject.tag.Contains("Order") || col.gameObject.tag.Contains("Trash"))
        {
            applianceCollided = 0;
            applianceCollidedObject = null;
        }
        if (col.gameObject.tag.Contains("Station"))
        {
            previousApplianceCollidedObject.GetComponent<ChangeSprite>().Change(false, null);
        }
    }

    IEnumerator Cook()
    {
        GameObject smoke = GameObject.Find("SmokeVFX" + toolNo);
        smoke.GetComponent<FadeOutAnimation>().Fade(false);
        //VFX
        // VisualEffect smoke =  GameObject.Find("SmokeVFX" + toolNo).GetComponent<VisualEffect>();
        // smoke.Play();
        // smoke.SetFloat("opacity", 0.5f);
        //Find process meter and tool to start animation
        GameObject CookingMeter = GameObject.Find("CookingMeter" + toolNo);
        Animator anim = CookingMeter.GetComponent<Animator>();
        Animator animM = applianceCollidedObject.GetComponent<Animator>();
        applianceCollidedObject.GetComponent<ChangeSprite>().Change(true, gameObject.name);
        bool isBonus = applianceCollidedObject.GetComponent<ChangeSprite>().GetIsBonus();
        int processTime = 50;
        int overProcessTime = 30;
        anim.speed = 1f;
        animM.speed = 1f;
        if (isBonus)
        {
            processTime /= 2;
            overProcessTime /= 2;
            anim.speed = 2f;
            animM.speed = 2f;
        }
        StartCoroutine("Move");
        //If is already Over-processed, do nothing
        if (state > 1.5)
        {
            applianceCollidedObject.GetComponent<ChangeSprite>().Change(false, null);
            // smoke.Stop();
            smoke.GetComponent<FadeOutAnimation>().Fade(true);
            yield break;
        }
        if (state < 1)
        {
            anim.SetBool("isCutting", true);
            animM.SetBool("isCooking", true);
            for (int i = 0; i < processTime; i++)
            {
                yield return new WaitForSeconds(0.1f);
                // If the object is forced to stop
                if (applianceCollided != 1)
                {
                    anim.SetBool("isCutting", false);
                    animM.SetBool("isCooking", false);
                    // smoke.Stop();
                    smoke.GetComponent<FadeOutAnimation>().Fade(true);
                    previousApplianceCollidedObject.GetComponent<ChangeSprite>().Change(false, null);
                    yield break;
                }
            }
            SyncState(state + 1);
        }
        // Cooked, start burning
        anim.SetBool("isOverCutting", true);
        animM.SetBool("isCooking", true);
        // smoke.SetFloat("opacity", 1f);
        for (int i = 0; i < overProcessTime; i++)
        {
            yield return new WaitForSeconds(0.1f);
            // If the object is forced to stop
            if (applianceCollided != 1)
            {
                anim.SetBool("isOverCutting", false);
                anim.SetBool("isCutting", false);
                animM.SetBool("isCooking", false);
                // smoke.Stop();
                smoke.GetComponent<FadeOutAnimation>().Fade(true);
                previousApplianceCollidedObject.GetComponent<ChangeSprite>().Change(false, null);
                yield break;
            }
        }
        anim.SetBool("isOverCutting", false);
        anim.SetBool("isCutting", false);
        animM.SetBool("isCooking", false);
        // smoke.Stop();
        smoke.GetComponent<FadeOutAnimation>().Fade(true);
        applianceCollidedObject.GetComponent<ChangeSprite>().Change(false, null);
        //If using protection powerup, can't be over processing
        if (!applianceCollidedObject.GetComponent<ChangeSprite>().GetIsProtected())
        {
            SyncState(state + 2);
        }
        
    }

    IEnumerator Cut()
    {
        GameObject impact = GameObject.Find("CutVFX" + toolNo);
        impact.GetComponent<FadeOutAnimation>().Fade(false);
        //VFX
        // VisualEffect vfx =  GameObject.Find("StarRandomVFX" + toolNo).GetComponent<VisualEffect>();
        // vfx.Play();
        //Find process meter and tool to start animation
        GameObject Knife = GameObject.Find("Knife" + toolNo);
        GameObject CuttingMeter = GameObject.Find("CuttingMeter" + toolNo);
        Animator anim = Knife.GetComponent<Animator>();
        Animator animM = CuttingMeter.GetComponent<Animator>();
        applianceCollidedObject.GetComponent<ChangeSprite>().Change(true, gameObject.name);
        bool isBonus = applianceCollidedObject.GetComponent<ChangeSprite>().GetIsBonus();
        int processTime = 50;
        int overProcessTime = 30;
        anim.speed = 1f;
        animM.speed = 1f;
        if (isBonus)
        {
            processTime /= 2;
            overProcessTime /= 2;
            anim.speed = 2f;
            animM.speed = 2f;
        }
        StartCoroutine("Move");
        //If is already Over-processed, do nothing
        if (state > 1.5)
        {
            applianceCollidedObject.GetComponent<ChangeSprite>().Change(false, null);
            // vfx.Stop();
            impact.GetComponent<FadeOutAnimation>().Fade(true);
            yield break;
        }

        if (state == 0 || state == 1)
        {
            animM.SetBool("isCutting", true);
            anim.SetBool("isCutting", true);
            for (int i = 0; i < processTime; i++)
            {
                yield return new WaitForSeconds(0.1f);
                // If the object is forced to stop
                if (applianceCollided != 2)
                {
                    animM.SetBool("isCutting", false);
                    anim.SetBool("isCutting", false);
                impact.GetComponent<FadeOutAnimation>().Fade(true);
                    // vfx.Stop();
                    previousApplianceCollidedObject.GetComponent<ChangeSprite>().Change(false, null);
                    yield break;
                }
            }
            animM.SetBool("isCutting", false);
            anim.SetBool("isCutting", false);
            SyncState(state + 0.5f);
        }
        // Cut, start over cutting
        animM.SetBool("isOverCutting", true);
        anim.SetBool("isCutting", true);
        for (int i = 0; i < overProcessTime; i++)
        {
            yield return new WaitForSeconds(0.1f);
            // If the object is forced to stop
            if (applianceCollided != 2)
            {
                animM.SetBool("isOverCutting", false);
                anim.SetBool("isCutting", false);
                // vfx.Stop();
                impact.GetComponent<FadeOutAnimation>().Fade(true);
                previousApplianceCollidedObject.GetComponent<ChangeSprite>().Change(false, null);
                yield break;
            }
        }
        animM.SetBool("isOverCutting", false);
        anim.SetBool("isCutting", false);
        // vfx.Stop();
        impact.GetComponent<FadeOutAnimation>().Fade(true);
        applianceCollidedObject.GetComponent<ChangeSprite>().Change(false, null);

        //If using protection powerup, can't be over processing
        if (!applianceCollidedObject.GetComponent<ChangeSprite>().GetIsProtected())
        {
            SyncState(state + 2);
        }
    }

    [PunRPC]
    void SyncState(float newState)
    {
        if (newState > state)
        state = newState;

        //UncutCooked
        if (state == 1)
        {
            renderer.sprite = cookedSrpite;
        }
        //CutRaw
        if (state == 0.5)
        {
            renderer.sprite = cutSprite;
        }
        //CutCooked
        if (state == 1.5)
        {
            renderer.sprite = finalSprite;
        }
        //OverCooked
        if (state > 1.5)
        {
            renderer.color = Color.black;
        }
    }

    //Ingredient Combination 
    void Combine()
    {
        StartCoroutine("Move");
        applianceCollidedObject.GetComponent<ChangeSprite>().Change(true, null);
        GameObject station1 = GameObject.Find("Station1");
        GameObject station2 = GameObject.Find("Station2");
        Debug.Log(station1.GetComponent<ChangeSprite>().GetState());
        if (station1.GetComponent<ChangeSprite>().GetState() && station2.GetComponent<ChangeSprite>().GetState())
        {
            // float direction = 0;
            GameObject otherIngredient;
            if (applianceCollidedObject.name == "Station1"){
                otherIngredient = station2.GetComponent<ChangeSprite>().GetIngredient();
            } else { 
                otherIngredient = station1.GetComponent<ChangeSprite>().GetIngredient();
            }
            Debug.Log(otherIngredient.name);
            Debug.Log(CheckCombination(otherIngredient));
            int index = CheckCombination(otherIngredient);
            if (index >= 0)
            {
                StartCoroutine(MoveToCombine(otherIngredient, index, true));
                StartCoroutine(otherIngredient.GetComponent<ChangeState>().MoveToCombine(gameObject, index, false));       
            }
        }
    }
    int CheckCombination(GameObject ingredient)
    {
        for (int i = 0; i < combinables.Length; i++)
        {
            Debug.Log(ingredient.name);
            Debug.Log(ingredient.GetComponent<ChangeState>().GetState());
            if (ingredient.name.Contains(combinables[i]) && ingredient.GetComponent<ChangeState>().GetState() == conditions[i] && state == selfConditions[i])
            {
                return i;
            }
        } 
        return -1;
    }

    public float GetState()
    {
        return state;
    }

    //Order Up!
    void OrderUp()
    {
        if (applianceCollidedObject.CompareTag("Order"))
        {
            StartCoroutine("Move");
            applianceCollidedObject.GetComponent<OrderSystem>().SetOrder(gameObject);
            Debug.Log(applianceCollidedObject.GetComponent<OrderSystem>().CheckOrder());
            if (applianceCollidedObject.GetComponent<OrderSystem>().CheckOrder())
            {
                StartCoroutine("ShrinkAndDestroy");
            } else {
                StartCoroutine("MoveOut");
            }

        } else { //VIP order
            StartCoroutine("Move");
            applianceCollidedObject.GetComponent<VIPOrderSystem>().SetOrder(gameObject);
            if (applianceCollidedObject.GetComponent<VIPOrderSystem>().CheckOrder())
            {
                StartCoroutine("ShrinkAndDestroy");
            } else {
                //DO NOTHING
            }
        }
    }


    //Make object moves slowly to the cooking position
    IEnumerator Move()
    {
        rb.velocity = new Vector2(0f, 0f);
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, appliancePosition, timeSinceStarted);

            //If the object has arrived, stop the coroutine
            if (transform.position == appliancePosition)
            {
                yield break;
            }

            //Otherwise, continue next frame
            yield return null;
        }
    }
    //Make object moves out slowly
    public IEnumerator MoveOut()
    {
        rb.velocity = new Vector2(0f, 0f);
        float timeSinceStarted = 0f;
        float randomX = Random.Range(-3, 3);
        float randomY = Random.Range(-2, 2);
        while (randomX > -2 && randomX < 2)
        {
            randomX = Random.Range(-3, 3);
        }
        while (randomY > -2 && randomY < 2)
        {
            randomY = Random.Range(-3, 0);
        }
        Vector3 destination = new Vector3(appliancePosition.x + randomX, appliancePosition.y + randomY, appliancePosition.z);
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, destination, timeSinceStarted);

            //If the object has arrived, stop the coroutine
            if (transform.position == destination)
            {
                yield break;
            }

            //Otherwise, continue next frame
            yield return null;
        }
    }

    IEnumerator MoveToCombine(GameObject otherIngredient, int index, bool isMain)
    {
        
        // VisualEffect star =  GameObject.Find("StarCircleVFX").GetComponent<VisualEffect>();
        // star.Play();
        rb.velocity = new Vector2(0f, 0f);
        float timeSinceStarted = 0f;
        float otherX = otherIngredient.transform.position.x;
        Vector3 destination = new Vector3((transform.position.x + otherX)/2, transform.position.y, transform.position.z);
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, destination, timeSinceStarted);

            //If the object has arrived, stop the coroutine
            if (transform.position == destination)
            {
                if (isMain)
                {
                    photonView.RPC("SpawnIngredient", RpcTarget.Others, index, (transform.position.x + otherX)/2);
                }
                // star.Stop();
                photonView.RPC("PUNDestroy",RpcTarget.All);
                Destroy(gameObject);
                yield break;
            }

            //Otherwise, continue next frame
            yield return null;
        }
    }

    IEnumerator ShrinkAndDestroy()
    {
        float elapsedTime = 0;
        Vector3 targetScale = new Vector3(0f,0f,0f);
        
        //Animation will take 2 seconds
        float timeTakes = 2f; 
        
        while (elapsedTime < timeTakes)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, (elapsedTime / timeTakes));
        
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //Destroy
        photonView.RPC("PUNDestroy",RpcTarget.All);
        Destroy(gameObject);
    }
    [PunRPC]
    void SpawnIngredient(int index, float positionX)
    {
        // Instantiate(myPrefab, transform.position, Quaternion.identity);
        GameObject clone = (GameObject)PhotonNetwork.Instantiate(meals[index], new Vector3(positionX, 3f, transform.position.z), Quaternion.identity);
    }
    [PunRPC]
    void PUNDestroy()
    {
        Destroy(gameObject);
    }
}
