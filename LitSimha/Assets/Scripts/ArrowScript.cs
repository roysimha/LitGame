using UnityEngine;
using System.Collections;
using System;

public class ArrowScript : MonoBehaviour
{
    private float m_MinAngle;
    private float m_MaxAngle;
    private float baseAngle = 0.0f;
    public static bool pressOnRing = false;
    //public LightBeamController2 lbc;
    public bool rotation = false;
    private float stepFactor = 1;
    //private bool startRotation = true;
    void start()
    {
        rotation = false;
        //lbc = GameObject.FindGameObjectWithTag("Player").GetComponent<LightBeamController2>();
    }

    void Update()
    {
        //transform.Rotate(Vector3.right * Time.deltaTime);
    }
    public void setStepFactor(float step)
    {
        stepFactor = step;
    }
    void OnMouseDown()
    {
        //	startRotation = false;
        pressOnRing = true;

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = (Input.mousePosition - pos);
        baseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        baseAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;

    }

    internal void setLimits(float minangle, float maxangle)
    {
        m_MaxAngle = maxangle;
        m_MinAngle = minangle;
    }

    void OnMouseUp()
    {
        pressOnRing = false;
        rotation = false;
        Debug.Log("Up");
    }

    void OnMouseDrag()
    {
        //	startRotation = false;
        rotation = true;
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = (Input.mousePosition - pos) ;
        float ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - baseAngle;
        ang *= stepFactor;
       
        Quaternion angle = Quaternion.AngleAxis(ang, Vector3.forward);
        if(!(angle.eulerAngles.z<m_MinAngle||angle.eulerAngles.z>m_MaxAngle))
            transform.rotation = angle;
        Debug.Log("success");
    }
}
