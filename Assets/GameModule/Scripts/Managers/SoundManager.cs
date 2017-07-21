using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Managers
{
    public class SoundManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private List<AudioClip> soundsHard;
        [SerializeField] private List<AudioClip> soundsLight;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //if (GameManager.instance.BBModule.IsEnabled)
            //{
            //    // check if Player is scared/anxious/nervous enough to play sound
            //}
            //else
            //{
            //    // play sounds at random time - but still choose the best awailable audio source
            //}

            // test:
            Debug.DrawLine(GameManager.instance.Player.transform.position, FindBestSoundSource().transform.position, Color.cyan);
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Finds best awailable game object with audio source.
        /// </summary>
        /// <returns>Game object with audio source component</returns>
        private GameObject FindBestSoundSource()
        {
            // choose the best sound source:
            // -> one which is behind the Player and is the closest to him:
            GameObject soundSource = null, soundSourceSecond = null;
            float minDistance = 100f, minDistanceSecond = 100f;
            foreach(Transform child in transform)
            {
                Vector3 playerToSoundSource = child.transform.position - GameManager.instance.Player.transform.position;
                // sound source is behind player:
                if (Vector3.Dot(playerToSoundSource, GameManager.instance.Player.transform.forward) <= 0)
                {
                    if (playerToSoundSource.magnitude < minDistance)
                    {
                        soundSource = child.gameObject;
                        minDistance = playerToSoundSource.magnitude;
                    }
                }
                // if sound source is in front of player:
                else
                {
                    if (playerToSoundSource.magnitude < minDistanceSecond)
                    {
                        soundSourceSecond = child.gameObject;
                        minDistanceSecond = playerToSoundSource.magnitude;
                    }
                }
            }

            // choose the best sound source:
            if (soundSource != null) return soundSource;
            else return soundSourceSecond;
        }
        #endregion
    }
}
