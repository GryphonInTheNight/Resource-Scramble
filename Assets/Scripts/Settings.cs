using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    private AudioSource backgroundMusic;

    public int Difficulty = 2;
    private const int numOfRecords = 5;
    private const int emptyScore = 1000;
    public bool playMusic = true;
    public bool playSound = true;
    public int[] easyScores;
    public int[] mediumScores;
    public int[] hardScores;

    [System.Serializable]
    class SaveData
    {
        public bool playMusic = true;
        public bool playSound = true;
        public int[] easyScores;
        public int[] mediumScores;
        public int[] hardScores;
    }
    public void SaveMyData()
    {
        SaveData data = new SaveData();
        data.playMusic = playMusic;
        data.playSound = playSound;
        data.easyScores = easyScores;
        data.mediumScores = mediumScores;
        data.hardScores = hardScores;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public bool LoadMyData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playMusic = data.playMusic;
            playSound = data.playSound;
            easyScores = HandleRecordLengthChange(data.easyScores);
            mediumScores = HandleRecordLengthChange(data.mediumScores);
            hardScores = HandleRecordLengthChange(data.hardScores);
            return true;
        }
        else return false;
    }
    public int[] HandleRecordLengthChange(int[] savedRecord)
    {
        if (numOfRecords == savedRecord.Length)
            return savedRecord;
        int[] temp = new int[numOfRecords];
        for (int i = 0; i < numOfRecords; i++)
        {
            if (i < savedRecord.Length)
                temp[i] = savedRecord[i];
            else
                temp[i] = emptyScore;
        }
        return temp;
    }

    public void CheckAndAddScore(int score)
    {
        if (Difficulty == 2)
            easyScores = AddScoreHelper(score, easyScores);
        else if (Difficulty == 3)
            mediumScores = AddScoreHelper(score, mediumScores);
        else
            hardScores = AddScoreHelper(score, hardScores);
    }
    private int[] AddScoreHelper(int score, int[] target)
    {
        for (int i = target.Length - 1; i >= 0; i -= 1)
        {
            if (target[i] <= score)
                return target;
            if (i < target.Length - 1)
                target[i + 1] = target[i];
            target[i] = score;
        }
        return target;
    }
    public string GetScoreboard(int targetDifficulty)
    {
        string result = "Best Scores:";
        if (targetDifficulty == 2)
        {
            for (int i = 0; i < easyScores.Length; i++)
                if (easyScores[i] != emptyScore)
                    result = result + "\n" + (i+1) + ". " + easyScores[i];
        }
        else if (targetDifficulty == 3)
        {
            for (int i = 0; i < mediumScores.Length; i++)
                if (mediumScores[i] != emptyScore)
                    result = result + "\n" + (i+1) + ". " + mediumScores[i];
        }
        else
        {
            for (int i = 0; i < hardScores.Length; i++)
                if (hardScores[i] != emptyScore)
                    result = result + "\n" + (i+1) + ". " + hardScores[i];
        }
        return result;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            backgroundMusic = gameObject.GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (!LoadMyData())
        {
            easyScores = HandleRecordLengthChange(new int[0]); ;
            mediumScores = HandleRecordLengthChange(new int[0]);
            hardScores = HandleRecordLengthChange(new int[0]);
        }
    }

    private void Start()
    {

        if (!playMusic)
            backgroundMusic.Stop();
        if (!playSound)
            backgroundMusic.mute = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        SaveMyData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public bool ToggleMusic()
    {
        playMusic = !playMusic;
        if (playMusic)
            backgroundMusic.Play();
        else
            backgroundMusic.Stop();
        return playMusic;
    }

    public bool ToggleMute()
    {
        playSound = !playSound;
        backgroundMusic.mute = !playSound;
        return playSound;
    }
}
