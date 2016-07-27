
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

using System;

[RequireComponent(typeof(LineRenderer))]

public class LightBeamController2 : MonoBehaviour
{
    public Action<string> AnnounceHit;
    public string bounceTag;
    public UnityEvent FireRay;
    public bool isFinish;
    public int laserDistance;
    public bool LaserDown = false;
    public int maxBounce;
    public UnityEvent OnFinished;
    public UnityEvent resetEverythingAfter;
    public UnityEvent resetEverythingBefore;
    internal GameController gamecontroller;
    internal LineRenderer mLineRenderer;
    internal ParticleHandler phChild;
    internal RaycastHit m_hit;
    internal RaycastHit2D m_hit2D;
    private Vector3 m_LaserDirection;
    private int m_LaserReflected;
    internal Vector3 m_LastPosition;
    internal bool m_loopActive;
    private Vector3 m_RayDirection;
    internal int m_VertexCounter;
    internal ParticleHandler ph;
    // Use this for initialization

    public void invokeOnce()
    {
        if (!IsInvoking("FireRay"))
        {
            FireRay.Invoke();
        }
    }

    public void setDirection(Vector3 angle)
    {
        m_RayDirection = angle;
        invokeOnce();
    }

    public void startRayDelay()
    {
        StartCoroutine(startAnimationDelay());

    }

