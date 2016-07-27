using UnityEngine;

public class LockLevel : MonoBehaviour
{
    public static int levels = 4; //number of levels

    private int levelIndex;

    private void Start()
    {
        PlayerPrefs.DeleteAll(); //erase data on start
        LockLevels();   //call function LockLevels
    }

    //function to lock the levels
    private void LockLevels()
    {
        //loop thorugh all the levels of all the worlds

        for (int j = 1; j < levels; j++)
        {
            levelIndex = (j + 1);
            //create a PlayerPrefs of that particular level and set it's to 0, if no key of that name exists
            if (!PlayerPrefs.HasKey("level:" + levelIndex.ToString()))
            {
                PlayerPrefs.SetInt("level:" + levelIndex.ToString(), 0);
            }
        }
    }
}