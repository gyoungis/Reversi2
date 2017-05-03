using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public int[,] boardState = new int[8, 8];
    public List<GameObject> pieceList;
    public GameObject[,] pieceArray = new GameObject[8, 8];
    public GameObject piece;
    public int playerScore = 2;
    public int oppositeScore = 2;
    public bool skipTurn = false;

    public int playerSide = 1;
    private int oppositeSide = 2;

    private bool counting;
    
    private int points;
    private int totalPoints;

    private List<GameObject> affectedPieces;

    // AI variables
    public int[,] simBoardState = new int[8, 8];
    public int levels;
    private int simPoints;
    private int simTotalPoints;
    private int simOverallPoints;
    private int bestRow;
    private int bestCol;
    private int levelBestIndex;
    private List<int> simAffectedRows;
    private List<int> simAffectedCols;
    private List<int> currentRows;
    private List<int> currentCols;
    private List<int> currentPoints;
    private List<int> simRowMoves;
    private List<int> simColMoves;
    private List<int> simMovePoints;

    private float timer = 0.0f;
    public float delay = 3.0f;
    
    
    
    // Use this for initialization
    void Start () {
        piece = GetComponent<GameObject>();
        affectedPieces = new List<GameObject>();



        boardState = new int[8, 8]{ { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 2, 1, 0, 0, 0},
                                    { 0, 0, 0, 1, 2, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0} };
        for(int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {

                pieceArray[y, x] = pieceList[y * 8 + x];

            }
        }
        
        // Initial Black
        pieceArray[3, 3].transform.position = new Vector3(3.5f, 10, 3.5f);
        pieceArray[3, 3].GetComponent<Rigidbody>().useGravity = true;
        pieceArray[4, 4].transform.position = new Vector3(4.5f, 10, 4.5f);
        pieceArray[4, 4].GetComponent<Rigidbody>().useGravity = true;

        // Intial White
        pieceArray[3, 4].transform.position = new Vector3(4.5f, 10, 3.5f);
        pieceArray[3, 4].transform.Rotate(180, 0, 0);
        pieceArray[3, 4].GetComponent<Rigidbody>().useGravity = true;
        pieceArray[4, 3].transform.position = new Vector3(3.5f, 10, 4.5f);
        pieceArray[4, 3].transform.Rotate(180, 0, 0);
        pieceArray[4, 3].GetComponent<Rigidbody>().useGravity = true;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && playerSide ==1)
        {
            float i;
            float j;
            
            int row;
            int col;
            Vector3 insertPoint;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 12.0f))
            {
                // World Coord
                i = Mathf.Floor(hit.point.x);
                j = Mathf.Floor(hit.point.z);

                // Matrix index
                col = (int)Mathf.Floor(hit.point.x);
                row = (int)Mathf.Floor(hit.point.z);
                Debug.Log("col is: " + col);
                Debug.Log("row is: " + row);
                if (i <= 7 && i >= 0 && j <= 7 && j >= 0 && boardState[row, col] == 0)
                {
                    if (playerSide == 1)
                    {
                        bool counting = false;
                        insertPoint = new Vector3(i+0.5f, 10, j+0.5f);
                        if (checkValidMove(row, col, playerSide, oppositeSide, counting))
                        {
                            boardState[row, col] = playerSide;
                            pieceArray[row, col].transform.position = insertPoint;
                            pieceArray[row, col].transform.Rotate(180, 0, 0);
                            pieceArray[row, col].GetComponent<Rigidbody>().useGravity = true;
                            playerScore += 1;
                            playerSide = 2;
                            oppositeSide = 1;

                            if (validMoves(2, 1) == 0)
                            {
                                playerSide = 1;
                                oppositeSide = 2;
                            }

                            for (int x = 0; x < 8; x++)
                            {
                                string s = "";
                                for (int y = 0; y < 8; y++)
                                {
                                    s += boardState[x, y];
                                    if (y == 7)
                                    {
                                        Debug.Log(s);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        counting = false;
                        if (checkValidMove(row, col, playerSide, oppositeSide, counting))
                        {
                            insertPoint = new Vector3(i + 0.5f, 10, j + 0.5f);
                            boardState[row, col] = playerSide;
                            pieceArray[row, col].transform.position = insertPoint;
                            pieceArray[row, col].GetComponent<Rigidbody>().useGravity = true;
                            oppositeScore += 1;
                            playerSide = 1;
                            oppositeSide = 2;

                            if (validMoves(1, 2) == 0)
                            {
                                playerSide = 2;
                                oppositeSide = 1;
                            }

                            for (int x = 0; x < 8; x++)
                            {
                                string s = "";
                                for (int y = 0; y < 8; y++)
                                {
                                    s += boardState[x, y];
                                    if (y == 7)
                                    {
                                        Debug.Log(s);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }


        if (playerSide == 2)
        {
            timer += Time.deltaTime;
            if (timer > delay)
            {
                Vector3 Insert;
                AIMove();
                checkValidMove(bestRow, bestCol, playerSide, oppositeSide, false);

                Insert = new Vector3(bestCol + 0.5f, 10, bestRow + 0.5f);
                boardState[bestRow, bestCol] = playerSide;
                pieceArray[bestRow, bestCol].transform.position = Insert;
                pieceArray[bestRow, bestCol].GetComponent<Rigidbody>().useGravity = true;
                oppositeScore += 1;
                playerSide = 1;
                oppositeSide = 2;

                if (validMoves(1, 2) == 0)
                {
                    playerSide = 2;
                    oppositeSide = 1;
                }

                timer = 0f;
            }
        }

    }

    bool checkValidMove(int row, int col, int side, int opSide, bool counting)
    {
        bool valid = false;
        totalPoints = 0;
        int march;

        if (col > 1) // Check for edge
        {
            if (boardState[row, col - 1] == opSide) // Left --------------------------------------
            {

                for (int a = 1; a <= col; a++) // march along pieces to edge
                {
                    affectedPieces.Add(pieceArray[row, col - a]);
                    if (boardState[row, col - a] == side) //Same Color
                    {
                        affectedPieces.Remove(pieceArray[row, col - a]); // Don't flip your own piece
                        valid = true;
                        Debug.Log("valid: left");
                        if (counting == false)
                        {
                            for (int c = 0; c < affectedPieces.Count; c++)
                            {
                                // Flip affected pieces

                                affectedPieces[c].transform.Rotate(0, 0, 180);
                                boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                            }
                        }

                        points = affectedPieces.Count;
                        totalPoints += points;
                        if (counting == false)
                        {
                            addPoints(side, points);
                        }
                        affectedPieces.Clear();
                        break;
                    }

                    if (boardState[row, col - a] == 0) // If you don't hit your own piece again
                    {
                        affectedPieces.Clear();
                        break;
                    }
                }
            }
        }

        if (col < 6)
        {
            if (boardState[row, col + 1] == opSide) // Right --------------------------------------
            {


                for (int a = 1; a <= 7 - col; a++)
                {
                    affectedPieces.Add(pieceArray[row, col + a]);
                    
                    if (boardState[row, col + a] == side) //Same Color
                    {
                        affectedPieces.Remove(pieceArray[row, col + a]);
                        
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < affectedPieces.Count; c++)
                            {
                                affectedPieces[c].transform.Rotate(0, 0, 180);
                                boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                            }
                        }
                        Debug.Log("valid: right");
                        points = affectedPieces.Count;
                        totalPoints += points;
                        if (counting == false)
                        {
                            addPoints(side, points);
                        }
                        affectedPieces.Clear();
                        break;
                    }

                    if (boardState[row, col + a] == 0)
                    {
                        affectedPieces.Clear();
                        break;
                    }
                }
            }
        }

        if (row < 6)
        {
            if (boardState[row + 1, col] == opSide) // Up --------------------------------------
            {


                for (int a = 1; a <= 7 - row; a++)
                {
                    affectedPieces.Add(pieceArray[row + a, col]);
                    if (boardState[row + a, col] == side) //Same Color
                    {
                        affectedPieces.Remove(pieceArray[row + a, col]);
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < affectedPieces.Count; c++)
                            {
                                affectedPieces[c].transform.Rotate(0, 0, 180);
                                boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                            }
                        }
                        Debug.Log("valid up");
                        points = affectedPieces.Count;
                        totalPoints += points;
                        if (counting == false)
                        {
                            addPoints(side, points);
                        }
                        affectedPieces.Clear();
                        break;
                    }

                    if (boardState[row + a, col] == 0)
                    {
                        affectedPieces.Clear();
                        break;
                    }
                }
            }
        }
        
        if (row > 1)
        {
            if (boardState[row - 1, col] == opSide) // Down --------------------------------------
            {
                for (int a = 1; a <= row; a++)
                {
                    Debug.Log("Test row: " + (row - a));
                    Debug.Log("Test col: " + (col));

                    affectedPieces.Add(pieceArray[row - a, col]);
                    
                    if (boardState[row - a, col] == side) //Same Color
                    {
                        affectedPieces.Remove(pieceArray[row - a, col]);
                        
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < affectedPieces.Count; c++)
                            {
                                affectedPieces[c].transform.Rotate(0, 0, 180);
                                boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                            }
                        }
                        Debug.Log("valid down");
                        points = affectedPieces.Count;
                        totalPoints += points;
                        if (counting == false)
                        {
                            addPoints(side, points);
                        }
                        affectedPieces.Clear();
                        break;
                    }

                    if (boardState[row - a, col] == 0)
                    {
                        affectedPieces.Clear();
                        break;
                    }
                }
            }
        }

        if (row < 6 && col > 1)
        {
            if (boardState[row + 1, col - 1] == opSide) // Top left --------------------------------------
            {

                march = marchLength(row, col, 0);
                for (int a = 1; a <= march; a++)
                {
                    affectedPieces.Add(pieceArray[row + a, col - a]);
                    if (boardState[row + a, col - a] == side) //Same Color
                    {
                        affectedPieces.Remove(pieceArray[row + a, col - a]);
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < affectedPieces.Count; c++)
                            {
                                affectedPieces[c].transform.Rotate(0, 0, 180);
                                boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                            }
                        }
                        Debug.Log("valid top left");
                        points = affectedPieces.Count;
                        totalPoints += points;
                        if (counting == false)
                        {
                            addPoints(side, points);
                        }
                        affectedPieces.Clear();
                        break;
                    }

                    if (boardState[row + a, col - a] == 0)
                    {
                        affectedPieces.Clear();
                        break;
                    }
                }
            }
        }

        if (row < 6 && col < 6)
        {
            if (boardState[row + 1, col + 1] == opSide) // Top Right --------------------------------------
            {

                march = marchLength(row, col, 1);
                for (int a = 1; a <= march; a++)
                {
                    affectedPieces.Add(pieceArray[row + a, col + a]);
                    if (boardState[row + a, col + a] == side) //Same Color
                    {
                        affectedPieces.Remove(pieceArray[row + a, col + a]);
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < affectedPieces.Count; c++)
                            {
                                affectedPieces[c].transform.Rotate(0, 0, 180);
                                boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                            }
                        }
                        Debug.Log("valid top right");
                        points = affectedPieces.Count;
                        totalPoints += points;
                        if (counting == false)
                        {
                            addPoints(side, points);
                        }
                        affectedPieces.Clear();
                        break;
                    }

                    if (boardState[row + a, col + a] == 0)
                    {
                        affectedPieces.Clear();
                        break;
                    }
                }
            }
        }

        if (row > 1 && col > 1)
        {
            if (boardState[row - 1, col - 1] == opSide) // Bottom Left --------------------------------------
            {

                march = marchLength(row, col, 2);
                for (int a = 1; a <= march; a++)
                {
                    affectedPieces.Add(pieceArray[row - a, col - a]);
                    if (boardState[row - a, col - a] == side) //Same Color
                    {
                        affectedPieces.Remove(pieceArray[row - a, col - a]);
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < affectedPieces.Count; c++)
                            {
                                affectedPieces[c].transform.Rotate(0, 0, 180);
                                boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                            }
                        }
                        Debug.Log("valid bottom left");
                        points = affectedPieces.Count;
                        totalPoints += points;
                        if (counting == false)
                        {
                            addPoints(side, points);
                        }
                        affectedPieces.Clear();
                        break;
                    }

                    if (boardState[row - a, col - a] == 0)
                    {
                        affectedPieces.Clear();
                        break;
                    }
                }
            }
        }

        if (row > 1 && col < 6)
        {
            if (boardState[row - 1, col + 1] == opSide) // Bottom Right --------------------------------------
            {

                march = marchLength(row, col, 3);
                for (int a = 1; a <= march; a++)
                {
                    affectedPieces.Add(pieceArray[row - a, col + a]);
                    
                    if (boardState[row - a, col + a] == side) //Same Color
                    {
                        affectedPieces.Remove(pieceArray[row - a, col + a]);
                        
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < affectedPieces.Count; c++)
                            {
                                affectedPieces[c].transform.Rotate(0, 0, 180);
                                boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                            }
                        }
                        Debug.Log("valid bottom right");
                        points = affectedPieces.Count;
                        totalPoints += points;
                        if (counting == false)
                        {
                            addPoints(side, points);
                        }
                        affectedPieces.Clear();
                        break;
                    }

                    if (boardState[row - a, col + a] == 0)
                    {
                        affectedPieces.Clear();
                        break;
                    }
                }
            }
        }

        return valid;
    }

    int marchLength(int row, int col, int dir)
    {
        int len = 0;
        int minRow;
        int minCol;

        if (dir == 0) // Top Left
        {
            minCol = col;
            minRow = 7 - row;
            if (minCol <= minRow)
            {
                len = minCol;
            }
            else
            {
                len = minRow;
            }
        }

        if (dir == 1) // Top Right
        {
            minCol = 7 - col;
            minRow = 7 - row;
            if (minCol <= minRow)
            {
                len = minCol;
            }
            else
            {
                len = minRow;
            }
        }

        if (dir == 2) // Bottom Left
        {
            minCol = col;
            minRow = row;
            if (minCol <= minRow)
            {
                len = minCol;
            }
            else
            {
                len = minRow;
            }
        }

        if (dir == 3) // Bottom Right
        {
            minCol = 7 - col;
            minRow = row;
            if (minCol <= minRow)
            {
                len = minCol;
            }
            else
            {
                len = minRow;
            }
        }

        return len;
    }

    void addPoints(int side, int points)
    {
        if (side == 1)
        {
            playerScore += points;
            oppositeScore -= points;
        }

        if (side == 2)
        {
            playerScore -= points;
            oppositeScore += points;
        }
    }

    int validMoves(int side, int opSide)
    {
        counting = true;
        int moves = 0;
        List<int> rows = new List<int>();
        List<int> cols = new List<int>();
        List<int> movePoints = new List<int>();
        movePoints.Clear();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (boardState[i, j] == 0)
                {
                    if (checkValidMove(i, j, side, opSide, counting) == true)
                    {
                        rows.Add(i);
                        cols.Add(j);
                        movePoints.Add(totalPoints);                                            
                    }
                }
            }
        }


        moves = rows.Count;
        rows.Clear();
        cols.Clear();
        movePoints.Clear();
        return moves;
    }

    //----------------------------------------MINI MAX--------------------------------------------------
    bool SimCheckValidMove(int row, int col, int side, int opSide, bool counting)
    {
        simAffectedRows = new List<int>();
        simAffectedCols = new List<int>();
        bool valid = false;
        simPoints = 0;
        simTotalPoints = 0;
        int march;
        simAffectedRows.Clear();
        simAffectedCols.Clear();

        if (col > 1) // Check for edge
        {
            if (simBoardState[row, col - 1] == opSide) // Left --------------------------------------
            {
                for (int a = 1; a <= col; a++) // march along pieces to edge
                {
                    simAffectedRows.Add(row);
                    simAffectedCols.Add(col - a);
                    if (simBoardState[row, col - a] == side) //Same Color
                    {
                        simAffectedRows.Remove(row);
                        simAffectedCols.Remove(col - a);
                        valid = true;
                        Debug.Log("valid: left");
                        if (counting == false)
                        {
                            for (int c = 0; c < simAffectedCols.Count; c++)
                            {
                                // Flip affected pieces when not counting all possible
                                simBoardState[simAffectedRows[c], simAffectedCols[c]] = side;
                            }
                        }
                        
                        simTotalPoints += simAffectedCols.Count;
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }

                    if (simBoardState[row, col - a] == 0) // If you don't hit your own piece again
                    {
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }
                }
            }
        }

        if (col < 6)
        {
            if (simBoardState[row, col + 1] == opSide) // Right --------------------------------------
            {
                for (int a = 1; a <= 7 - col; a++)
                {
                    simAffectedRows.Add(row);
                    simAffectedCols.Add(col + a);
                    if (simBoardState[row, col + a] == side) //Same Color
                    {
                        simAffectedRows.Remove(row);
                        simAffectedCols.Remove(col + a);
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < simAffectedCols.Count; c++)
                            {
                                simBoardState[simAffectedRows[c], simAffectedCols[c]] = side;
                            }
                        }
                        simTotalPoints += simAffectedCols.Count;
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }

                    if (simBoardState[row, col + a] == 0)
                    {
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }
                }
            }
        }

        if (row < 6)
        {
            if (simBoardState[row + 1, col] == opSide) // Up --------------------------------------
            {


                for (int a = 1; a <= 7 - row; a++)
                {
                    simAffectedRows.Add(row + a);
                    simAffectedCols.Add(col);
                    if (simBoardState[row + a, col] == side) //Same Color
                    {
                        simAffectedRows.Remove(row + a);
                        simAffectedCols.Remove(col);
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < simAffectedRows.Count; c++)
                            {
                                simBoardState[simAffectedRows[c], simAffectedCols[c]] = side;
                            }
                        }
                        simTotalPoints += simAffectedCols.Count;
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }

                    if (simBoardState[row + a, col] == 0)
                    {
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }
                }
            }
        }

        if (row > 1)
        {
            if (simBoardState[row - 1, col] == opSide) // Down --------------------------------------
            {
                for (int a = 1; a <= row; a++)
                {
                    simAffectedRows.Add(row - a);
                    simAffectedCols.Add(col);
                    if (simBoardState[row - a, col] == side) //Same Color
                    {
                        simAffectedRows.Remove(row - a);
                        simAffectedCols.Remove(col);
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < simAffectedRows.Count; c++)
                            {
                                simBoardState[simAffectedRows[c], simAffectedCols[c]] = side;
                            }
                        }
                        simTotalPoints += simAffectedCols.Count;
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }

                    if (simBoardState[row - a, col] == 0)
                    {
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }
                }
            }
        }

        if (row < 6 && col > 1)
        {
            if (simBoardState[row + 1, col - 1] == opSide) // Top left --------------------------------------
            {
                march = marchLength(row, col, 0);
                for (int a = 1; a <= march; a++)
                {
                    simAffectedRows.Add(row + a);
                    simAffectedCols.Add(col - a);
                    if (simBoardState[row + a, col - a] == side) //Same Color
                    {
                        simAffectedRows.Remove(row + a);
                        simAffectedCols.Remove(col - a);
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < simAffectedRows.Count; c++)
                            {
                                simBoardState[simAffectedRows[c], simAffectedCols[c]] = side;
                            }
                        }
                        simTotalPoints += simAffectedCols.Count;
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }

                    if (simBoardState[row + a, col - a] == 0)
                    {
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }
                }
            }
        }

        if (row < 6 && col < 6)
        {
            if (simBoardState[row + 1, col + 1] == opSide) // Top Right --------------------------------------
            {
                march = marchLength(row, col, 1);
                for (int a = 1; a <= march; a++)
                {
                    simAffectedRows.Add(row + a);
                    simAffectedCols.Add(col + a);
                    if (simBoardState[row + a, col + a] == side) //Same Color
                    {
                        simAffectedRows.Remove(row + a);
                        simAffectedCols.Remove(col + a);
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < simAffectedRows.Count; c++)
                            {
                                simBoardState[simAffectedRows[c], simAffectedCols[c]] = side;
                            }
                        }
                        simTotalPoints += simAffectedCols.Count;
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }

                    if (simBoardState[row + a, col + a] == 0)
                    {
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }
                }
            }
        }

        if (row > 1 && col > 1)
        {
            if (simBoardState[row - 1, col - 1] == opSide) // Bottom Left --------------------------------------
            {
                march = marchLength(row, col, 2);
                for (int a = 1; a <= march; a++)
                {
                    simAffectedRows.Add(row - a);
                    simAffectedCols.Add(col - a);
                    if (simBoardState[row - a, col - a] == side) //Same Color
                    {
                        simAffectedRows.Remove(row - a);
                        simAffectedCols.Remove(col - a);
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < affectedPieces.Count; c++)
                            {
                                simBoardState[simAffectedRows[c], simAffectedCols[c]] = side;
                            }
                        }
                        simTotalPoints += simAffectedCols.Count;
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }

                    if (simBoardState[row - a, col - a] == 0)
                    {
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }
                }
            }
        }

        if (row > 1 && col < 6)
        {
            if (simBoardState[row - 1, col + 1] == opSide) // Bottom Right --------------------------------------
            {
                march = marchLength(row, col, 3);
                for (int a = 1; a <= march; a++)
                {
                    simAffectedRows.Add(row - a);
                    simAffectedCols.Add(col + a);
                    if (simBoardState[row - a, col + a] == side) //Same Color
                    {
                        simAffectedRows.Remove(row - a);
                        simAffectedCols.Remove(col + a);
                        valid = true;
                        if (counting == false)
                        {
                            for (int c = 0; c < affectedPieces.Count; c++)
                            {
                                simBoardState[simAffectedRows[c], simAffectedCols[c]] = side;
                            }
                        }
                        simTotalPoints += simAffectedCols.Count;
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }

                    if (simBoardState[row - a, col + a] == 0)
                    {
                        simAffectedRows.Clear();
                        simAffectedCols.Clear();
                        break;
                    }
                }
            }
        }

        return valid;
    }

    void SimValidMoves(int side, int opSide, int level, int index)
    {
        
        simRowMoves.Clear();
        simColMoves.Clear();
        simMovePoints.Clear();
        int addedValue;
        int arrayIndex;
        // Find all possible moves in simulated board
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (simBoardState[i, j] == 0)
                {
                    if (SimCheckValidMove(i, j, side, opSide, true) == true)
                    {
                        simRowMoves.Add(i);
                        simColMoves.Add(j);
                        simMovePoints.Add(simTotalPoints);
                    }
                }
            }
        }

        if (level%2 != 0) // AI turn
        {
            // Get max points for AI turn
            addedValue = simMovePoints.Max();
            arrayIndex = simMovePoints.ToList().IndexOf(addedValue);
            currentPoints[index] += addedValue;
            bestRow = simRowMoves[arrayIndex];
            bestCol = simColMoves[arrayIndex];
        }
        else // Sim Player Move
        {
            // Get min points for sim Player
            addedValue = simMovePoints.Min();
            arrayIndex = simMovePoints.ToList().IndexOf(addedValue);
            currentPoints[index] -= addedValue;
            bestRow = simRowMoves[arrayIndex];
            bestCol = simColMoves[arrayIndex];
        }



    }

    void AIMove()
    {
        currentRows = new List<int>();
        currentCols = new List<int>();
        currentPoints = new List<int>();
        simRowMoves = new List<int>();
        simColMoves = new List<int>();
        simMovePoints = new List<int>();
        int side = 2;
        int opSide = 1;
        simBoardState = boardState;
        for (int i = 0; i < 8; i++) // Get possible moves
        {
            for (int j = 0; j < 8; j++)
            {
                if (simBoardState[i, j] == 0)
                {
                    if (SimCheckValidMove(i, j, side, opSide, true) == true)
                    {
                        currentRows.Add(i); // Don't Change
                        currentCols.Add(j); // Don't Change
                        currentPoints.Add(simTotalPoints);
                        simRowMoves.Add(i);
                        simColMoves.Add(j);
                        
                    }
                }
            }
        }


        for (int a = 0; a < currentCols.Count; a++) // For each current move
        {
            bestRow = currentRows[a];
            bestCol = currentCols[a];
            for (int l = 1; l <= levels; l++)
            {
                if (levels % 2 == 0)
                {
                    side = 1; // Even levels is sim Player
                    opSide = 2;
                }
                else
                {
                    side = 2; // Odd is AI
                    opSide = 1;
                }
                SimCheckValidMove(bestRow, bestCol, side, opSide, false);
                SimValidMoves(side, opSide, l, a);
            }
        }
        if (currentPoints != null)
        {
            simOverallPoints = currentPoints.Max();
            levelBestIndex = currentPoints.ToList().IndexOf(simOverallPoints);
            bestRow = currentRows[levelBestIndex];
            bestCol = currentCols[levelBestIndex];
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(3);
    }
}
