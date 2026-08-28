using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
public class CameraRotate : MonoBehaviour
{
    private bool clicked = false;
    private float xAxis;
    private float yAxis;
    public SmoothFollow SmoothFollowCamera;
    public GameObject RotatePanel;
    
    public float xSpeed, yspeed;
    public float minHeightLimit, maxheightlimit;
    public MenuManager manager;
    public float maxZoom, minZoom;
    void Start()
    {
        minHeightLimit = SmoothFollowCamera.height;
#if UNITY_EDITOR
        xSpeed = 70f;
        yspeed = 10f;
#elif UNITY_IPHONE
  xSpeed = 65f;
  yspeed = 10f;
#elif UNITY_ANDROID
  xSpeed = 70f;
  yspeed = 5f;
#endif
    }
    void LateUpdate()
    {
        if (clicked)
        {
            xAxis = Input.GetAxis("Mouse X") * (xSpeed) * Time.deltaTime;
            manager.vehicle.GetComponent<carInfo>().carView.Rotate(0, xAxis, 0);
        }
    }
    public void Click1()
    {
        clicked = true;
    }
    public void click2()
    {
        clicked = false;
    }
    void zoom(float distance)
    {
        SmoothFollowCamera.distance = Mathf.Clamp(SmoothFollowCamera.distance - distance, minZoom, maxZoom);
    }
    private void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (prevTouch0 - prevTouch1).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            zoom(difference * 0.01f);
        }
      
            zoom(Input.GetAxis("Mouse ScrollWheel"));
    }
}
