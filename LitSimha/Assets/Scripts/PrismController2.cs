using UnityEngine;
using System.Collections;

public class PrismController2 : LightBeamController2 {
    LightBeamController2 lbc;
	// Use this for initialization
	void Start ()
    {
        lbc = GameObject.FindGameObjectWithTag("Player").GetComponent<LightBeamController2>();
        lbc.resetEverythingBefore.AddListener(this.resetRay);
        setDirection(Vector3.right);
        FireRay.AddListener(CreateRay);
        gamecontroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        isFinish = false;
        mLineRenderer = this.GetComponent<LineRenderer>();
        ph = this.GetComponent<ParticleHandler>();
        phChild = ph;

        //Add to reset
        lbc.resetEverythingAfter.AddListener(ph.DestroyParticles);
        lbc.resetEverythingAfter.AddListener(phChild.DestroyParticles);
        lbc.resetEverythingBefore.AddListener(phChild.startNewIteration);
        lbc.resetEverythingBefore.AddListener(ph.startNewIteration);

        //add to Hit
        AnnounceHit += HandleCollision;
    }
	public void InvokeWithDirection(Vector3 i_Direction)
    {
        setDirection(i_Direction);
        invokeOnce();

    }
	// Update is called once per frame
	void Update () {
	
	}
    internal override void HandleCollision(string i_collisonTag)
    {
        base.HandleCollision(i_collisonTag);
        switch (i_collisonTag)
        {
            default:
                {
                    hitMiscObject(ref m_VertexCounter, m_hit.point, m_LastPosition);
                    phChild.addParticle(m_hit.point);
                    m_loopActive = false;
                    break;
                }
        }
    }
}
