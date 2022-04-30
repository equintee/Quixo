using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeController : MonoBehaviour
{
    // Start is called before the first frame update
    private int cubeValue = 0; //0=empty, 1=X, 2=O
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
