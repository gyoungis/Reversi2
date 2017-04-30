using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public int[,] boardState = new int[8, 8];
    public List<GameObject> pieceList;
    public GameObject[,] pieceArray = new GameObject[8, 8];
    public GameObject piece;

    private int playerSide = 1;
    private int oppositeSide = 2;

    private int playerScore = 2;
    private int oppositeScore = 2;
    private int points;


    
    private List<GameObject> affectedPieces;
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

                pieceArray[x, y] = pieceList[x * 8 + y];

            }
        }
        
        // Initial Black
        pieceArray[3, 3].transform.position = new Vector3(3.5f, 10, 3.5f);
        pieceArray[3, 3].GetComponent<Rigidbody>().useGravity = true;
        pieceArray[4, 4].transform.position = new Vector3(4.5f, 10, 4.5f);
        pieceArray[4, 4].GetComponent<Rigidbody>().useGravity = true;

        // Intial White
        pieceArray[3, 4].transform.position = new Vector3(3.5f, 10, 4.5f);
        pieceArray[3, 4].transform.Rotate(180, 0, 0);
        pieceArray[3, 4].GetComponent<Rigidbody>().useGravity = true;
        pieceArray[4, 3].transform.position = new Vector3(4.5f, 10, 3.5f);
        pieceArray[4, 3].transform.Rotate(180, 0, 0);
        pieceArray[4, 3].GetComponent<Rigidbody>().useGravity = true;
        //playerSide = 1;
        //oppositeSide = 2;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
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
                        insertPoint = new Vector3(i+0.5f, 10, j+0.5f);
                        if (checkValidMove(row, col, playerSide, oppositeSide))
                        {
                            boardState[row, col] = playerSide;
                            pieceArray[row, col].transform.position = insertPoint;
                            pieceArray[row, col].transform.Rotate(180, 0, 0);
                            pieceArray[row, col].GetComponent<Rigidbody>().useGravity = true;
                            playerSide = 2;
                            oppositeSide = 1;
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
                        if (checkValidMove(row, col, playerSide, oppositeSide))
                        {
                            insertPoint = new Vector3(i + 0.5f, 10, j + 0.5f);
                            boardState[row, col] = playerSide;
                            pieceArray[row, col].transform.position = insertPoint;
                            pieceArray[row, col].GetComponent<Rigidbody>().useGravity = true;
                            playerSide = 1;
                            oppositeSide = 2;
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

    }

    bool checkValidMove(int row, int col, int side, int opSide)
    {
        
        bool valid = false;

        int march;


        if (boardState[row, col - 1] == opSide && col > 1) // Left --------------------------------------
        {
            
            
            for (int a = 1; a <= col; a++)
            {
                affectedPieces.Add(pieceArray[row, col - a]);
                if (boardState[row, col - a] == side) //Same Color
                {
                    affectedPieces.Remove(pieceArray[row, col - a]);
                    valid = true;
                    Debug.Log("valid: left");
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180);
                        boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;                      
                    }
                    
                    points = affectedPieces.Count;
                    addPoints(side, points);
                    affectedPieces.Clear();
                    break;
                }

                if (boardState[row, col - a] == 0)
                {
                    affectedPieces.Clear();
                    break;
                }
            }
        }

        if (boardState[row, col + 1] == opSide && col < 6) // Right --------------------------------------
        {
            

            for (int a = 1; a <= 7 - col; a++)
            {
                affectedPieces.Add(pieceArray[row, col + a]);
                //affectedPieces.Add(pieceArray[col + a, row]);
                if (boardState[row, col + 1] == side) //Same Color
                {
                    affectedPieces.Remove(pieceArray[row, col + a]);
                    //affectedPieces.Remove(pieceArray[col + a, row]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180);
                        boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                    }
                    Debug.Log("valid: right");
                    points = affectedPieces.Count;
                    addPoints(side, points);
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

        if (boardState[row + 1, col] == opSide && row < 6) // Up --------------------------------------
        {
            

            for (int a = 1; a <= 7 - row; a++)
            {
                affectedPieces.Add(pieceArray[row + a, col]);
                if (boardState[row + a, col] == side) //Same Color
                {
                    affectedPieces.Remove(pieceArray[row + a, col]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180);
                        boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                    }
                    Debug.Log("valid up");
                    points = affectedPieces.Count;
                    addPoints(side, points);
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

        if (boardState[row - 1, col] == opSide && row > 1) // Down --------------------------------------
        {
            

            for (int a = 1; a <= row; a++)
            {
                affectedPieces.Add(pieceArray[row - a, col]);
                //affectedPieces.Add(pieceArray[col, row - a]);
                if (boardState[row - a, col] == side) //Same Color
                {
                    affectedPieces.Remove(pieceArray[row - a, col]);
                    //affectedPieces.Remove(pieceArray[col, row - a]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        Debug.Log("Objects: " + affectedPieces[c]);
                        Debug.Log("size: " + affectedPieces.Count);
                        affectedPieces[c].transform.Rotate(0, 0, 180);
                        boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                    }
                    Debug.Log("valid down");
                    points = affectedPieces.Count;
                    addPoints(side, points);
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

        if (boardState[row + 1, col - 1] == opSide && row < 6 && col > 1) // Top left --------------------------------------
        {
            
            march = marchLength(row, col, 0);
            for (int a = 1; a <= march; a++)
            {
                affectedPieces.Add(pieceArray[row + a, col - a]);
                if (boardState[row + a, col - a] == side) //Same Color
                {
                    affectedPieces.Remove(pieceArray[row + a, col - a]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180);
                        boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                    }
                    Debug.Log("valid top left");
                    points = affectedPieces.Count;
                    addPoints(side, points);
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

        if (boardState[row + 1, col + 1] == opSide && row < 6 && col < 6) // Top Right --------------------------------------
        {
            
            march = marchLength(row, col, 1);
            for (int a = 1; a <= march; a++)
            {
                affectedPieces.Add(pieceArray[row + a, col + a]);
                if (boardState[row + a, col + a] == side) //Same Color
                {
                    affectedPieces.Remove(pieceArray[row + a, col + a]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180);
                        boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                    }
                    Debug.Log("valid top right");
                    points = affectedPieces.Count;
                    addPoints(side, points);
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

        if (boardState[row - 1, col - 1] == opSide && row > 1 && col > 1) // Bottom Left --------------------------------------
        {
            
            march = marchLength(row, col, 2);
            for (int a = 1; a <= march; a++)
            {
                affectedPieces.Add(pieceArray[row - a, col - a]);
                if (boardState[row - a, col - a] == side) //Same Color
                {
                    affectedPieces.Remove(pieceArray[row - a, col - a]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180);
                        boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                    }
                    Debug.Log("valid bottom left");
                    points = affectedPieces.Count;
                    addPoints(side, points);
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

        if (boardState[row - 1, col + 1] == opSide && row > 1 && col < 6) // Bottom Right --------------------------------------
        {
            
            march = marchLength(row, col, 3);
            for (int a = 1; a <= march; a++)
            {
                affectedPieces.Add(pieceArray[row - a, col + a]);
                //affectedPieces.Add(pieceArray[col - a, row - a]);
                if (boardState[row - a, col + a] == side) //Same Color
                {
                    affectedPieces.Remove(pieceArray[row - a, col + a]);
                    //affectedPieces.Remove(pieceArray[col - a, row - a]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180);
                        boardState[(int)affectedPieces[c].transform.position.z, (int)affectedPieces[c].transform.position.x] = side;
                    }
                    Debug.Log("valid bottom right");
                    points = affectedPieces.Count;
                    addPoints(side, points);
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
            oppositeScore -= points;
        }
    }

}
