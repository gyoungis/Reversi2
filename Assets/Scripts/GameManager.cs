using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public int[,] boardState = new int[8, 8];
    public List<GameObject> pieceList;
    public GameObject[,] pieceArray = new GameObject[8, 8];
    public GameObject piece;

    private int playerSide;
    private int oppositeSide;

    
    private List<GameObject> affectedPieces;
    // Use this for initialization
    void Start () {
        piece = GetComponent<GameObject>();


        boardState = new int[8, 8]{ { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 1, 2, 0, 0, 0},
                                    { 0, 0, 0, 2, 1, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0} };
        for(int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                pieceArray[x, 7 - y] = pieceList[(7 - x) * 7 + y];
            }
        }
        // Set initial White pieces
        pieceArray[3, 3].transform.position = new Vector3(3, 10, 3);
        pieceArray[3, 3].transform.Rotate(180, 0, 0);
        pieceArray[3, 3].GetComponent<Rigidbody>().useGravity = true;
        pieceArray[4, 4].transform.position = new Vector3(4, 10, 4);
        pieceArray[4, 4].transform.Rotate(180, 0, 0);
        pieceArray[4, 4].GetComponent<Rigidbody>().useGravity = true;

        // Set initial Black pieces
        pieceArray[3, 4].transform.position = new Vector3(3, 10, 4);
        pieceArray[3, 4].GetComponent<Rigidbody>().useGravity = true;
        pieceArray[4, 3].transform.position = new Vector3(4, 10, 3);
        pieceArray[4, 3].GetComponent<Rigidbody>().useGravity = true;
        playerSide = 1;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            float i;
            float j;
            int x;
            int y;
            Vector3 insertPoint;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 12.0f))
            {
                // World Coord
                i = Mathf.Floor(hit.point.x);
                j = Mathf.Floor(hit.point.z);

                // Matrix index
                x = (int)Mathf.Floor(hit.point.x);
                y = (int)Mathf.Floor(hit.point.z) * -1 + 7;

                if (i <= 7 && i >= 0 && j <= 7 && j >= 0 && boardState[x, y] == 0)
                {
                    if (playerSide == 1)
                    {
                        insertPoint = new Vector3(i, 10, j);
                        boardState[x, y] = playerSide;
                        pieceArray[x, y].transform.position = insertPoint;
                        pieceArray[x, y].transform.Rotate(180, 0, 0);
                        pieceArray[x, y].GetComponent<Rigidbody>().useGravity = true;
                        playerSide = 2;
                        oppositeSide = 1;
                    }
                    else
                    {
                        insertPoint = new Vector3(i, 10, j);
                        boardState[x, y] = playerSide;
                        pieceArray[x, y].transform.position = insertPoint;
                        pieceArray[x, y].GetComponent<Rigidbody>().useGravity = true;
                        playerSide = 1;
                        oppositeSide = 2;
                    }
                }
            }
        }

    }

    bool checkValidMove()
    {
        bool valid = false;

        return valid;
    }
}
