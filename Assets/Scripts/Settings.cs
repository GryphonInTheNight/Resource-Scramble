using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    private AudioSource backgroundMusic;

    public int Difficulty = 2;
    public bool playMusic = true;
    public bool playSound = true;


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
