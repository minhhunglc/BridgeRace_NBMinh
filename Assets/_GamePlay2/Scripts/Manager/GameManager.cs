using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStatus { MainMenu, Playing, GameOver, Win }

public class GameManager : Singleton<GameManager>
{
    public GameStatus GameStat;
    public Player Player;
    private void Start()
    {
        Play();
    }
    public void Play()
    {
        GameStat = GameStatus.Playing;
        Player.transform.position = LevelManager.Ins.currentLevelSettings.SpawnPoint.position;
        Player.ResetPlayer();
    }
    public bool IsPlaying() { return GameStat == GameStatus.Playing; }
    public void GameOver()
    {
        if (GameStat == GameStatus.GameOver)
            return;

        GameStat = GameStatus.GameOver;
        UIManager.Ins.ActivateGameOverPanel(true);
    }
    public void Win()
    {
        if (GameStat == GameStatus.Win)
            return;

        GameStat = GameStatus.Win;
        UIManager.Ins.ActivateWinPanel(true);
    }
    public void NextLevel()
    {
        UIManager.Ins.ActivateWinPanel(false);
        LevelManager.Ins.NextLevel();
        Play();
    }
    public void FirstLevel()
    {
        UIManager.Ins.ActivateWinPanel(false);
        LevelManager.Ins.FirstLevel();
        Play();
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        if (GameStat == GameStatus.MainMenu)
            return;
        GameStat = GameStatus.MainMenu;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        Play();
    }
    public void Exit()
    {
        Application.Quit();
    }
}
