using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField name_input;


    public void Two_Player_Game()
    {
        PlayerPrefs.SetString(Constant.PREF_NAME, name_input.text);
        SceneManager.LoadScene(1);
    }

    public void Three_Player_Game()
    {
        PlayerPrefs.SetString(Constant.PREF_NAME, name_input.text);
        SceneManager.LoadScene(2);

    }

}
