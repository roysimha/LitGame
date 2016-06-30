using UnityEngine;
using System.Collections;
public class ParticleHandler : MonoBehaviour {
    public Transform hitParticles;
    public int maxParticlesInLevel=20;
    private int currentIndex;
    public Transform startParticle;
    private GameObject startParticleObject;
    private static int particlecount = 0;
    private GameObject[] particleObjects;
    private Vector3 hiddenLocation;
    private ArrayList currentIterationIndices;
    private ArrayList overallIndices;
    private bool isactive;
    struct particleData
    {
        string Name;
        Vector3 Location;
    }
	// Use this for initialization
    public void startParticles()
    {
        startParticleObject.transform.position =this.transform.position;
        startParticleObject.transform.position += new Vector3(0.5f, 0.3f,-4);
        startParticleObject.SetActive(true);
      
    }
    public void initializeStartParticles()
    {
        if (startParticle != null)
        {
            this.Start();
            startParticleObject = (Instantiate(startParticle) as Transform).gameObject;
           
                startParticleObject.SetActive(false);
            
        }
    }
    public void setActivetoFalse()
    {
        isactive = true;
    }
	void Start ()
    {
        particleObjects = new GameObject[maxParticlesInLevel];
           currentIterationIndices= new ArrayList();
      overallIndices=new ArrayList();
       
    // initialize array of particles
    hiddenLocation = new Vector3(200,200,80);
        currentIndex = 0;
        for (int i = 0; i < particleObjects.Length; i++)
        {
            
            Transform spawn =(Transform) Instantiate(hitParticles, hiddenLocation, Quaternion.identity);
            particleObjects[i] = spawn.gameObject;

             }
    }

    public  void startNewIteration()
    {
        currentIterationIndices.Clear();
    }
    private bool isLocationInArray(Vector3 location,double offset)
    {
        for (int i = 0; i < particleObjects.Length; i++)
        {
            Vector3 position = particleObjects[i].transform.position;
            Vector3 diff = position - location;
            double diffSize = diff.x + diff.y;
            if (diffSize < offset)
            {
                return true;
            }
        }
        return false;
    }
	public void addParticle(Vector3 Location)
    {
        if (currentIndex < maxParticlesInLevel)
        {
            if (!isLocationInArray(Location, 0.4))
            {
                Location += new Vector3(0, 0, -1);
                particleObjects[currentIndex].transform.position = Location;
                if (isactive)
                {
                    particleObjects[currentIndex].SetActive(false);
                }
                particleObjects[currentIndex].SetActive(true);
                overallIndices.Add(currentIndex);
                currentIterationIndices.Add(currentIndex);
                currentIndex++;
            }
        }
        else
        {
            int loc = fetchIndex();
            if (loc != -1)
            {
                Location += new Vector3(0, 0, -1);
                particleObjects[loc].transform.position = Location;
                particleObjects[loc].SetActive(true);
                overallIndices.Add(loc);
                currentIterationIndices.Add(loc);
            }
        }
    }

    private int fetchIndex()
    {
        for (int i = 0; i < maxParticlesInLevel; i++)
        {
            if(particleObjects[i].transform.position==hiddenLocation)
            {
                return i;
            }
        }
        return -1;
    }
    public void DestroyParticles()
    {
        for(int i=overallIndices.Count-1;i>-1;i--)
        {
            int index = (int) overallIndices[i];
            if (!isIncurrentIteration(index))
            {
               particleObjects[index].transform.position = hiddenLocation;
                overallIndices.RemoveAt(i);
            }
        }
    }

    private bool isIncurrentIteration(int val)
    {
        for (int i = 0; i < currentIterationIndices.Count; i++)
        {
            int x = (int)currentIterationIndices[i];
            if (val == x)
            {
                return true;
            }
        }
        
        return false;
    }
	// Update is called once per frame
	void Update () {
	
	}
}

