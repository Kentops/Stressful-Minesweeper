using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionButtons : MonoBehaviour
{
    public GameObject[] thingsOnScreen1;
    public GameObject[] thingsOnScreen2;
    public BoardHandler gameMaster;

    private static int currentScreen;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void swapScreens(int screenNumber)
    {
        /// <summary>
        /// Swaps screens to screenNumber
        /// </summary>

        //Precondition
        if (currentScreen == screenNumber || gameMaster.difficulty != -1)
        {
            return;
        }
        else if (screenNumber == 1)
        {
            gameMaster.gameSounds[2].Play();
            for (int i = 0; i < thingsOnScreen2.Length; i++)
            {
                thingsOnScreen2[i].SetActive(false);
            }
            for(int i = 0; i < thingsOnScreen1.Length; i++)
            {
                thingsOnScreen1[i].SetActive(true);
            }
        }
        else //Screen Number = 2
        {
            gameMaster.gameSounds[2].Play();
            for (int i = 0; i < thingsOnScreen1.Length; i++)
            {
                thingsOnScreen1[i].SetActive(false);
            }
            for(int i = 0; i < thingsOnScreen2.Length; i++)
            {
                thingsOnScreen2[i].SetActive(true);
            }
        }
        //Hide itself
        gameObject.SetActive(false);
    }
}
