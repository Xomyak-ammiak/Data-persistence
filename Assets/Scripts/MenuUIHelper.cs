using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHelper : MonoBehaviour
{
    public static MenuUIHelper Instance;

    public string Name;
    public string RecordName;
    public int Score;

    public Text Text;
    public Text MaxScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();

        if(Score > 0)
            MaxScore.text = RecordName + ": " + Score;
        
    }

    public void StartNew()
    {
        Name = Text.text;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public string RecordName;
        public int Score;
    }

    public void Save()
    {
        SaveData data = new SaveData();
        data.RecordName = Name;
        data.Score = Score;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            RecordName = data.RecordName;
            Score = data.Score;
        }
    }
}
