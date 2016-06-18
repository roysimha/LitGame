using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    
    private GameObject pauseMenuPanel;
    private LightBeamController2 lbc;
    private mirrorController mc;

    //variable for checking if the game is paused 
    private bool isPaused = false;
    

    void Start()
    {
        lbc = GameObject.FindGameObjectWithTag("Player").GetComponent<LightBeamController2>();
        mc = GameObject.FindGameObjectWithTag("mirror").GetComponent<mirrorController>();
        //unpause the game on start
        Time.timeScale = 1;
        pauseMenuPanel = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenuPanel.SetActive(false);

    }

    
    public void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
        
            
    }

    //function to pause the game
    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        isPaused = true;
        //freeze the timescale
        Time.timeScale = 0;
        lbc.enabled = false;
        mc.enabled = false;    
        
    }
    //function to unpause the game
    public void UnpauseGame()
    {
        pauseMenuPanel.SetActive(false);
        //set back the time scale to normal time scale
        Time.timeScale = 1;
        isPaused = false;
        lbc.enabled = true; 
        mc.enabled = true;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}