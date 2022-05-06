using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public Camera mainCamera;
    public static List<List<GameObject>> cubeList;

    private GameObject selectedCube;
    public static bool rotatingSelectedCube = false;
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
    void Update()
    {
        if(Input.touches.Length > 0 && !touchRotation.isBoardRotating)
        {
            RaycastHit hit;
            touch = Input.GetTouch(0);
            //if (touch.phase == TouchPhase.Moved) deltaTime = 0;
            //else deltaTime += Time.deltaTime;
            var ray = mainCamera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out hit))
            {
                if (!checkIfCubeIsLegalToMove(hit.transform.name))
                {
                    rotatingSelectedCube = false;
                    return;
                }

                if (selectedCube.gameObject.name != hit.transform.gameObject.name)
                {
                    selectedCube.gameObject.GetComponent<cubeController>().isSelected = false;
                    selectedCube.gameObject.GetComponent<cubeController>().resetCube();
                    selectedCube = hit.transform.gameObject;
                }

                if (!selectedCube.GetComponent<cubeController>().isSelected)
                {
                    selectedCube.GetComponent<cubeController>().isSelected = true;
                }
                rotatingSelectedCube = true;
            }
            else rotatingSelectedCube = false;
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
        return checkHorizantalEdges(nameOfHittedCube) || checkVerticalEdges(nameOfHittedCube);
    }
}
