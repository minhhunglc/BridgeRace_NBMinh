using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Panels")]

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject restartPanel;

    [Header("Texts")]
    [SerializeField] private Text levelText;

    public void ActivateGameOverPanel(bool activate) => gameOverPanel.SetActive(activate);
    public void ActivateWinPanel(bool activate) => winPanel.SetActive(activate);
    public void ActivateRestartPanel(bool activate) => restartPanel.SetActive(activate);
    public void UpdateLevelText(int level)
    {
        if (level > 3)
        {
            level = 1;
        }
        levelText.text = Constant.STR_LEVEL + level.ToString();
    }

}