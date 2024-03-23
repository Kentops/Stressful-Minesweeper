using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardHandler : MonoBehaviour
{
    public Material[] keyMaterials;
    public MineSquare[][] board;
    public TextMeshProUGUI flagCounter;
    public int flagCount;
    public AudioSource[] gameSounds;
    public int digs;
    public int difficulty = -1;

    //Delegates
    public delegate void winLoss();
    public static winLoss loss; //public static event winLoss loss;
    public static winLoss win;
    public static winLoss start;
    public static winLoss reset;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the board 2d array
        board = new MineSquare[12][];
        for (int i = 0; i < 12; i++)
        {
            board[i] = new MineSquare[20];
            for (int j = 0; j < 20; j++)
            {
                board[i][j] = transform.GetChild( (i * 20) + j ).gameObject.GetComponent<MineSquare>();
                board[i][j].mineNumber = (20 * i) + j;
            }
        }

        reset += onReset;
        start += onStart;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initializeField(int numb)
    {

        //Set Mines
        for (int i = 0; i < flagCount; i++)
        {
            int row = Random.Range(0, 12);
            int column = Random.Range(0, 20);

            if(Mathf.Abs(row - (numb/20)) > 1 || Mathf.Abs(column-(numb%20)) > 1)
            {
                //Mine location is vaid
                if (board[row][column].isMine == false)
                {
                    board[row][column].isMine = true;
                    //Debug.Log("row: " + row + " column: " + column + " is a mine");
                    //board[row][column].GetComponent<Renderer>().material = keyMaterials[10];
                }
                else
                {
                    //Space is already a mine
                    i--; //Go again
                    //Debug.Log("mine overlap");
                }
                
            }
            else
            {
                //Mine is too close to starting point (3 by 3 grid)
                i--; //Go again
                //Debug.Log("row:" + row + " column:" + column + " is too close!");
            }
        }

        //Pop the start after mines are calculated
        board[numb / 20][numb % 20].pop();
    }

    public void dig()
    {
        if(digs > 1)
        {
            digs--;
        }
        else
        {
            digs--;
            win();
        }
    }

    public void onReset()
    {
        difficulty = -1;
        digs = 200;
        flagCounter.gameObject.SetActive(false);
    }

    public void onStart()
    {
        flagCount = 36;
        digs = 204;
        flagCounter.text = "Flags: " + flagCount;
    }

}
