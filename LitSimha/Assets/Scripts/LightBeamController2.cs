
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

using System;

[RequireComponent(typeof(LineRenderer))]

public class LightBeamController2 : MonoBehaviour
{
public bool isFinish;
	public int laserDistance;
	private LineRenderer mLineRenderer;
	public string bounceTag;
	public int maxBounce;
	private float timer = 0;
	private Vector3 prismDirection;
	private ParticleHandler ph;
	public bool LaserDown = false;
	private prismController pc;
	private prismController pcChild;
	private prismController[] ps;
	private GameController gamecontroller;
	bool prismHit = true;
	private int numOfPrismsInScene = 0;
	private ParticleHandler phChild;
    public UnityEvent OnFinished;
    public UnityEvent resetEverythingAfter;
    public UnityEvent resetEverythingBefore;
    public UnityEvent FireRay;
    public Action<string> AnnounceHit;
    private bool m_isFinished;
    private Vector3 m_RayDirection;

    private bool m_loopActive;
    private int m_LaserReflected;
    private int m_VertexCounter;
    private RaycastHit m_hit;
    private Vector3 m_LaserDirection;
    private Vector3 m_LastPosition;

    // Use this for initialization

    void Start ()
	{
        m_RayDirection = Vector3.right;
        FireRay.AddListener(CreateRay);
		gamecontroller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		isFinish = false;
		mLineRenderer = this.GetComponent<LineRenderer> ();
		ph = GameObject.FindGameObjectWithTag ("Player").GetComponent<ParticleHandler> ();
		phChild = ph.transform.GetChild (0).GetComponent<ParticleHandler> ();
		ps = new prismController[30];

        //Add to reset
        resetEverythingAfter.AddListener(ph.DestroyParticles);
        resetEverythingAfter.AddListener(phChild.DestroyParticles);
        resetEverythingBefore.AddListener(phChild.startNewIteration);
        resetEverythingBefore.AddListener(ph.startNewIteration);

        //add to Hit
        AnnounceHit+=HandleCollision;
	}
    public void startRayDelay()
    {
        StartCoroutine(startAnimationDelay());

    }
    IEnumerator startAnimationDelay()
    {
        ph.initializeStartParticles();
        ph.startParticles();
       
        yield return new WaitForSeconds(2);
        invokeOnce();
    }
    public void invokeOnce()
    {
        if (!IsInvoking("FireRay"))
        {
            FireRay.Invoke();
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void setDirection(Vector3 angle)
    {
        m_RayDirection = angle;
        invokeOnce();
    }
    private void CreateRay()
    {
        resetEverythingBefore.Invoke();
        //Variable Decleration
        mLineRenderer.enabled = true;
        m_LaserReflected = 1; //How many times it got reflected
        m_VertexCounter = 1; //How many line segments are there
        m_loopActive = true; //Is the reflecting loop active?
        m_isFinished = false;
        m_LaserDirection = m_RayDirection; //direction of the next laser
        m_LastPosition = transform.localPosition; //origin of the next laser

        //initalize line renderer
        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, transform.position + new Vector3(0.4f, 0.3f, 0));
        while (m_loopActive)
        {
            Ray ray = new Ray(m_LastPosition, m_LaserDirection);

            // if There is collision with something
            if ((Physics.Raycast(ray, out m_hit, laserDistance)))
            {
                AnnounceHit.Invoke(m_hit.transform.gameObject.tag);
            }

            //no collision
            else {


                m_LaserReflected++;
                m_VertexCounter++;
                mLineRenderer.SetVertexCount(m_VertexCounter);
                mLineRenderer.SetPosition(m_VertexCounter - 1, m_LastPosition + (m_LaserDirection.normalized * laserDistance));

                m_loopActive = false;
            }
            }
            if (m_LaserReflected > maxBounce)
                m_loopActive = false;
            resetEverythingAfter.Invoke();
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
                m_isFinished = true;
                break;

            case "portal":
                Vector3 secondLocationOfPortal = m_hit.transform.GetChild(0).transform.position;
                m_LaserReflected++;
                hitPortalObject(ref m_VertexCounter, ref m_LastPosition, secondLocationOfPortal, m_hit.point);
                phChild.addParticle(m_hit.point);
                phChild.addParticle(secondLocationOfPortal);
                break;

            case "Prism":
                {

                }
                break;

            case "mirror":
                m_LaserReflected++;
                hitMirrorObject(ref m_VertexCounter, ref m_LastPosition, ref m_LaserDirection, m_hit);
                break;

            case "score":
                gamecontroller.addScore(300);
                m_hit.transform.gameObject.SetActive(false);
                break;
            default:
                hitMiscObject(ref m_VertexCounter, m_hit.point, m_LastPosition);
                phChild.addParticle(m_hit.point);
                m_loopActive = false;
                break;
        }
    }
    private void hitMiscObject(ref int vertexCounter, Vector3 hitPoint, Vector3 lastLaserPosition)
    {
        vertexCounter += 3;
        mLineRenderer.SetVertexCount(vertexCounter);
        mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hitPoint, lastLaserPosition, 0.01f));
        mLineRenderer.SetPosition(vertexCounter - 2, hitPoint);
        mLineRenderer.SetPosition(vertexCounter - 1, hitPoint);
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

    private void hitFinishObject(ref int vertexCounter, Vector3 location)
    {
        isFinish = true;
        vertexCounter++;
        mLineRenderer.SetVertexCount(vertexCounter);
        mLineRenderer.SetPosition(vertexCounter - 1, location);
    }
}


