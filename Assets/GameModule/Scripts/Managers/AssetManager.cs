﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages common assets resources.
    /// </summary>
    public class AssetManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private Color highlightColor;
        [SerializeField] private Color hintColor;
        [SerializeField] private float interactionRange = 2f;
        [SerializeField] private float hintRange = 4f;
        [SerializeField] private List<AudioClip> metalDoorSqueak;
        [SerializeField] private List<AudioClip> woodenDoorSqueak;
        #endregion


        #region Public fields
        /// <summary>Highlight color.</summary>
        public Color HighlightColor { get { return highlightColor; } }
        /// <summary>Hint color.</summary>
        public Color HintColor { get { return hintColor; } }
        /// <summary>Interaction range.</summary>
        public float InteractionRange { get { return interactionRange; } }
        /// <summary>Hint range.</summary>
        public float HintRange { get { return hintRange; } }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            metalDoorSqueak = new List<AudioClip>();
            woodenDoorSqueak = new List<AudioClip>();

            metalDoorSqueak.AddRange(Resources.LoadAll<AudioClip>("Audio/metalSqueak"));
            woodenDoorSqueak.AddRange(Resources.LoadAll<AudioClip>("Audio/woodenSqueak"));
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Public methods
        /// <summary>
        /// Returns random metal door squeak sound.
        /// </summary>
        /// <returns>Random squeak sound</returns>
        public AudioClip GetMetalSqueakSound()
        {
            if (metalDoorSqueak.Count > 0)
            {
                int index = Random.Range(0, metalDoorSqueak.Count);
                return metalDoorSqueak[index];
            }
            else return null;
        }

        /// <summary>
        /// Returns random wooden door squeak sound.
        /// </summary>
        /// <returns>Random squeak sound</returns>
        public AudioClip GetWoodenSqueakSound()
        {
            if (woodenDoorSqueak.Count > 0)
            {
                int index = Random.Range(0, woodenDoorSqueak.Count);
                return woodenDoorSqueak[index];
            }
            else return null;
        }
        #endregion
    }
}