    internal void CreateRay()
    {
        resetEverythingBefore.Invoke();
        //Variable Decleration
        mLineRenderer.enabled = true;
        m_LaserReflected = 1; //How many times it got reflected
        m_VertexCounter = 1; //How many line segments are there
        m_loopActive = true; //Is the reflecting loop active?
        m_LaserDirection = m_RayDirection; //direction of the next laser
        m_LastPosition = transform.localPosition; //origin of the next laser
        bool is2DHit=false;
        //initalize line renderer
        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, transform.position + new Vector3(0.4f, 0.3f, 0));
        while (m_loopActive)
        {
            Ray ray = new Ray(m_LastPosition, m_LaserDirection);

           
            is2DHit = Static2DCollisonHandler.CheckCollison(m_LaserDirection, m_LastPosition, out m_hit2D, laserDistance);
            // if there is a 3D collison
            if ((Physics.Raycast(ray, out m_hit, laserDistance)))
            {
                // and 2D collision, check which one is closer
                if (is2DHit && (m_hit2D.distance < m_hit.distance))
                {
                    AnnounceHit.Invoke(m_hit2D.transform.tag);
                }
                else {
                    AnnounceHit.Invoke(m_hit.transform.gameObject.tag);
                }
            }
            // if there is only 2D collision
            else if (is2DHit)
            {
                AnnounceHit.Invoke(m_hit2D.transform.tag);
            }
            //no collision
            else {

                ContinuenoCollision();
             
            }
        }
        if (m_LaserReflected > maxBounce)
            m_loopActive = false;
        resetEverythingAfter.Invoke();
    }
    private void ContinuenoCollision()
    {
        m_LaserReflected++;
        m_VertexCounter++;
        mLineRenderer.SetVertexCount(m_VertexCounter);
        mLineRenderer.SetPosition(m_VertexCounter - 1, m_LastPosition + (m_LaserDirection.normalized * laserDistance));

        m_loopActive = false;
    }
    internal virtual void HandleCollision(string i_collisonTag)
    {
        switch (i_collisonTag)
        {
            case "Finish":
                OnFinished.Invoke();
                phChild.addParticle(m_hit.point);
                hitFinishObject(ref m_VertexCounter, m_hit.point);
                m_loopActive = false;
                break;

            case "portal":
                Vector3 secondLocationOfPortal = m_hit.transform.GetChild(0).transform.position;
                m_LaserReflected++;
                hitPortalObject(ref m_VertexCounter, ref m_LastPosition, secondLocationOfPortal, m_hit.point);
                phChild.addParticle(m_hit.point);
                phChild.addParticle(secondLocationOfPortal);
                break;

            case "Prism":
                
                    m_loopActive = false;
                    phChild.addParticle(m_hit.point);
                    hitMiscObject(ref m_VertexCounter, m_hit.point, m_LastPosition);
                    m_hit.transform.GetComponent<PrismSplitter>().invokeBoth(m_LaserDirection,m_hit.point);
                
                break;

            case "mirror":
                m_LaserReflected++;
                hitMirrorObject(ref m_VertexCounter, ref m_LastPosition, ref m_LaserDirection, m_hit);
                break;

            case "score":
                //gamecontroller.addScore(300);
                // m_hit.transform.gameObject.SetActive(false);
                ContinuenoCollision();
                break;
            default:
                hitMiscObject(ref m_VertexCounter, m_hit.point, m_LastPosition);
                phChild.addParticle(m_hit.point);
                m_loopActive = false;
                break;
        }
    }

    internal void resetRay()
    {
        mLineRenderer.enabled = true;
        m_LaserReflected = 1; //How many times it got reflected
        m_VertexCounter = 1; //How many line segments are there
                             // m_loopActive = true; //Is the reflecting loop active?
        m_LaserDirection = m_RayDirection; //direction of the next laser
        m_LastPosition = transform.localPosition; //origin of the next laser
        m_LastPosition.z = -1;
        //initalize line renderer
        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, transform.position + new Vector3(0.4f, 0.3f, 0));
    }

    private void hitFinishObject(ref int vertexCounter, Vector3 location)
    {
        isFinish = true;
        vertexCounter++;
        mLineRenderer.SetVertexCount(vertexCounter);
        mLineRenderer.SetPosition(vertexCounter - 1, location);
    }

    private void hitMirrorObject(ref int vertexCounter, ref Vector3 lastLaserPosition, ref Vector3 laserDirection, RaycastHit hit)
    {
        vertexCounter += 3;
        mLineRenderer.SetVertexCount(vertexCounter);
        mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
        mLineRenderer.SetPosition(vertexCounter - 2, hit.point);
        mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
        //mLineRenderer.SetWidth(.1f, .1f);
        lastLaserPosition = hit.point;
        ph.addParticle(hit.point);
        laserDirection = Vector3.Reflect(laserDirection, hit.normal);
    }

    internal void hitMiscObject(ref int vertexCounter, Vector3 hitPoint, Vector3 lastLaserPosition)
    {
        vertexCounter += 3;
        mLineRenderer.SetVertexCount(vertexCounter);
        mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hitPoint, lastLaserPosition, 0.01f));
        mLineRenderer.SetPosition(vertexCounter - 2, hitPoint);
        mLineRenderer.SetPosition(vertexCounter - 1, hitPoint);
    }

    private void hitPortalObject(ref int vertexCounter, ref Vector3 lastLaserPosition, Vector3 secondLocationOfPortal, Vector3 hitPoint)
    {
        vertexCounter += 6;
        mLineRenderer.SetVertexCount(vertexCounter);
        mLineRenderer.SetPosition(vertexCounter - 6, Vector3.MoveTowards(hitPoint, lastLaserPosition, 0.01f));
        mLineRenderer.SetPosition(vertexCounter - 5, hitPoint);
        mLineRenderer.SetPosition(vertexCounter - 4, hitPoint + new Vector3(0.1f, 0, 0));
        mLineRenderer.SetPosition(vertexCounter - 3, hitPoint + new Vector3(0, 0, -20));
        mLineRenderer.SetPosition(vertexCounter - 2, secondLocationOfPortal + new Vector3(0, 0, -20));
        mLineRenderer.SetPosition(vertexCounter - 1, secondLocationOfPortal);
        lastLaserPosition = secondLocationOfPortal;
    }

    void Start()
    {
        m_RayDirection = Vector3.right;
        FireRay.AddListener(CreateRay);
		gamecontroller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		isFinish = false;
		mLineRenderer = this.GetComponent<LineRenderer> ();
		ph = GameObject.FindGameObjectWithTag ("Player").GetComponent<ParticleHandler> ();
		phChild = ph.transform.GetChild (0).GetComponent<ParticleHandler> ();

        //Add to reset
        resetEverythingAfter.AddListener(ph.DestroyParticles);
        resetEverythingAfter.AddListener(phChild.DestroyParticles);
        resetEverythingBefore.AddListener(phChild.startNewIteration);
        resetEverythingBefore.AddListener(ph.startNewIteration);

        //add to Hit
        AnnounceHit+=HandleCollision;
	}
    IEnumerator startAnimationDelay()
    {
        ph.initializeStartParticles();
        ph.startParticles();
       
        yield return new WaitForSeconds(2);
        invokeOnce();
    }
    // Update is called once per frame
    void Update()
    {
    }
}


