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

    [Header("Audio clip")]
    public AudioClip audioClip;
    [Min(0f)]
    public float maxAudioClipLength;

    AudioClip previousAudioClip;

    [Header("Other sound settings")]
    public bool playHereWhenTrashCanIsHit;

    [Header("Small force index")]
    public float smallForceIndex = 0.005f;

    [Header("Point effector settings")]
    public float forceMagnitude;
    public EffectorForceMode2D forceMode;
    public float colliderRadius = 1f;

    [Header("Drag DiscValues here to copy values")]
    public DiscValues copyFrom;
    public bool allowCopyFromValues;

    void OnValidate()
    {
        Funcs.minMaxValueCorrectionMove(ref minSpeedRange, ref maxSpeedRange, ref previousMinSpeedRange);
        Funcs.minMaxValueCorrectionMove(ref minVolume, ref maxVolume, ref previousMinVolume);
        Funcs.minMaxValueCorrectionMove(ref minPitch, ref maxPitch, ref previousMinPitch);

        if (audioClip != previousAudioClip)
            maxAudioClipLength = audioClip.length;
        else if (audioClip.length < maxAudioClipLength)
            maxAudioClipLength = audioClip.length;

        previousAudioClip = audioClip;

        if (copyFrom != null)
        {
            if (allowCopyFromValues)
                SetPublicFields(copyFrom);
            copyFrom = null;
        }
    }

    private void SetPublicFields(DiscValues setTo)
    {
        minSpeedRange = setTo.minSpeedRange;
        maxSpeedRange = setTo.maxSpeedRange;
        minVolume = setTo.minVolume;
        maxVolume = setTo.maxVolume;
        minPitch = setTo.minPitch;
        maxPitch = setTo.maxPitch;
        audioClip = setTo.audioClip;
        maxAudioClipLength = setTo.maxAudioClipLength;
        playHereWhenTrashCanIsHit = setTo.playHereWhenTrashCanIsHit;
    }
}
