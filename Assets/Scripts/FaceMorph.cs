using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMorph : MonoBehaviour
{
    public Material[] faces;
    public AudioSource[] faceSounds;
    public Clicker theClicker;
    private Renderer myRenderer;
    private bool canChange = true;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        myRenderer.material = faces[0];
        BoardHandler.loss += onLoss;
        BoardHandler.win += onWin;
        BoardHandler.reset += onReset;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && canChange && myRenderer.material != faces[1])
        {
            myRenderer.material = faces[1];
        }
        else if (Input.GetMouseButton(0) == false && canChange && myRenderer.material != faces[0])
        {
            myRenderer.material = faces[0];
        }
    }

    private void onLoss()
    {
        canChange = false;
        myRenderer.material = faces[2];
    }
    private void onWin()
    {
        canChange = false;
        myRenderer.material = faces[3];
    }
    private void onReset()
    {
        canChange = true;
        myRenderer.material = faces[0];
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            theClicker.eatCookie();
            Destroy(other.gameObject);
            //Play Sound

            //Check if face was hungry
        }
    }

    public void getHungry()
    {

    }
}
