using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class FinishScript : MonoBehaviour
{

    private GameObject FinishPanel;
    private GameObject mirror;
    private mirrorController mc;
    private LightBeamController2 lbc;
    private int prismHit = 0;
    private ParticleHandler ph;
    public Transform ClickUI;
    // Use this for initialization
    void Start()
    {
        FinishPanel = GameObject.FindGameObjectWithTag("FinishPanel");
        lbc = GameObject.FindGameObjectWithTag("Player").GetComponent<LightBeamController2>();
        lbc.OnFinished.AddListener(OpenFinishUI);
        lbc.resetEverythingBefore.AddListener(setFinishToFalse);
        ClickUI.gameObject.SetActive(false);
        ClickUI.GetComponent<ButtonSprite>().onClick.AddListener(finishTheLevel);
        ph = GetComponent<ParticleHandler>();
        ph.setActivetoFalse();
        mc = GameObject.FindGameObjectWithTag("mirror").GetComponent<mirrorController>();
        FinishPanel.SetActive(false);
    }
    private void OpenFinishUI()
    {
        if (ClickUI != null)
            ClickUI.gameObject.SetActive(true);
    }
    private void setFinishToFalse()
    {
        ph.DestroyParticles();
        ClickUI.gameObject.SetActive(false);
    }
    public void addHit()
    {
        prismHit++;

    }


    // Update is called once per frame
    void Update()
    { 
        if (prismHit == 2)
        {
            OpenFinishUI();
        }
	}
    private void finishTheLevel()
    {
        StartCoroutine(i_finishLevel());
    }
    private IEnumerator i_finishLevel()
    {

        ph.startNewIteration();

        ph.addParticle(transform.position);
        yield return new WaitForSeconds(2);
        int curFireFly = GameController.controller.m_CurLvllFireFlies;
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
        Data.SaveData(SceneManager.GetActiveScene().buildIndex, true, curFireFly);
        GameController.controller.m_LevelsCompleted++;
        GameController.controller.UpdatePlayerData();
        Debug.Log(string.Format("Fireflys = {0}, LevelCompleted = {1}, Score = {2}", GameController.controller.m_CurLvllFireFlies, GameController.controller.m_LevelsCompleted, GameController.controller.m_GlobalScore));
        GameController.controller.m_CurLvllFireFlies = 0;

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
        SceneManager.LoadScene(1);
    }

    public void NextLevel()
    {
        int gameLevel = SceneManager.GetActiveScene().buildIndex;
        gameLevel++;
        SceneManager.LoadScene(gameLevel);

    }
}
