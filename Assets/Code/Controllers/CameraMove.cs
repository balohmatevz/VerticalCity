using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public static CameraMove obj;

    public Vector2 DragStartMousePosition;
    public Vector2 LastMousePosition;
    public bool IsDrag = false;
    public Camera Cam;

    public const float CAMERA_X_MIN = -20f;
    public const float CAMERA_X_MAX = 20f;
    public const float CAMERA_Y_MIN = 0f;
    public const float CAMERA_Y_MAX = 20f;
    public const float CAMERA_POS_Z = -10;

    public const float SCROLL_SPEED = 0.3f;

    public const float DRAG_THRESHOLD = 0.05f;

    private void Awake()
    {
        obj = this;
    }

    // Use this for initialization
    void Start()
    {
        Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsDrag = false;
            DragStartMousePosition = Cam.ScreenToWorldPoint(Input.mousePosition);
            LastMousePosition = DragStartMousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Cam.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(DragStartMousePosition, LastMousePosition) > DRAG_THRESHOLD)
            {
                IsDrag = true;
            }

            if (IsDrag)
            {
                Cam.transform.Translate(LastMousePosition - mousePosition);

                //CLAMP position
                Vector3 camPosition = Cam.transform.position;
                camPosition.x = Mathf.Clamp(camPosition.x, CAMERA_X_MIN, CAMERA_X_MAX);
                camPosition.y = Mathf.Clamp(camPosition.y, CAMERA_Y_MIN, CAMERA_Y_MAX);
                camPosition.z = CAMERA_POS_Z;
                Cam.transform.position = camPosition;
            }
        }

        LastMousePosition = Cam.ScreenToWorldPoint(Input.mousePosition);

        Cam.orthographicSize += Input.mouseScrollDelta.y * SCROLL_SPEED;
        Cam.orthographicSize = Mathf.Clamp(Cam.orthographicSize, 3, 8);
    }
}
