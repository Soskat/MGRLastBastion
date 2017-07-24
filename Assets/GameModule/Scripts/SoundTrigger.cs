﻿using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game
{
    /// <summary>
    /// Component that enables playing assigned sound on trigger (collision).
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class SoundTrigger : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private AudioClip triggeredSound;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(triggeredSound);
        }
        
        // Use this for initialization
        void Start()
        {
            GetComponent<AudioSource>().playOnAwake = false;
            GetComponent<AudioSource>().loop = false;
        }

        // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                PlaySound();
            }
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Plays the assigned sounds.
        /// </summary>
        public void PlaySound()
        {
            GetComponent<AudioSource>().PlayOneShot(triggeredSound);
        }
        #endregion
    }
}