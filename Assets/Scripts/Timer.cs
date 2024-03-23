using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public int timeRemaining = 300;
    public Light[] lights;
    public FaceMorph face;

    private BoardHandler gameMaster;
    private bool canLose = true;
    private bool lightsRed = false;
    private bool enableSmashing = false; //;)
    private int startFlashing;
    private int startSmashing;
    private int smashChance;
    private AudioSource heartTimer;
    private int heartCount;
    private bool newMineDug = false;

    public delegate void timeEvent();
    public static timeEvent keySmash;

    // Start is called before the first frame update
    void Start()
    {
        BoardHandler.start += startTimer;
        BoardHandler.win += onWin;
        BoardHandler.loss += onWin;
        BoardHandler.reset += onReset;
        MineSquare.mineDug += smashKeys;
        gameMaster = GetComponent<BoardHandler>();
        heartTimer = gameMaster.gameSounds[9];
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator countdown()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
            if (timeRemaining < 1)
            {
                BoardHandler.loss();
            }
            else if(timeRemaining < startFlashing && !lightsRed)
            {
                lightsRed = true;
                StartCoroutine("red");
            }
            else if(timeRemaining < startSmashing && enableSmashing == false)
            {
                enableSmashing = true;
            }

            if (canLose == false)
            {
                StopCoroutine("countdown");
            }
        }
        
    }
    public void startTimer()
    {
        if(gameMaster.difficulty == 0)
        {
            return;
        }
        StartCoroutine("countdown");
        StartCoroutine("heartache");
        startFlashing = Random.Range(80, 125);


        if (gameMaster.difficulty == 1)
        {
            startSmashing = Random.Range(180, 210);
            smashChance = 2;
            heartCount = 15;
        }
        else
        {
            startSmashing = 301;
            smashChance = 3;
            heartCount = 12;
        }
    }
    public void onWin()
    {
        canLose = false;
        StopCoroutine("heartache");
        StopCoroutine("soundTheAlarm");
        heartTimer.pitch = 1;
        gameMaster.gameSounds[7].Stop();
        gameMaster.gameSounds[8].Stop();
        gameMaster.gameSounds[9].Stop();
    }
    public void onReset()
    {
        if (lightsRed)
        {
            StartCoroutine("blue");
            lightsRed = false;
        }
        timeRemaining = 300;
        canLose = true;
        enableSmashing = false;
    }
    public void smashKeys()
    {
        newMineDug = true;
        int didTheySmash = Random.Range(1, 11);
        if(enableSmashing && didTheySmash <= smashChance)
        {
            keySmash();
        }
    }

    IEnumerator red()
    {
        //Fade out
        while (lights[0].intensity >= 0.01f)
        {
            lights[0].intensity -= Time.deltaTime;
            lights[1].intensity -= Time.deltaTime;
            yield return null;
        }

        lights[0].color = new Color(0.8113208f, 0.1275661f, 0.2189886f);
        lights[1].color = new Color(0.8113208f, 0.1275661f, 0.2189886f);

        StartCoroutine("soundTheAlarm");
        while (canLose)
        {
            while (lights[0].intensity <= 1.4f)
            {
                lights[0].intensity += Time.deltaTime;
                lights[1].intensity += Time.deltaTime;
                yield return null;
            }
            while (lights[0].intensity >= 0.8f)
            {
                lights[0].intensity -= Time.deltaTime;
                lights[1].intensity -= Time.deltaTime;
                yield return null;
            }
        }
        //win or lose
        StopCoroutine("red");
    }

    IEnumerator blue()
    {
        while (lights[0].intensity > 0.05f)
        {
            lights[0].intensity -= Time.deltaTime;
            lights[1].intensity += Time.deltaTime;
            yield return null;
        }
        lights[0].color = new Color(0.0405838f, 0.3396226f, 0.3365079f);
        lights[1].color = new Color(0.0405838f, 0.3396226f, 0.3365079f);
        while (lights[0].intensity < 1f)
        {
            lights[0].intensity += Time.deltaTime;
            lights[1].intensity += Time.deltaTime;
            yield return null;
        }
        //No more increase/decrease
        lights[0].intensity = 1;
        lights[1].intensity = 1;

    }

    IEnumerator soundTheAlarm()
    {
        gameMaster.gameSounds[7].Play();
        while (gameMaster.gameSounds[7].isPlaying == true)
        {
            yield return null;
        }
        gameMaster.gameSounds[8].Play();
    }

    IEnumerator heartache()
    {
        int lastClickTime = 0;

        while(timeRemaining > 0)
        {
            if(lastClickTime > 3)
            {

                heartTimer.Play();

                if(newMineDug == true)
                {
                    heartTimer.Stop();
                    lastClickTime = 0;
                    newMineDug = false;
                    heartTimer.pitch = 1;
                }
                else if(lastClickTime > heartCount)
                {
                    heartTimer.Stop();
                    heartTimer.pitch = 1;
                    BoardHandler.loss();
                }
                else if(heartCount - lastClickTime < 5)
                {
                    heartTimer.pitch = 6 - (heartCount-lastClickTime);
                }

                

            }

            //Increment lastClickTime and wait
            yield return new WaitForSeconds(1);
            lastClickTime++;
        }
    }


}
