using UnityEngine;
using System.Collections;
using System;

public class UICharacterPress : MonoBehaviour {

    private float m_MinAngle;
    private float m_MaxAngle;
    private float baseAngle = 0.0f;
    public static bool pressOnRing = false;
    public LightBeamController2 lbc;
    public bool rotation = false;
    private float stepFactor = 1;
    private ClickHandler m_MainCharAnimation;
    //private bool startRotation = true;
    void Start()
    {
        rotation = false;
        //lbc = gameObject.GetComponentInParent<LightBeamController2>();
        m_MinAngle = 0;
        m_MaxAngle = 180;
        m_MainCharAnimation = GetComponentInParent<ClickHandler>();
    }

    void Update()
    {
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
        pos = (Input.mousePosition - pos);
        float ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - baseAngle;
        ang *= stepFactor;
        
        Quaternion angle = Quaternion.AngleAxis(ang, Vector3.forward);
        if (!(angle.eulerAngles.z < m_MinAngle || angle.eulerAngles.z > m_MaxAngle))
         {
            lbc.setDirection(angle * Vector3.right);
            transform.rotation = angle;
            setRightAnimation(angle.eulerAngles.z);
        }
       
    }

    private void setRightAnimation(float z)
    {
        if (z < 45)
        {
            m_MainCharAnimation.setAnimation(1);
        }
        if (z > 45 && z<80)
        {
            m_MainCharAnimation.setAnimation(2);
        }
        if (z > 80 && z < 120)
        {
            m_MainCharAnimation.setAnimation(8);
        }
        if (z > 120 && z<140 )
        {
            m_MainCharAnimation.setAnimation(4);
        }
        if (z > 140)
        {
            m_MainCharAnimation.setAnimation(5);
        }
    }
}
