using UnityEngine;

/// <summary>
/// This handles the camera's movement
/// </summary>
public class MoveCamera : MonoBehaviour
{
    private float speed = 1; //The speed of the camera
    private float camXPos { get => mainCam.transform.position.x; } //Reference to the camera's x position
    private float camYPos { get => mainCam.transform.position.y; } //Reference to the camera's y position

    private Camera mainCam; //Reference to the main camera

    //--------------------------------------------------------------------
    //At the start of the script set the camera at the center of the map
    private void Start()
    {
        mainCam = Camera.main;
        mainCam.transform.position = new Vector3(Settings.WorldSize / 3, Settings.WorldSize / 3, Camera.main.transform.position.z);
    }

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
            mainCam.transform.position = new Vector3(camXPos, camYPos + (0.1f * speed), -10);
        }
        if (Input.GetKey(KeyCode.S))
        {
            mainCam.transform.position = new Vector3(camXPos, camYPos - (0.1f * speed), -10);
        }
        if (Input.GetKey(KeyCode.A))
        {
            mainCam.transform.position = new Vector3(camXPos - (0.1f * speed), camYPos, -10);
        }
        if (Input.GetKey(KeyCode.D))
        {
            mainCam.transform.position = new Vector3(camXPos + (0.1f * speed), camYPos, -10);
        }
        //Handles zooming
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            if (mainCam.orthographicSize > 5)
                mainCam.orthographicSize = Camera.main.orthographicSize - (0.1f * speed);
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            if (mainCam.orthographicSize < 1000)
                mainCam.orthographicSize = Camera.main.orthographicSize + (0.1f * speed);
        }
        //Reset speed back
        speed = 1;
    }
}