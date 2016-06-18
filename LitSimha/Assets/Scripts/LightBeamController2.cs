
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
    public UnityEvent resetEverything;
    private bool m_isFinished;
    private Vector3 m_RayDirection;
	// Use this for initialization
	void Start ()
	{
        m_RayDirection = Vector3.right;
		gamecontroller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		isFinish = false;
		mLineRenderer = this.GetComponent<LineRenderer> ();
		ph = GameObject.FindGameObjectWithTag ("Player").GetComponent<ParticleHandler> ();
		phChild = ph.transform.GetChild (0).GetComponent<ParticleHandler> ();
		ps = new prismController[30];
		int i = 0;
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Prism")) {
			numOfPrismsInScene++;
			ps [i] = g.GetComponent<prismController> ();
			i++;
		}
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
        LaserDown = true;
    }
    private void resetPrisms()
    {
        for (int i = 0; i < numOfPrismsInScene; i++)
        {
            ps[i].clearPrism();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (LaserDown)
        {
            timer = 0;
            StartCoroutine("FireMahLazer");
            phChild.DestroyParticles();
            ph.DestroyParticles();
            LaserDown = false;

        }
    }
    public void setDirection(Vector3 angle)
    {
        m_RayDirection = angle;
        LaserDown = true;
    }
	IEnumerator FireMahLazer ()
	{
        //Debug.Log("Running");
        mLineRenderer.enabled = true;
		int laserReflected = 1; //How many times it got reflected
		int vertexCounter = 1; //How many line segments are there
		bool loopActive = true; //Is the reflecting loop active?
		bool prismSet = false;
        m_isFinished = false;
        ph.startNewIteration();
        phChild.startNewIteration();
        Vector3 laserDirection =m_RayDirection; //direction of the next laser
		Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser
		mLineRenderer.SetVertexCount (1);
		mLineRenderer.SetPosition (0, transform.position+new Vector3(0.4f,0.3f,0));
		RaycastHit hit;
		while (loopActive) {
			prismSet = false;
			Ray ray = new Ray (lastLaserPosition, laserDirection);
			if ((Physics.Raycast (ray, out hit, laserDistance))) {
				switch (hit.transform.gameObject.tag) {
				case "Finish":
                        OnFinished.Invoke();
                        phChild.addParticle(hit.point);
                        hitFinishObject (ref vertexCounter, hit.point);
                        loopActive = false;
                        m_isFinished = true;
					break;

				case "portal":
					Vector3 secondLocationOfPortal = hit.transform.GetChild (0).transform.position;
					laserReflected++;
					hitPortalObject (ref vertexCounter, ref lastLaserPosition, secondLocationOfPortal, hit.point);
					phChild.addParticle (hit.point);
					phChild.addParticle (secondLocationOfPortal);
					break;

				case "Prism":
					{
						hitMiscObject (ref vertexCounter, hit.point, lastLaserPosition);        
						prismController p = hit.transform.GetComponent<prismController> ();
						prismController pChild = p.transform.GetChild (0).GetComponent<prismController> ();
						loopActive = false;
                            
						prismDirection = laserDirection;
						p.laserDirection = prismDirection + new Vector3 (1, 0, 0);
						p.LaserDown = true;
						prismSet = true;
						pChild.laserDirection = prismDirection + new Vector3 (-1, 0, 0);
						pChild.LaserDown = true;
						phChild.addParticle (hit.point);
					}
					break;

				case "mirror":
					laserReflected++;
					hitMirrorObject (ref vertexCounter, ref lastLaserPosition, ref laserDirection, hit);
					break;

				case "score":
					gamecontroller.addScore();
                        hit.transform.gameObject.SetActive(false);
                        break;
				default:
					hitMiscObject (ref vertexCounter, hit.point, lastLaserPosition);
					phChild.addParticle (hit.point);
					loopActive = false;
					break;
				}
			} else {


				laserReflected++;
				vertexCounter++;
				mLineRenderer.SetVertexCount (vertexCounter);
				Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
				mLineRenderer.SetPosition (vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));

				loopActive = false;
			}
		}
		if (laserReflected > maxBounce)
			loopActive = false;
		if (!prismSet) {
			resetPrisms ();
           
		}
        if (m_isFinished == false)
        {
            resetEverything.Invoke();
        }
        if (LaserDown)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            phChild.DestroyParticles();
            ph.DestroyParticles();
            StartCoroutine("FireMahLazer");
        }
        else {
            yield return null;
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
        Debug.Log("isFnish = true");
        vertexCounter++;
        mLineRenderer.SetVertexCount(vertexCounter);
        mLineRenderer.SetPosition(vertexCounter - 1, location);
    }
}


