using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public Vector2 LastMousePosition;
    public Camera Cam;

    public const float CAMERA_X_MIN = -20f;
    public const float CAMERA_X_MAX = 20f;
    public const float CAMERA_Y_MIN = 0f;
    public const float CAMERA_Y_MAX = 20f;
    public const float CAMERA_POS_Z = -10;

    // Use this for initialization
    void Start()
    {
        Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Cam.ScreenToWorldPoint(Input.mousePosition);
            Cam.transform.Translate(LastMousePosition - mousePosition);

            //CLAMP position
            Vector3 camPosition = Cam.transform.position;
            camPosition.x = Mathf.Clamp(camPosition.x, CAMERA_X_MIN, CAMERA_X_MAX);
            camPosition.y = Mathf.Clamp(camPosition.y, CAMERA_Y_MIN, CAMERA_Y_MAX);
            camPosition.z = CAMERA_POS_Z;
            Cam.transform.position = camPosition;
        }

        LastMousePosition = Cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
