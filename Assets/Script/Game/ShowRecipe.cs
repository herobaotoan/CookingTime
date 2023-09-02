using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRecipe : MonoBehaviour
{
    [SerializeField] GameObject recipeBook;
    [SerializeField] GameObject book;
    [SerializeField] GameObject buttonLeft;
    [SerializeField] GameObject buttonRight;
    Animator anim;
    bool isClosed = true;
    public Image image;    

    int activePage = 0;
    int previousActivePage;

    void OnEnable()
    {
        anim = book.GetComponent<Animator>();
    }

    public void OnClick()
    {
        if (isClosed)
        {
            StartCoroutine("OpenBook");
        } else {
            StartCoroutine("CloseBook");
        }
    }
    IEnumerator OpenBook()
    {
        if (activePage == 0)
        {
            buttonLeft.SetActive(false);
        } else if (activePage == 3)
        {
            buttonRight.SetActive(false);
        }
        image.color = Color.red;
        recipeBook.SetActive(true);
        isClosed = false;
        anim.SetBool("isClosed", false);
        anim.SetBool("isTurning", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("isTurning", false);
        anim.SetInteger("page", activePage);
    }

    public void StartCloseBook()
    {
        StartCoroutine("CloseBook");
    }

    IEnumerator CloseBook()
    {
        anim.SetBool("isTurning", true);
        anim.SetBool("isClosed", true);
        yield return new WaitForSeconds(1.2f);
        image.color = Color.white;
        recipeBook.SetActive(false);
        isClosed = true;
    }

    public void NextPage()
    {
        StartCoroutine("TurnNextPage");
    }
    IEnumerator TurnNextPage()
    {
        activePage += 1;
        previousActivePage = activePage;
        if (previousActivePage > 1)
        {
            previousActivePage = 1;
        }
        Checkpage();
        anim.SetTrigger("left");
        anim.SetBool("isTurning", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("isTurning", false);
        anim.SetInteger("page", activePage);
    }

    public void PreviousPage()
    {
        StartCoroutine("TurnLastPage");
    }
    IEnumerator TurnLastPage()
    {
        activePage -= 1;
        previousActivePage = activePage;
        if (previousActivePage > 1)
        {
            previousActivePage = 1;
        }
        Checkpage();
        anim.SetTrigger("right");
        anim.SetBool("isTurning", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("isTurning", false);
        anim.SetInteger("page", activePage);
    }

    public void Bookmark(int number)
    {
        if (number != activePage)
        {
            if (number < activePage)
            {
                StartCoroutine("TurnNextBookmark", number);
            } else if (number > activePage) {
                StartCoroutine("TurnLastBookmark", number);
            }
        }
    }
    IEnumerator TurnNextBookmark(int number)
    {
        activePage = number;
        if (number == 0)
        {
            activePage = previousActivePage;
        }
        Checkpage();
        anim.SetTrigger("right");
        anim.SetBool("isTurning", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("isTurning", false);
        anim.SetInteger("page", activePage);
    }
    IEnumerator TurnLastBookmark(int number)
    {
        activePage = number;
        Checkpage();
        anim.SetTrigger("left");
        anim.SetBool("isTurning", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("isTurning", false);
        anim.SetInteger("page", activePage);
    }

    void Checkpage()
    {
        if (activePage == 3)
        {
            buttonRight.SetActive(false);
            buttonLeft.SetActive(true);
        } else if (activePage == 0){
            buttonRight.SetActive(true);
            buttonLeft.SetActive(false);
        } else 
        {
            buttonRight.SetActive(true);
            buttonLeft.SetActive(true);
        }
    }
}
