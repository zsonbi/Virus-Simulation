using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float speed = 1;
    private float camXPos { get => Camera.main.transform.position.x; }
    private float camYPos { get => Camera.main.transform.position.y; }

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 5;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position = new Vector3(camXPos, camYPos + (0.1f * speed), -10);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position = new Vector3(camXPos, camYPos - (0.1f * speed), -10);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position = new Vector3(camXPos - (0.1f * speed), camYPos, -10);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position = new Vector3(camXPos + (0.1f * speed), camYPos, -10);
        }
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            if (Camera.main.orthographicSize > 5)
                Camera.main.orthographicSize = Camera.main.orthographicSize - (0.1f * speed);
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            if (Camera.main.orthographicSize < 1000)
                Camera.main.orthographicSize = Camera.main.orthographicSize + (0.1f * speed);
        }
        speed = 1;
    }
}