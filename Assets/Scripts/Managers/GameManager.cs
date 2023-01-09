﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    public GameObject defeatUI;
    public GameObject victoryUI;

    public GameObject tutorialOneScreenSelectSeed;
    public GameObject tutorialTwoScreenPlantSeed;
    public GameObject tutorialThreeScreenSelectScythe;
    public GameObject tutorialFourScreenHarvestSquirrel;
    public GameObject tutorialFiveScreenGoToShop;

    private bool paused = false;
    private bool gameOver = false;
    private bool winGame = false;
    private bool tutorialScreenOpen = false;

    private int tutorialState = 1;

    void Start() {
        LeanTween.init(10000);

        // defeatUI.SetActive(false);
        // victoryUI.SetActive(false);

        ShowTutorialOneSelectSeed();

    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        #endif

        if (GameOverCondition()) {
            GameOver();
        }

        if (InventoryManager.Instance.CanSpendSoul(200)) {
            WinGame();
        }
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
        foreach (GameObject crop in GameObject.FindGameObjectsWithTag("Plantable")) {
            Destroy(crop);
        }
        victoryUI.SetActive(true);
    }

    private bool GameOverCondition() {
        if (!InventoryManager.Instance.OutOfSouls()) {
            return false;
        }
        if (GameObject.FindGameObjectsWithTag("Plantable").Length != 0) {
            return false;
        }
        if (GameObject.FindGameObjectsWithTag("Collectable").Length == 0) {
            return false;
        }
        return true;
    }

    public void GameOver() {
        gameOver = true;
        Pause();
        foreach (GameObject crop in GameObject.FindGameObjectsWithTag("Plantable")) {
            Destroy(crop);
        }

        defeatUI.SetActive(true);
    }

    public void RestartLevel() {
        Play();
        gameOver = false;
        tutorialState = 6;
        defeatUI.SetActive(false);
        victoryUI.SetActive(false);
        PlayerController.Instance.transform.position = Vector3.zero;
        InventoryManager.Instance.Reset();
    }

    public bool InTutorial() {
        return tutorialState <= 5;
    }

    public bool TutorialMovementFrozen() {
        return tutorialState < 5;
    }

    public bool TutorialScreenUp() {
        return tutorialScreenOpen;
    }

    public void DelayedHideTutorialFive() {
        Invoke("TutoralFiveGoToShopClick", 0.6f);
    }

    public void HideTutorialScreens() {
        tutorialOneScreenSelectSeed.SetActive(false);
        tutorialTwoScreenPlantSeed.SetActive(false);
        tutorialThreeScreenSelectScythe.SetActive(false);
        tutorialFourScreenHarvestSquirrel.SetActive(false);
        tutorialFiveScreenGoToShop.SetActive(false);
    }

    public void ShowTutorialOneSelectSeed() {
        tutorialOneScreenSelectSeed.SetActive(true);
        tutorialState = 1;
        tutorialScreenOpen = true;
    }

    public void TutoralOneSelectSeedClick() {
        InventoryManager.Instance.SetCurrentItemIndex(1);
        HideTutorialScreens();
        tutorialScreenOpen = false;
        ShowTutorialTwoPlantSeed();
    }

    public void ShowTutorialTwoPlantSeed() {
        tutorialTwoScreenPlantSeed.SetActive(true);
        tutorialState = 2;
        tutorialScreenOpen = true;
    }

    public void TutoralTwoPlantSeedClick() {
        InventoryManager.Instance.TryUseCurrentItem();
        HideTutorialScreens();
        tutorialScreenOpen = false;
        Invoke("ShowTutorialThreeSelectScythe", 3.2f);
    }

    public void ShowTutorialThreeSelectScythe() {
        tutorialThreeScreenSelectScythe.SetActive(true);
        tutorialState = 3;
        tutorialScreenOpen = true;
    }

    public void TutoralThreeSelectScytheClick() {
        InventoryManager.Instance.SetCurrentItemIndex(0);
        HideTutorialScreens();
        tutorialScreenOpen = false;
        ShowTutorialFourHarvestSquirrel();
    }

    public void ShowTutorialFourHarvestSquirrel() {
        tutorialFourScreenHarvestSquirrel.SetActive(true);
        tutorialState = 4;
        tutorialScreenOpen = true;
    }

    public void TutoralFourHarvestSquirrelClick() {
        InventoryManager.Instance.TryUseCurrentItem();
        HideTutorialScreens();
        tutorialScreenOpen = false;
        Invoke("ShowTutorialFiveGoToShop", 1f);
    }

    public void ShowTutorialFiveGoToShop() {
        tutorialFiveScreenGoToShop.SetActive(true);
        tutorialState = 5;
        tutorialScreenOpen = true;
    }

    public void TutoralFiveGoToShopClick() {
        HideTutorialScreens();
        tutorialScreenOpen = false;
        tutorialState = 6;
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
