using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AudioManager : MonoBehaviour {
    public LightBeamController2 lbc;
    private AudioSource m_AudioSource;
    private AudioSource m_LightBeam;
    private bool isStarted = false;
    private Dictionary<string, AudioClip> m_audioClipsDictionary;
    // Use this for initialization
    public void Awake()
    {
        lbc.AnnounceHit+=PlayCollisionSound;
        lbc.resetEverythingAfter.AddListener(LightBeamStarted);
        m_audioClipsDictionary = new Dictionary<string, AudioClip>();
        
        m_LightBeam = lbc.GetComponent<AudioSource>();
        m_audioClipsDictionary.Add("score", Resources.Load("fireflyhit") as AudioClip);
        //.Add("Finish", Resources.Load("gamewinhit") as AudioClip);
        m_audioClipsDictionary.Add("Finish", Resources.Load("mirrorhit") as AudioClip);


    }
    void Start () {
        m_AudioSource = GetComponent<AudioSource>();
    }
    private void LightBeamStarted()
    {
        if (!isStarted)
        {
            m_LightBeam.Play();
            isStarted = true;
        }
    }
	public void PlayCollisionSound(string i_AudioName)
        {
        AudioClip ac=null;
        switch (i_AudioName)
        {
            case "Finish":
            case "mirror":
            case "score":
                m_audioClipsDictionary.TryGetValue(i_AudioName, out ac);
                break;
            default:
                break;
        }
        if (ac!=null&& m_AudioSource!=null)
        {
            m_AudioSource.clip = ac;
            m_AudioSource.Play();
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
