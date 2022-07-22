using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    public void To_Main_Menu()
    {
        StartCoroutine(get_Main_Menu(1f));
    }

    IEnumerator get_Main_Menu(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(0);
    }
}
