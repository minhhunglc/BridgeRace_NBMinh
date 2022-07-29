using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{

    [Header("Levels")]
    [SerializeField] private LevelSettings[] levels;
    public int currentLevel;
    public LevelSettings currentLevelSettings;


    private void Start()
    {
        ReloadLevel();
        if (currentLevel > 2)
        {
            currentLevel = 0;
        }
    }

    public void ActiveCurrentLevel()
    {
        foreach (LevelSettings lev in levels)
            if (lev.gameObject.activeSelf)
                lev.gameObject.SetActive(false);

        int levelIndex = currentLevel;
        if (levelIndex > levels.Length - 1)
            levelIndex = currentLevel % (levels.Length);


        LevelSettings level = levels[levelIndex];
        level.gameObject.SetActive(true);

        currentLevelSettings = level;
        level.GetObjects();

        UIManager.Ins.UpdateLevelText(currentLevel + 1);
    }

    public void ReloadLevel()
    {
        currentLevel = PlayerPrefs.GetInt(Constant.PREF_LEVEL);
        ActiveCurrentLevel();
    }

    public void NextLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt(Constant.PREF_LEVEL, currentLevel);
        ActiveCurrentLevel();

    }
    public void FirstLevel()
    {
        currentLevel = 0;
        PlayerPrefs.SetInt(Constant.PREF_LEVEL, currentLevel);
        ActiveCurrentLevel();
    }

}
