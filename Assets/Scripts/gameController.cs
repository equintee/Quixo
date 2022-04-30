using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public Camera mainCamera;
    protected List<List<GameObject>> cubeList;

    private GameObject selectedCube;
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
    }

    private float deltaTime = 0f;
    private Touch touch;
    void Update()
    {
        if(Input.touches.Length > 0 && !touchRotation.isBoardRotating)
        {
            RaycastHit hit;
            touch = Input.GetTouch(0);
            deltaTime -= Time.deltaTime;
            //if (touch.phase == TouchPhase.Moved) deltaTime = 0;
            //else deltaTime += Time.deltaTime;
            var ray = mainCamera.ScreenPointToRay(touch.position);
            if(Physics.Raycast(ray,out hit) && deltaTime <= 0)
            {
                deltaTime = 1f;
                string nameOfHittedCube = hit.transform.name;

                //To check horizantal edges
                if (!checkHorizantalEdges(nameOfHittedCube))
                {
                    checkVerticalEdges(nameOfHittedCube);      
                }
                if (selectedCube != null)
                {
                    selectedCube.SetActive(false);
                }
            }
        }
    }

    private bool checkHorizantalEdges(string nameOfHittedCube)
    {
        for (int i = 0; i < 5; i++)
        {
            //Top horizantal edge
            if (cubeList[0][i].transform.name == nameOfHittedCube)
            {
                selectedCube = cubeList[0][i];
                return true;
            }
            //Bottom horizantal edge;
            if (cubeList[4][i].transform.name == nameOfHittedCube)
            {
                selectedCube = cubeList[4][i];
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
                selectedCube = cubeList[i][0];
                return true;
            }

            if(cubeList[i][4].transform.name == nameOfHittedCube)
            {
                selectedCube = cubeList[i][0];
                return true;
            }            
        }
        return false;
    }
}
