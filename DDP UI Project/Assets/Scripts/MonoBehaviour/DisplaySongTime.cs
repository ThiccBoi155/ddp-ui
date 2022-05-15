using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class DisplaySongTime : MonoBehaviour
{
    private const float secondsInAnHour = 60f * 60f;

    [Header("References")]
    public TextMeshPro tMPro;

    private void Awake()
    {
        Clear();
    }

    public void Display(float currentTime, float duration)
    {
        TimeSpan currentTimeSpan = TimeSpan.FromSeconds(currentTime);
        TimeSpan durationTimeSpan = TimeSpan.FromSeconds(duration);

        string formatString = secondsInAnHour < duration ? @"hh\:mm\:ss" : @"mm\:ss";

        string currentTimeString = currentTimeSpan.ToString(formatString);
        string durationString = durationTimeSpan.ToString(formatString);

        tMPro.text = $"{currentTimeString} - {durationString}";
    }

    public void Clear()
    {
        tMPro.text = "";
    }
}
