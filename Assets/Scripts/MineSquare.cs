using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class MineSquare : MonoBehaviour
{
    private BoardHandler gameMaster;
    private Renderer myRenderer;
    private bool searched = false; //Has it been revealed
    private bool isFlagged = false; //Flag state
    private bool isAnimating = false;

    private static bool isFirstClick = true; //Starts the game

    public int mineNumber;
    public bool isMine = false;
    public int neighbors = 0;

    public delegate void mines();
    public static mines mineDug; 

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = transform.parent.GetComponent<BoardHandler>();
        myRenderer = GetComponent<Renderer>();
        myRenderer.material = gameMaster.keyMaterials[0];
        BoardHandler.loss += onLoss;
        BoardHandler.reset += onReset;
        Timer.keySmash += getSmashed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This is called when the square is clicked or otherwise activated
    public void pop()
    {
        //Precondition
        if(searched == true || isFlagged == true)
        {
            return;
        }
        //Good to go

        //Check if it is the first
        if(isFirstClick == true)
        {
            //This works because static variables are shared with every instance of the class
            isFirstClick = false;
            gameMaster.initializeField(mineNumber);
            return; //It will be popped later
            
        }

        searched = true;
        checkNeighbors();
        StartCoroutine("fakeAnimation");
        if(isMine == true)
        {
            myRenderer.material = gameMaster.keyMaterials[10];
            //Start loss sequence
            BoardHandler.loss();
        }
        else if(neighbors != 0)
        {
            gameMaster.dig();
            gameMaster.gameSounds[0].Play();
            myRenderer.material = gameMaster.keyMaterials[neighbors];
            mineDug();

            //if: Miranda play minesweeper, Then: Win
        }
        else
        {
            gameMaster.dig();
            myRenderer.material = gameMaster.keyMaterials[11];
            popNeighbors();
            gameMaster.gameSounds[1].Play();
            mineDug();
        }
    }

    public void flag()
    {
        //Precondition
        if (searched == true )
        {
            return;
        }

        StartCoroutine("fakeAnimation");
        if(isFlagged == false)
        {
            if(gameMaster.flagCount > 0)
            {
                isFlagged = true;
                myRenderer.material = gameMaster.keyMaterials[9];
                //Adjust flag count
                gameMaster.flagCount--;
                gameMaster.flagCounter.text = "Flags: " + gameMaster.flagCount;
                gameMaster.gameSounds[2].Play();
            }
        }
        else
        {
            isFlagged = false;
            myRenderer.material = gameMaster.keyMaterials[0];
            //Adjust flag count
            gameMaster.flagCount++;
            gameMaster.flagCounter.text = "Flags: " + gameMaster.flagCount;
            gameMaster.gameSounds[3].Play();
        }
    }

    //Checks the adjacent tiles for mines
    public void checkNeighbors()
    {
        int row = mineNumber / 20;
        int column = mineNumber % 20;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                //Check for validity
                if (row - 1 + i >= 12 || column-1+j >= 20 ||
                    row -1 + i < 0 || column - 1 + j < 0)
                {
                    
                }
                //Debug.Log("r: " + (row -1 +i) + " c: " + (column-1+j));
                else if (gameMaster.board[row - 1 + i][column - 1 + j].isMine == true)
                {
                    neighbors++;
                }
            }
        }
    }

    public void popNeighbors()
    {
        int row = mineNumber / 20;
        int column = mineNumber % 20;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                //Check for validity
                if(row - 1 + i >= 12 || row - 1 + i < 0
                    || column - 1 + j >= 20 || column - 1 + j < 0)
                {
                  
                }
                else
                {
                    gameMaster.board[row - 1 + i][column - 1 + j].pop();
                }
            }
        }
    }

    public void onLoss()
    {
        if(isFlagged == true && isMine == false)
        {
            myRenderer.material = gameMaster.keyMaterials[12];
        }
        else if (isMine == true && isFlagged == false)
        {
            myRenderer.material = gameMaster.keyMaterials[10];
        }
    }
    public void onReset()
    {
        //resets the board
        searched = false;
        isFlagged = false;
        isMine = false;
        neighbors = 0;
        isFirstClick = true;
        myRenderer.material = gameMaster.keyMaterials[0];
    }

    //Randomizes key materials
    public void getSmashed()
    {
        //Precondition
        if(searched == false || isMine == true || neighbors == 0)
        {
            return;
        }
        float randomValue = Random.Range(1, 10);
        if (randomValue <= 1.8f)
        {
            myRenderer.material = gameMaster.keyMaterials[12 + neighbors];
            if(isAnimating == false)
            {
                StartCoroutine("fakeAnimation");
            }
        }
        else if(randomValue <= 3.0f)
        {
            myRenderer.material = gameMaster.keyMaterials[20 + neighbors];
            if(isAnimating == false)
            {
                StartCoroutine("fakeAnimation");
            }
        }
        else
        {
            //Not a special key
            myRenderer.material = gameMaster.keyMaterials[neighbors];
        }
    }

    IEnumerator fakeAnimation()
    {
        isAnimating = true;
        Vector3 target = transform.position + new Vector3(0, -0.115f, 0);
        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime);
            yield return null;
        }
        target += new Vector3(0, 0.115f, 0);
        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime);
            yield return null;
        }
        //Set the key to its original position
        transform.position = target;
        isAnimating = false;
    }
}
