using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController controller;

    public int m_LevelsCompleted { get; set; }
    public int m_NumberOfFireFlies { get; set; }
    public int m_CurLvllFireFlies { get; set; }
    public int m_GlobalScore { get; set; }
    public bool m_IsFirstTime { get; set; }

    void Awake()
    {
        if (controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;
            m_GlobalScore = 0;
            m_LevelsCompleted = 0;
            m_NumberOfFireFlies = 0;
            m_CurLvllFireFlies = 0;
            m_IsFirstTime = true;
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
    }

    public void addScore(int i_ScoreToAdd)
    {
        m_CurLvllFireFlies++; ;
        m_GlobalScore += i_ScoreToAdd;
        Debug.Log(string.Format("Added {0} points, total score is {1}", i_ScoreToAdd, m_GlobalScore));
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.GlobalScore = m_GlobalScore;
        data.LevelsCompleted = m_LevelsCompleted;
        data.NumberOfFireFlies = m_NumberOfFireFlies;
        data.IsFirstTime = m_IsFirstTime;


        bf.Serialize(file, data);

        file.Close();
        Debug.Log("Player Data Created");
    }

    public void UpdatePlayerData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

        PlayerData data = new PlayerData();
        data.GlobalScore = m_GlobalScore;
        data.LevelsCompleted = m_LevelsCompleted;
        data.NumberOfFireFlies += m_CurLvllFireFlies;
        data.IsFirstTime = m_IsFirstTime;


        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Player Data updated");
    }

    public void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            File.Delete(Application.persistentDataPath + "/playerInfo.dat");
            Debug.Log("Save deleted");
        }
        else
        {
            Debug.Log("File does not exist");
        }
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);

            file.Close();

            m_GlobalScore = data.GlobalScore;
            m_NumberOfFireFlies = data.NumberOfFireFlies;
            m_LevelsCompleted = data.LevelsCompleted;
            m_IsFirstTime = data.IsFirstTime;

        }
        else
        {
            Save();
        }
    }

    [Serializable]
    class PlayerData
    {
        public int LevelsCompleted;
        public int NumberOfFireFlies;
        public int GlobalScore;
        public bool IsFirstTime;
    }
}