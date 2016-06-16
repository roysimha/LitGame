using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour {
    public Canvas quitMenu;
    public Button startText;
    public Button exitText;
    public Button settingsText;


    // Use this for initialization
    void Start() {
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        settingsText = settingsText.GetComponent<Button>();
        quitMenu.enabled = false;
    }

    // Update is called once per frame
    void Update() {

    }

    public void exitPressed()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
        settingsText.enabled = false;
        
        

    }

    public void noPressed()
    {
        quitMenu.enabled = false;
        startText.enabled = true;
        settingsText.enabled = true;
        exitText.enabled = true;
    }
        
    public void startPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
