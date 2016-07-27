using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenuScript : MonoBehaviour
{
    private bool dead;
    public GameObject deadMenu;

    // Use this for initialization
    private void Start()
    {
        deadMenu.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (dead)
        {
            popDeadMenu();
        }

        if (Input.GetKeyDown("space"))
        {
            dead = true;
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        deadMenu.SetActive(false);
    }

    private void popDeadMenu()
    {
        deadMenu.SetActive(true);
        Time.timeScale = 0;
    }
}