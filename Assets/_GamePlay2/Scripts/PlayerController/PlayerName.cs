using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI player_name;

    void Start()
    {
        player_name.text = PlayerPrefs.GetString(Constant.PREF_NAME);
    }
}
