using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameController : MonoBehaviour
{
    public GameObject pauseGame;
    public bool pausedGame = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausedGame) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseGame.SetActive(false);
        Time.timeScale = 1;
        pausedGame = false;
    }

    public void Pause()
    {
        pauseGame.SetActive(true);
        Time.timeScale = 0;
        pausedGame = true;
    }
}
