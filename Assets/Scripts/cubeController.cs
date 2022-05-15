using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeController : MonoBehaviour
{
    // Start is called before the first frame update
    public int cubeValue = 2; //0:X 1:O 2:empty
    private Touch touch;
    public bool cubeAnimated = false;
    public bool cubeMoving = false;
    public Vector3 defaultPosition;
    void Start()
    {
        cubeValue = 2;
        defaultPosition = transform.localPosition;
    }

    // Update is called once per frame

    void Update()
    {
        if (cubeAnimated) animateCube();


    }

    private bool animateUpwards = true;
    private float deltaTime = 0f;
    public void animateCube()
    {
        deltaTime += Time.deltaTime;
        Vector3 heightCap = defaultPosition + new Vector3(0, 1.5f, 0);
        if (animateUpwards)
        {
            transform.localPosition = Vector3.Lerp(defaultPosition, heightCap, deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(heightCap, defaultPosition, deltaTime);
        }
        
        if(deltaTime > 1f)
        {
            animateUpwards = !animateUpwards;
            deltaTime = 0f;
        }
    }
    

    
    public void resetCube()
    {
        for(int i=0; i<5; i++)
        {
            for(int j=0; j<5; j++)
            {
                if(gameController.cubeList[i][j].transform.name == this.name)
                {
                    defaultPosition = new Vector3((2-j) * 0.86f,0, (i-2) * 0.86f);
                    transform.localPosition = defaultPosition;

                    transform.localRotation = cubeValue == 2 ? Quaternion.Euler(90, 0, 0) : Quaternion.Euler(cubeValue * 180 + 180, 0, 0);

                    cubeAnimated = false;
                    cubeMoving = false;
                    deltaTime = 0;
                    return;
                }
            }
        }
    }
    
    public void rotateCube() //0:X 1:O
    {
        transform.localRotation = cubeValue == 2 ? Quaternion.Euler(90, 0, 0) : Quaternion.Euler(cubeValue * 180 + 180, 0, 0);
    }
}
