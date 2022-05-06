using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchRotation : MonoBehaviour
{
    private Touch touch;
    public float rotationSpeed = 0.1f;
    Rigidbody rb;
    public GameObject gameBoard;
    private float _angle = 0.5f;

    public static bool isBoardRotating = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.touches.Length > 0 && !gameController.rotatingSelectedCube)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                isBoardRotating = true;
                var delta = touch.deltaPosition.x * rotationSpeed * Time.deltaTime * -1;
                gameBoard.transform.eulerAngles += new Vector3(0f, delta, 0f);

                _angle += (touch.deltaPosition.y / Screen.height) * 2;
                if (_angle >= Mathf.PI)
                {
                    _angle = Mathf.PI;
                    return;
                }
                else if (_angle <= 0f)
                {
                    _angle = 0f;
                    return;
                }


                var offset = gameBoard.transform.position + new Vector3(0, Mathf.Sin(_angle), Mathf.Cos(_angle)) * 10;
                transform.position = offset;

                Rigidbody rb = GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezePosition;

                Vector3 directionToGameBoard = gameBoard.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(directionToGameBoard);

                rb.constraints &= ~RigidbodyConstraints.FreezePosition;
            }
            else if (touch.phase == TouchPhase.Ended) isBoardRotating = false;
        }

        
        

    }

    

}
