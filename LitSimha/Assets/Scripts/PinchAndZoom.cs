using UnityEngine;
using System.Collections;
using System;

public class PinchAndZoom : MonoBehaviour
{

    private float orthoZoomSpeed = 0.05f;        // The rate of change of the orthographic size in orthographic mode.
    private float orthoCamSize;
    private Vector3 camPos;
    private float maxZoom;
    private float minZoom = 2;
    private float panSpeed = -0.1f;
    public float ScreenWidth;
    public float SideMenuWidth;
    public float topRightX;
    public static bool isPanning;        // Is the mycamera being panned?
    private Camera mycamera;
    Vector3 bottomLeft;
    Vector3 topRight;

    float cameraMaxY;
    float cameraMinY;
    float cameraMaxX;
    float cameraMinX;

    void Start()
    {
        ScreenWidth = Screen.width;
        SideMenuWidth = Screen.width * 0.1953f;
        topRightX = ScreenWidth;
        isPanning = false;
        maxZoom = Camera.main.orthographicSize;
        orthoCamSize = maxZoom;
        camPos = Camera.main.transform.position;
        mycamera = Camera.main;
        //set max mycamera bounds (assumes mycamera is max zoom and centered on Start)
        topRight = mycamera.ScreenToWorldPoint(new Vector3(topRightX, mycamera.pixelHeight, -transform.position.z));
        bottomLeft = mycamera.ScreenToWorldPoint(new Vector3(0, 0, -transform.position.z));
        cameraMaxX = topRight.x;
        cameraMaxY = topRight.y;
        cameraMinX = bottomLeft.x;
        cameraMinY = bottomLeft.y;
    }

    void Update()
    {
#if UNITY_EDITOR
        //click and drag
        if (Input.GetMouseButton(0))
        {
            isPanning = true;
            float x = Input.GetAxis("Mouse X") * panSpeed;
            float y = Input.GetAxis("Mouse Y") * panSpeed;
            transform.Translate(x, y, 0);
            isPanning = false;
        }
#endif


        // One Finger Pan
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            isPanning = true;
            //            Touch touchZero = Input.GetTouch(0);
            //            float x = touchZero.position.x * panSpeed;
            //            float y = touchZero.position.y * panSpeed;
            float x =Input.GetTouch(0).deltaPosition.x * panSpeed;
            float y = Input.GetTouch(0).deltaPosition.y * panSpeed;
            transform.Translate(x, y, 0);
            isPanning = false;

        }

#if UNITY_EDITOR
        //zoom
        if ((Input.GetAxis("Mouse ScrollWheel") > 0) && Camera.main.orthographicSize > minZoom) // forward
        {
            Camera.main.orthographicSize = Camera.main.orthographicSize - orthoZoomSpeed;
        }

        if ((Input.GetAxis("Mouse ScrollWheel") < 0) && Camera.main.orthographicSize < maxZoom) // back          
        {
            Camera.main.orthographicSize = Camera.main.orthographicSize + orthoZoomSpeed;
        }
#endif

        // 2 finger Zoom
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the mycamera is orthographic...
            if (mycamera.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                mycamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                mycamera.orthographicSize = Mathf.Max(mycamera.orthographicSize, minZoom);

                // Make sure the orthographic size never goes above original size.
                mycamera.orthographicSize = Mathf.Min(mycamera.orthographicSize, maxZoom);
            }
        }


        // On double tap image will be set at original position and scale
        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).tapCount == 2)
        {
            mycamera.orthographicSize = orthoCamSize;
            Camera.main.transform.position = camPos;
        }


        //check if mycamera is out-of-bounds, if so, move back in-bounds
        topRight = mycamera.ScreenToWorldPoint(new Vector3(topRightX, mycamera.pixelHeight, -transform.position.z));
        bottomLeft = mycamera.ScreenToWorldPoint(new Vector3(0, 0, -transform.position.z));

        if (topRight.x > cameraMaxX)
        {
            transform.position = new Vector3(transform.position.x - (topRight.x - cameraMaxX), transform.position.y, transform.position.z);
        }

        if (topRight.y > cameraMaxY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (topRight.y - cameraMaxY), transform.position.z);
        }

        if (bottomLeft.x < cameraMinX)
        {
            transform.position = new Vector3(transform.position.x + (cameraMinX - bottomLeft.x), transform.position.y, transform.position.z);
        }

        if (bottomLeft.y < cameraMinY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (cameraMinY - bottomLeft.y), transform.position.z);
        }


        // If back button press andriod
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    }