using UnityEngine;
using System.Collections;

public class ClickHandler : MonoBehaviour
{
    private LightBeamController2 lbc;
    private MainCharAnimationController m_MainCharAnimation;
    // Use this for initialization
    void Start()
    {
        m_MainCharAnimation = transform.GetChild(0).GetComponent<MainCharAnimationController>();
        transform.GetChild(1).gameObject.SetActive(false);
        lbc = GetComponent<LightBeamController2>();
    }
    void OnMouseDown()
    {
        m_MainCharAnimation.ChangeToAnimation(0);
    }
    void OnMouseUp()
    {
        m_MainCharAnimation.ChangeToAnimation(1);
        lbc.startRayDelay();
        transform.GetChild(1).gameObject.SetActive(true);
    }
    public void setAnimation(int i)
    {
        m_MainCharAnimation.ChangeToAnimation(i);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UICharacterPress.pressOnRing)
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}