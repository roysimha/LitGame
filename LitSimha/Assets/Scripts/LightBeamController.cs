using UnityEngine;
using System.Collections;

public class LightBeamController : MonoBehaviour {
	LineRenderer lightBeam;
	string reftag;
	private int limit=100;
	private int verti=1;
	public int rotation=30;
	private bool iactive;
	private Vector3 curpos;
	private Vector3 currout;
	private float offsetx=0;
	private ArrayList hitlist;
    private static bool isDrawlightWorking = false;
    public float debugNum;
    bool fixLine;
	// Use this for initialization
	void Start () {
		lightBeam = this.GetComponent<LineRenderer> ();
        fixLine = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space)||Input.GetKeyUp(KeyCode.Space)){
            DrawLight();
		}
	}


	private void createHitParticle(Transform hit)
	{
		if (!hitlist.Contains (hit.gameObject)) {
			mirrorController mc = hit.GetComponent<mirrorController> ();
			mc.turnOnHit ();
		}
	}
	private void clearparticles()
	{
		foreach (GameObject hit in GameObject.FindGameObjectsWithTag("lighteffect")) {
			if (!hitlist.Contains(hit)) {
				mirrorController mc = hit.GetComponent<mirrorController> ();
				mc.turnOffHit ();
			}

		}
	}
    public void DrawLight() {
        if(!isDrawlightWorking)
        {
            isDrawlightWorking = true;
            initiateLight();
            isDrawlightWorking = false;
        }
    }
    private void initiateLight() { 
		int maxDistance = 20;
		int bounceLimit = 10;

		int bounceCount = 1;
		int lineSegments = 1;

		bool loopActive = true;
        RaycastHit2D lasthit=new RaycastHit2D();
		Vector2 lightDirection = transform.right;
		Vector2 lastPosition = transform.position;
		lightBeam.SetVertexCount (1);
		lightBeam.SetPosition (0, transform.position);
		while (loopActive) {
			RaycastHit2D hit = Physics2D.Raycast (lastPosition, lightDirection, maxDistance);
				if (hit) {
                if (hit.point.Equals(lasthit.point))
                {
                    Debug.Log("shit son");
                    fixLine = true;
                }


                        bounceCount++;
                        lineSegments += 2;
                        lightBeam.SetVertexCount(lineSegments);
                        lightBeam.SetPosition(lineSegments - 2, Vector2.MoveTowards(hit.point, lastPosition, 0.01f));
                        //lightBeam.SetPosition (lineSegments - 2, hit.point);
                        lightBeam.SetPosition(lineSegments - 1, hit.point);
                        lastPosition = hit.point;
                if(fixLine)
                {
                    fixLine = false;
                }
                        lightDirection = handleDirection(lightDirection, hit.normal);
                        lasthit = hit;
                        //particle effects
                        //createHitParticle (hit.transform);
                    }
				
				
            else
            {
					bounceCount++;
					lineSegments++;
					lightBeam.SetVertexCount (lineSegments);
					lightBeam.SetPosition (lineSegments - 1, lastPosition + (lightDirection.normalized * maxDistance));
					loopActive = false;
				}
				if (lineSegments > bounceLimit) {
					loopActive = false;
				}

		}

		//clearparticles ();
	}

    private Vector2 handleDirection(Vector2 lightDirection, Vector2 normal)
    {
        Vector2 result = Vector2.Reflect(lightDirection,normal);
        return result;
    }
}

