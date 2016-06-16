using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class FinishScript : MonoBehaviour {

    private GameObject FinishPanel;

    private GameObject mirror;
    private mirrorController mc;
    private LightBeamController2 lbc;
	private int prismHit=0;
	private GameController gc;
    // Use this for initialization
    void Start () {
        Debug.Log("start");
		gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();   

        FinishPanel = GameObject.FindGameObjectWithTag("FinishPanel");
        lbc = GameObject.FindGameObjectWithTag("Player").GetComponent<LightBeamController2>();
        mc = GameObject.FindGameObjectWithTag("mirror").GetComponent<mirrorController>();
        FinishPanel.SetActive(false);
    }
	public void addHit(){
		prismHit++;

	}
	// Update is called once per frame
	void Update () {
		if (lbc.isFinish||prismHit==2)
        {
            StartCoroutine(FinishLevel());
			
        }
	}

    private IEnumerator FinishLevel()
    {
        yield return new WaitForSeconds(5);
        int curFireFly = gc.getScore();
        //int curScore = gc.getGlobalScore();
        for (int i = 3; i < curFireFly + 3; i++)
        {
            // disable black firefly
            GameObject fireflyblack = FinishPanel.transform.GetChild(i + 3).transform.gameObject;
            fireflyblack.SetActive(false);

            // enable firefly
            GameObject firefly1 = FinishPanel.transform.GetChild(i).transform.gameObject;
            firefly1.SetActive(true);

        }
        Debug.Log("finished");
        FinishPanel.SetActive(true);
        lbc.enabled = false;
        mc.enabled = false;
        Time.timeScale = 0;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void NextLevel()
    {
        int gameLevel = SceneManager.GetActiveScene().buildIndex;
		gameLevel++;
        SceneManager.LoadScene(gameLevel);

    }
}
