using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
public class ParticleTest : MonoBehaviour
{
    #region Private fields
    [SerializeField] private AudioClip triggeredSound;
    [SerializeField] private AudioSource audioSource;
    #endregion


    #region MonoBehaviour methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Assert.IsNotNull(triggeredSound);
        Assert.IsNotNull(audioSource);
    }

    // Use this for initialization
    void Start()
    {
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    private void OnCollisionExit(Collision collision)
    {
        PlaySound();
        Debug.Log("Collision enter");
    }

    // OnParticleTrigger is called when any particles in a particle system meet the conditions in the trigger module
    private void OnParticleTrigger()
    {
        PlaySound();
        Debug.Log("Particle trigger");
    }
    #endregion


    #region Public methods
    /// <summary>
    /// Plays the assigned sounds.
    /// </summary>
    public void PlaySound()
    {
        audioSource.PlayOneShot(triggeredSound);
    }
    #endregion
}
