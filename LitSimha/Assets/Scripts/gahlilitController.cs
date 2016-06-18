using UnityEngine;
using System.Collections;

public class gahlilitController : MonoBehaviour {
    private ParticleSystem ps;
    private LightBeamController2 lbc;
	// Use this for initialization
	void Start () {
       // ps = GetComponent<ParticleSystem>();
       // ps.Stop();
       // lbc = GameObject.FindGameObjectWithTag("Player").GetComponent<LightBeamController2>();
       // lbc.resetEverything.AddListener(stopParticle);
    }
    private void OnTriggerEnter(Collider other)
    {
        //ps.Play();
    }
    private void stopParticle()
    {
       // ps.Stop();
    }
    // Update is called once per frame
    void Update () {
	
	}
}
