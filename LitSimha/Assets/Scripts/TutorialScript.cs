using System.Collections;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    private GameObject panel1;
    private GameObject panel2;

    private void Start()
    {
        panel1 = GameObject.FindGameObjectWithTag("Panel1");
        panel2 = GameObject.FindGameObjectWithTag("Panel2");
        panel1.SetActive(false);
        panel2.SetActive(false);

        if (GameController.controller.m_IsFirstTime)
        {
            StartCoroutine(waitABit());
            GameController.controller.m_IsFirstTime = false;
            GameController.controller.UpdatePlayerData();
        }
    }

    private IEnumerator waitABit()
    {
        yield return new WaitForSeconds(3);
        panel1.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void next()
    {
        panel1.SetActive(false);
        panel2.SetActive(true);
    }

    public void play()
    {
        panel2.SetActive(false);
    }
}