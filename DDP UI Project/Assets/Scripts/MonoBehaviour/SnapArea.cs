using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SnapArea : MonoBehaviour
{
    public Collider2D Area { get { return trigger; } }

    [Header("References")]
    public Collider2D trigger;
    public AudioSource snapAudioSource;
    public Disc currentDisc = null;

    [Header("Snap sounds")]
    public AudioClip snapSound;
    [Range(0f, 1f)]
    public float snapVolume = 1f;
    [Range(-3f, 3f)]
    public float enterPitch = 1f;
    [Range(-3f, 3f)]
    public float leavePitch = 1f;
    //public AudioClip enterClip;
    //public AudioClip leaveClip;

    [Header("Settings")]
    public Vector3 offset;

    protected bool holding = false;

    protected void Awake()
    {
        if (Area == null)
            Debug.Log("The area for SnapArea has not been assigned");

        MTouchController.snapAreas.Add(this);
    }

    protected void Update()
    {
        if (currentDisc != null)
            currentDisc.transform.position = transform.position + offset;
    }

    public virtual void Enter(MTouch mt)
    {
        Disc disc = mt.currentMTble as Disc;

        Enter(disc);
    }

    public virtual void Enter(Disc disc)
    {
        currentDisc = disc;

        disc.rid.velocity = Vector2.zero;
        disc.rid.constraints = RigidbodyConstraints2D.FreezePosition;
        disc.SetLayerOrderToBack();

        holding = true;

        //PlaySound(enterClip);
        PlaySnapSound(enterPitch);
    }

    public virtual void Holding()
    {
        
    }

    public virtual void Stay()
    {
        holding = false;
    }

    public virtual void HoldAgain()
    {
        holding = true;
    }

    public virtual void Leave()
    {
        currentDisc.rid.constraints = RigidbodyConstraints2D.None;
        currentDisc.UpdateLayerOrder();

        currentDisc = null;

        holding = false;

        //PlaySound(leaveClip);
        PlaySnapSound(leavePitch);
    }

    /*/
    protected void PlayEnterSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    protected void PlayLeaveSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
    /*/

    /*/
    protected void PlaySound(AudioClip clip)
    {
        if (clip != null && snapAudioSource != null)
        {
            snapAudioSource.clip = clip;
            snapAudioSource.Play();
        }
    }
    /*/

    protected void PlaySnapSound(float pitch)
    {
        if (snapSound != null && snapAudioSource != null)
        {
            snapAudioSource.volume = snapVolume;
            snapAudioSource.pitch = pitch;
            snapAudioSource.clip = snapSound;
            snapAudioSource.Play();
        }
    }
}
