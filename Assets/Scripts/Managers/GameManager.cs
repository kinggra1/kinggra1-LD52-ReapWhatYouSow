using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    public GameObject defeatUI;
    public GameObject victoryUI;

    private bool paused = false;
    private bool gameOver = false;
    private bool winGame = false;

    void Start() {
        defeatUI.SetActive(false);
        victoryUI.SetActive(false);

        LeanTween.init(10000);
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        #endif
    }

    public void Pause() {
        paused = true;
    }

    public void Play() {
        paused = false;
    }
     
    public void RestartGame() {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public void WinGame() {
        winGame = true;
        Pause();
        foreach (GameObject crop in GameObject.FindGameObjectsWithTag("Crop")) {
            Destroy(crop);
        }
        victoryUI.SetActive(true);
    }

    public void GameOver() {
        gameOver = true;
        Pause();
        foreach (GameObject crop in GameObject.FindGameObjectsWithTag("Crop")) {
            Destroy(crop);
        }

        defeatUI.SetActive(true);
    }

    public void RestartLevel() {
        gameOver = false;
        defeatUI.SetActive(false);
        PlayerController.Instance.transform.position = Vector3.zero;
    }

    public bool IsPaused() {
        return paused;
    }

    public bool IsGameOver() {
        return gameOver;
    }

    public bool IsWinGame() {
        return winGame;
    }
}
