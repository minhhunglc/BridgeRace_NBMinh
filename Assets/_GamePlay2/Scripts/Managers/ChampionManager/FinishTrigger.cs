using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _success_Text, _failed_Text, _main_menu_button;

    void Start()
    {
        _success_Text.SetActive(false);
        _failed_Text.SetActive(false);
        _main_menu_button.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.TAG_PLAYER))
        {
            _success_Text.SetActive(true);
            DOTween.KillAll();
            _main_menu_button.SetActive(true);
        }

        else
        {
            DOTween.KillAll();
            _main_menu_button.SetActive(true);
            _failed_Text.SetActive(true);
        }
    }
}
