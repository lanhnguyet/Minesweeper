using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    private bool pause = false;
    public GameObject pauseUI;

    void Start()
    {
        pauseUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            pause = true;
        if (pause)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }

        //if (pause == false)
        //{
        //    pauseUI.SetActive(false);
        //    Time.timeScale = 1;
        //}
    }
    //public static UI_PlayAgain Instance { get; private set; }

    //private void Awake()
    //{
    //    Instance = this;
    //}
    //public void Scores()
    //{

    //}

    public void resume()
    {
        pause = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1;
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        pause = false;
    }
    public void exit()
    {
        Application.LoadLevel("MainMenu");
    }
}
