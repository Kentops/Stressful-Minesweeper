using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    //Tutorial used: https://www.youtube.com/watch?v=_mDLu_obzkk 

    private Ray myRay;
    private RaycastHit myHit;
    private bool canClick = false;
    private bool clickingCookie = false;
    private bool endCookie = false;
    [SerializeField] private LayerMask mineLayer;
    [SerializeField] private LayerMask cookieLayer;
    [SerializeField] private LayerMask buttonLayer;

    // Start is called before the first frame update
    void Start()
    {
        BoardHandler.start += onStart;
        BoardHandler.loss += restrictClick;
        BoardHandler.win += restrictClick;
    }

    // Update is called once per frame
    void Update()
    {
        if (canClick)
        {
            Vector2 mousePos = Input.mousePosition;
            myRay = GetComponent<Camera>().ScreenPointToRay(mousePos);


            //One-click mouse support
            if (Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.LeftControl))
            {
                if (Physics.Raycast(myRay, out myHit, 100f, mineLayer))
                {
                    myHit.transform.GetComponent<MineSquare>().flag();
                }
            }
            else if (Input.GetMouseButtonUp(0)) //Left mouse button
            {
                //Check if I hit anything, and that it is on the mineLayer layer
                //Raycast targets need a collider!
                if (Physics.Raycast(myRay, out myHit, 100f, mineLayer))
                {
                    myHit.transform.GetComponent<MineSquare>().pop();
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (Physics.Raycast(myRay, out myHit, 100f, mineLayer))
                {
                    myHit.transform.GetComponent<MineSquare>().flag();
                }
            }
            /**else if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(myRay, out myHit, 100f, cookieLayer) && !clickingCookie)
                {
                    clickingCookie = true;
                    StartCoroutine("cookieClicker"); 
                }
                else if (Physics.Raycast(myRay, out myHit, 100f, buttonLayer))
                {
                    myHit.transform.gameObject.GetComponent<CookieButton>().getClicked();
                }
            }**/
        }

    }

    public void onStart()
    {
        canClick = true;
    }

    public void restrictClick()
    {
        canClick = false;
    }

    public void eatCookie()
    {
        endCookie = true;
    }

    //While loops should not be called every frame
    IEnumerator cookieClicker()
    {
        RaycastHit newHit;
        while (Input.GetMouseButton(0))
        {
            if(Physics.Raycast(myRay, out newHit, 100f, ~cookieLayer) && !endCookie)
            {
                //The ~ means it will ignore anything in the cookieLayer layer mask
                myHit.transform.position = newHit.point;
            }
            if(endCookie == true)
            {
                endCookie = false;
                StopCoroutine("cookieClickler");
            }
            yield return null;
        }
        clickingCookie = false;
    }
}
