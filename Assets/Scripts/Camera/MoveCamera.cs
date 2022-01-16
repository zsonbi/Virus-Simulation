using UnityEngine;

/// <summary>
/// This handles the camera's movement
/// </summary>
public class MoveCamera : MonoBehaviour
{
    private float speed = 1; //The speed of the camera
    private float camXPos { get => Camera.main.transform.position.x; } //Reference to the camera's x position
    private float camYPos { get => Camera.main.transform.position.y; } //Reference to the camera's y position

    //----------------------------------------------------------------
    //Called every frame
    public void Update()
    {
        //Handles speed up with shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 5;
        }

        //Handles wasd movement
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
        //Handles zooming
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
        //Reset speed back
        speed = 1;
    }
}