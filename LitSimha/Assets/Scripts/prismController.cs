
using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(LineRenderer))]

public class prismController : MonoBehaviour
{
    public bool isFinish;
    public int laserDistance;
    private LineRenderer mLineRenderer;
    public Vector3 laserDirection;
    public string bounceTag;
    public int maxBounce;
    private float timer = 0;
    private ParticleHandler ph;
    private int FinishHit = 0;
    public bool LaserDown = false;
    bool prismHit = true;
	private FinishScript fs;
    private ParticleHandler currentHitParticleHandler;
    // Use this for initialization
    void Start()
    {
        isFinish = false;
        mLineRenderer = this.GetComponent<LineRenderer>();
        ph = this.GetComponent<ParticleHandler>();
		fs = GameObject.FindGameObjectWithTag ("PrismFinish").GetComponent<FinishScript> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (LaserDown)
        {
            timer = 0;
            StartCoroutine("FireMahLazer");
            LaserDown = false;
        }
    }
    public void clearPrism()
    {
        ph.startNewIteration();
        ph.DestroyParticles();
            mLineRenderer.SetVertexCount(1);
            mLineRenderer.SetPosition(0, transform.position);
        
    }
    IEnumerator FireMahLazer()
    {
        //Debug.Log("Running");
        mLineRenderer.enabled = true;
        int laserReflected = 1; //How many times it got reflected
        int vertexCounter = 1; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?
        Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser
        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;
        ph.startNewIteration();
        while (loopActive)
        {
            Ray ray = new Ray(lastLaserPosition, laserDirection);
            if ((Physics.Raycast(ray, out hit, laserDistance)))
            {
                switch (hit.transform.gameObject.tag)
                {
                    case "PrismFinish":
                        currentHitParticleHandler = hit.transform.gameObject.GetComponent<ParticleHandler>();
                        currentHitParticleHandler.setActivetoFalse();
                        currentHitParticleHandler.startNewIteration();
                        currentHitParticleHandler.addParticle(hit.point);
                        hitFinishObject(ref vertexCounter, hit.point);
                        loopActive = false;
                        break;

                    case "portal":
                        Vector3 secondLocationOfPortal = hit.transform.GetChild(0).transform.position;
                        laserReflected++;
                        hitPortalObject(ref vertexCounter, ref lastLaserPosition, secondLocationOfPortal, hit.point);
                        break;

                    case "Prism":
                        {
                            hitMiscObject(ref vertexCounter, hit.point, lastLaserPosition);
                            loopActive = false;
                        }
                        break;

                    case "mirror":
                        laserReflected++;
                        hitMirrorObject(ref vertexCounter, ref lastLaserPosition, ref laserDirection, hit);
                        break;

                    default:
                        hitMiscObject(ref vertexCounter, hit.point, lastLaserPosition);
                        ph.addParticle(hit.point);
                        loopActive = false;
                        break;
                }
            }
            else
            {


                laserReflected++;
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
                mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));

                loopActive = false;
            }
        }
        if (laserReflected > maxBounce)
            loopActive = false;


        ph.DestroyParticles();
        
      //  if (LaserDown)
     //{
          // yield return new WaitForEndOfFrame();
           // timer += Time.deltaTime;
           //StartCoroutine("FireMahLazer");
       // }
       // else
       //{
            yield return null;
      // }
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
		if(!isFinish)
		fs.addHit ();
		isFinish = true;
        vertexCounter++;
        mLineRenderer.SetVertexCount(vertexCounter);
        mLineRenderer.SetPosition(vertexCounter - 1, location);
    }
}


