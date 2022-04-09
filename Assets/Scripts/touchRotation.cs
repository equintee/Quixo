using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchRotation : MonoBehaviour
{
    private Touch touch;
    public float rotationSpeed = 1f;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                var delta = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
                this.transform.eulerAngles += new Vector3(0f,delta,0f);
            }
        }
    }
}
