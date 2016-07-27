using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour
{
    private int levelIndex;

    private void Start()
    {
        CheckLockedLevels();
    }

    //Level to load on button click. Will be used for Level button click event
    public void Selectlevel(string index)
    {
        SceneManager.LoadScene("Level" + index); //load the level
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    // update the level that are locked
    private void CheckLockedLevels()
    {
        // remove the lock of the unlocked level
        for (int j = 1; j < LockLevel.levels; j++)
        {
            levelIndex = (j + 1);
            if ((PlayerPrefs.GetInt("level" + levelIndex.ToString())) == 1)
            {
                GameObject.Find("LockedLevel" + (j + 1)).SetActive(false);
                Debug.Log("Unlocked");
            }
        }
    }
}