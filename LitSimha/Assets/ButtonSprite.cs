using UnityEngine;
using System.Collections;
using UnityEngine.Events;
public class ButtonSprite : MonoBehaviour {
    public UnityEvent onClick;
    private bool m_isClicked;
	// Use this for initialization
	void Start () {
        m_isClicked = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void OnMouseDown()
    {
        m_isClicked = true;
    }
    private void OnMouseUp()
    {
        if (m_isClicked)
        {
            onClick.Invoke();
            m_isClicked = false;
        }
    }
}
