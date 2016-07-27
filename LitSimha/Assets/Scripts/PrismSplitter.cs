using UnityEngine;
using System.Collections;

public class PrismSplitter : MonoBehaviour {
    public PrismController2 FirstPrismRay;
    public PrismController2 SecondPrismRay;
    public int m_degreeOfSplit = 45;
	// Use this for initialization
	void Start () {
        if (FirstPrismRay == null)
        {
            this.GetComponent<PrismController2>();
        }
        if (SecondPrismRay == null)
        {
            SecondPrismRay=transform.GetChild(0).GetComponent<PrismController2>();
        }
        m_degreeOfSplit = m_degreeOfSplit % 360;
	}
	public void invokeBoth(Vector3 i_Direction,Vector3 position)
    {
        Vector3 firstDirection= Quaternion.AngleAxis(360-m_degreeOfSplit, position) * i_Direction;//rotate by 45
        Vector3 SecondDirection = Quaternion.AngleAxis(m_degreeOfSplit,position) * i_Direction;//rotate by 45
        firstDirection.z =0;
        SecondDirection.z = 0;
        Debug.Log(firstDirection + " is the fuckin first");
        Debug.Log(SecondDirection + " fucking second");
        FirstPrismRay.InvokeWithDirection(firstDirection);
        SecondPrismRay.InvokeWithDirection(SecondDirection);
    }
	void Update ()
    {
	
	}

}
