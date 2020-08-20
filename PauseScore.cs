using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScore : MonoBehaviour
{
    //private bool pause = false;
    //public GameObject pauseScore;

    void Start()
    {
        //pauseScore.SetActive(false);
    }

    public void Score()
    {
        HighscoreTable.Instance.Show();
        UI_PlayAgain.Instance.Show();
    }
}
