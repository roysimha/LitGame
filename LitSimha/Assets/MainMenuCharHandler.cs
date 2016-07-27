using UnityEngine;
using System.Collections;

public class MainMenuCharHandler : MonoBehaviour {

    // Use this for initialization
    private MainCharAnimationController mcam;
	void Start () {
        mcam = GetComponent<MainCharAnimationController>();
        mcam.ChangeToAnimation(1);
	}
	public void onTouch()
    {
        mcam.ChangeToAnimation(0);
    }
    public void ReleaseTouch()
    {
        mcam.ChangeToAnimation(1);
    }
    // Update is called once per frame
    void Update () {
	
	}
}
