using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMover : MonoBehaviour
{
    public GameObject flagText;
    public AudioSource startSound;
    public BoardHandler gameMaster;

    private bool isShaking = false;

    // Start is called before the first frame update
    void Start()
    {
        BoardHandler.loss += retrunLoss;
        BoardHandler.win += returnWin;
        Timer.keySmash += startTheShakes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getOnOverToThatBoardAndSetDifficulty(int difficulty)
    {
        if(gameMaster.difficulty == -1)
        {
            gameMaster.difficulty = difficulty;
            gameMaster.gameSounds[6].Play();
            StartCoroutine("moveToBoard");
        }
    }
    public void retrunLoss()
    {
        gameMaster.gameSounds[3].Play(); //Loss beep
        StartCoroutine("delayReturn",false);
    }

    public void returnWin()
    {
        gameMaster.gameSounds[2].Play(); //Win beep
        StartCoroutine("delayReturn", true);
    }

    public void startTheShakes()
    {
        if(isShaking == true)
        {
            return;
        }
        isShaking = true;
        StartCoroutine("theShakes");
    }

    //Final position: x26.2 y25.8 z-36.2 xrot70

    IEnumerator moveToBoard()
    {
        Vector3 target = new Vector3(26.2f, 25.8f, -36.2f);
        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime *4f);
            yield return null; //Tells unity to pause for a bit, this is done in the update() function implicitly
        }
        //Sets the final position
        transform.position = target;
        StartCoroutine("rotateToBoard");

    }

    IEnumerator rotateToBoard()
    {
        Quaternion target = Quaternion.Euler(70f, 0, 0);
        while(transform.rotation != target) //eulerAngles is the vector3 representation
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
            yield return null;
        }
        transform.rotation = target;
        Debug.Log("Game started");
        flagText.SetActive(true);
        startSound.Play();
        BoardHandler.start();
    }

    IEnumerator moveToScreen()
    {
        Vector3 target = new Vector3(26.9f, 18.1f, -28.6f);
        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 4f);
            yield return null;
        }
        transform.position = target;
        BoardHandler.reset();
        gameMaster.difficulty = -1;
    }
    IEnumerator rotateToScreen()
    {
        Quaternion target = Quaternion.identity;
        while(transform.rotation != target)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
            yield return null;
        }
        transform.rotation = target;
    }

    IEnumerator delayReturn(bool win)
    {
        yield return new WaitForSeconds(2f);
        if (win)
        {
            gameMaster.gameSounds[10].Play();
            yield return new WaitForSeconds(1f);
            StartCoroutine("moveToScreen");
            StartCoroutine("rotateToScreen");
        }
        else
        {
            gameMaster.gameSounds[6].Play();
            yield return new WaitForSeconds(1f);
            StartCoroutine("moveToScreen");
            StartCoroutine("rotateToScreen");
        }
        
    }

    IEnumerator theShakes()
    {
        gameMaster.gameSounds[5].Play();
        Vector3 target = transform.position + new Vector3(4, 0, 4);

        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 80 * Time.deltaTime);
            yield return null;
        }
        target += new Vector3(-7, 0, -7);
        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 80 *Time.deltaTime);
            yield return null;
        }
        target += new Vector3(3, 0, 3);
        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 80 * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
        isShaking = false;
    }

}
