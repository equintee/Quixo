using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public float verticalDistance = 1f;
    public float horizantalDistance = 0.9f;
    protected List<List<GameObject>> cubeList;
    // Start is called before the first frame update
    void Start()
    {
        cubeList = new List<List<GameObject>>();

        for (int i = 0; i < 25; i+=5)
        {
            List<GameObject> tempVerticalCubeList = new List<GameObject>();
            
            for(int j = i; j<i+5; j++)
            {
                tempVerticalCubeList.Add(this.transform.GetChild(i).gameObject);
            }

            cubeList.Add(tempVerticalCubeList);
            Debug.Log("yasda");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
