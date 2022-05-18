using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public Camera mainCamera;
    public static List<List<GameObject>> cubeList;

    public static int turn = 0; //0:X 1:O
    private GameObject selectedCube;
    public static bool lockCameraRotation = false;
    private bool movingCube = false;
    public float temp;
    void Start()
    {
        cubeList = new List<List<GameObject>>();

        for (int i = 0; i < 25; i+=5)
        {
            List<GameObject> tempVerticalCubeList = new List<GameObject>();
            
            for(int j = i; j<i+5; j++)
            {
                tempVerticalCubeList.Add(this.transform.GetChild(j).gameObject);
            }
            cubeList.Add(tempVerticalCubeList);           
        }
        selectedCube = cubeList[0][0];
    }

    private Touch touch;
    bool validCube = false;
    int cubeValueBeforePlaced = 2;
    void Update()
    {
        if (Input.touches.Length > 0 && !touchRotation.isBoardRotating)
        {
            RaycastHit hit;
            touch = Input.GetTouch(0);

            var ray = mainCamera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out hit))
            {

                if (selectedCube.gameObject.name != hit.transform.gameObject.name && !movingCube && checkIfCubeIsLegalToMove(hit.transform.name))
                {
                    selectedCube.gameObject.GetComponent<cubeController>().cubeAnimated = false;
                    selectedCube.gameObject.GetComponent<cubeController>().cubeValue = cubeValueBeforePlaced;
                    selectedCube.gameObject.GetComponent<cubeController>().resetCube();
                    selectedCube = hit.transform.gameObject;
                    if (selectedCube.GetComponent<cubeController>().cubeValue == 2 || selectedCube.GetComponent<cubeController>().cubeValue == turn)
                    {
                        cubeValueBeforePlaced = selectedCube.GetComponent<cubeController>().cubeValue;
                        selectedCube.GetComponent<cubeController>().cubeValue = turn;
                        selectedCube.GetComponent<cubeController>().resetCube();
                        validCube = true;
                    }
                    else validCube = false;
                    //selectedCube.GetComponent<cubeController>().cubeAnimated = (selectedCube.GetComponent<cubeController>().cubeValue == 2 || selectedCube.GetComponent<cubeController>().cubeValue == turn);
                    lockCameraRotation = false;
                }
                
                if (touch.phase == TouchPhase.Moved && validCube)
                {

                    Vector3 cubeMovment = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                    cubeMovment.y = 1;
                    selectedCube.GetComponent<cubeController>().cubeAnimated = false;
                    selectedCube.transform.position = cubeMovment;
                    movingCube = true;
                    lockCameraRotation = true;

                    int[] positionOnList = positionOnCubeList();
                    bool movingRight, movingUp;

                    if (selectedCube.transform.localPosition.z > selectedCube.GetComponent<cubeController>().defaultPosition.z) movingUp = false;
                    else movingUp = true;

                    if (selectedCube.transform.localPosition.x < selectedCube.GetComponent<cubeController>().defaultPosition.x) movingRight = true;
                    else movingRight = false;

                    if (movingUp)
                    {
                        float destinationRatio = 1 - (-1.72f - selectedCube.transform.localPosition.z) / (-1.72f - selectedCube.GetComponent<cubeController>().defaultPosition.z);
                        if (destinationRatio < 1 && destinationRatio > 0)
                        {
                            for (int i = positionOnList[0] - 1; i >= 0; i--)
                            {
                                GameObject cubeToMove = cubeList[i][positionOnList[1]];
                                float destination = (0.86f * destinationRatio) + cubeToMove.GetComponent<cubeController>().defaultPosition.z;
                                cubeToMove.transform.localPosition = new Vector3(cubeToMove.transform.localPosition.x, cubeToMove.transform.localPosition.y, destination);
                            }
                        }
                    }
                    else
                    {
                        float destinationRatio = 1 - (1.72f - selectedCube.transform.localPosition.z) / (1.72f - selectedCube.GetComponent<cubeController>().defaultPosition.z);
                        if (destinationRatio < 1 && destinationRatio > 0)
                        {
                            for (int i = positionOnList[0] + 1; i <= 4; i++)
                            {
                                GameObject cubeToMove = cubeList[i][positionOnList[1]];
                                float destination = (-0.86f * destinationRatio) + cubeToMove.GetComponent<cubeController>().defaultPosition.z;
                                cubeToMove.transform.localPosition = new Vector3(cubeToMove.transform.localPosition.x, cubeToMove.transform.localPosition.y, destination);
                            }
                        }
                    }

                    if (movingRight)
                    {
                        float destinationRatio = 1 - (-1.72f - selectedCube.transform.localPosition.x) / (-1.72f - selectedCube.GetComponent<cubeController>().defaultPosition.x);
                        if (destinationRatio < 1 && destinationRatio > 0)
                        {
                            for (int i = positionOnList[1] + 1; i <= 4; i++)
                            {
                                GameObject cubeToMove = cubeList[positionOnList[0]][i];
                                float destination = (0.86f * destinationRatio) + cubeToMove.GetComponent<cubeController>().defaultPosition.x;
                                cubeToMove.transform.localPosition = new Vector3(destination, cubeToMove.transform.localPosition.y, cubeToMove.transform.localPosition.z);
                            }
                        }
                    }
                    else
                    {
                        float destinationRatio = 1 - (1.72f - selectedCube.transform.localPosition.x) / (1.72f - selectedCube.GetComponent<cubeController>().defaultPosition.x);
                        if (destinationRatio < 1 && destinationRatio > 0)
                        {
                            for (int i = positionOnList[1] - 1; i >= 0; i--)
                            {
                                GameObject cubeToMove = cubeList[positionOnList[0]][i];
                                float destination = (-0.86f * destinationRatio) + cubeToMove.GetComponent<cubeController>().defaultPosition.x;
                                cubeToMove.transform.localPosition = new Vector3(destination, cubeToMove.transform.localPosition.y, cubeToMove.transform.localPosition.z);
                            }
                        }
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    bool cubePlaced = placeCube();
                    if (cubePlaced)
                    {
                        selectedCube.GetComponent<cubeController>().cubeValue = turn;
                        cubeValueBeforePlaced = turn;
                        validCube = false;
                        checkGameStatus();
                        turn = turn == 1 ? 0 : 1;
                    }

                    resetAllCubes();
                    selectedCube.GetComponent<cubeController>().cubeAnimated = (selectedCube.GetComponent<cubeController>().cubeValue == 2 || selectedCube.GetComponent<cubeController>().cubeValue == turn);
                    lockCameraRotation = false;
                    movingCube = false;
                }
                         
            }else lockCameraRotation = false;
        }
    }

    private bool checkHorizantalEdges(string nameOfHittedCube)
    {
        for (int i = 0; i < 5; i++)
        {
            //Top horizantal edge
            if (cubeList[0][i].transform.name == nameOfHittedCube)
            {
                return true;
            }
            //Bottom horizantal edge;
            if (cubeList[4][i].transform.name == nameOfHittedCube)
            {
                return true;
            }
        }
        return false;
    }

    private bool checkVerticalEdges(string nameOfHittedCube)
    {
        for(int i = 1; i < 4; i++)
        {
            if(cubeList[i][0].transform.name == nameOfHittedCube)
            {
                return true;
            }

            if(cubeList[i][4].transform.name == nameOfHittedCube)
            {
                return true;
            }            
        }
        return false;
    }

    private bool checkIfCubeIsLegalToMove(string nameOfHittedCube)
    {

        return (checkHorizantalEdges(nameOfHittedCube) || checkVerticalEdges(nameOfHittedCube));
    }

    private int[] positionOnCubeList()
    {
        int[] position = new int[2];

        for(int i = 0; i<5; i++)
        {
            for(int j = 0; j<5; j++)
            {
                if(cubeList[i][j] == selectedCube)
                {
                    position[0] = i;
                    position[1] = j;
                    goto endLoop;
                }
            }
        }
        endLoop:
            return position;
    }

    private void resetAllCubes()
    {
        foreach(List<GameObject> row in cubeList)
        {
            foreach(GameObject cube in row)
            {
                cube.GetComponent<cubeController>().resetCube();
            }
        }
    }

    private bool placeCube()
    {
        int? rotation = null; // 0:down, 1:up, 2:left, 3:right
        if(selectedCube.GetComponent<cubeController>().defaultPosition.z != selectedCube.transform.localPosition.z && selectedCube.GetComponent<cubeController>().defaultPosition.x + 0.86f > selectedCube.transform.localPosition.x && selectedCube.transform.localPosition.x > selectedCube.GetComponent<cubeController>().defaultPosition.x - 0.86f)
        {
            if (selectedCube.transform.localPosition.z > 1f && selectedCube.transform.localPosition.z < 2.5f)
            {
                Debug.Log("Cube placed down.");
                rotation = 0;
            }

            if (selectedCube.transform.localPosition.z < -1f && selectedCube.transform.localPosition.z > -2.5f)
            {
                Debug.Log("Cube placed up.");
                rotation = 1;
            }
        }

        if (selectedCube.GetComponent<cubeController>().defaultPosition.x != selectedCube.transform.localPosition.x && selectedCube.GetComponent<cubeController>().defaultPosition.z + 0.86f > selectedCube.transform.localPosition.z && selectedCube.transform.localPosition.z > selectedCube.GetComponent<cubeController>().defaultPosition.z - 0.86f)
        {
            if (selectedCube.transform.localPosition.x > 1f && selectedCube.transform.localPosition.x < 2.5f)
            {
                Debug.Log("Cube placed left.");
                rotation = 2;
            }

            if (selectedCube.transform.localPosition.x < -1f && selectedCube.transform.localPosition.x > -2.5f)
            {
                rotation = 3;
                Debug.Log("Cube placed right.");
            }
        }

        return changeCubeList(rotation);
    }

    private bool changeCubeList(int? rotation) // 0:down, 1:up, 2:left, 3:right
    {
        if (rotation == null)
        {
            Debug.Log("No changes made.");
            return false;
        }

        int[] position = positionOnCubeList();

        if (position[0] == 4 && rotation == 0) return false;
        if (position[0] == 0 && rotation == 1) return false;
        if (position[1] == 0 && rotation == 2) return false;
        if (position[1] == 4 && rotation == 3) return false;

        switch (rotation)
        {
            case 0:
                for(int i = position[0] + 1; 5 > i; i++)
                {
                    cubeList[i - 1][position[1]] = cubeList[i][position[1]]; 
                }
                cubeList[4][position[1]] = selectedCube;
                return true;

            case 1:
                for(int i = position[0] - 1; i > -1; i--)
                {
                    cubeList[i + 1][position[1]] = cubeList[i][position[1]]; 
                }
                cubeList[0][position[1]] = selectedCube;
                return true;
            case 2:
                for(int i = position[1] - 1; i > -1; i--)
                {
                    cubeList[position[0]][i + 1] = cubeList[position[0]][i];
                }
                cubeList[position[0]][0] = selectedCube;
                return true;
            case 3:
                for(int i = position[1] + 1; 5 > i; i++)
                {
                    cubeList[position[0]][i - 1] = cubeList[position[0]][i];
                }
                cubeList[position[0]][4] = selectedCube;
                return true;

        }
        return false;     
                
    }

    private int checkGameStatus() //-1 no winner, 1:X, 2:O
    {
        int winner = -1;
        for(int i = 0; i<5; i++)
        {
            int verticalWinner = cubeList[i][0].GetComponent<cubeController>().cubeValue;
            for(int j = 0; j<5; j++)
            {
                if (cubeList[i][j].GetComponent<cubeController>().cubeValue == 2) break;
                if (verticalWinner != cubeList[i][j].GetComponent<cubeController>().cubeValue) break;
                if(j == 4)
                {
                    winner = verticalWinner;
                    if (winner != turn) return endGameScreen(winner);
                }
            }
        }

        for(int i = 0; i<5; i++)
        {
            int horizantalWinner = cubeList[i][0].GetComponent<cubeController>().cubeValue;
            for (int j = 0; j<5; j++)
            {
                if (cubeList[j][i].GetComponent<cubeController>().cubeValue == 2) break;
                if (horizantalWinner != cubeList[j][i].GetComponent<cubeController>().cubeValue) break;
                if(j == 4)
                {
                    winner = horizantalWinner;
                    if (winner != turn) return endGameScreen(winner);
                }
            }
        }

        return endGameScreen(winner);
    }

    private int endGameScreen(int winner)
    {
        if (winner == -1) Debug.Log("No one wins.");
        if (winner == 0) Debug.Log("X wins.");
        if (winner == 1) Debug.Log("O wins.");

        return winner;
    }
}
