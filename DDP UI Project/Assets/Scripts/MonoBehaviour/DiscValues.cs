using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create new disc values")]
public class DiscValues : ScriptableObject
{
    [Header("The impact is calculated between theese values")]
    public float minSpeedRange = .5f;
    public float maxSpeedRange = 10f;

    float previousMinSpeedRange;

    [Header("Volume")]
    [Range(0f, 1f)]
    public float minVolume = .6f;
    [Range(0f, 1f)]
    public float maxVolume = .6f;

    float previousMinVolume;

    [Header("Pitch")]
    [Range(0f, 3f)]
    public float minPitch = 1f;
    [Range(0f, 3f)]
    public float maxPitch = 1f;
    public bool invertPitchRange = false;

    float previousMinPitch;

    private DiscValues previousValues;

    void OnValidate()
    {
        Funcs.minMaxValueCorrectionMove(ref minSpeedRange, ref maxSpeedRange, ref previousMinSpeedRange);
        Funcs.minMaxValueCorrectionMove(ref minVolume, ref maxVolume, ref previousMinVolume);
        Funcs.minMaxValueCorrectionMove(ref minPitch, ref maxPitch, ref previousMinPitch);
    }
}
