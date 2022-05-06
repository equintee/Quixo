using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeController : MonoBehaviour
{
    // Start is called before the first frame update
    private int cubeValue = 0; //0=empty, 1=X, 2=O
    public bool isSelected = false;
    public Vector3 defaultPosition;
    void Start()
    {
        defaultPosition = transform.localPosition;
    }

    // Update is called once per frame

    void Update()
    {
        if (isSelected) animateCube();


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
                    isSelected = false;
                    return;
                }
            }
        }
    }
    public void changeCubeToX()
    {
        cubeValue = 1;
        float deltaTime = 0f;
        while(deltaTime <= 2f)
        {
            deltaTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(90,0,0,0), deltaTime);
        }
    }
}
