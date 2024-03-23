using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieButton : MonoBehaviour
{
    public GameObject cookie;
    public BoardHandler gameMaster;
    private GameObject currentCookie;
    private Animator myAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getClicked()
    {
        //if(currentCookie != null)
        //{
        //    Destroy(currentCookie);
        //}
        myAnim.Play("Cookie Button");
        gameMaster.gameSounds[0].Play();
        //currentCookie = Instantiate(cookie, new Vector3(25,36,-25), Quaternion.identity);
    }
}
