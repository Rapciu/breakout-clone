using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndStatsDisplayer : MonoBehaviour
{
    GameManager gameManagerComp;
    TextMeshProUGUI statsTextComp;

    void Start()
    {
        gameManagerComp = FindObjectOfType<GameManager>();
        statsTextComp = GetComponent<TextMeshProUGUI>();

        if (gameManagerComp == null) return;

        (int minutes, int seconds) = gameManagerComp.GetConvertedTime();

        statsTextComp.text = $"Score\n{gameManagerComp.currentPoints}\n\nTime\n{minutes}m {seconds}s";
    }
}
