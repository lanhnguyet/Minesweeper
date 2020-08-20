using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;
using UnityEngine.SceneManagement;

public class MinesweeperGameHandler : MonoBehaviour
{
    [SerializeField] private GridPrefabVisual gridPrefabVisual;
    [SerializeField] private TMPro.TextMeshPro timerText;

    private Map map;
    private float timer;
    private bool isGameActive;
    private bool pause = false;
    private int song = 0;

    private void Start()
    {
        map = new Map();
        gridPrefabVisual.Setup(map.GetGrid());
        isGameActive = true;

        map.OnEnTireMapRevealed += Map_OnEnTireMapRevealed;
    }

    private void Map_OnEnTireMapRevealed(object sender, System.EventArgs e)
    {
        isGameActive = false;
        int timeScore = Mathf.FloorToInt(timer);
        song = 2;

        UI_Blocker.Show_Static();
        
        UI_InputWindow.Show_Static("Player Name", " ", 5, () => {
            // Cancel
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }, (string playerName) => {
            // Ok
            HighscoreTable.Instance.Show();
            HighscoreTable.Instance.AddHighscoreEntry1(timeScore, playerName);
            UI_PlayAgain.Instance.Show();
        });
    }

    private void music()
    {       
        if (timer == 0)
        {
            SoundManager.Instance.Playbase();
        }
        if (song == 2)
        {
            song = 3;
            SoundManager.Instance.Stop();
            SoundManager.Instance.Playsound("Swordland");
        }
        if (song == 1)
        {
            song = 4;
            SoundManager.Instance.Stop();
            SoundManager.Instance.Playsound("lose");
        }
    }

    private void Update()
    {
        Vector3 worldPosition = UtilsClass.GetMouseWorldPosition();
      
        music();

        if (Input.GetMouseButtonDown(0))
        {
            if (isGameActive)
            {
                MapGridObject.Type gridType = map.RevealGridPosition(worldPosition);
                if (gridType == MapGridObject.Type.Mine)
                {
                    // Revealed a Mine, Game Over!                    
                    isGameActive = false;
                    song = 1;
                    map.RevealEntireMap();
                    UI_Blocker.Show_Static();
                    UI_GameOver.Instance.Show();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (isGameActive)
            {
                map.FlagGridPosition(worldPosition);
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            gridPrefabVisual.SetRevealEntireMap(true);
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            gridPrefabVisual.SetRevealEntireMap(false);
        }
        HandleTimer();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
        }

        if (pause)
        {
            SoundManager.Instance.Pause();
            isGameActive = false;
            UI_Blocker.Show_Static();
        }
    }

    private void HandleTimer()
    {
        if (isGameActive)
        {
            timer += Time.deltaTime;
            timerText.text = Mathf.FloorToInt(timer).ToString();
        }
    }

    public void resume()
    {
        pause = false;
        isGameActive = true;
        SoundManager.Instance.UnPause();
    }
}
