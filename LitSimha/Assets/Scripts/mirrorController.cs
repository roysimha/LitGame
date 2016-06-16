using UnityEngine;
using System.Collections;

public class mirrorController : MonoBehaviour
{
	private float rotation;
	public float maxangle;
	private float minangle;
	private float mousey;
	private bool isPressed;
	private GameObject arrows;
	public float StepFactor = 0.1f;
	private ArrowScript arrow;
	private LightBeamController2 lbc;
	// Use this for initialization
	void Start ()
	{
		arrows = null;
		minangle = transform.eulerAngles.z - maxangle;
		maxangle = maxangle + transform.eulerAngles.z;
		rotation = transform.eulerAngles.z;
		lbc = GameObject.FindGameObjectWithTag ("Player").GetComponent<LightBeamController2> ();
        //turnOffHit ();
    }

	// Update is called once per frame
	void Update ()
	{
		if (isPressed) {
			if (Input.GetMouseButtonDown (0) && !ArrowScript.pressOnRing) {
				StartCoroutine (removeArrorws ());
			}
			arrow.transform.position = transform.position;
			transform.rotation = arrow.transform.rotation;
		}
		if (arrows != null) {
           arrow.setLimits(minangle, maxangle);
			if (arrow.rotation) {
				lbc.LaserDown = true;
				arrow.setStepFactor (StepFactor);
			}
		}
	}

	private IEnumerator removeArrorws ()
	{
		yield return new WaitForSeconds (0.1f);
		isPressed = false;
		Destroy (arrows);
	}

	void OnMouseDown ()
	{
		StartCoroutine (clickOnMirror ());


		//mousey = Input.mousePosition.y;
	}

	private IEnumerator clickOnMirror ()
	{
		yield return new WaitForSeconds (0.1f);
		isPressed = true;
		Destroy (arrows);
		arrows = Instantiate (Resources.Load ("Arrows"), new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
		arrows.transform.rotation = transform.rotation;
		arrow = arrows.GetComponent<ArrowScript> ();
		lbc.LaserDown = true;

	}

	public void turnOnHit ()
	{
		transform.GetChild (1).gameObject.SetActive (true);
	}

	public void turnOffHit ()
	{
		
		transform.GetChild (1).gameObject.SetActive (false);
	}

}
