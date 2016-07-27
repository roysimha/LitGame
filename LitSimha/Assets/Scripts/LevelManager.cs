using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public string LevelText;
        public int UnLocked;
        public bool isInteractable;

        public Button.ButtonClickedEvent OnClickEvent;
    }

    public GameObject levelButton;
    public Transform spacer;
    public List<Level> LevelList;

    // Use this for initialization
    private void Start()
    {
        fillList();
    }

    private void fillList()
    {
        foreach (var level in LevelList)
        {
            GameObject newButton = Instantiate(levelButton) as GameObject;
            //LevelButton button = newButton.GetComponent<LevelButton>();

            //button.LevelText.text = level.LevelText;

            newButton.transform.SetParent(spacer, false);
        }
    }
}